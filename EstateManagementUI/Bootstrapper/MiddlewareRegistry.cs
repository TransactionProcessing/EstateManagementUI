using EstateManagement.Client;
using EstateManagmentUI.BusinessLogic.RequestHandlers;
using Hydro.Configuration;
using IdentityModel;
using Lamar;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Shared.General;
using Shared.Middleware;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;

namespace EstateManagementUI.Bootstrapper
{
    public class MiddlewareRegistry : ServiceRegistry
    {
        public MiddlewareRegistry() {
            
        }
    }

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

    public class ClientRegistry : ServiceRegistry
    {
        public ClientRegistry()
        {
            //this.AddSingleton<IConfigurationService, ConfigurationService>();
            this.AddSingleton<IApiClient, ApiClient>();
            this.AddSingleton<IEstateClient, EstateClient>();
            //this.AddSingleton<IFileProcessorClient, FileProcessorClient>();
            //this.AddSingleton<ITransactionProcessorClient, TransactionProcessorClient>();
            //this.AddSingleton<IEstateReportingApiClient, EstateReportingApiClient>();
            this.AddSingleton<Func<String, String>>(container => (serviceName) =>
            {
                return ConfigurationReader.GetBaseServerUri(serviceName).OriginalString;
            });
            this.AddSingleton<HttpClient>();
        }
    }

    [ExcludeFromCodeCoverage]
    public class MediatorRegistry : ServiceRegistry {
        public MediatorRegistry() {
            this.AddTransient<IMediator, Mediator>();

            this.AddSingleton<IRequestHandler<Queries.GetEstateQuery, EstateModel>, EstateRequestHandler>();
        }
    }

    public static class Helpers
    {
        public static (String authorityAddress, String issuerAddress) GetSecurityServiceAddresses(String authority, String securityServiceLocalPort, String securityServicePort)
        {
            Console.WriteLine($"authority is {authority}");
            Console.WriteLine($"securityServiceLocalPort is {securityServiceLocalPort}");
            Console.WriteLine($"securityServicePort is {securityServicePort}");

            if (String.IsNullOrEmpty(securityServiceLocalPort))
            {
                securityServiceLocalPort = "5001";
            }

            if (String.IsNullOrEmpty(securityServicePort))
            {
                securityServicePort = "5001";
            }

            Uri u = new Uri(authority);

            var authorityAddress = u.Port switch
            {
                _ when u.Port.ToString() != securityServiceLocalPort => $"{u.Scheme}://{u.Host}:{securityServiceLocalPort}{u.AbsolutePath}",
                _ => authority
            };

            var issuerAddress = u.Port switch
            {
                _ when u.Port.ToString() != securityServicePort => $"{u.Scheme}://{u.Host}:{securityServicePort}{u.AbsolutePath}",
                _ => authority
            };

            if (authorityAddress.EndsWith("/"))
            {
                authorityAddress = $"{authorityAddress.Substring(0, authorityAddress.Length - 1)}";
            }
            if (issuerAddress.EndsWith("/"))
            {
                issuerAddress = $"{issuerAddress.Substring(0, issuerAddress.Length - 1)}";
            }

            Console.WriteLine($"authorityAddress is {authorityAddress}");
            Console.WriteLine($"issuerAddress is {issuerAddress}");

            return (authorityAddress, issuerAddress);
        }
    }
}
