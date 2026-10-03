using EstateManagementUI.BlazorServer.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Authentication;

public sealed class OidcUserAccessTokenProviderTests
{
    [Fact]
    public async Task GetAccessTokenAsync_ReturnsAccessTokenFromAuthenticatedUserSession()
    {
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(OidcUserAccessTokenProvider.AccessTokenClaimType, "user-access-token")
        }, "TestAuthentication"));
        var provider = new OidcUserAccessTokenProvider(
            new StubAuthenticationStateProvider(principal));

        var result = await provider.GetAccessTokenAsync(CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.Data.ShouldBe("user-access-token");
    }

    [Fact]
    public async Task GetAccessTokenAsync_WithoutAuthenticatedSession_ReturnsFailure()
    {
        var provider = new OidcUserAccessTokenProvider(
            new StubAuthenticationStateProvider(new ClaimsPrincipal(new ClaimsIdentity())));

        var result = await provider.GetAccessTokenAsync(CancellationToken.None);

        result.IsFailed.ShouldBeTrue();
    }

    private sealed class StubAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal Principal;

        public StubAuthenticationStateProvider(ClaimsPrincipal principal)
        {
            this.Principal = principal;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
            => Task.FromResult(new AuthenticationState(this.Principal));
    }
}
