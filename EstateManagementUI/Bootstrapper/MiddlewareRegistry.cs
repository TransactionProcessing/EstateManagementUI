using Lamar;
using Shared.Extensions;
using Shared.Middleware;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Bootstrapper
{
    [ExcludeFromCodeCoverage]
    public class MiddlewareRegistry : ServiceRegistry {
        public MiddlewareRegistry() {
            RequestResponseMiddlewareLoggingConfig config =
                new RequestResponseMiddlewareLoggingConfig(LogLevel.Information, true, true);

            this.AddSingleton(config);

            this.AddHealthChecks().AddSecurityService(this.ApiEndpointHttpHandler).AddTransactionProcessorService(this.ApiEndpointHttpHandler)
                .AddFileProcessorService(this.ApiEndpointHttpHandler);
        }

        private HttpClientHandler ApiEndpointHttpHandler(IServiceProvider serviceProvider)
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message,
                                                             cert,
                                                             chain,
                                                             errors) =>
                {
                    return true;
                }
            };
        }
    }


}
