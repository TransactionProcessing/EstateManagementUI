using EstateManagementUI.BusinessLogic.Client;
using Microsoft.AspNetCore.Components.Authorization;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Authentication;

public sealed class OidcUserAccessTokenProvider : IUserAccessTokenProvider
{
    public static string AccessTokenClaimType => "estate_management_access_token";

    private readonly AuthenticationStateProvider AuthenticationStateProvider;

    public OidcUserAccessTokenProvider(AuthenticationStateProvider authenticationStateProvider)
    {
        this.AuthenticationStateProvider = authenticationStateProvider;
    }

    public async Task<Result<string>> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var authenticationState = await this.AuthenticationStateProvider.GetAuthenticationStateAsync();
        if (authenticationState.User.Identity?.IsAuthenticated != true)
            return Result.Failure("The current user is not authenticated.");

        var accessToken = authenticationState.User.FindFirst(AccessTokenClaimType)?.Value;
        if (String.IsNullOrWhiteSpace(accessToken))
            return Result.Failure("The current user authentication state does not contain an access token.");

        return Result.Success<string>(accessToken);
    }
}
