namespace EstateAdministrationUI.TokenManagement
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using IdentityModel.Client;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents" />
    [ExcludeFromCodeCoverage]
    public class AutomaticTokenManagementCookieEvents : CookieAuthenticationEvents
    {
        /// <summary>
        /// The token endpoint service
        /// </summary>
        private readonly TokenEndpointService TokenEndpointService;
        /// <summary>
        /// The automatic token management options
        /// </summary>
        private readonly AutomaticTokenManagementOptions AutomaticTokenManagementOptions;
        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger Logger;
        /// <summary>
        /// The system clock
        /// </summary>
        private readonly ISystemClock SystemClock;

        /// <summary>
        /// The pending refresh token requests
        /// </summary>
        private static readonly ConcurrentDictionary<String, Boolean> PendingRefreshTokenRequests =
            new ConcurrentDictionary<String, Boolean>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomaticTokenManagementCookieEvents"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="options">The options.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="clock">The clock.</param>
        public AutomaticTokenManagementCookieEvents(
            TokenEndpointService service,
            IOptions<AutomaticTokenManagementOptions> options,
            ILogger<AutomaticTokenManagementCookieEvents> logger,
            ISystemClock clock)
        {
            this.TokenEndpointService = service;
            this.AutomaticTokenManagementOptions = options.Value;
            this.Logger = logger;
            this.SystemClock = clock;
        }

        /// <summary>
        /// Implements the interface method by invoking the related delegate method.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            IEnumerable<AuthenticationToken> tokens = context.Properties.GetTokens();
            if (tokens == null || !tokens.Any())
            {
                this.Logger.LogInformation("No tokens found in cookie properties. SaveTokens must be enabled for automatic token refresh.");
                return;
            }

            AuthenticationToken refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
            if (refreshToken == null)
            {
                this.Logger.LogInformation("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
                return;
            }

            AuthenticationToken expiresAt = tokens.SingleOrDefault(t => t.Name == "expires_at");
            if (expiresAt == null)
            {
                this.Logger.LogInformation("No expires_at value found in cookie properties.");
                return;
            }

            DateTimeOffset dtExpires = DateTimeOffset.Parse(expiresAt.Value, CultureInfo.InvariantCulture);
            DateTimeOffset dtRefresh = dtExpires.Subtract(this.AutomaticTokenManagementOptions.RefreshBeforeExpiration);

            if (dtRefresh < this.SystemClock.UtcNow)
            {
                Boolean shouldRefresh = AutomaticTokenManagementCookieEvents.PendingRefreshTokenRequests.TryAdd(refreshToken.Value, true);
                if (shouldRefresh)
                {
                    try
                    {
                        TokenResponse response = await this.TokenEndpointService.RefreshTokenAsync(refreshToken.Value);

                        if (response.IsError)
                        {
                            this.Logger.LogWarning("Error refreshing token: {error}", response.Error);
                            return;
                        }

                        context.Properties.UpdateTokenValue("access_token", response.AccessToken);
                        context.Properties.UpdateTokenValue("refresh_token", response.RefreshToken);

                        DateTime newExpiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(response.ExpiresIn);
                        context.Properties.UpdateTokenValue("expires_at", newExpiresAt.ToString("o", CultureInfo.InvariantCulture));

                        await context.HttpContext.SignInAsync(context.Principal, context.Properties);
                    }
                    finally
                    {
                        AutomaticTokenManagementCookieEvents.PendingRefreshTokenRequests.TryRemove(refreshToken.Value, out _);
                    }
                }
            }
        }

        /// <summary>
        /// Implements the interface method by invoking the related delegate method.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task SigningOut(CookieSigningOutContext context)
        {
            if (this.AutomaticTokenManagementOptions.RevokeRefreshTokenOnSignout == false) return;

            AuthenticateResult result = await context.HttpContext.AuthenticateAsync();

            if (!result.Succeeded)
            {
                this.Logger.LogInformation("Can't find cookie for default scheme. Might have been deleted already.");
                return;
            }

            IEnumerable<AuthenticationToken> tokens = result.Properties.GetTokens();
            if (tokens == null || !tokens.Any())
            {
                this.Logger.LogInformation("No tokens found in cookie properties. SaveTokens must be enabled for automatic token revocation.");
                return;
            }

            AuthenticationToken refreshToken = tokens.SingleOrDefault(t => t.Name == OpenIdConnectParameterNames.RefreshToken);
            if (refreshToken == null)
            {
                this.Logger.LogInformation("No refresh token found in cookie properties. A refresh token must be requested and SaveTokens must be enabled.");
                return;
            }

            TokenRevocationResponse response = await this.TokenEndpointService.RevokeTokenAsync(refreshToken.Value);

            if (response.IsError)
            {
                this.Logger.LogWarning("Error revoking token: {error}", response.Error);
                return;
            }
        }
    }
}