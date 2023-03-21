using System.Diagnostics;
using System.IO.Pipes;
using System.Runtime.InteropServices;
using MediaFileProcessor.Models.Settings;
namespace MediaFileProcessor;
//TODO replace indexes in all files to avoid unnecessary memory allocation
//TODO change cancelationToken to default

/// <summary>
/// Represents a media file processing class
/// </summary>
public class MediaFileProcess : IDisposable
{
    private Process Process { get; }

    /// <summary>
    /// The processing settings for the media file process.
    /// </summary>
    private ProcessingSettings Settings { get; }

    /// <summary>
    /// The input streams for the process.
    /// </summary>
    private Stream[]? InputStreams { get; set; }

    /// <summary>
    /// Named pipes through which data will be transferred
    /// </summary>
    private NamedPipeServerStream[]? NamedPipes { get; set; }

    /// <summary>
    /// The pipe names for the process.
    /// </summary>
    private string[]? PipeNames { get; set; }

    /// <summary>
    /// Flag indicating whether the executable process has been executed.
    /// If true then you should no longer try to execute this process.
    /// Any arguments like named pipes or input or output streams can be closed or their pointers shifted
    /// </summary>
    private bool IsDisposed { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MediaFileProcess"/> class.
    /// </summary>
    /// <param name="processFileName">The name of the process file.</param>
    /// <param name="arguments">The arguments for the process file.</param>
    /// <param name="settings">The processing settings for the media file process.</param>
    /// <param name="inputStreams">The input streams for the process. Optional parameter.</param>
    /// <param name="pipeNames">The pipe names for the process. Optional parameter.</param>
    public MediaFileProcess(string processFileName,
                            string arguments,
                            ProcessingSettings settings,
                            Stream[]? inputStreams = null,
                            string[]? pipeNames = null)
    {
        Process = new Process();
        Settings = settings;
        InputStreams = inputStreams;
        PipeNames = pipeNames;
        Process.StartInfo.FileName = processFileName;
        Process.StartInfo.Arguments = arguments;
        Process.StartInfo.CreateNoWindow = Settings.CreateNoWindow;
        Process.StartInfo.UseShellExecute = Settings.UseShellExecute;
        Process.StartInfo.RedirectStandardInput = inputStreams is not null;
        Process.StartInfo.RedirectStandardOutput = settings.IsStandartOutputRedirect;
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

        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
        AppDomain.CurrentDomain.ProcessExit += DomainProcessExitHandler;
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
        IsDisposed = true;
    }

    /// <summary>
    /// Asynchronously executes the process and returns the output as a memory stream if
    /// RedirectStandardOutput is set to true.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>The memory stream containing the output of the process, or null if
    /// RedirectStandardOutput is set to false</returns>
    public async Task<MemoryStream?> ExecuteAsync(CancellationToken cancellationToken)
    {
        if(IsDisposed)
            throw new ObjectDisposedException("The process is no longer executable. "
                                            + "Any arguments like named pipes or input or output streams can be closed or their pointers shifted");

        MemoryStream? outputStream = null;
        if(Process.StartInfo.RedirectStandardOutput)
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
                        Dispose();
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
        NamedPipes = new NamedPipeServerStream[InputStreams!.Length];

        // Loop through the pipes array and initialize each instance of NamedPipeServerStream with the name of the pipe
        // and other required parameters for pipe transmission.
        for (var i = 0; i < NamedPipes.Length; i++)
            NamedPipes[i] = new NamedPipeServerStream(PipeNames![i], PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

        // Create an array of tasks with the size of the pipes array
        var tasks = new Task[NamedPipes.Length];

        // Loop through the pipes array and connect each pipe with its corresponding input stream
        for (var i = 0; i < NamedPipes.Length; i++)
        {
            var pipe = NamedPipes[i];
            var inputStream = InputStreams[i];

            // Wait for the connection to be established
            pipe.WaitForConnection();

            // Create a new task to copy data from the input stream to the pipe
            tasks[i] = inputStream.CopyToAsync(pipe)
                                  .ContinueWith(_ =>
                                  {
                                      //TODO fix tiff format
                                      //TODO fix 3gp format add audio
                                      //TODO fix m2ts format add audio
                                      //TODO fix mpeg, mxf format add audio
                                      //TODO fix m4v, mkv, mov, mp4 format add audio
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

    /// <summary>
    /// Implements the IDisposable interface to release unmanaged resources used by this object.
    /// </summary>
    public void Dispose()
    {
        if (InputStreams != null)
        {
            foreach(var inputStream in InputStreams)
                inputStream.Dispose();
            InputStreams = null;
        }

        if (NamedPipes != null)
        {
            foreach(var namedPipe in NamedPipes)
                namedPipe.Dispose();
            NamedPipes = null;
        }

        if (PipeNames != null)
        {
            foreach (var pipeFile in PipeNames)
            {
                if(File.Exists(pipeFile))
                    File.Delete(pipeFile);
            }

            PipeNames = null;
        }

        // Dispose of the process and kill it
        Process.Dispose();

        try
        {
            if(Process.HasExited)
                Process.Kill();
        }
        catch (InvalidOperationException e)
        {
            if(e.Message != "No process is associated with this object.")
                throw;
        }

        IsDisposed = true;
    }

    /// <summary>
    /// This method is executed in the event of an application crash so that child executable processes do not remain alive in the background
    /// </summary>
    private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
    {
        Dispose();
    }

    /// <summary>
    /// This method is executed in the event of an application exit so that child executable processes do not remain alive in the background
    /// </summary>
    private void DomainProcessExitHandler(object sender, EventArgs e)
    {
        Dispose();
    }
}