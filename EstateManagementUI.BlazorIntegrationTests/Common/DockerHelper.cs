using Testcontainers.Builders;
using Testcontainers.Containers;
using Testcontainers.Images;

namespace EstateManagementUI.IntegrationTests.Common;

/// <summary>
/// Clean, minimal Docker helper for Testcontainers-based integration testing.
/// Builds and manages the BlazorServer container lifecycle.
/// </summary>
public class DockerHelper : IAsyncDisposable
{
    private IImage? _blazorServerImage;
    private IContainer? _blazorServerContainer;
    
    /// <summary>
    /// Gets the mapped public port for the Blazor Server application.
    /// </summary>
    public int EstateManagementUiPort { get; private set; }
    
    /// <summary>
    /// Gets whether the container is currently running.
    /// </summary>
    public bool IsRunning => _blazorServerContainer != null;

    /// <summary>
    /// Builds the Docker image from the BlazorServer Dockerfile and starts the container.
    /// </summary>
    public async Task StartContainerAsync()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  Starting Testcontainers-based Integration Test Setup");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        
        // Build the Docker image from Dockerfile
        await BuildBlazorServerImageAsync();
        
        // Start the container
        await StartBlazorServerContainerAsync();
        
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine($"  Blazor Server ready at: https://localhost:{EstateManagementUiPort}");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
    }

    /// <summary>
    /// Stops and cleans up the container and image.
    /// </summary>
    public async Task StopContainerAsync()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  Stopping Testcontainers and cleaning up resources");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        
        if (_blazorServerContainer != null)
        {
            try
            {
                await _blazorServerContainer.StopAsync();
                Console.WriteLine("✓ Container stopped successfully");
                
                await _blazorServerContainer.DisposeAsync();
                Console.WriteLine("✓ Container disposed successfully");
                
                _blazorServerContainer = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error stopping container: {ex.Message}");
            }
        }

        if (_blazorServerImage != null)
        {
            try
            {
                await _blazorServerImage.DisposeAsync();
                Console.WriteLine("✓ Image cleaned up successfully");
                
                _blazorServerImage = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠ Error cleaning up image: {ex.Message}");
            }
        }
        
        Console.WriteLine("═══════════════════════════════════════════════════════════");
    }

    private async Task BuildBlazorServerImageAsync()
    {
        Console.WriteLine("Building Docker image from Dockerfile...");
        
        // Get the path to the repository root (from test bin directory up to repo root)
        // Typical path: /path/to/repo/EstateManagementUI.BlazorIntegrationTests/bin/Debug/net10.0
        string currentDirectory = Directory.GetCurrentDirectory();
        string repoRoot = Path.GetFullPath(Path.Combine(currentDirectory, "../../../.."));
        
        Console.WriteLine($"  Repository root: {repoRoot}");
        Console.WriteLine($"  Dockerfile: EstateManagementUI.BlazorServer/Dockerfile");
        
        // Verify the Dockerfile exists
        string dockerfilePath = Path.Combine(repoRoot, "EstateManagementUI.BlazorServer", "Dockerfile");
        if (!File.Exists(dockerfilePath))
        {
            throw new FileNotFoundException($"Dockerfile not found at: {dockerfilePath}");
        }
        
        // Build the image using ImageFromDockerfileBuilder
        var imageBuilder = new ImageFromDockerfileBuilder()
            .WithDockerfileDirectory(repoRoot)
            .WithDockerfile("EstateManagementUI.BlazorServer/Dockerfile")
            .WithName($"estatemanagementuiblazor-test:{Guid.NewGuid():N}")
            .WithCleanUp(true);  // Clean up intermediate images after build

        Console.WriteLine("  Building image (this may take a few minutes on first run)...");
        _blazorServerImage = await imageBuilder.Build();
        
        Console.WriteLine($"✓ Image built successfully: {_blazorServerImage.FullName}");
    }

    private async Task StartBlazorServerContainerAsync()
    {
        if (_blazorServerImage == null)
        {
            throw new InvalidOperationException("Image must be built before starting container");
        }
        
        Console.WriteLine("Starting Blazor Server container...");
        
        // Configure environment variables for Test mode
        var environmentVariables = new Dictionary<string, string>
        {
            // Set to Test environment to use appsettings.Test.json
            ["ASPNETCORE_ENVIRONMENT"] = "Test",
            
            // Enable test mode (uses in-memory test data, no external API calls)
            ["AppSettings:TestMode"] = "true",
            
            // Test user configuration
            ["AppSettings:TestUserRole"] = "Estate",
            
            // HTTPS URL binding
            ["urls"] = "https://*:5004",
            
            // Ignore certificate errors for testing
            ["AppSettings:HttpClientIgnoreCertificateErrors"] = "true"
        };

        // Build the container
        var containerBuilder = new ContainerBuilder()
            .WithImage(_blazorServerImage)
            .WithName($"blazorserver-test-{Guid.NewGuid():N}")
            .WithPortBinding(5004, true)  // Map container port 5004 to random host port
            .WithEnvironment(environmentVariables)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5004));

        _blazorServerContainer = containerBuilder.Build();
        
        Console.WriteLine("  Starting container...");
        await _blazorServerContainer.StartAsync();
        
        // Get the mapped public port
        EstateManagementUiPort = _blazorServerContainer.GetMappedPublicPort(5004);
        
        Console.WriteLine($"✓ Container started successfully");
        Console.WriteLine($"  Container ID: {_blazorServerContainer.Id[..12]}");
        Console.WriteLine($"  Host Port: {EstateManagementUiPort}");
        
        // Give the application a moment to fully start
        Console.WriteLine("  Waiting for application to initialize...");
        await Task.Delay(3000);
        Console.WriteLine("✓ Application ready");
    }

    public async ValueTask DisposeAsync()
    {
        await StopContainerAsync();
    }
}
