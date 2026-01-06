using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Components;
using EstateManagementUI.BlazorServer.Services;
using EstateManagementUI.BlazorServer.TokenManagement;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Load hosting.json configuration for port settings
builder.Configuration.AddJsonFile("hosting.json", optional: true, reloadOnChange: true);

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

// Clear default claims mapping
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Check if running in test mode
var testMode = builder.Configuration.GetValue<bool>("AppSettings:TestMode", false);
Console.WriteLine($"Application running in Test Mode: {testMode}");

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configure authentication based on mode
if (testMode)
{
    // Test mode: Use test authentication handler to bypass OIDC
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = TestAuthenticationHandler.SchemeName;
        options.DefaultChallengeScheme = TestAuthenticationHandler.SchemeName;
    })
    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(
        TestAuthenticationHandler.SchemeName, 
        options => { });
}
else
{
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
        else {
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
}

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Register MediatR service based on test mode
if (testMode)
{
    Console.WriteLine("Registering TestMediatorService with in-memory test data store");
    builder.Services.AddSingleton<ITestDataStore, TestDataStore>();
    builder.Services.AddSingleton<IMediator, TestMediatorService>();
}
else
{
    Console.WriteLine("Registering StubbedMediatorService");
    builder.Services.AddSingleton<IMediator, StubbedMediatorService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add login endpoint - behavior depends on test mode
if (testMode)
{
    app.MapGet("/login", (HttpContext context) =>
    {
        // In test mode, redirect directly to home since authentication is automatic
        return Results.Redirect("/");
    }).AllowAnonymous();
}
else
{
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
}

// Add logout endpoint - behavior depends on test mode
if (testMode)
{
    app.MapGet("/logout", (HttpContext context) =>
    {
        // In test mode, just redirect to home
        return Results.Redirect("/");
    }).RequireAuthorization();
}
else
{
    app.MapGet("/logout", async (HttpContext context) =>
    {
        await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
        await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Results.Redirect("/");
    }).RequireAuthorization();
}

app.Run();
