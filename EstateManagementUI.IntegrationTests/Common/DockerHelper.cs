using Ductus.FluentDocker.Common;
using Ductus.FluentDocker.Services;
using EventStore.Client;
using NLog;
using Reqnroll;
using SecurityService.Client;
using SecurityService.DataTransferObjects;
using Shared.IntegrationTesting;
using Shared.IntegrationTesting.TestContainers;
using Shared.Logger;
using Shared.Serialisation;
using Shouldly;
using System.Diagnostics;
using TransactionProcessor.Client;
using TransactionProcessor.IntegrationTesting.Helpers;
using ContainerBuilder = DotNet.Testcontainers.Builders.ContainerBuilder;
using ReqnrollTableHelper = Shared.IntegrationTesting.ReqnrollTableHelper;
using String = System.String;

namespace EstateManagementUI.IntegrationTests.Common
{
    public class DockerHelper : global::Shared.IntegrationTesting.TestContainers.DockerHelper
    {
        #region Fields

        public ITransactionProcessorClient TransactionProcessorClient;

        public HttpClient HttpClient;

        public ISecurityServiceClient SecurityServiceClient;

        public EventStoreProjectionManagementClient ProjectionManagementClient;

        public HttpClient TestHostHttpClient;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DockerHelper"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public DockerHelper()
        {
            this.TestingContext = new TestingContext();
            StringSerialiser.Initialise((IStringSerialiser)new SystemTextJsonSerializer(SystemTextJsonSerializer.GetDefaultJsonSerializerOptions()));
        }

        String Serialise(Object arg)
        {
            return StringSerialiser.Serialise<Object>(arg, new SerialiserOptions(SerialiserPropertyFormat.SnakeCase));
        }

        Object Deserialise(String arg, Type type)
        {
            return StringSerialiser.DeserializeObject<Object>(arg, type, new SerialiserOptions(SerialiserPropertyFormat.SnakeCase));
        }

        #endregion

        #region Methods
        
        private readonly TestingContext TestingContext;

        public override void SetupContainerNames()
        {
            base.SetupContainerNames();
            this.SecurityServiceContainerName = $"identity-server{this.TestId:N}";
            this.EstateManagementUiContainerName = $"estateadministrationui{this.TestId:N}";
            Environment.SetEnvironmentVariable("SecurityServiceContainerName", this.SecurityServiceContainerName);
        }

        private static void AddEntryToHostsFile(String ipaddress,
                                                String hostname)
        {
            if (FdOs.IsWindows())
            {
                var hostsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
                using (StreamWriter w = File.AppendText(hostsPath))
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

        public override async Task CreateSubscriptions()
        {
            List<(String streamName, String groupName, Int32 maxRetries)> subscriptions = new();
            subscriptions.AddRange(MessagingService.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());
            subscriptions.AddRange(TransactionProcessor.IntegrationTesting.Helpers.SubscriptionsHelper.GetSubscriptions());

            // TODO: Add File Processor Subscriptions

            foreach ((String streamName, String groupName, Int32 maxRetries) subscription in subscriptions)
            {
                var x = subscription;
                x.maxRetries = 2;
                await this.CreatePersistentSubscription(x);
            }
        }

        protected override List<String> GetRequiredProjections()
        {
            List<String> requiredProjections = new List<String>();

            requiredProjections.Add("EstateAggregator.js");
            requiredProjections.Add("MerchantAggregator.js");
            requiredProjections.Add("MerchantBalanceCalculator.js");
            requiredProjections.Add("MerchantBalanceProjection.js");

            return requiredProjections;
        }

        //public override DotNet.Testcontainers.Builders.ContainerBuilder SetupTransactionProcessorContainer()
        //{

        //    Dictionary<String, String> variables = new ();
        //    variables.Add($"OperatorConfiguration:PataPawaPrePay:Url","http://{this.TestHostContainerName}:{DockerPorts.TestHostPort}/api/patapawaprepay");

        //    this.AdditionalVariables.Add(ContainerType.TransactionProcessor, variables);

        //    return base.SetupTransactionProcessorContainer();
        //}

        /// <summary>
        /// Starts the containers for scenario run.
        /// </summary>
        /// <param name="scenarioName">Name of the scenario.</param>
        public override async Task StartContainersForScenarioRun(String scenarioName, DockerServices dockerServices)
        {
            await base.StartContainersForScenarioRun(scenarioName, dockerServices);
            Environment.SetEnvironmentVariable("SecurityServiceLocalPort", DockerPorts.SecurityServiceDockerPort.ToString());
            Environment.SetEnvironmentVariable("SecurityServicePort", this.SecurityServicePort.ToString());

            //this.SetupEstateManagementUiContainer();

            // Setup the base address resolvers
            String TransactionProcessorBaseAddressResolver(String api) => $"http://127.0.0.1:{this.TransactionProcessorPort}";

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
            this.TransactionProcessorClient = new TransactionProcessorClient(TransactionProcessorBaseAddressResolver, httpClient, this.Serialise, this.Deserialise);
            Func<String, String> securityServiceBaseAddressResolver = api => $"https://127.0.0.1:{this.SecurityServicePort}";
            this.SecurityServiceClient = new SecurityServiceClient(securityServiceBaseAddressResolver, httpClient, this.Serialise, this.Deserialise);
            this.TestHostHttpClient = new HttpClient(clientHandler);
            this.TestHostHttpClient.BaseAddress = new Uri($"http://127.0.0.1:{this.TestHostServicePort}");
            this.ProjectionManagementClient = new EventStoreProjectionManagementClient(ConfigureEventStoreSettings());
        }

        protected override ContainerBuilder SetupEstateManagementUiContainer() {
            Trace("About to Start Estate Management UI Container");

            Dictionary<String, String> environmentVariables = this.GetCommonEnvironmentVariables();

            environmentVariables.Remove("AppSettings:ClientId");
            environmentVariables.Remove("AppSettings:ClientSecret");

            environmentVariables.Add("Authentication:Authority", $"https://{this.SecurityServiceContainerName}:0");  // The port is set to 0 to stop defaulting to 443
            //environmentVariables.Add("AppSettings:SecurityService", $"https://{this.SecurityServiceContainerName}:0");  // The port is set to 0 to stop defaulting to 443
            environmentVariables.Add("AppSettings:SecurityServiceLocalPort", $"{DockerPorts.SecurityServiceDockerPort}");
            environmentVariables.Add("AppSettings:SecurityServicePort", $"{this.SecurityServicePort}");
            environmentVariables.Add("AppSettings:HttpClientIgnoreCertificateErrors", $"true");
            //environmentVariables.Add($"AppSettings:PermissionsBypass=true");
            environmentVariables.Add("AppSettings:IsIntegrationTest", "true");
            environmentVariables.Add("ASPNETCORE_ENVIRONMENT", "Development");
            environmentVariables.Add("EstateManagementScope", "estateManagement");
            environmentVariables.Add("urls", "https://*:5004");
            environmentVariables.Add($"AppSettings:ClientId", "estateUIClient");
            environmentVariables.Add($"AppSettings:ClientSecret", "Secret1");
            environmentVariables.Add($"Authentication:ClientId", "estateUIClient");
            environmentVariables.Add($"Authentication:ClientSecret", "Secret1");
            environmentVariables.Add($"AppSettings:BackEndClientId", "serviceClient");
            environmentVariables.Add($"AppSettings:BackEndClientSecret", "Secret1");
            environmentVariables.Add($"DataReloadConfig:DefaultInSeconds", "1");
            environmentVariables.Add("ConnectionStrings:TransactionProcessorReadModel", this.SetConnectionString("TransactionProcessorReadModel", this.UseSecureSqlServerDatabase));
            environmentVariables.Add("AppSettings:EstateReportingApi", $"http://{this.EstateReportingContainerName}:{DockerPorts.EstateReportingDockerPort}");

            (String imageName, Boolean useLatest) imageDetails = this.GetImageDetails(ContainerType.EstateManagementUI).Data;

            ContainerBuilder containerBuilder = new ContainerBuilder()
                .WithName(this.EstateManagementUiContainerName)
                .WithImage(imageDetails.imageName)
                .WithEnvironment(environmentVariables)
                .MountHostFolder(this.DockerPlatform, this.HostTraceFolder)
                .WithPortBinding(DockerPorts.EstateManagementUIDockerPort, true);

            return containerBuilder;
        }

        public override ContainerBuilder SetupEstateReportingContainer() {
            Dictionary<String, String> environmentVariables = new();
            environmentVariables.Add("ConnectionStrings:TransactionProcessorReadModel", this.SetConnectionString("TransactionProcessorReadModel", this.UseSecureSqlServerDatabase));
            this.AdditionalVariables.Add(ContainerType.EstateReporting, environmentVariables);

            return base.SetupEstateReportingContainer();
        }

        /*private async Task<IContainerService> StartEstateManagementUiContainer(List<INetworkService> networkServices,
                                                                               Int32 securityServiceContainerPort,
                                                                               Int32 securityServiceLocalPort)
        {
            TraceX("About to Start Estate Management UI Container");

            Dictionary<String, String>? environmentVariables = this.GetCommonEnvironmentVariables();
            environmentVariables.Add($"AppSettings:Authority","https://{this.SecurityServiceContainerName}:0");  // The port is set to 0 to stop defaulting to 443
            environmentVariables.Add($"AppSettings:SecurityServiceLocalPort", $"{securityServiceLocalPort}");
            environmentVariables.Add($"AppSettings:SecurityServicePort", $"{securityServiceContainerPort}");
            environmentVariables.Add("AppSettings:HttpClientIgnoreCertificateErrors","true");
            //environmentVariables.Add($"AppSettings:PermissionsBypass=true");
            environmentVariables.Add($"AppSettings:IsIntegrationTest","true");
            environmentVariables.Add($"ASPNETCORE_ENVIRONMENT","Development");
            environmentVariables.Add($"EstateManagementScope","estateManagement");
            environmentVariables.Add("urls","https://*:5004");
            environmentVariables.Add($"AppSettings:ClientId","estateUIClient");
            environmentVariables.Add($"AppSettings:ClientSecret","Secret1");
            environmentVariables.Add($"DataReloadConfig:DefaultInSeconds","1");
            environmentVariables.Add("AppSettings:HttpClientIgnoreCertificateErrors","true");
            environmentVariables.Add("ConnectionStrings:TransactionProcessorReadModel", this.SetConnectionString("TransactionProcessorReadModel", this.UseSecureSqlServerDatabase));

            (String imageName, Boolean useLatest) imageDetails = this.GetImageDetails(ContainerType.EstateManangementUI).Data;

            TraceX("About to Built Estate Management UI Container");
            DotNet.Testcontainers.Builders.ContainerBuilder containerBuilder = new DotNet.Testcontainers.Builders.ContainerBuilder()
                                                             .WithName(this.EstateManagementUiContainerName)
                                                             .WithImage(imageDetails.imageName)
                                                             .WithEnvironment(environmentVariables)
                                                             //.UseNetwork(networkServices.ToArray())
                                                             .MountHostFolder(this.DockerPlatform, this.HostTraceFolder);

            // Mount the folder to upload files
            String uploadFolder = (this.DockerPlatform, isCi) switch
            {
                (DockerEnginePlatform.Windows, false) => "C:\\home\\txnproc\\reqnroll",
                (DockerEnginePlatform.Windows, true) => "C:\\Users\\runneradmin\\txnproc\\reqnroll",
                _ => "/home/txnproc/reqnroll"
            };

            if (this.DockerPlatform == DockerEnginePlatform.Windows && isCi)
            {
                Directory.CreateDirectory(uploadFolder);
            }

            //String containerFolder = this.DockerPlatform == DockerEnginePlatform.Windows ? "C:\\home\\txnproc\\bulkfiles" : "/home/txnproc/bulkfiles";
            //fileProcessorContainer.Mount(uploadFolder, containerFolder, MountType.ReadWrite);
            return containerBuilder;

            
            IContainerService builtContainer = containerBuilder.Build();

            try
            {

                builtContainer.Start();
                builtContainer.WaitForPort("5004/tcp", 30000);
                this.EstateManagementUiPort = builtContainer.ToHostExposedEndpoint($"5004/tcp").Port;

                await Task.Delay(5000);

                TraceX("Estate Management UI Started");

                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                // TODO: Refactor this code once it works...
                using (HttpClient client = new HttpClient(handler))
                {
                    HttpRequestMessage createRolesRequest = new(HttpMethod.Post,
                        $"https://localhost:{this.EstateManagementUiPort}/api/Permissions/createRoles");

                    List<String> roles = ["Administrator"];

                    createRolesRequest.Content = new StringContent(JsonConvert.SerializeObject(roles), Encoding.UTF8,
                        "application/json");

                    var response = await client.SendAsync(createRolesRequest, CancellationToken.None);

                    if (response.IsSuccessStatusCode == false)
                    {
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
            catch (Exception ex)
            {
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
        }*/
        
        public override DotNet.Testcontainers.Builders.ContainerBuilder SetupSecurityServiceContainer()
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
            

            Dictionary<String, String>? environmentVariables = this.GetCommonEnvironmentVariables();
            environmentVariables.Add($"ServiceOptions:PublicOrigin",$"https://{this.SecurityServiceContainerName}:{DockerPorts.SecurityServiceDockerPort}");
            environmentVariables.Add($"ServiceOptions:IssuerUrl",$"https://{this.SecurityServiceContainerName}:{DockerPorts.SecurityServiceDockerPort}");
            environmentVariables.Add("ASPNETCORE_ENVIRONMENT","IntegrationTest");
            environmentVariables.Add($"urls",$"https://*:{DockerPorts.SecurityServiceDockerPort}");

            environmentVariables.Add($"ServiceOptions:PasswordOptions:RequiredLength","6");
            environmentVariables.Add($"ServiceOptions:PasswordOptions:RequireDigit","false");
            environmentVariables.Add($"ServiceOptions:PasswordOptions:RequireUpperCase","false");
            environmentVariables.Add($"ServiceOptions:UserOptions:RequireUniqueEmail","false");
            environmentVariables.Add($"ServiceOptions:SignInOptions:RequireConfirmedEmail","false");

            environmentVariables.Add("ConnectionStrings:PersistedGrantDbContext", this.SetConnectionString( $"PersistedGrantStore-{this.TestId}", this.UseSecureSqlServerDatabase));
            environmentVariables.Add("ConnectionStrings:ConfigurationDbContext", this.SetConnectionString( $"Configuration-{this.TestId}", this.UseSecureSqlServerDatabase));
            environmentVariables.Add("ConnectionStrings:AuthenticationDbContext", this.SetConnectionString( $"Authentication-{this.TestId}", this.UseSecureSqlServerDatabase));

            environmentVariables.Add("Logging:LogLevel:Microsoft","Information");
            environmentVariables.Add("Logging:LogLevel:Default","Information");
            environmentVariables.Add("Logging:EventLog:LogLevel:Default","None");

            (String imageName, Boolean useLatest) imageDetails = this.GetImageDetails(ContainerType.SecurityService).Data;

            ContainerBuilder? securityServiceContainer = new DotNet.Testcontainers.Builders.ContainerBuilder().WithName(this.SecurityServiceContainerName)
                                                                     .WithEnvironment(environmentVariables)
                                                                     .WithImage(imageDetails.imageName)
                                                                     .WithPortBinding(DockerPorts.SecurityServiceDockerPort, true)
                                                                     .MountHostFolder(this.DockerPlatform, this.HostTraceFolder);

            // Now build and return the container                
            return securityServiceContainer;
        }

        /// <summary>
        /// Stops the containers for scenario run.
        /// </summary>
        public override async Task StopContainersForScenarioRun(DockerServices sharedDockerServices)
        {
            await base.StopContainersForScenarioRun(sharedDockerServices);
        }
        
        //private async Task<IPermissionsRepository> CreatePermissionsRepository(String dbConnString, CancellationToken cancellationToken)
        //{
        //    var optionsBuilder = new DbContextOptionsBuilder<PermissionsContext>();
        //    optionsBuilder.UseSqlite(dbConnString); // Configure for your database provider

        //    var serviceProvider = new ServiceCollection()
        //        .AddLogging(config => config.AddConsole())  // Add logging if needed
        //        .BuildServiceProvider();

        //    // Create the DbContextFactory instance
        //    var contextFactory = new DbContextFactory<PermissionsContext>(serviceProvider, optionsBuilder.Options, new DbContextFactorySource<PermissionsContext>());

        //    //var ctx = await contextFactory.CreateDbContextAsync(cancellationToken);

        //    return new PermissionsRepository(contextFactory);
        //}

        public void TraceX(String msg)
        {
            Trace(msg);
            Console.WriteLine(msg);
        }

        #endregion
    }

    public class TestingContext
    {
        public EstateDetails GetEstateDetails(DataTableRow tableRow, Guid? testId = null)
        {
            String estateName = ReqnrollTableHelper.GetStringRowValue(tableRow, "EstateName").Replace("[id]", testId.Value.ToString("N"));

            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateName == estateName);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        public List<Guid> GetAllEstateIds()
        {
            return this.Estates.Select(e => e.EstateId).ToList();
        }

        public EstateDetails GetEstateDetails(String estateName)
        {
            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateName == estateName);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        public EstateDetails GetEstateDetails(Guid estateId)
        {
            EstateDetails estateDetails = this.Estates.SingleOrDefault(e => e.EstateId == estateId);

            estateDetails.ShouldNotBeNull();

            return estateDetails;
        }

        public List<EstateDetails> Estates;
        public void AddEstateDetails(Guid estateId, String estateName, String estateReference)
        {
            this.Estates.Add(EstateDetails.Create(estateId, estateName, estateReference));
        }

        public String AccessToken { get; set; }

        public DockerHelper DockerHelper { get; set; }

        public NlogLogger Logger { get; set; }

        public Dictionary<String, String> Users;
        public Dictionary<String, String> Roles;

        public List<ClientDetails> Clients;

        public List<String> ApiResources;
        public List<String> IdentityResources;

        public TokenResponse TokenResponse;

        public TestingContext()
        {
            this.Users = new Dictionary<String, String>();
            this.Roles = new Dictionary<String, String>();
            this.Clients = new List<ClientDetails>();
            this.ApiResources = new List<String>();
            this.Estates = new List<EstateDetails>();
            this.IdentityResources = new List<String>();
        }

        public ClientDetails GetClientDetails(String clientId)
        {
            ClientDetails clientDetails = this.Clients.SingleOrDefault(c => c.ClientId == clientId);

            clientDetails.ShouldNotBeNull();

            return clientDetails;
        }

        public void AddClientDetails(String clientId,
                                     String clientSecret,
                                     List<String> grantTypes)
        {
            this.Clients.Add(ClientDetails.Create(clientId, clientSecret, grantTypes));
        }
    }

    public class ClientDetails
    {
        public String ClientId { get; private set; }
        public String ClientSecret { get; private set; }
        public List<String> GrantTypes { get; private set; }

        private ClientDetails(String clientId,
                              String clientSecret,
                              List<String> grantTypes)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.GrantTypes = grantTypes;
        }

        public static ClientDetails Create(String clientId,
                                           String clientSecret,
                                           List<String> grantTypes)
        {
            return new ClientDetails(clientId, clientSecret, grantTypes);
        }
    }

    [Binding]
    public class Setup
    {
        public static (String usename, String password) SqlCredentials = ("sa", "thisisalongpassword123!");
        public static (String url, String username, String password) DockerCredentials = ("https://www.docker.com", "stuartferguson", "Sc0tland");

        public static async Task GlobalSetup(DockerHelper dockerHelper)
        {
            ShouldlyConfiguration.DefaultTaskTimeout = TimeSpan.FromMinutes(5);
        }
    }

    [Binding]
    [Scope(Tag = "base")]
    public class GenericSteps
    {
        private readonly ScenarioContext ScenarioContext;

        private readonly TestingContext TestingContext;

        public GenericSteps(ScenarioContext scenarioContext,
                            TestingContext testingContext)
        {
            this.ScenarioContext = scenarioContext;
            this.TestingContext = testingContext;
        }

        [BeforeScenario(Order = 0)]
        public async Task StartSystem()
        {
            // Initialise a logger
            String scenarioName = this.ScenarioContext.ScenarioInfo.Title.Replace(" ", "");
            NlogLogger logger = new NlogLogger();
            logger.Initialise(LogManager.GetLogger(scenarioName), scenarioName);
            LogManager.AddHiddenAssembly(typeof(NlogLogger).Assembly);

            DockerServices dockerServices = DockerServices.SqlServer | DockerServices.CallbackHandler | DockerServices.EventStore |
                                            DockerServices.FileProcessor | DockerServices.MessagingService | DockerServices.SecurityService |
                                            DockerServices.SqlServer | DockerServices.TransactionProcessor |
                                            DockerServices.TransactionProcessorAcl | DockerServices.EstateManagementUI |
                                            DockerServices.EstateReporting;

            this.TestingContext.DockerHelper = new DockerHelper();
            this.TestingContext.DockerHelper.Logger = logger;
            this.TestingContext.Logger = logger;
            this.TestingContext.DockerHelper.RequiredDockerServices = dockerServices;
            this.TestingContext.Logger.LogInformation("About to Start Global Setup");

            await Setup.GlobalSetup(this.TestingContext.DockerHelper);

            this.TestingContext.DockerHelper.DockerCredentials = Setup.DockerCredentials;
            this.TestingContext.DockerHelper.SqlCredentials = Setup.SqlCredentials;
            this.TestingContext.DockerHelper.SqlServerContainerName = "sharedsqlserver";

            this.TestingContext.DockerHelper.SetImageDetails(ContainerType.EstateManagementUI, ("estatemanagementuiblazorserver", false));

            this.TestingContext.Logger = logger;
            this.TestingContext.Logger.LogInformation("About to Start Containers for Scenario Run");
            await this.TestingContext.DockerHelper.StartContainersForScenarioRun(scenarioName, dockerServices).ConfigureAwait(false);
            this.TestingContext.Logger.LogInformation("Containers for Scenario Run Started");
        }

        [AfterScenario(Order = 1)]
        public async Task StopSystem()
        {

            DockerServices sharedDockerServices = DockerServices.None;

            this.TestingContext.Logger.LogInformation("About to Stop Containers for Scenario Run");
            await this.TestingContext.DockerHelper.StopContainersForScenarioRun(sharedDockerServices).ConfigureAwait(false);
            this.TestingContext.Logger.LogInformation("Containers for Scenario Run Stopped");
        }
    }
}
