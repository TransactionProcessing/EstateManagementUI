using EstateManagementUI.BlazorServer.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Authentication;

public sealed class OidcUserAccessTokenProviderTests
{
    [Fact]
    public async Task GetAccessTokenAsync_ReturnsAccessTokenFromAuthenticatedUserSession()
    {
        var httpContext = new DefaultHttpContext();
        var authenticationProperties = new AuthenticationProperties();
        authenticationProperties.StoreTokens(new[]
        {
            new AuthenticationToken { Name = "access_token", Value = "user-access-token" }
        });
        var authenticationService = new StubAuthenticationService(
            AuthenticateResult.Success(new AuthenticationTicket(
                new System.Security.Claims.ClaimsPrincipal(new System.Security.Claims.ClaimsIdentity("Cookies")),
                authenticationProperties,
                CookieAuthenticationDefaults.AuthenticationScheme)));
        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton<IAuthenticationService>(authenticationService)
            .BuildServiceProvider();
        var provider = new OidcUserAccessTokenProvider(
            new HttpContextAccessor { HttpContext = httpContext });

        var result = await provider.GetAccessTokenAsync(CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldBe("user-access-token");
    }

    [Fact]
    public async Task GetAccessTokenAsync_WithoutAuthenticatedSession_ReturnsFailure()
    {
        var httpContext = new DefaultHttpContext
        {
            RequestServices = new ServiceCollection()
                .AddSingleton<IAuthenticationService>(new StubAuthenticationService(AuthenticateResult.NoResult()))
                .BuildServiceProvider()
        };
        var provider = new OidcUserAccessTokenProvider(
            new HttpContextAccessor { HttpContext = httpContext });

        var result = await provider.GetAccessTokenAsync(CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    private sealed class StubAuthenticationService : IAuthenticationService
    {
        public AuthenticateResult Result { get; }

        public StubAuthenticationService(AuthenticateResult result)
        {
            this.Result = result;
        }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
            => Task.FromResult(this.Result);

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task SignInAsync(HttpContext context, string? scheme, System.Security.Claims.ClaimsPrincipal principal, AuthenticationProperties? properties)
            => Task.CompletedTask;

        public Task SignOutAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
            => Task.CompletedTask;
    }
}
