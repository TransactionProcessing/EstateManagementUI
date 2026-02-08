using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Components;
using System.IdentityModel.Tokens.Jwt;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args).LoadConfiguration().ConfigureKestrel();

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

builder = testMode switch {
    TestMode.BackedByTestDataStore => builder.RegisterTestMediator(),
    TestMode.Full => builder.RegisterTestMediator(),
    _ => builder.RegisterProductionMeriator().RegisterClients().RegisterUIServices()
};

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

app.Run();


enum TestMode {
    Disabled,
    AuthenticationOnly,
    BackedByTestDataStore,
    Full
}