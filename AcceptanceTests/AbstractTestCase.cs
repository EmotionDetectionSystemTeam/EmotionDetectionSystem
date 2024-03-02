using AcceptanceTests.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcceptanceTests
{
    [TestFixture]
    public abstract class AbstractTestCase
    {
        protected EDS eds;
        private string serverUri = "https://localhost:7069/";
        [SetUp]
        public async void TestStarted()
        {
            //need to start the emotion detection server
            eds = new EDS(serverUri, new HttpClient());
            // Replace "path/to/your/program.exe" with the actual path to your executable file
            string exePath = @"""C:\Users\gigin\OneDrive - Hewlett Packard Enterprise\Desktop\לימודים\EMS\EmotionDetectionServer\bin\Debug\net7.0\EmotionDetectionServer.exe""";
            // Create a CancellationTokenSource to manage the cancellation token
            var cancellationTokenSource = new CancellationTokenSource();

            // Run the process on a separate thread
            Task processTask = Task.Run(() => RunProcess(exePath, cancellationTokenSource.Token));

            // Simulate some work on the main thread
            Console.WriteLine("Main thread is doing some work...");

            // Allow some time for the process to run (adjust as needed)
            await Task.Delay(5000);

            // Request cancellation of the process thread
            cancellationTokenSource.Cancel();

            // Wait for the process thread to complete
            await processTask;

            Console.WriteLine("Main thread completed.");
        }
        static void RunProcess(string exePath, CancellationToken cancellationToken)
        {
            Process process = new Process();
            process.StartInfo.FileName = exePath;

            try
            {
                process.Start();

                // Wait for the process to exit or cancellation is requested
                while (!process.HasExited && !cancellationToken.IsCancellationRequested)
                {
                    // Add a short delay to avoid high CPU usage
                    Thread.Sleep(100);
                }

                if (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Executable closed successfully. Base Priority: {process.BasePriority}");
                }
                else
                {
                    Console.WriteLine("Executable closed, but the process may not have exited properly.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Ensure the process is closed to free up resources
                if (!process.HasExited)
                    process.Kill();

                process.Dispose();
            }
        }
    }
}
