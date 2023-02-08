using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor;

/// <summary>
/// Represents a media file processing class
/// </summary>
public class MediaFileProcess
{
    private Process Process { get; }

    /// <summary>
    /// The processing settings for the media file process.
    /// </summary>
    private ProcessingSettings Settings { get; }

    /// <summary>
    /// The input streams for the process.
    /// </summary>
    private Stream[]? InputStreams { get; }

    /// <summary>
    /// The pipe names for the process.
    /// </summary>
    private string[]? PipeNames { get; }

    /// <summary>
    /// Specifies whether to redirect the output to a stream.
    /// </summary>
    private bool RedirectOutputToStream { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MediaFileProcess"/> class.
    /// </summary>
    /// <param name="processFileName">The name of the process file.</param>
    /// <param name="arguments">The arguments for the process file.</param>
    /// <param name="settings">The processing settings for the media file process.</param>
    /// <param name="inputStreams">The input streams for the process. Optional parameter.</param>
    /// <param name="redirectOutputToStream">Specifies whether to redirect the output to a stream. Optional parameter, defaults to false.</param>
    /// <param name="pipeNames">The pipe names for the process. Optional parameter.</param>
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

    /// <summary>
    /// The event handler that will be triggered when the process exits.
    /// It waits for the process to exit and unsubscribes from the event.
    /// </summary>
    /// <param name="sender">The sender object</param>
    /// <param name="e">The event arguments</param>
    private void ProcessOnExited(object sender, EventArgs e)
    {
        Process.WaitForExit();
        Process.Exited -= ProcessOnExited;
    }

    /// <summary>
    /// Asynchronously executes the process and returns the output as a memory stream if
    /// <see cref="RedirectOutputToStream"/> is set to true.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>The memory stream containing the output of the process, or null if
    /// <see cref="RedirectOutputToStream"/> is set to false</returns>
    public async Task<MemoryStream?> ExecuteAsync(CancellationToken cancellationToken)
    {
        MemoryStream? outputStream = null;
        if(RedirectOutputToStream)
            outputStream = new MemoryStream();

        await Task.Factory.StartNew(() => Run(outputStream, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default);

        outputStream?.Seek(0, SeekOrigin.Begin);

        return outputStream;
    }

    /// <summary>
    /// This method is responsible for running the process.
    /// </summary>
    /// <param name="outputStream">The stream to which the result of the process will be written
    /// if it is specified and the process is configured to issue the result in StandardOutput</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    private void Run(MemoryStream? outputStream, CancellationToken cancellationToken)
    {
        try
        {
            //Wrapping the process in a using statement to ensure it is disposed properly.
            using (Process)
            {
                //A flag to keep track if the process has already exited.
                var processExited = false;

                //Starting the process.
                Process.Start();

                //Registers a callback for when the cancellation token is cancelled.
                //This will kill the process if it is still running.
                cancellationToken.Register(() =>
                {
                    try
                    {
                        //Check if the process has already exited or if the process exit has already been handled.
                        if (processExited || Process.HasExited)
                            return;

                        //Killing the process.
                        Process.Kill();
                    }
                    catch(Exception)
                    {
                        //Ignoring any exceptions that may occur while trying to kill the process.
                    }
                });

                //If an error data received handler is set, start reading error data.
                if(Settings.ErrorDataReceivedHandler is not null)
                    Process.BeginErrorReadLine();

                //If an output data received handler is set, start reading output data.
                if(Settings.OutputDataReceivedEventHandler is not null)
                    Process.BeginOutputReadLine();

                //Tasks for handling input and output streams.
                Task? inputTask = null;
                Task? outputTask = null;

                //If input streams are available, start a task to write the input to the process' standard input.
                if(InputStreams is not null)
                    inputTask = Task.Run(WriteStandartInput, cancellationToken);

                //If an output stream is available, start a task to read the process' standard output.
                if(outputStream is not null)
                    outputTask = Task.Run(() => ReadStandartOutput(outputStream), cancellationToken);

                //Wait for both tasks to complete.
                Task.WaitAll(inputTask ?? Task.CompletedTask, outputTask ?? Task.CompletedTask);

                //Wait for the process to exit.
                Process.WaitForExit();

                //Set the processExited flag to true.
                processExited = true;
            }
        }
        catch (Exception e)
        {
            //Write the exception to the console.
            Console.WriteLine(e);

            //Rethrow the exception.
            throw;
        }
    }

    // This method writes the input data to the standard input of the process
    private void WriteStandartInput()
    {
        // If there is only one input stream
        if(InputStreams?.Length == 1)
            try
            {
                // Copy data from the input stream to the standard input of the process
                InputStreams[0].CopyTo(Process.StandardInput.BaseStream);

                // Flush the data to the standard input of the process
                Process.StandardInput.Flush();

                // Close the standard input stream
                Process.StandardInput.Close();
            }
            catch (Exception)
            {
                // ignore any exceptions that occur during the copying process
            }

        // If there are multiple input streams
        if (!(InputStreams?.Length > 1))
            return;

        // Based on the operating system, use the appropriate method for handling multiple input streams
        if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            MultiInputWindowsOS();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            MultiInpuLinuxOS();
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))

            // Throw an exception if the operating system is not supported for multi inputs
            throw new Exception("Operating System not supported for multi inputs");
        else

            // Throw an exception if the operating system cannot be recognized
            throw new Exception("Operating System not recognized");
    }

    /// <summary>
    /// This method is used to handle the scenario when the OS is Windows and multiple input streams are being processed.
    /// </summary>
    private void MultiInputWindowsOS()
    {
        // Create an array of NamedPipeServerStream instances with the size of the input streams
        var pipes = new NamedPipeServerStream[InputStreams!.Length];

        // Loop through the pipes array and initialize each instance of NamedPipeServerStream with the name of the pipe
        // and other required parameters for pipe transmission.
        for (var i = 0; i < pipes.Length; i++)
            pipes[i] = new NamedPipeServerStream(PipeNames![i], PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

        // Create an array of tasks with the size of the pipes array
        var tasks = new Task[pipes.Length];

        // Loop through the pipes array and connect each pipe with its corresponding input stream
        for (var i = 0; i < pipes.Length; i++)
        {
            var pipe = pipes[i];
            var inputStream = InputStreams[i];

            // Wait for the connection to be established
            pipe.WaitForConnection();

            // Create a new task to copy data from the input stream to the pipe
            tasks[i] = inputStream.CopyToAsync(pipe)
                                  .ContinueWith(_ =>
                                   {
                                       // Wait for the data to be transmitted completely
                                       pipe.WaitForPipeDrain();

                                       // Disconnect the pipe after the data has been transmitted
                                       pipe.Disconnect();
                                   });
        }

        // Wait for all tasks to complete
        Task.WaitAll(tasks);
    }

    // MultiInputLinuxOS is a method that handles the input streams and pipes them to the process when the
    // operating system is Linux.
    private void MultiInpuLinuxOS()
    {
        // Create an array of tasks to handle each input stream
        var tasks = new Task[InputStreams!.Length];

        // Loop through the pipe names and open a file stream for each one
        for (var i = 0; i < PipeNames!.Length; i++)
        {
            var fs = File.OpenWrite($"{PipeNames[i]}");

            // Start a task to copy the input stream data to the file stream
            tasks[i] = InputStreams[i].CopyToAsync(fs);
        }

        // Wait for all tasks to complete
        Task.WaitAll(tasks);

        // Loop through the pipe names and delete the files
        foreach (var pipeFile in PipeNames)
            File.Delete(pipeFile);
    }

    // ReadStandartOutput is a method that reads the standard output from the process and writes it to the
    // provided output stream.
    private void ReadStandartOutput(MemoryStream outputStream)
    {
        // Create a buffer to store the data being read from the process
        var buffer = new byte[64 * 1024];

        // Variable to store the number of bytes read in the last read operation
        int lastRead;

        // Read data from the process while there is still data available
        do
        {
            // Read data from the process and store the number of bytes read
            lastRead = Process.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);

            // Write the data to the output stream
            outputStream.Write(buffer, 0, lastRead);
        } while (lastRead > 0);
    }
}