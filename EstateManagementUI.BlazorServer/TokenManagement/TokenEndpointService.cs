namespace EstateManagementUI.BlazorServer.TokenManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Net.Http;
    using System.Threading.Tasks;
    using IdentityModel;
    using IdentityModel.Client;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.OpenIdConnect;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.IdentityModel.Protocols.OpenIdConnect;

    /// <summary>
    /// Service for handling token endpoint operations
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TokenEndpointService
    {
        private readonly IAuthenticationSchemeProvider AuthenticationSchemeProvider;
        private readonly AutomaticTokenManagementOptions AutomaticTokenManagementOptions;
        private readonly HttpClient HttpClient;
        private readonly ILogger<TokenEndpointService> Logger;
        private readonly IOptionsSnapshot<OpenIdConnectOptions> OidcOptionsSnapshot;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenEndpointService"/> class.
        /// </summary>
        public TokenEndpointService(IOptions<AutomaticTokenManagementOptions> managementOptions,
                                    IOptionsSnapshot<OpenIdConnectOptions> oidcOptions,
                                    IAuthenticationSchemeProvider schemeProvider,
                                    HttpClient httpClient,
                                    ILogger<TokenEndpointService> logger)
        {
            this.AutomaticTokenManagementOptions = managementOptions.Value;
            this.OidcOptionsSnapshot = oidcOptions;
            this.AuthenticationSchemeProvider = schemeProvider;
            this.HttpClient = httpClient;
            this.Logger = logger;
        }

        /// <summary>
        /// Refreshes the token asynchronous.
        /// </summary>
        public async Task<TokenResponse> RefreshTokenAsync(String refreshToken)
        {
            OpenIdConnectOptions oidcOptions = await this.GetOidcOptionsAsync();
            OpenIdConnectConfiguration configuration = await oidcOptions.ConfigurationManager.GetConfigurationAsync(default);

            return await this.HttpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                Address = configuration.TokenEndpoint,
                ClientId = oidcOptions.ClientId,
                ClientSecret = oidcOptions.ClientSecret,
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        /// Revokes the token asynchronous.
        /// </summary>
        public async Task<TokenRevocationResponse> RevokeTokenAsync(String refreshToken)
        {
            OpenIdConnectOptions oidcOptions = await this.GetOidcOptionsAsync();
            OpenIdConnectConfiguration configuration = await oidcOptions.ConfigurationManager.GetConfigurationAsync(default);
            
            return await this.HttpClient.RevokeTokenAsync(new TokenRevocationRequest
            {
                Address = configuration.AdditionalData[OidcConstants.Discovery.RevocationEndpoint].ToString(),
                ClientId = oidcOptions.ClientId,
                ClientSecret = oidcOptions.ClientSecret,
                Token = refreshToken,
                TokenTypeHint = OidcConstants.TokenTypes.RefreshToken
            });
        }

        /// <summary>
        /// Gets the oidc options asynchronous.
        /// </summary>
        private async Task<OpenIdConnectOptions> GetOidcOptionsAsync()
        {
            if (string.IsNullOrEmpty(this.AutomaticTokenManagementOptions.Scheme))
            {
                AuthenticationScheme scheme = await this.AuthenticationSchemeProvider.GetDefaultChallengeSchemeAsync();
                return this.OidcOptionsSnapshot.Get(scheme.Name);
            }

            return this.OidcOptionsSnapshot.Get(this.AutomaticTokenManagementOptions.Scheme);
        }
    }
}
