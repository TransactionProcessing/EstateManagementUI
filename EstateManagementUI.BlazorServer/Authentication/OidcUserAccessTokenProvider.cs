using EstateManagementUI.BusinessLogic.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Authentication;

public sealed class OidcUserAccessTokenProvider : IUserAccessTokenProvider
{
    private readonly IHttpContextAccessor HttpContextAccessor;

    public OidcUserAccessTokenProvider(IHttpContextAccessor httpContextAccessor)
    {
        this.HttpContextAccessor = httpContextAccessor;
    }

    public async Task<Result<string>> GetAccessTokenAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var httpContext = this.HttpContextAccessor.HttpContext;
        if (httpContext is null)
            return Result.Failure("No active HTTP context is available.");

        var authenticationService = httpContext.RequestServices.GetRequiredService<IAuthenticationService>();
        var authenticationResult = await authenticationService.AuthenticateAsync(
            httpContext,
            CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authenticationResult.Succeeded || authenticationResult.Properties is null)
            return Result.Failure("The current user is not authenticated.");

        var accessToken = authenticationResult.Properties.GetTokens()
            .SingleOrDefault(token => token.Name == "access_token")?.Value;
        if (String.IsNullOrWhiteSpace(accessToken))
            return Result.Failure("The current user session does not contain an access token.");

        return Result.Success<string>(accessToken);
    }
}
