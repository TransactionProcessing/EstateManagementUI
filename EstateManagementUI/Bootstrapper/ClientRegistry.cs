using EstateManagement.Client;
using EstateManagementUI.BusinessLogic.Clients;
using Lamar;
using Shared.General;

namespace EstateManagementUI.Bootstrapper;

public class ClientRegistry : ServiceRegistry
{
    public ClientRegistry()
    {
        //this.AddSingleton<IConfigurationService, ConfigurationService>();
        this.AddSingleton<IApiClient, ApiClient>();
        this.AddSingleton<IEstateClient, EstateClient>();
        //this.AddSingleton<IFileProcessorClient, FileProcessorClient>();
        //this.AddSingleton<ITransactionProcessorClient, TransactionProcessorClient>();
        //this.AddSingleton<IEstateReportingApiClient, EstateReportingApiClient>();
        this.AddSingleton<Func<String, String>>(container => (serviceName) =>
        {
            return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString;
        });
        this.AddSingleton<HttpClient>();
    }
}