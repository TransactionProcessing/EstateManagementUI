using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace EstateManagementUI.OfflineIntegrationTests.Common
{
    /// <summary>
    /// Manages the lifecycle of the Blazor application for testing
    /// Automatically starts and stops the application
    /// </summary>
    public class ApplicationHost : IDisposable
    {
        private Process? applicationProcess;
        private readonly string applicationUrl = "https://localhost:5004";
        private readonly int startupTimeoutSeconds = 60;
        private bool isDisposed = false;

        /// <summary>
        /// Starts the Blazor application with TestMode enabled
        /// </summary>
        public async Task StartApplicationAsync()
        {
            if (applicationProcess != null)
            {
                Console.WriteLine("Application is already running");
                return;
            }

            Console.WriteLine("Starting Blazor application...");

            // Find the Blazor Server project directory
            var testProjectDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"Current directory: {testProjectDirectory}");
            
            // Find the solution directory by looking for .sln file
            var solutionDirectory = FindSolutionDirectory(testProjectDirectory);
            
            if (solutionDirectory == null)
            {
                throw new InvalidOperationException($"Could not find solution directory from: {testProjectDirectory}");
            }

            Console.WriteLine($"Solution directory found: {solutionDirectory}");

            var blazorProjectPath = Path.Combine(solutionDirectory, "EstateManagementUI.BlazorServer");
            
            if (!Directory.Exists(blazorProjectPath))
            {
                throw new InvalidOperationException($"Blazor Server project not found at: {blazorProjectPath}");
            }

            // Build the application first
            Console.WriteLine("Building Blazor application...");
            await BuildApplicationAsync(blazorProjectPath);

            // Start the application process
            var startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "run --no-build",
                WorkingDirectory = blazorProjectPath,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true,
            };

            // Set TestMode environment variable
            startInfo.Environment["TestMode"] = "true";
            startInfo.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";

            applicationProcess = new Process { StartInfo = startInfo };
            
            // Capture output for debugging
            applicationProcess.OutputDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine($"[APP] {args.Data}");
                }
            };
            
            applicationProcess.ErrorDataReceived += (sender, args) =>
            {
                if (!string.IsNullOrEmpty(args.Data))
                {
                    Console.WriteLine($"[APP ERROR] {args.Data}");
                }
            };

            applicationProcess.Start();
            applicationProcess.BeginOutputReadLine();
            applicationProcess.BeginErrorReadLine();

            Console.WriteLine($"Application process started (PID: {applicationProcess.Id})");

            // Wait for the application to be ready
            await WaitForApplicationReadyAsync();

            Console.WriteLine("Blazor application is ready!");
        }

        /// <summary>
        /// Finds the solution directory by traversing up the directory tree
        /// </summary>
        private string? FindSolutionDirectory(string startPath)
        {
            var directory = new DirectoryInfo(startPath);
            
            // Traverse up the directory tree looking for a .sln file
            while (directory != null)
            {
                // Check if this directory contains a .sln file
                var solutionFiles = directory.GetFiles("*.sln");
                if (solutionFiles.Length > 0)
                {
                    return directory.FullName;
                }
                
                // Move up to parent directory
                directory = directory.Parent;
            }
            
            return null;
        }

        /// <summary>
        /// Builds the Blazor application
        /// </summary>
        private async Task BuildApplicationAsync(string projectPath)
        {
            var buildProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "build --configuration Debug",
                    WorkingDirectory = projectPath,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };

            buildProcess.Start();
            
            var output = await buildProcess.StandardOutput.ReadToEndAsync();
            var error = await buildProcess.StandardError.ReadToEndAsync();
            
            await buildProcess.WaitForExitAsync();

            if (buildProcess.ExitCode != 0)
            {
                Console.WriteLine($"Build output: {output}");
                Console.WriteLine($"Build errors: {error}");
                throw new InvalidOperationException($"Failed to build Blazor application. Exit code: {buildProcess.ExitCode}");
            }

            Console.WriteLine("Blazor application built successfully");
        }

        /// <summary>
        /// Waits for the application to be ready by polling the health endpoint
        /// </summary>
        private async Task WaitForApplicationReadyAsync()
        {
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };

            using var httpClient = new HttpClient(httpClientHandler)
            {
                Timeout = TimeSpan.FromSeconds(5)
            };

            var startTime = DateTime.UtcNow;
            var timeout = TimeSpan.FromSeconds(startupTimeoutSeconds);

            while (DateTime.UtcNow - startTime < timeout)
            {
                try
                {
                    // Try to connect to the application
                    var response = await httpClient.GetAsync(applicationUrl);
                    
                    // If we get any response, the application is running
                    if (response != null)
                    {
                        Console.WriteLine($"Application responded with status: {response.StatusCode}");
                        
                        // Give it a little more time to fully initialize
                        await Task.Delay(2000);
                        return;
                    }
                }
                catch (HttpRequestException)
                {
                    // Application not ready yet, continue waiting
                    Console.WriteLine("Waiting for application to start...");
                }
                catch (TaskCanceledException)
                {
                    // Timeout on individual request, continue waiting
                    Console.WriteLine("Request timeout, retrying...");
                }

                await Task.Delay(1000);
            }

            throw new TimeoutException($"Application did not start within {startupTimeoutSeconds} seconds");
        }

        /// <summary>
        /// Stops the Blazor application
        /// </summary>
        public void StopApplication()
        {
            if (applicationProcess != null && !applicationProcess.HasExited)
            {
                Console.WriteLine("Stopping Blazor application...");
                
                try
                {
                    // Try graceful shutdown first
                    applicationProcess.Kill(entireProcessTree: true);
                    applicationProcess.WaitForExit(5000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error stopping application: {ex.Message}");
                }
                finally
                {
                    applicationProcess?.Dispose();
                    applicationProcess = null;
                }

                Console.WriteLine("Blazor application stopped");
            }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                StopApplication();
                isDisposed = true;
            }
        }
    }
}
