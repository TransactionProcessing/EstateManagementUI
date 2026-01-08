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
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TestAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration,
        IHttpContextAccessor httpContextAccessor)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        // Check for role switch in query parameter
        string? roleName = null;
        if (_httpContextAccessor.HttpContext?.Request.Query.TryGetValue("switchRole", out var roleQuery) == true)
        {
            roleName = roleQuery.ToString();
            // Store in session for persistence
            _httpContextAccessor.HttpContext.Session.SetString("TestUserRole", roleName);
        }
        
        // Try to get from session first
        if (string.IsNullOrEmpty(roleName))
        {
            roleName = _httpContextAccessor.HttpContext?.Session.GetString("TestUserRole");
        }
        
        // Fall back to configuration
        if (string.IsNullOrEmpty(roleName))
        {
            roleName = _configuration.GetValue<string>("AppSettings:TestUserRole", "Administrator");
        }
        
        // Get user name based on role
        var userName = GetUserNameForRole(roleName);
        
        // Create test user claims
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, $"test-user-{roleName.ToLower()}"),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, $"{roleName.ToLower()}@test.com"),
            new Claim("estateId", "11111111-1111-1111-1111-111111111111"),
            new Claim(ClaimTypes.Role, roleName),
            new Claim("role", roleName)
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
    
    private string GetUserNameForRole(string roleName)
    {
        return roleName switch
        {
            "Administrator" => "Admin User",
            "Estate" => "Estate Manager",
            "Viewer" => "View Only User",
            _ => "Test User"
        };
    }
}
