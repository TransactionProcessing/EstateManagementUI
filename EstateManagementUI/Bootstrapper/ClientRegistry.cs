using EstateManagement.Client;
using EstateManagementUI.BusinessLogic.Clients;
using Lamar;
using Shared.General;
using System.Net.Http;
using FileProcessor.Client;
using System.Diagnostics.CodeAnalysis;
using EstateReportingAPI.Client;

namespace EstateManagementUI.Bootstrapper;

[ExcludeFromCodeCoverage]
public class ClientRegistry : ServiceRegistry
{
    public ClientRegistry()
    {
        //this.AddSingleton<IConfigurationService, ConfigurationService>();
        this.AddSingleton<IApiClient, ApiClient>();
        this.AddSingleton<IEstateClient, EstateClient>();
        this.AddSingleton<IFileProcessorClient, FileProcessorClient>();
        //this.AddSingleton<ITransactionProcessorClient, TransactionProcessorClient>();
        this.AddSingleton<IEstateReportingApiClient, EstateReportingApiClient>();
        this.AddSingleton<Func<String, String>>(container => (serviceName) =>
        {
            return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString;
        });

        Boolean httpClientIgnoreCertificateErrors =
            ConfigurationReader.GetValueOrDefault<Boolean>("AppSettings",
                "HttpClientIgnoreCertificateErrors", false);
        HttpClient httpClient = new HttpClient();
        if (httpClientIgnoreCertificateErrors) {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            httpClient = new HttpClient(handler);
        }
        this.AddSingleton(httpClient);
    }
}