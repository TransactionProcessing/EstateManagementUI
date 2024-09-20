using System.Diagnostics;
using System.Text;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Common;
using Ductus.FluentDocker.Executors;
using Ductus.FluentDocker.Extensions;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Ductus.FluentDocker.Services.Extensions;
using EstateManagement.Client;
using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService.Database;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using EstateManagementUI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SecurityService.Client;
using Shared.IntegrationTesting;

namespace EstateManagementUI.IntegrationTests.Common
{
    public class DockerHelper : global::Shared.IntegrationTesting.DockerHelper
    {
        #region Fields

        /// <summary>
        /// The estate client
        /// </summary>
        public IEstateClient EstateClient;

        /// <summary>
        /// The HTTP client
        /// </summary>
        public HttpClient HttpClient;

        /// <summary>
        /// The security service client
        /// </summary>
        public ISecurityServiceClient SecurityServiceClient;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DockerHelper()
        {
            this.TestingContext = new TestingContext();
        }

        #endregion

        #region Methods
        
        public Int32 EstateManagementUiPort;
        
        protected String EstateManagementUiContainerName;

        private readonly TestingContext TestingContext;

        public override void SetupContainerNames() {
            base.SetupContainerNames();
            this.SecurityServiceContainerName = $"identity-server{this.TestId:N}";
            this.EstateManagementUiContainerName = $"estateadministrationui{this.TestId:N}";
        }

        private static void AddEntryToHostsFile(String ipaddress,
                                                String hostname)
        {
            if (FdOs.IsWindows())
            {
                using (StreamWriter w = File.AppendText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts")))
                {
                    w.WriteLine($"{ipaddress} {hostname}");
                }
            }
            else if (FdOs.IsLinux())
            {
                DockerHelper.ExecuteBashCommand($"echo {ipaddress} {hostname} | sudo tee -a /etc/hosts");
            }
        }

        /// <summary>
        /// Executes the bash command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns></returns>
        private static void ExecuteBashCommand(String command)
        {
            // according to: https://stackoverflow.com/a/15262019/637142
            // thans to this we will pass everything as one command
            command = command.Replace("\"", "\"\"");

            var proc = new Process
                       {
                           StartInfo = new ProcessStartInfo
                                       {
                                           FileName = "/bin/bash",
                                           Arguments = "-c \"" + command + "\"",
                                           UseShellExecute = false,
                                           RedirectStandardOutput = true,
                                           CreateNoWindow = true
                                       }
                       };
            Console.WriteLine(proc.StartInfo.Arguments);

            proc.Start();
            proc.WaitForExit();
        }

        public override async Task CreateSubscriptions(){
            List<(String streamName, String groupName, Int32 maxRetries)> subscriptions = new();
            subscriptions.AddRange(MessagingService.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());
            subscriptions.AddRange(EstateManagement.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());
            subscriptions.AddRange(TransactionProcessor.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());

            // TODO: Add File Processor Subscriptions

            foreach ((String streamName, String groupName, Int32 maxRetries) subscription in subscriptions)
            {
                var x = subscription;
                x.maxRetries = 2;
                await this.CreatePersistentSubscription(x);
            }
        }

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName, DockerServices dockerServices)
        {
            await base.StartContainersForScenarioRun(scenarioName, dockerServices);

            await this.StartEstateManagementUiContainer(this.TestNetworks, this.SecurityServicePort, DockerPorts.SecurityServiceDockerPort);
            
            // Setup the base address resolvers
            String EstateManagementBaseAddressResolver(String api) => $"http://127.0.0.1:{this.EstateManagementPort}";

            HttpClientHandler clientHandler = new HttpClientHandler
                                              {
                                                  ServerCertificateCustomValidationCallback = (message,
                                                                                               certificate2,
                                                                                               arg3,
                                                                                               arg4) =>             
                                                  {
                                                    return true;
                                                  }

                                              };
            HttpClient httpClient = new HttpClient(clientHandler);
            this.EstateClient = new EstateClient(EstateManagementBaseAddressResolver, httpClient);
            Func<String, String> securityServiceBaseAddressResolver = api => $"https://127.0.0.1:{this.SecurityServicePort}";
            this.SecurityServiceClient = new SecurityServiceClient(securityServiceBaseAddressResolver, httpClient);
        }
        
        private async Task<IContainerService> StartEstateManagementUiContainer(List<INetworkService> networkServices,
                                                                               Int32 securityServiceContainerPort,
                                                                               Int32 securityServiceLocalPort)
        {
            TraceX("About to Start Estate Management UI Container");

            List<String> environmentVariables = this.GetCommonEnvironmentVariables();
            environmentVariables.Add($"AppSettings:Authority=https://{this.SecurityServiceContainerName}:0");  // The port is set to 0 to stop defaulting to 443
            environmentVariables.Add($"AppSettings:SecurityServiceLocalPort={securityServiceLocalPort}");
            environmentVariables.Add($"AppSettings:SecurityServicePort={securityServiceContainerPort}");
            environmentVariables.Add("AppSettings:HttpClientIgnoreCertificateErrors=true");
            //environmentVariables.Add($"AppSettings:PermissionsBypass=true");
            environmentVariables.Add($"AppSettings:IsIntegrationTest=true");
            environmentVariables.Add($"ASPNETCORE_ENVIRONMENT=Development");
            environmentVariables.Add($"EstateManagementScope=estateManagement");
            environmentVariables.Add("urls=https://*:5004");
            environmentVariables.Add($"AppSettings:ClientId=estateUIClient");
            environmentVariables.Add($"AppSettings:ClientSecret=Secret1");
            environmentVariables.Add($"DataReloadConfig:DefaultInSeconds=1");
            environmentVariables.Add("AppSettings:HttpClientIgnoreCertificateErrors=true");

            TraceX("About to Built Estate Management UI Container");
            ContainerBuilder containerBuilder = new Builder().UseContainer()
                                                             .WithName(this.EstateManagementUiContainerName)
                                                             .UseImageDetails(("estatemanagementui", false))
                                                             .WithEnvironment(environmentVariables.ToArray())
                                                             //.UseNetwork(networkServices.ToArray())
                                                             .ExposePort(5004)
                                                             .MountHostFolder(this.DockerPlatform, this.HostTraceFolder)
                                                             .SetDockerCredentials(this.DockerCredentials);
            TraceX("About to Call .Start()");
            IContainerService builtContainer = containerBuilder.Build();
            
            try{

                builtContainer.Start();
                builtContainer.WaitForPort("5004/tcp", 30000);
                this.EstateManagementUiPort = builtContainer.ToHostExposedEndpoint($"5004/tcp").Port;

                await Task.Delay(5000);

                TraceX("Estate Management UI Started");


                // TODO: Refactor this code once it works...
                using (HttpClient client = new HttpClient()) {
                    HttpRequestMessage createRolesRequest = new (HttpMethod.Post,
                        $"https://localhost:{this.EstateManagementUiPort}/api/Permissions/createRoles");

                    List<String> roles = ["Administrator"];

                    createRolesRequest.Content = new StringContent(JsonConvert.SerializeObject(roles), Encoding.UTF8,
                        "application/json");

                    var response = await client.SendAsync(createRolesRequest, CancellationToken.None);

                    if (response.IsSuccessStatusCode == false) {
                        TraceX($"createRolesRequest failed [{response.StatusCode}]");
                    }
                    HttpRequestMessage addUserToRoleRequest = new(HttpMethod.Post,
                        $"https://localhost:{this.EstateManagementUiPort}/api/Permissions/addUserToRole");
                    List<AddUserToRole> userRolesList = new List<AddUserToRole> {
                        new AddUserToRole { UserName = "estateuser@testestate1.co.uk", RoleName = "Administrator" }
                    };

                    addUserToRoleRequest.Content = new StringContent(JsonConvert.SerializeObject(userRolesList), Encoding.UTF8,
                        "application/json");

                    response = await client.SendAsync(addUserToRoleRequest, CancellationToken.None);
                    if (response.IsSuccessStatusCode == false)
                    {
                        TraceX($"addUserToRoleRequest failed [{response.StatusCode}]");
                    }


                    HttpRequestMessage getRolePermissionsRequest = new(HttpMethod.Get,
                        $"https://localhost:{this.EstateManagementUiPort}/api/Permissions/getRolePermissions?roleName=Administrator");
                    
                    response = await client.SendAsync(getRolePermissionsRequest, CancellationToken.None);

                    if (response.IsSuccessStatusCode == false)
                    {
                        TraceX($"getRolePermissionsRequest failed [{response.StatusCode}]");
                    }

                    var x = await response.Content.ReadAsStringAsync(CancellationToken.None);

                    RolePermissionsObject rolePermissionsObject = JsonConvert.DeserializeObject<RolePermissionsObject>(x);

                    List<(int, string, int, string, bool)> Permissions = new();
                    foreach (ApplicationSection applicationSection in rolePermissionsObject.ApplicationSections)
                    {
                        List<(Function, bool)> functionAccess = rolePermissionsObject.PermissionsList.Where(p =>
                            p.ApplicationSection.ApplicationSectionId == applicationSection.ApplicationSectionId).Select(x => (x.Function, x.HasAccess)).ToList();

                        foreach ((Function, bool) function in functionAccess)
                        {
                            Permissions.Add((applicationSection.ApplicationSectionId, applicationSection.Name, function.Item1.FunctionId, function.Item1.Name, function.Item2));
                        }
                    }

                    List<(int, int, bool)> newPermissions = Permissions.Select(p => (p.Item1, p.Item3, true)).ToList();

                    HttpRequestMessage addRolePermissionsRequest = new(HttpMethod.Post,
                        $"https://localhost:{this.EstateManagementUiPort}/api/Permissions/addRolePermissions");

                    List<RolePermissions> rolePermissions = new() {
                        new RolePermissions { NewPermissions = newPermissions, RoleName = "Administrator" }
                    };

                    addRolePermissionsRequest.Content = new StringContent(JsonConvert.SerializeObject(rolePermissions), Encoding.UTF8,
                        "application/json");

                    response = await client.SendAsync(addRolePermissionsRequest, CancellationToken.None);
                    if (response.IsSuccessStatusCode == false)
                    {
                        TraceX($"addRolePermissionsRequest failed [{response.StatusCode}]");
                    }
                }
            }
            catch(Exception ex){
                TraceX(ex.GetCombinedExceptionMessages());
                ConsoleStream<String> logs = builtContainer.Logs(true, CancellationToken.None);
                IList<String> xx = logs.ReadToEnd();
                while (xx.Any())
                {
                    foreach (String s in xx)
                    {
                        TraceX($"Logs|{s}");
                    }
                    xx = logs.ReadToEnd();
                }
            }

            TraceX("About to attach networkServices");
            foreach (INetworkService networkService in networkServices)
            {
                networkService.Attach(builtContainer, false);
            }

            //Trace("About to get port");
            ////  Do a health check here
            //var x = builtContainer.ToHostExposedEndpoint($"5004/tcp");
            //if (x == null){
            //    Trace("x is null");
            //}

            

            
            this.Containers.Add(((DockerServices)1024, builtContainer));
            //await Retry.For(async () =>
            //{
            //    String healthCheck =
            //    await this.HealthCheckClient.PerformHealthCheck("http", "127.0.0.1", this.EstateManagementUiPort, CancellationToken.None);

            //    var result = JsonConvert.DeserializeObject<HealthCheckResult>(healthCheck);
            //    result.Status.ShouldBe(HealthCheckStatus.Healthy.ToString(), $"Details {healthCheck}");
            //});

            return builtContainer;
        }
        
        public override ContainerBuilder SetupSecurityServiceContainer()
        {
            this.TraceX("About to Start Security Container");

            Retry.For(() => {
                          DockerHelper.AddEntryToHostsFile("127.0.0.1", SecurityServiceContainerName);
                          return Task.CompletedTask;
                      });

            Retry.For(() => {
                DockerHelper.AddEntryToHostsFile("localhost", SecurityServiceContainerName);
                return Task.CompletedTask;
            });

            

            List<String> environmentVariables = this.GetCommonEnvironmentVariables();
            environmentVariables.Add($"ServiceOptions:PublicOrigin=https://{this.SecurityServiceContainerName}:{DockerPorts.SecurityServiceDockerPort}");
            environmentVariables.Add($"ServiceOptions:IssuerUrl=https://{this.SecurityServiceContainerName}:{DockerPorts.SecurityServiceDockerPort}");
            environmentVariables.Add("ASPNETCORE_ENVIRONMENT=IntegrationTest");
            environmentVariables.Add($"urls=https://*:{DockerPorts.SecurityServiceDockerPort}");

            environmentVariables.Add($"ServiceOptions:PasswordOptions:RequiredLength=6");
            environmentVariables.Add($"ServiceOptions:PasswordOptions:RequireDigit=false");
            environmentVariables.Add($"ServiceOptions:PasswordOptions:RequireUpperCase=false");
            environmentVariables.Add($"ServiceOptions:UserOptions:RequireUniqueEmail=false");
            environmentVariables.Add($"ServiceOptions:SignInOptions:RequireConfirmedEmail=false");
            
            environmentVariables.Add("Logging:LogLevel:Microsoft=Information");
            environmentVariables.Add("Logging:LogLevel:Default=Information");
            environmentVariables.Add("Logging:EventLog:LogLevel:Default=None");
            
            ContainerBuilder securityServiceContainer = new Builder().UseContainer().WithName(this.SecurityServiceContainerName)
                                                                     .WithEnvironment(environmentVariables.ToArray())
                                                                     .UseImageDetails(this.GetImageDetails(ContainerType.SecurityService))
                                                                     .ExposePort(DockerPorts.SecurityServiceDockerPort)
                                                                     .MountHostFolder(this.DockerPlatform, this.HostTraceFolder)
                                                                     .SetDockerCredentials(this.DockerCredentials);
            
            // Now build and return the container                
            return securityServiceContainer;
        }

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun(DockerServices sharedDockerServices)
        {
            await this.RemoveEstateReadModel().ConfigureAwait(false);

            await base.StopContainersForScenarioRun(sharedDockerServices);
        }

        private async Task RemoveEstateReadModel()
        {
            //List<Guid> estateIdList = this.TestingContext.GetAllEstateIds();

            //foreach (Guid estateId in estateIdList)
            //{
            //    String databaseName = $"EstateReportingReadModel{estateId}";

            //    // Build the connection string (to master)
            //    String connectionString = Setup.GetLocalConnectionString(databaseName);
            //    await Retry.For(async () =>
            //                    {
            //                        EstateReportingSqlServerContext context = new EstateReportingSqlServerContext(connectionString);
            //                        await context.Database.EnsureDeletedAsync(CancellationToken.None);
            //                    },
            //                    retryFor: TimeSpan.FromMinutes(2),
            //                    retryInterval: TimeSpan.FromSeconds(30));
            //}
        }

        private async Task<IPermissionsRepository> CreatePermissionsRepository(String dbConnString, CancellationToken cancellationToken) {
            var optionsBuilder = new DbContextOptionsBuilder<PermissionsContext>();
            optionsBuilder.UseSqlite(dbConnString); // Configure for your database provider

            var serviceProvider = new ServiceCollection()
                .AddLogging(config => config.AddConsole())  // Add logging if needed
                .BuildServiceProvider();

            // Create the DbContextFactory instance
            var contextFactory = new DbContextFactory<PermissionsContext>(serviceProvider, optionsBuilder.Options, new DbContextFactorySource<PermissionsContext>());

            //var ctx = await contextFactory.CreateDbContextAsync(cancellationToken);

            return new PermissionsRepository(contextFactory);
        }

        public void TraceX(String msg) {
            Trace(msg);
            Console.WriteLine(msg);
        }

        #endregion
    }
}
