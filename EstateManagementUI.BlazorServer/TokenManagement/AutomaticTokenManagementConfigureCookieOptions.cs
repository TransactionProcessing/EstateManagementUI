namespace EstateManagementUI.BlazorServer.TokenManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Configures cookie options for automatic token management
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AutomaticTokenManagementConfigureCookieOptions : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        private readonly AuthenticationScheme Scheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticTokenManagementConfigureCookieOptions"/> class.
        /// </summary>
        public AutomaticTokenManagementConfigureCookieOptions(IAuthenticationSchemeProvider provider)
        {
            this.Scheme = provider.GetDefaultSignInSchemeAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Invoked to configure a TOptions instance.
        /// </summary>
        public void Configure(CookieAuthenticationOptions options)
        {
            // Nothing to be done in here, just satisfying the interface
        }

        /// <summary>
        /// Invoked to configure a TOptions instance.
        /// </summary>
        public void Configure(String name, CookieAuthenticationOptions options)
        {
            if (name == this.Scheme.Name)
            {
                options.EventsType = typeof(AutomaticTokenManagementCookieEvents);
            }
        }
    }
}
