using TestEDS.Logic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestEDS
{
    [TestFixture]
    public abstract class AbstractTestCase
    {
        protected EDS eds;
        private string serverUri = "https://localhost:7069/";
        CancellationTokenSource cancellationTokenSource;
        [SetUp]
        public void TestStarted()
        {
            //need to start the emotion detection server
            eds = new EDS(serverUri, new HttpClient());
            //// Replace "path/to/your/program.exe" with the actual path to your executable file
            //string exePath = @"""C:\Users\gigin\OneDrive - Hewlett Packard Enterprise\Desktop\לימודים\EMS\EmotionDetectionServer\bin\Debug\net7.0\EmotionDetectionServer.exe""";
            //// Create a CancellationTokenSource to manage the cancellation token
            //cancellationTokenSource = new CancellationTokenSource();

            //// Run the process on a separate thread
            //Task processTask = Task.Run(() => RunProcess(exePath, cancellationTokenSource.Token));
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
        [TearDown]
        public void AfterTest()
        {
            //cancellationTokenSource.Cancel();
            //Thread.Sleep(5000);//wait all tasks to finish
        }
    }
}
