using System.Diagnostics;
using System.IO.Pipes;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor;

public class MediaFileProcess
{
    private Process Process { get; }

    private ProcessingSettings Settings { get; }

    private Stream[]? InputStreams { get; }

    private string[]? PipeNames { get; }

    private bool RedirectOutputToStream { get; }

    public MediaFileProcess(string processFileName,
                            string arguments,
                            ProcessingSettings settings,
                            Stream[]? inputStreams = null,
                            bool redirectOutputToStream = false,
                            string[]? pipeNames = null)
    {
        Process = new Process();
        Settings = settings;
        InputStreams = inputStreams;
        PipeNames = pipeNames;
        RedirectOutputToStream = redirectOutputToStream;
        Process.StartInfo.FileName = processFileName;
        Process.StartInfo.Arguments = arguments;
        Process.StartInfo.CreateNoWindow = Settings.CreateNoWindow;
        Process.StartInfo.UseShellExecute = Settings.UseShellExecute;
        Process.StartInfo.RedirectStandardInput = inputStreams is not null;
        Process.StartInfo.RedirectStandardOutput = RedirectOutputToStream;
        Process.StartInfo.RedirectStandardError = Settings.RedirectStandardError;
        Process.EnableRaisingEvents = Settings.EnableRaisingEvents;
        Process.StartInfo.WindowStyle = Settings.WindowStyle;
        if(Settings.ProcessOnExitedHandler is not null)
            Process.Exited += Settings.ProcessOnExitedHandler;
        else
            Process.Exited += ProcessOnExited;

        if(Settings.OutputDataReceivedEventHandler is not null)
            Process.OutputDataReceived += Settings.OutputDataReceivedEventHandler;

        if(Settings.ErrorDataReceivedHandler is not null)
            Process.ErrorDataReceived += Settings.ErrorDataReceivedHandler;
    }

    private void ProcessOnExited(object sender, EventArgs e)
    {
        Process.WaitForExit();
        Process.Exited -= ProcessOnExited;
    }

    public async Task<MemoryStream?> ExecuteAsync(CancellationToken cancellationToken)
    {
        MemoryStream? outputStream = null;
        if(RedirectOutputToStream)
            outputStream = new MemoryStream();

        await Task.Factory.StartNew(() => Run(outputStream, cancellationToken),
                                    cancellationToken,
                                    TaskCreationOptions.LongRunning,
                                    TaskScheduler.Default);

        outputStream?.Seek(0, SeekOrigin.Begin);

        return outputStream;
    }

    private void Run(MemoryStream? outputStream, CancellationToken cancellationToken)
    {
        try
        {
            using (Process)
            {
                var processExited = false;
                Process.Start();

                cancellationToken.Register(() =>
                {
                    try
                    {
                        if (processExited || Process.HasExited)
                            return;

                        Process.Kill();
                    }
                    catch(Exception)
                    {
                        // ignored
                    }
                });


                if(Settings.ErrorDataReceivedHandler is not null)
                    Process.BeginErrorReadLine();
                if(Settings.OutputDataReceivedEventHandler is not null)
                    Process.BeginOutputReadLine();

                Task? inputTask = null;
                Task? outputTask = null;

                if(InputStreams is not null)
                    inputTask = Task.Run(WriteStandartInput, cancellationToken);

                if(outputStream is not null)
                    outputTask = Task.Run(() => ReadStandartOutput(outputStream), cancellationToken);

                Task.WaitAll(inputTask ?? Task.CompletedTask, outputTask ?? Task.CompletedTask);

                Process.WaitForExit();
                processExited = true;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            throw;
        }
    }

    private void WriteStandartInput()
    {
        if(InputStreams?.Length == 1)
            try
            {
                InputStreams[0].CopyTo(Process.StandardInput.BaseStream);
                Process.StandardInput.Close();
            }
            catch (Exception e)
            {
                if(e.Message != "Канал был закрыт.")
                    throw;
            }

        if(InputStreams?.Length > 1)
        {
            var pipes = new NamedPipeServerStream[InputStreams.Length];

            for (var i = 0; i < pipes.Length; i++)
                pipes[i] = new NamedPipeServerStream(PipeNames![i], PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

            var tasks = new Task[pipes.Length];

            for (var i = 0;
                 i < pipes.Length;
                 i++)
            {
                var pipe = pipes[i];
                var inputStream = InputStreams[i];

                pipe.WaitForConnection();

                tasks[i] = inputStream.CopyToAsync(pipe)
                                      .ContinueWith(_ =>
                                      {
                                          pipe.WaitForPipeDrain();
                                          pipe.Disconnect();
                                      });
            }

            Task.WaitAll(tasks);
        }
    }

    private void ReadStandartOutput(MemoryStream outputStream)
    {
        var buffer = new byte[64 * 1024];

        int lastRead;

        do
        {
            lastRead = Process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);
            outputStream.Write(buffer, 0, lastRead);
        } while (lastRead > 0);
    }
}