using System.Net;
using System.Security.Cryptography.X509Certificates;
using ClientProxyBase;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.TokenManagement;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.BackendAPI;
using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SecurityService.Client;
using Shared.General;
using TransactionProcessor.Client;

namespace EstateManagementUI.BlazorServer.Common;

public static class BoostrapperExtensions {
    public static WebApplication ConfigureLiveLogin(this WebApplication app) {
        app.MapGet("/login", (HttpContext context) =>
        {
            return Results.Challenge(
                properties: new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    RedirectUri = "/",
                    Items =
                    {
                        { "prompt", "login" } // Force the user to re-enter credentials
                    }
                },
                authenticationSchemes: new[] { OpenIdConnectDefaults.AuthenticationScheme }
            );
        }).AllowAnonymous();

        app.MapGet("/logout", async (HttpContext context) =>
        {
            await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Results.Redirect("/entry");
        }).RequireAuthorization();
        return app;
    }

    public static WebApplication ConfigureTestLogin(this WebApplication app) {
        app.MapGet("/login", (HttpContext context) =>
        {
            // In test mode, redirect directly to home since authentication is automatic
            return Results.Redirect("/");
        }).AllowAnonymous();

        app.MapGet("/logout", (HttpContext context) =>
        {
            // In test mode, just redirect to home
            return Results.Redirect("/entry");
        }).RequireAuthorization();

        return app;
    }

    public static WebApplicationBuilder ConfigureKestrel(this WebApplicationBuilder builder) {
        // Configure Kestrel with certificate for HTTPS
        builder.WebHost.UseKestrel(options =>
        {
            var port = 5004;

            options.Listen(IPAddress.Any, port, listenOptions =>
            {
                // Enable support for HTTP1 and HTTP2
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;

                // Configure Kestrel to use a certificate from a local .PFX file for hosting HTTPS
                var certificatePath = Path.Combine(AppContext.BaseDirectory, "Certificates");
                Console.WriteLine($"Looking for certificates in: {certificatePath}");
                Console.WriteLine($"AppContext.BaseDirectory: {AppContext.BaseDirectory}");
                Console.WriteLine($"Current Directory: {Directory.GetCurrentDirectory()}");

                if (!Directory.Exists(certificatePath))
                {
                    throw new InvalidOperationException($"Certificates folder not found at: {certificatePath}");
                }

                var certificateFiles = Directory.GetFiles(certificatePath, "*.pfx");
                if (certificateFiles.Length == 0)
                {
                    throw new InvalidOperationException($"No .pfx certificate file found in {certificatePath}");
                }

                var certificateFile = certificateFiles.First();
                Console.WriteLine($"Loading certificate from: {certificateFile}");

                try
                {
                    var certificate = new X509Certificate2(certificateFile, "password");
                    Console.WriteLine($"Certificate loaded successfully. Subject: {certificate.Subject}");
                    listenOptions.UseHttps(certificate);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading certificate: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                    throw new InvalidOperationException($"Failed to load certificate from {certificateFile}: {ex.Message}", ex);
                }
            });
        });
        return builder;
    }

    public static WebApplicationBuilder LoadConfiguration(this WebApplicationBuilder builder) {
        builder.Configuration
            .AddJsonFile("/home/txnproc/config/appsettings.json", true, true)
            .AddJsonFile($"/home/txnproc/config/appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        ConfigurationReader.Initialise(builder.Configuration);
        return builder;
    }

    public static WebApplicationBuilder ConfigureLiveAuthentication(this WebApplicationBuilder builder) {
        // Production mode: Use OIDC authentication
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAutomaticTokenManagement(o => {
                o.RefreshBeforeExpiration = TimeSpan.FromSeconds(30);
                o.RevokeRefreshTokenOnSignout = true;
                o.Scheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
            {
                // Read configuration values
                var authority = builder.Configuration["Authentication:Authority"];
                var securityServiceLocalPort = builder.Configuration["AppSettings:SecurityServiceLocalPort"];
                var securityServicePort = builder.Configuration["AppSettings:SecurityServicePort"];
                var httpClientIgnoreCertificateErrors = builder.Configuration.GetValue<bool>("AppSettings:HttpClientIgnoreCertificateErrors", false);

                // Use helper method to get adjusted addresses for integration testing
                var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
                    authority,
                    securityServiceLocalPort,
                    securityServicePort);

                // Configure certificate validation bypass for CI/CD testing
                if (httpClientIgnoreCertificateErrors)
                {
                    Console.WriteLine("WARNING: Certificate validation is disabled for HttpClient backchannel communication");
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                    };
                    options.BackchannelHttpHandler = handler;
                }
                else
                {
                    Console.WriteLine("WARNING: Certificate validation is enabled for HttpClient backchannel communication");
                }

                // Configure OpenID Connect settings
                options.Authority = authorityAddress;
                options.ClientId = builder.Configuration["Authentication:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:ClientSecret"];
                options.ResponseType = "code id_token";
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                // Set the callback path - REQUIRED for OIDC to work
                options.CallbackPath = builder.Configuration["Authentication:CallbackPath"] ?? "/signin-oidc";

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("offline_access");

                // Add additional scopes from old app
                options.Scope.Add("fileProcessor");
                options.Scope.Add("transactionProcessor");

                options.RequireHttpsMetadata = false; // For development - set to true in production

                // Map claims
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = false,
                    NameClaimType = "name",
                    RoleClaimType = "role"
                };
                options.ClaimActions.MapAllExcept("iss", "nbf", "exp", "aud", "nonce", "iat", "c_hash");

                // Set MetadataAddress to use the authority address
                options.MetadataAddress = $"{authorityAddress}/.well-known/openid-configuration";

                // Handle prompt parameter for forcing re-authentication
                options.Events = new OpenIdConnectEvents
                {
                    OnRedirectToIdentityProvider = context =>
                    {
                        // Pass prompt parameter if specified in authentication properties
                        if (context.Properties.Items.TryGetValue("prompt", out var prompt))
                        {
                            context.ProtocolMessage.Prompt = prompt;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        return builder;
    }

    public static WebApplicationBuilder ConfigureTestAuthentication(this WebApplicationBuilder builder) {
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = TestAuthenticationHandler.SchemeName;
                options.DefaultChallengeScheme = TestAuthenticationHandler.SchemeName;
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
                TestAuthenticationHandler.SchemeName,
                options => { });
        return builder;
    }

    public static WebApplicationBuilder RegisterPermissionServices(this WebApplicationBuilder builder) {
        builder.Services.AddSingleton<IPermissionStore, InMemoryPermissionStore>();
        builder.Services.AddScoped<IPermissionService, PermissionService>();
        builder.Services.AddScoped<IPermissionKeyProvider, PermissionKeyProvider>();
        // Console.WriteLine("Registered Permission services");
        return builder;
    }

    public static WebApplicationBuilder RegisterTestMediator(this WebApplicationBuilder builder) {
        Console.WriteLine("Registering TestMediatorService with in-memory test data store");
        builder.Services.AddSingleton<ITestDataStore, TestDataStore>();
        builder.Services.AddSingleton<IMediator, TestMediatorService>();
        return builder;
    }

    public static WebApplicationBuilder RegisterProductionMeriator(this WebApplicationBuilder builder) {
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(EstateRequestHandler).Assembly));
        builder.Services.AddSingleton<IApiClient, ApiClient>();
        builder.Services.AddSingleton<Func<String, String>>(container => (serviceName) => ConfigurationReader.GetBaseServerUri(serviceName).OriginalString);
        return builder;
    }

    public static WebApplicationBuilder RegisterUIServices(this WebApplicationBuilder builder) {
        builder.Services.AddSingleton<IEstateUIService, EstateUIService>();
        builder.Services.AddSingleton<IOperatorUIService, OperatorUIService>();
        builder.Services.AddSingleton<IContractUIService, ContractUIService>();
        builder.Services.AddSingleton<IMerchantUIService, MerchantUIService>();
        return builder;
    }
    public static WebApplicationBuilder RegisterClients(this WebApplicationBuilder builder) {
        builder.Services.RegisterHttpClient<IEstateReportingApiClient, EstateReportingApiClient>();
        builder.Services.RegisterHttpClient<ISecurityServiceClient, SecurityServiceClient>();
        builder.Services.RegisterHttpClient<ITransactionProcessorClient, TransactionProcessorClient>();
        return builder;
    }
}