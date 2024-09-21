using Lamar;
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
        }
    }
}
