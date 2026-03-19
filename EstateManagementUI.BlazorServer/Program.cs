using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Components;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Sentry.Extensibility;
using Shared.General;
using Spectre.Console;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args).LoadConfiguration().ConfigureKestrel();


//builder.ConfigureAppConfiguration((context, configBuilder) =>
//{
//    var env = context.HostingEnvironment;

//    configBuilder.SetBasePath(fi.Directory.FullName)
//        .AddJsonFile("hosting.json", optional: true)
//        .AddJsonFile($"hosting.{env.EnvironmentName}.json", optional: true)
//        .AddJsonFile("/home/txnproc/config/appsettings.json", optional: true, reloadOnChange: true)
//        .AddJsonFile($"/home/txnproc/config/appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
//        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
//        .AddEnvironmentVariables();

//    // Build a snapshot of configuration so we can use it immediately (e.g. for Sentry)
//    var builtConfig = configBuilder.Build();

//    // Keep existing static usage (if you must), and initialise the ConfigurationReader now.
//    //Startup.Configuration = builtConfig;
//    //ConfigurationReader.Initialise(Startup.Configuration);

// Configure Sentry on the webBuilder using the config snapshot.
var sentrySection = ConfigurationReader.GetValueOrDefault("SentryConfiguration", "Dsn", "N/A");
if (sentrySection != "N/A")
{
    // Replace the condition below if you intended to only enable Sentry in certain environments.
    if (builder.Environment.IsDevelopment() == false)
    {
        builder.WebHost.UseSentry(o =>
        {
            o.Dsn = sentrySection;
            o.SendDefaultPii = true;
            o.MaxRequestBodySize = RequestSize.Always;
            o.CaptureBlockingCalls = true;
            o.Release = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "unknown";
        });
    }
}

// Clear default claims mapping
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Check if running in test mode
var testModeConfig = builder.Configuration.GetValue<String>("AppSettings:TestMode", "Disabled");
// Convert to enum
var testMode = Enum.Parse<TestMode>(testModeConfig, ignoreCase: true);

Console.WriteLine($"Application running in Test Mode: {testMode}");

// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

// Add session support for test mode role switching
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder = testMode switch {
    TestMode.AuthenticationOnly => builder.ConfigureTestAuthentication(),
    TestMode.Full => builder.ConfigureTestAuthentication(),
    _ => builder.ConfigureLiveAuthentication()
};

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Register Permission services
builder = builder.RegisterPermissionServices();

//builder = testMode switch {
//    TestMode.BackedByTestDataStore => builder.RegisterTestMediator(),
//    TestMode.Full => builder.RegisterTestMediator(),
//    _ => builder.RegisterProductionMeriator().RegisterClients().RegisterUIServices()
//};
builder.RegisterProductionMeriator().RegisterClients().RegisterUIServices();

// Add Health Checks - read URLs from configuration
var estateReportingApiUrl = builder.Configuration.GetValue<string>("AppSettings:EstateReportingApi") ?? "http://localhost:5011";
var securityServiceUrl = builder.Configuration.GetValue<string>("AppSettings:SecurityService") ?? "http://localhost:5001";

// Validate URLs and create Uri objects
Uri ValidateAndCreateUri(string url, string configKey)
{
    try
    {
        return new Uri(url);
    }
    catch (UriFormatException ex)
    {
        throw new InvalidOperationException($"Invalid URL configured for {configKey}: '{url}'", ex);
    }
}

var estateReportingUri = ValidateAndCreateUri(estateReportingApiUrl, "AppSettings:EstateReportingApi");
var securityServiceUri = ValidateAndCreateUri(securityServiceUrl, "AppSettings:SecurityService");

builder.Services.AddHealthChecks()
    .AddUrlGroup(estateReportingUri, name: "Estate Reporting API", tags: new[] { "estateapi" })
    .AddUrlGroup(securityServiceUri, name: "Security Service API", tags: new[] { "securityapi" });

builder.Host.UseWindowsService();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app = testMode switch {
    TestMode.AuthenticationOnly => app.ConfigureTestLogin(),
    TestMode.Full => app.ConfigureTestLogin(),
    _ => app.ConfigureLiveLogin()
};

// Map Health Check endpoints
// /health - standard JSON health check endpoint
app.MapHealthChecks("/health", new HealthCheckOptions
{
    Predicate = _ => true
});

// /healthui - detailed UI-formatted health check endpoint for monitoring dashboards
app.MapHealthChecks("/healthui", new HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.Run();


enum TestMode {
    Disabled,
    AuthenticationOnly,
    BackedByTestDataStore,
    Full
}