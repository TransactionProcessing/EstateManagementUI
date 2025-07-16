using EstateManagementUI.BusinessLogic.Clients;
using Lamar;
using Shared.General;
using System.Net.Http;
using FileProcessor.Client;
using System.Diagnostics.CodeAnalysis;
using ClientProxyBase;
using EstateReportingAPI.Client;
using TransactionProcessor.Client;

namespace EstateManagementUI.Bootstrapper;

[ExcludeFromCodeCoverage]
public class ClientRegistry : ServiceRegistry
{
    public ClientRegistry()
    {
        //this.AddSingleton<IConfigurationService, ConfigurationService>();
        this.AddHttpContextAccessor();
        this.AddSingleton<IApiClient, ApiClient>();
        this.RegisterHttpClient<IFileProcessorClient, FileProcessorClient>();
        this.RegisterHttpClient<ITransactionProcessorClient, TransactionProcessorClient>();
        this.RegisterHttpClient<IEstateReportingApiClient, EstateReportingApiClient>();
        this.AddSingleton<Func<String, String>>(container => (serviceName) => ConfigurationReader.GetBaseServerUri(serviceName).OriginalString);
    }
}