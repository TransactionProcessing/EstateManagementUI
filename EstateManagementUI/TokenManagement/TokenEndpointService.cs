namespace EstateAdministrationUI.TokenManagement
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
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TokenEndpointService
    {
        #region Fields

        /// <summary>
        /// The authentication scheme provider
        /// </summary>
        private readonly IAuthenticationSchemeProvider AuthenticationSchemeProvider;

        /// <summary>
        /// The automatic token management options
        /// </summary>
        private readonly AutomaticTokenManagementOptions AutomaticTokenManagementOptions;

        /// <summary>
        /// The HTTP client factory
        /// </summary>
        private readonly HttpClient HttpClient;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TokenEndpointService> Logger;

        /// <summary>
        /// The oidc options snapshot
        /// </summary>
        private readonly IOptionsSnapshot<OpenIdConnectOptions> OidcOptionsSnapshot;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenEndpointService"/> class.
        /// </summary>
        /// <param name="managementOptions">The management options.</param>
        /// <param name="oidcOptions">The oidc options.</param>
        /// <param name="schemeProvider">The scheme provider.</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="logger">The logger.</param>
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

        #endregion

        #region Methods

        /// <summary>
        /// Refreshes the token asynchronous.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
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
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns></returns>
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
        /// <returns></returns>
        private async Task<OpenIdConnectOptions> GetOidcOptionsAsync()
        {
            if (string.IsNullOrEmpty(this.AutomaticTokenManagementOptions.Scheme))
            {
                AuthenticationScheme scheme = await this.AuthenticationSchemeProvider.GetDefaultChallengeSchemeAsync();
                return this.OidcOptionsSnapshot.Get(scheme.Name);
            }

            return this.OidcOptionsSnapshot.Get(this.AutomaticTokenManagementOptions.Scheme);
        }

        #endregion
    }
}