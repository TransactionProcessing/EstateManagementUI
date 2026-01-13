namespace EstateAdministrationUI.TokenManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
        /// 
        /// </summary>
        [ExcludeFromCodeCoverage]
        public static class AutomaticTokenManagementBuilderExtensions
        {
            #region Methods

            /// <summary>
            /// Adds the automatic token management.
            /// </summary>
            /// <param name="builder">The builder.</param>
            /// <param name="options">The options.</param>
            /// <returns></returns>
            public static AuthenticationBuilder AddAutomaticTokenManagement(this AuthenticationBuilder builder,
                                                                            Action<AutomaticTokenManagementOptions> options)
            {
                builder.Services.Configure(options);
                return builder.AddAutomaticTokenManagement();
            }

            /// <summary>
            /// Adds the automatic token management.
            /// </summary>
            /// <param name="builder">The builder.</param>
            /// <returns></returns>
            public static AuthenticationBuilder AddAutomaticTokenManagement(this AuthenticationBuilder builder)
            {
                builder.Services.AddTransient<TokenEndpointService>();

                builder.Services.AddTransient<AutomaticTokenManagementCookieEvents>();
                builder.Services.AddSingleton<IConfigureOptions<CookieAuthenticationOptions>, AutomaticTokenManagementConfigureCookieOptions>();

                return builder;
            }

            #endregion
        }
}