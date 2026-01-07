using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace EstateManagementUI.BlazorServer.Common;

/// <summary>
/// Test authentication handler that bypasses OIDC for integration testing
/// Allows setting test user context directly without requiring authentication services
/// </summary>
public class TestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "TestAuthentication";
    private readonly IConfiguration _configuration;

    public TestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Get role from configuration, default to "Estate" if not specified
        var roleName = _configuration.GetValue<string>("AppSettings:TestUserRole", "Estate");
        
        // Create test user claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Email, "testuser@test.com"),
            new Claim("estateId", "11111111-1111-1111-1111-111111111111"),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("role", roleName)
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
