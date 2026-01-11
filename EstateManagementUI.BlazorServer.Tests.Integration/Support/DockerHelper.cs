using Testcontainers.Builders;
using Testcontainers.Containers;

namespace EstateManagementUI.BlazorServer.Tests.Integration.Support;

/// <summary>
/// Clean, minimal Docker helper for Testcontainers-based integration testing.
/// Manages the BlazorServer container lifecycle using a pre-built Docker image.
/// </summary>
public class DockerHelper : IAsyncDisposable
{
    private string? _blazorServerImageName;
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
    /// Starts the container using a pre-built Docker image.
    /// </summary>
    public async Task StartContainerAsync()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        Console.WriteLine("  Starting Testcontainers-based Integration Test Setup");
        Console.WriteLine("═══════════════════════════════════════════════════════════");
        
        // Use pre-built image
        _blazorServerImageName = "estatemanagementui-blazor";
        Console.WriteLine($"  Using pre-built image: {_blazorServerImageName}");
        
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

        // Image cleanup is handled by pre-built image - no cleanup needed
        if (_blazorServerImageName != null)
        {
            Console.WriteLine($"✓ Using pre-built image: {_blazorServerImageName}");
            _blazorServerImageName = null;
        }
        
        Console.WriteLine("═══════════════════════════════════════════════════════════");
    }

    private async Task StartBlazorServerContainerAsync()
    {
        if (_blazorServerImageName == null)
        {
            throw new InvalidOperationException("Image name must be set before starting container");
        }
        
        Console.WriteLine($"Starting Blazor Server container from image: {_blazorServerImageName}...");
        
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
            .WithImage(_blazorServerImageName)
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
