using System.Diagnostics.CodeAnalysis;
using System.Net.Mime;
using System.Reflection;
using EstateAdministrationUI.TokenManagement;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Database;
using EstateManagementUI.Common;
using IdentityModel;
using Lamar;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using Shared.General;

namespace EstateManagementUI.Bootstrapper;

[ExcludeFromCodeCoverage]
public class AuthenticationRegistry : ServiceRegistry {
    public AuthenticationRegistry() {
        Boolean httpClientIgnoreCertificateErrors =
            ConfigurationReader.GetValueOrDefault<Boolean>("AppSettings",
                "HttpClientIgnoreCertificateErrors", false);
        
        this.AddAuthentication(options => {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        })
        .AddAutomaticTokenManagement(o => {
            o.RefreshBeforeExpiration = TimeSpan.FromSeconds(30);
            o.RevokeRefreshTokenOnSignout = true;
            o.Scheme = "oidc";
        })
            .AddCookie("Cookies").AddOpenIdConnect("oidc", options => {
            String authority = ConfigurationReader.GetValue("Authority");
            String securityServiceLocalPort =
                ConfigurationReader.GetValueOrDefault<String>("AppSettings", "SecurityServiceLocalPort", null);
            String securityServicePort =
                ConfigurationReader.GetValueOrDefault<String>("AppSettings", "SecurityServicePort", null);

            (string authorityAddress, string issuerAddress) results =
                Helpers.GetSecurityServiceAddresses(authority, securityServiceLocalPort, securityServicePort);

            if (httpClientIgnoreCertificateErrors) {
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                options.BackchannelHttpHandler = handler;
            }

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

            //options.Scope.Add("estateReporting");
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
            options.Events.OnAccessDenied = context => {
                
                var r = context.Result;
                if (r.Failure != null) {
                    Shared.Logger.Logger.LogWarning($"context.Exception.Message {r.Failure.Message}");
                }

                if (r.Succeeded == false) {
                    Shared.Logger.Logger.LogWarning($"r.Succeeded == false");
                }
                return Task.CompletedTask;
            };
            options.Events.OnAuthenticationFailed = context => {
                Shared.Logger.Logger.LogWarning($"context.Exception.Message {context.Exception.Message}");
                Shared.Logger.Logger.LogWarning($"context.ProtocolMessage.ErrorUri {context.ProtocolMessage.ErrorUri}");
                Shared.Logger.Logger.LogWarning($"context.ProtocolMessage.Error {context.ProtocolMessage.Error}");
                Shared.Logger.Logger.LogWarning($"context.ProtocolMessage.ErrorDescription {context.ProtocolMessage.ErrorDescription}");
                return Task.CompletedTask;
            };

        });
            
        this.AddAuthorization();
        this.AddSingleton<IConfigurationService, ConfigurationService>();
        this.AddSingleton<IPermissionsService, PermissionsService>();
        this.AddSingleton<IPermissionsRepository, PermissionsRepository>();
        String dbFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "permissions.db");
        var fi = new FileInfo(dbFileName);
        Shared.Logger.Logger.LogWarning($"File {dbFileName} exists [{fi.Exists}]");
        String dbFileName1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "permissions.db-shm");
        var fi1 = new FileInfo(dbFileName1);
        Shared.Logger.Logger.LogWarning($"File {dbFileName1} exists [{fi1.Exists}]");
        String dbFileName2 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "permissions.db-wal");
        var fi2 = new FileInfo(dbFileName2);
        Shared.Logger.Logger.LogWarning($"File {dbFileName2} exists [{fi2.Exists}]");
        String connectionString = $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "permissions.db")}";
        Shared.Logger.Logger.LogWarning($"Permissions connection string {connectionString}");
        this.AddSingleton<IDbContextFactory<PermissionsContext>, DbContextFactory<PermissionsContext>>();
        this.AddDbContextFactory<PermissionsContext>(options =>
            options.UseSqlite(connectionString));
    }
}