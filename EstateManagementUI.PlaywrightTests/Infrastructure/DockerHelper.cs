using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using System.Runtime.InteropServices;

namespace EstateManagementUI.PlaywrightTests.Infrastructure;

public class DockerHelper
{
    private INetwork? _network;
    private IContainer? _securityServiceContainer;
    private IContainer? _estateManagementUiContainer;
    
    public int SecurityServicePort { get; private set; }
    public int EstateManagementUiPort { get; private set; }
    
    private readonly Guid _testId;
    private readonly string _securityServiceContainerName;
    private readonly string _estateManagementUiContainerName;

    public DockerHelper()
    {
        _testId = Guid.NewGuid();
        _securityServiceContainerName = $"identity-server{_testId:N}";
        _estateManagementUiContainerName = $"estateadministrationui{_testId:N}";
    }

    public async Task StartContainers()
    {
        // Create network
        _network = new NetworkBuilder()
            .WithName($"testnetwork{_testId:N}")
            .Build();
        
        await _network.CreateAsync();

        // Start Security Service (Authorization Server)
        await StartSecurityServiceContainer();
        
        // Start Estate Management UI
        await StartEstateManagementUiContainer();
        
        // Give containers time to fully start
        await Task.Delay(10000);
    }

    private async Task StartSecurityServiceContainer()
    {
        AddEntryToHostsFile("127.0.0.1", _securityServiceContainerName);
        AddEntryToHostsFile("localhost", _securityServiceContainerName);

        var environmentVariables = new Dictionary<string, string>
        {
            { "ServiceOptions:PublicOrigin", $"https://{_securityServiceContainerName}:5001" },
            { "ServiceOptions:IssuerUrl", $"https://{_securityServiceContainerName}:5001" },
            { "ASPNETCORE_ENVIRONMENT", "IntegrationTest" },
            { "urls", "https://*:5001" },
            { "ServiceOptions:PasswordOptions:RequiredLength", "6" },
            { "ServiceOptions:PasswordOptions:RequireDigit", "false" },
            { "ServiceOptions:PasswordOptions:RequireUpperCase", "false" },
            { "ServiceOptions:UserOptions:RequireUniqueEmail", "false" },
            { "ServiceOptions:SignInOptions:RequireConfirmedEmail", "false" },
            { "ConnectionStrings:PersistedGrantDbContext", GetConnectionString("PersistedGrantStore") },
            { "ConnectionStrings:ConfigurationDbContext", GetConnectionString("Configuration") },
            { "ConnectionStrings:AuthenticationDbContext", GetConnectionString("Authentication") },
            { "Logging:LogLevel:Microsoft", "Information" },
            { "Logging:LogLevel:Default", "Information" },
            { "Logging:EventLog:LogLevel:Default", "None" }
        };

        _securityServiceContainer = new ContainerBuilder()
            .WithName(_securityServiceContainerName)
            .WithImage("securityservice:latest")
            .WithEnvironment(environmentVariables)
            .WithNetwork(_network)
            .WithPortBinding(5001, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5001))
            .Build();

        await _securityServiceContainer.StartAsync();
        SecurityServicePort = _securityServiceContainer.GetMappedPublicPort(5001);
    }

    private async Task StartEstateManagementUiContainer()
    {
        var environmentVariables = new Dictionary<string, string>
        {
            { "AppSettings:Authority", $"https://{_securityServiceContainerName}:0" },
            { "AppSettings:SecurityServiceLocalPort", "5001" },
            { "AppSettings:SecurityServicePort", "5001" },
            { "AppSettings:HttpClientIgnoreCertificateErrors", "true" },
            { "AppSettings:IsIntegrationTest", "true" },
            { "ASPNETCORE_ENVIRONMENT", "Development" },
            { "EstateManagementScope", "estateManagement" },
            { "urls", "https://*:5004" },
            { "AppSettings:ClientId", "estateUIClient" },
            { "AppSettings:ClientSecret", "Secret1" },
            { "AppSettings:BackEndClientId", "serviceClient" },
            { "AppSettings:BackEndClientSecret", "Secret1" },
            { "DataReloadConfig:DefaultInSeconds", "1" }
        };

        _estateManagementUiContainer = new ContainerBuilder()
            .WithName(_estateManagementUiContainerName)
            .WithImage("estatemanagementui:latest")
            .WithEnvironment(environmentVariables)
            .WithNetwork(_network)
            .WithPortBinding(5004, true)
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5004))
            .Build();

        await _estateManagementUiContainer.StartAsync();
        EstateManagementUiPort = _estateManagementUiContainer.GetMappedPublicPort(5004);
    }

    public async Task StopContainers()
    {
        if (_estateManagementUiContainer != null)
        {
            await _estateManagementUiContainer.StopAsync();
            await _estateManagementUiContainer.DisposeAsync();
        }

        if (_securityServiceContainer != null)
        {
            await _securityServiceContainer.StopAsync();
            await _securityServiceContainer.DisposeAsync();
        }

        if (_network != null)
        {
            await _network.DeleteAsync();
            await _network.DisposeAsync();
        }
    }

    private string GetConnectionString(string databaseName)
    {
        // Using pure in-memory SQLite for tests
        return "Data Source=:memory:;Cache=Shared";
    }

    private static void AddEntryToHostsFile(string ipaddress, string hostname)
    {
        try
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                ExecuteBashCommand($"echo {ipaddress} {hostname} | sudo tee -a /etc/hosts");
            }
            // Windows support could be added here if needed
        }
        catch
        {
            // Silently ignore if we can't modify hosts file
            // Tests might still work depending on the environment
        }
    }

    private static void ExecuteBashCommand(string command)
    {
        command = command.Replace("\"", "\"\"");
        var proc = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = "-c \"" + command + "\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            }
        };
        proc.Start();
        proc.WaitForExit();
    }
}
