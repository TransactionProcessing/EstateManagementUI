using EstateManagementUI.BlazorServer.Components;
using EstateManagementUI.BlazorServer.Services;
using EstateManagementUI.BlazorServer.TokenManagement;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Load hosting.json configuration for port settings
builder.Configuration.AddJsonFile("hosting.json", optional: true, reloadOnChange: true);

// Apply URLs from configuration
var urls = builder.Configuration["urls"];
if (!string.IsNullOrEmpty(urls))
{
    builder.WebHost.UseUrls(urls);
}

// Clear default claims mapping
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add authentication
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
    // Configure OpenID Connect settings from appsettings.json
    options.Authority = builder.Configuration["Authentication:Authority"];
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
});

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Add HTTP context accessor
builder.Services.AddHttpContextAccessor();

// Register stubbed MediatR service
builder.Services.AddSingleton<IMediator, StubbedMediatorService>();

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

// Add login endpoint to trigger OIDC authentication
app.MapGet("/login", (HttpContext context) =>
{
    return Results.Challenge(
        properties: new Microsoft.AspNetCore.Authentication.AuthenticationProperties
        {
            RedirectUri = "/"
        },
        authenticationSchemes: new[] { OpenIdConnectDefaults.AuthenticationScheme }
    );
}).AllowAnonymous();

// Add logout endpoint
app.MapGet("/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/");
}).RequireAuthorization();

app.Run();
