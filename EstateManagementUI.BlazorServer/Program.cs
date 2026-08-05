using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Components;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using EstateManagementUI.BlazorServer.Testing;
using Sentry.Extensibility;
using Shared.Extensions;
using Shared.General;
using Shared.Serialisation;
using Spectre.Console;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Components.Server;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args).LoadConfiguration().ConfigureKestrel();

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
            o.CaptureBlockingCalls = ConfigurationReader.GetValueOrDefault("SentryConfiguration", "CaptureBlockingCalls", false);
            o.IncludeActivityData = ConfigurationReader.GetValueOrDefault("SentryConfiguration", "IncludeActivityData", false);
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

var dataProtectionBuilder = builder.Services.AddDataProtection()
    .SetApplicationName("EstateManagementUI");
if (testMode != TestMode.Disabled)
{
    dataProtectionBuilder.PersistKeysToFileSystem(
        new DirectoryInfo(Path.Combine(Path.GetTempPath(), "EstateManagementUI-DataProtection")));

    builder.Logging.ClearProviders();
    builder.Logging.AddConsole();
    builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);
}

builder = testMode switch {
    TestMode.AuthenticationOnly => builder.ConfigureTestAuthentication(),
    TestMode.Full => builder.ConfigureTestAuthentication(),
    TestMode.BackedByTestDataStore => builder.ConfigureTestAuthentication(),
    _ => builder.ConfigureLiveAuthentication()
};

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Register Permission services
builder = builder.RegisterPermissionServices();

if (testMode == TestMode.BackedByTestDataStore)
{
    builder = builder.RegisterTestModeServices();
}
else
{
    builder.RegisterProductionMeriator().RegisterClients();
}

builder.RegisterUIServices().RegisterSerialiser();

// Add Health Checks - read URLs from configuration
var estateReportingApiUrl = builder.Configuration.GetValue<string>("AppSettings:EstateReportingApi") ?? "http://localhost:5011";

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

var estateReportingUri = ValidateAndCreateUri($"{estateReportingApiUrl}/health", "AppSettings:EstateReportingApi");

builder.Services.AddHealthChecks().AddSecurityService().AddUrlGroup(estateReportingUri, name: "Estate Reporting API", tags: new[] { "estateapi" });

if (testMode == TestMode.Disabled)
{
    builder.Host.UseWindowsService();
}

WebApplication app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serialiser = scope.ServiceProvider.GetRequiredService<IStringSerialiser>();
    StringSerialiser.Initialise(serialiser);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
if (testMode == TestMode.Disabled)
{
    app.UseHttpsRedirection();
}

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
    TestMode.BackedByTestDataStore => app.ConfigureTestLogin(),
    _ => app.ConfigureLiveLogin()
};

if (testMode == TestMode.BackedByTestDataStore)
{
    app.MapTestSupportEndpoints();
}

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
