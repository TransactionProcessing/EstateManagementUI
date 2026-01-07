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

    public TestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Create test user claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user-id"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Email, "testuser@test.com"),
            new Claim("estateId", "11111111-1111-1111-1111-111111111111"),
            new Claim("role", "Estate")
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
