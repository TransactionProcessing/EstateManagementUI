using EstateManagementUI.Common;
using IdentityModel;
using Lamar;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Shared.General;

namespace EstateManagementUI.Bootstrapper;

public class AuthenticationRegistry : ServiceRegistry {
    public AuthenticationRegistry() {
        this.AddAuthentication(options => {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        }).AddCookie("Cookies").AddOpenIdConnect("oidc", options => {
            String authority = ConfigurationReader.GetValue("Authority");
            String securityServiceLocalPort =
                ConfigurationReader.GetValueOrDefault<String>("AppSettings", "SecurityServiceLocalPort", null);
            String securityServicePort =
                ConfigurationReader.GetValueOrDefault<String>("AppSettings", "SecurityServicePort", null);

            (string authorityAddress, string issuerAddress) results =
                Helpers.GetSecurityServiceAddresses(authority, securityServiceLocalPort, securityServicePort);

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback =
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            options.BackchannelHttpHandler = handler;

            options.Authority = results.authorityAddress;
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateAudience = false, NameClaimType = JwtClaimTypes.Name, RoleClaimType = JwtClaimTypes.Role,
            };

            options.ClientSecret = ConfigurationReader.GetValue("ClientSecret");
            options.ClientId = ConfigurationReader.GetValue("ClientId");

            options.MetadataAddress = $"{results.authorityAddress}/.well-known/openid-configuration";

            options.ResponseType = "code id_token";

            options.Scope.Clear();
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.Scope.Add("offline_access");

            options.Scope.Add("estateManagement");
            options.Scope.Add("fileProcessor");
            options.Scope.Add("transactionProcessor");

            options.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash");

            options.GetClaimsFromUserInfoEndpoint = true;
            options.SaveTokens = true;

            options.Events.OnRedirectToIdentityProvider = context => {
                // Intercept the redirection so the browser navigates to the right URL in your host
                context.ProtocolMessage.IssuerAddress = $"{results.issuerAddress}/connect/authorize";
                return Task.CompletedTask;
            };
        });
            
        this.AddAuthorization();
    }
}