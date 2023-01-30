﻿using System.Diagnostics;
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
        // Process.StartInfo.Arguments = "-y -ss 00:00:27.500 -i outpipe1.avi -frames:v 1 -f image2 result%03d.jpg";
        Process.StartInfo.Arguments = "-y -i outpipe1 -i outpipe2 -filter_complex \"overlay=5:5\" -f avi result.avi";
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
            }
            catch (Exception e)
            {
                if(e.Message != "Канал был закрыт.")
                    throw;
            }
            finally
            {
                Process.StandardInput.Flush();
                Process.StandardInput.Close();
            }


        if(InputStreams?.Length > 1)
        {
            var tasks = new Task[PipeNames!.Length];

            for (var i = 0;
                 i < PipeNames!.Length;
                 i++)
            {
                var pipe = PipeNames![i];
                var inputStream = InputStreams[i];

                Console.WriteLine("OpenWrite " + pipe + $" {Process.StartInfo.Arguments}");
                var fs = File.OpenWrite($"{pipe}");
                tasks[i] = Task.Run(() =>
                {
                    try
                    {
                        Console.WriteLine(((FileStream)inputStream).Name + " " + pipe);
                        inputStream.CopyToAsync(fs);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("ERROR " + PipeNames[i]);

                        // ignored
                    }
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