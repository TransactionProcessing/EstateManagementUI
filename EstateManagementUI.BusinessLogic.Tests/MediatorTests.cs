using EstateManagementUI.Testing;
using MediatR;
using MediatR.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Lamar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;
using Moq;
using Shared.Logger;
using static EstateManagmentUI.BusinessLogic.Requests.Queries;

namespace EstateManagementUI.BusinessLogic.Tests {
    public class MediatorTests {
        private readonly List<IBaseRequest> Requests = new();

        private IMediator mediator;

        public MediatorTests() {
            // Queries
            this.Requests.Add(TestData.GetEstateQuery);
            this.Requests.Add(TestData.GetMerchantsQuery);
            this.Requests.Add(TestData.GetMerchantQuery);
            this.Requests.Add(TestData.GetOperatorsQuery);
            this.Requests.Add(TestData.GetOperatorQuery);
            this.Requests.Add(TestData.GetContractsQuery);
            this.Requests.Add(TestData.GetContractQuery);
            this.Requests.Add(TestData.GetFileImportLogsListQuery);
            this.Requests.Add(TestData.GetFileImportLogQuery);
            this.Requests.Add(TestData.GetFileDetailsQuery);
            this.Requests.Add(TestData.GetComparisonDatesQuery);
            this.Requests.Add(TestData.GetTodaysSalesQuery);
            this.Requests.Add(TestData.GetTodaysSettlementQuery);
            this.Requests.Add(TestData.GetTodaysSalesCountByHourQuery);
            this.Requests.Add(TestData.GetTodaysSalesValueByHourQuery);
            this.Requests.Add(TestData.GetMerchantKpiQuery);
            this.Requests.Add(TestData.GetTodaysFailedSalesQuery);
            this.Requests.Add(TestData.GetTopProductDataQuery);
            this.Requests.Add(TestData.GetBottomProductDataQuery);
            this.Requests.Add(TestData.GetTopMerchantDataQuery);
            this.Requests.Add(TestData.GetBottomMerchantDataQuery);
            this.Requests.Add(TestData.GetTopOperatorDataQuery);
            this.Requests.Add(TestData.GetBottomOperatorDataQuery);
            this.Requests.Add(TestData.GetLastSettlementQuery);
            this.Requests.Add(TestData.GetProductPerformanceQuery);
            this.Requests.Add(TestData.GetSettlementSummaryQuery);

            // Commands
            this.Requests.Add(TestData.AddNewOperatorCommand);
            this.Requests.Add(TestData.UpdateOperatorCommand);
            
            this.Requests.Add(TestData.AddNewMerchantCommand);
            this.Requests.Add(TestData.UpdateMerchantCommand);
            this.Requests.Add(TestData.UpdateMerchantAddressCommand);
            this.Requests.Add(TestData.UpdateMerchantContactCommand);
            this.Requests.Add(TestData.AssignOperatorToMerchantCommand);
            this.Requests.Add(TestData.RemoveOperatorFromMerchantCommand);
            this.Requests.Add(TestData.AssignContractToMerchantCommand);
            this.Requests.Add(TestData.RemoveContractFromMerchantCommand);
            this.Requests.Add(TestData.AssignDeviceToMerchantCommand);
            this.Requests.Add(TestData.CreateContractCommand);
            this.Requests.Add(TestData.CreateContractProductCommand);
            this.Requests.Add(TestData.CreateContractProductTransactionFeeCommand);
            this.Requests.Add(TestData.MakeDepositCommand);



            Mock<IWebHostEnvironment> hostingEnvironment = new Mock<IWebHostEnvironment>();
            hostingEnvironment.Setup(he => he.EnvironmentName).Returns("IntegrationTest");
            hostingEnvironment.Setup(he => he.ContentRootPath).Returns("/home");
            hostingEnvironment.Setup(he => he.ApplicationName).Returns("Test Application");

            ServiceRegistry services = new ServiceRegistry();
            Startup s = new Startup(hostingEnvironment.Object);
            Startup.Configuration = this.SetupMemoryConfiguration();

            this.AddTestRegistrations(services, hostingEnvironment.Object);
            s.ConfigureContainer(services);
            
            Startup.Container.AssertConfigurationIsValid(AssertMode.Full);
            Shared.Logger.Logger.Initialise(new NullLogger());
            this.mediator = Startup.Container.GetService<IMediator>();
        }

        [Fact]
        public async Task Mediator_Send_RequestHandled() {
            List<String> errors = new List<String>();

            foreach (IBaseRequest baseRequest in this.Requests) {
                try {
                    await mediator.Send(baseRequest);
                }
                catch (Exception ex) {
                    errors.Add($"{ex.Message} Request type {baseRequest.GetType()}");
                }
            }

            if (errors.Any() == true) {
                String errorMessage = String.Join(Environment.NewLine, errors);
                throw new Exception(errorMessage);
            }
        }

        private IConfigurationRoot SetupMemoryConfiguration()
        {
            Dictionary<String, String> configuration = new Dictionary<String, String>();

            IConfigurationBuilder builder = new ConfigurationBuilder();

            configuration.Add("ConnectionStrings:HealthCheck", "HeathCheckConnString");
            configuration.Add("SecurityConfiguration:Authority", "https://127.0.0.1");
            configuration.Add("EventStoreSettings:ConnectionString", "esdb://127.0.0.1:2113");
            configuration.Add("EventStoreSettings:ConnectionName", "UnitTestConnection");
            configuration.Add("EventStoreSettings:UserName", "admin");
            configuration.Add("EventStoreSettings:Password", "changeit");
            configuration.Add("AppSettings:UseConnectionStringConfig", "false");
            configuration.Add("AppSettings:SecurityService", "http://127.0.0.1");
            configuration.Add("AppSettings:MessagingServiceApi", "http://127.0.0.1");
            configuration.Add("AppSettings:TransactionProcessorApi", "http://127.0.0.1");
            configuration.Add("AppSettings:FileProcessorApi", "http://127.0.0.1");
            configuration.Add("AppSettings:DatabaseEngine", "SqlServer");
            configuration.Add("AppSettings:EstateReportingApi", "http://127.0.0.1");

            builder.AddInMemoryCollection(configuration);

            return builder.Build();
        }

        private void AddTestRegistrations(ServiceRegistry services,
                                          IWebHostEnvironment hostingEnvironment)
        {
            services.AddLogging();
            DiagnosticListener diagnosticSource = new DiagnosticListener(hostingEnvironment.ApplicationName);
            services.AddSingleton<DiagnosticSource>(diagnosticSource);
            services.AddSingleton<DiagnosticListener>(diagnosticSource);
            services.AddSingleton<IWebHostEnvironment>(hostingEnvironment);
            services.AddSingleton<IHostEnvironment>(hostingEnvironment);
            services.AddSingleton<IConfiguration>(Startup.Configuration);

            //services.OverrideServices(s => {
            //    s.AddSingleton<IMerchantDomainService, DummyMerchantDomainService>();
            //    s.AddSingleton<IEstateDomainService, DummyEstateDomainService>();
            //    s.AddSingleton<IContractDomainService, DummyContractDomainService>();
            //    s.AddSingleton<IMerchantStatementDomainService, DummyMerchantStatementDomainService>();
            //    s.AddSingleton<IEstateManagementManager, DummyEstateManagementManager>();
            //    s.AddSingleton<IOperatorDomainService, DummyOperatorDomainService>();
            //});
        }
    }
}