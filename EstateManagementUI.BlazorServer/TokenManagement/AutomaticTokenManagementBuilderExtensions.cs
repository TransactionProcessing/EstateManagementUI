namespace EstateManagementUI.BlazorServer.TokenManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extension methods for adding automatic token management
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class AutomaticTokenManagementBuilderExtensions
    {
        /// <summary>
        /// Adds automatic token management with options.
        /// </summary>
        public static AuthenticationBuilder AddAutomaticTokenManagement(
            this AuthenticationBuilder builder,
            Action<AutomaticTokenManagementOptions> options)
        {
            builder.Services.Configure(options);
            return builder.AddAutomaticTokenManagement();
        }

        /// <summary>
        /// Adds automatic token management.
        /// </summary>
        public static AuthenticationBuilder AddAutomaticTokenManagement(this AuthenticationBuilder builder)
        {
            builder.Services.AddHttpClient<TokenEndpointService>();
            builder.Services.AddTransient<AutomaticTokenManagementCookieEvents>();
            builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, AutomaticTokenManagementConfigureCookieOptions>();

            return builder;
        }
    }
}
