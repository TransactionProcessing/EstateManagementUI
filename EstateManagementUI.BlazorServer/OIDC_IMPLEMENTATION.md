# OIDC Authentication Implementation

## Overview

This document describes the OpenID Connect (OIDC) authentication implementation for the EstateManagementUI.BlazorServer application. The implementation is based on the configuration from the original EstateManagementUI project and includes automatic token management with refresh capabilities.

## What Was Implemented

### 1. Token Management Infrastructure

Added a complete token management system under `TokenManagement/` folder with the following components:

#### AutomaticTokenManagementOptions.cs
- Configuration options for token management
- Configurable token refresh timing (default: 30 seconds before expiration)
- Option to revoke refresh tokens on sign-out

#### TokenEndpointService.cs
- Handles communication with the OIDC token endpoint
- Manages token refresh operations
- Handles token revocation during logout

#### AutomaticTokenManagementCookieEvents.cs
- Hooks into cookie authentication events
- Automatically refreshes access tokens when they're about to expire
- Validates tokens on each request
- Manages concurrent refresh requests to prevent race conditions
- Revokes refresh tokens on sign-out

#### AutomaticTokenManagementConfigureCookieOptions.cs
- Configures cookie authentication to use automatic token management events

#### AutomaticTokenManagementBuilderExtensions.cs
- Extension methods to easily add token management to the authentication pipeline
- Registers all required services

### 2. Program.cs Configuration

Updated the application startup to include:

#### Authentication Setup
```csharp
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
    // Configuration from appsettings.json
    options.Authority = builder.Configuration["Authentication:Authority"];
    options.ClientId = builder.Configuration["Authentication:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:ClientSecret"];
    options.ResponseType = "code id_token";
    options.SaveTokens = true;
    options.GetClaimsFromUserInfoEndpoint = true;
    
    // Scopes
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("offline_access");      // For refresh tokens
    options.Scope.Add("fileProcessor");        // Custom API scope
    options.Scope.Add("transactionProcessor"); // Custom API scope
});
```

#### Authentication Endpoints
- **Login Endpoint**: `/login` - Triggers OIDC authentication flow
- **Logout Endpoint**: `/logout` - Signs out from both OIDC and local cookies

### 3. Configuration (appsettings.json)

```json
{
  "Authentication": {
    "Authority": "https://localhost:5001",
    "ClientId": "estateUIClient",
    "ClientSecret": "Secret1",
    "CallbackPath": "/signin-oidc",
    "ResponseType": "code id_token",
    "Scopes": [
      "openid",
      "profile",
      "email",
      "offline_access",
      "fileProcessor",
      "transactionProcessor"
    ]
  }
}
```

## Key Features

### 1. Automatic Token Refresh
- Tokens are automatically refreshed 30 seconds before expiration
- Refresh happens transparently during cookie validation
- Prevents authentication errors due to expired tokens

### 2. Hybrid Flow
- Uses `code id_token` response type
- Provides both front-channel and back-channel security
- Suitable for server-side Blazor applications

### 3. Custom Scopes
- Includes standard OIDC scopes (openid, profile, email)
- Adds `offline_access` for refresh token support
- Includes custom API scopes (fileProcessor, transactionProcessor)

### 4. Claims Mapping
- Clears default JWT claim type mappings
- Uses standard claim types (name, role)
- Disables audience validation for flexibility

### 5. Token Storage
- Tokens are saved in authentication cookies
- Includes access token, refresh token, and expiration time
- Automatically updated when tokens are refreshed

## How to Test

### Prerequisites
1. OIDC Provider (Identity Server, Auth0, Okta, etc.) running at the configured Authority URL
2. Client registered in the OIDC provider with:
   - Client ID: `estateUIClient`
   - Client Secret: `Secret1`
   - Allowed callback URL: `https://localhost:[port]/signin-oidc`
   - All required scopes enabled

### Testing Login Flow
1. Navigate to `https://localhost:[port]/`
2. Click on a login button or navigate to `/login`
3. You will be redirected to the OIDC provider login page
4. Enter valid credentials
5. After successful authentication, you'll be redirected back to the application
6. User will be authenticated and can access protected resources

### Testing Logout Flow
1. While authenticated, navigate to `/logout` or click a logout button
2. The application will:
   - Sign out from the OIDC provider
   - Clear local authentication cookies
   - Revoke the refresh token (if configured)
   - Redirect to the home page

### Testing Token Refresh
Token refresh happens automatically, but you can verify it by:
1. Login to the application
2. Check the browser's cookies or authentication state
3. Wait for the access token to approach expiration (based on your OIDC provider's token lifetime)
4. Continue using the application - the token should refresh automatically
5. Check application logs for token refresh messages

## Configuration Options

### Changing the Authority URL
Update `appsettings.json`:
```json
"Authentication": {
  "Authority": "https://your-identity-server.com"
}
```

### Adjusting Token Refresh Timing
In `Program.cs`, modify:
```csharp
.AddAutomaticTokenManagement(o => {
    o.RefreshBeforeExpiration = TimeSpan.FromMinutes(2); // Refresh 2 minutes before expiration
})
```

### Disabling Token Revocation on Logout
In `Program.cs`, modify:
```csharp
.AddAutomaticTokenManagement(o => {
    o.RevokeRefreshTokenOnSignout = false;
})
```

### Adding/Removing Scopes
In `Program.cs`, modify the OpenIdConnect configuration:
```csharp
options.Scope.Add("your-custom-scope");
// or
options.Scope.Remove("fileProcessor");
```

## Security Considerations

### Production Settings
For production deployment, ensure:

1. **Enable HTTPS Metadata Validation**
   ```csharp
   options.RequireHttpsMetadata = true;
   ```

2. **Use Secure Client Secrets**
   - Store secrets in Azure Key Vault, AWS Secrets Manager, or similar
   - Never commit secrets to source control
   - Use environment variables or configuration providers

3. **Configure Proper Callback URLs**
   - Register only specific, known callback URLs with your OIDC provider
   - Use HTTPS for all callback URLs

4. **Cookie Security**
   - Ensure cookies are set with secure flags
   - Configure appropriate SameSite settings

5. **Token Lifetime**
   - Configure appropriate token lifetimes in your OIDC provider
   - Balance security with user experience

## Troubleshooting

### Common Issues

1. **"The authority is invalid" Error**
   - Verify the Authority URL is correct
   - Check that the OIDC provider is accessible
   - Ensure `.well-known/openid-configuration` endpoint is available

2. **"Invalid redirect_uri" Error**
   - Check that the callback URL is registered in the OIDC provider
   - Verify the port number matches
   - Ensure the URL format is correct (https://localhost:port/signin-oidc)

3. **Token Refresh Not Working**
   - Verify `offline_access` scope is requested and granted
   - Check that `SaveTokens = true` is set
   - Ensure the refresh token is present in cookies

4. **Logout Not Working**
   - Verify the OIDC provider supports logout
   - Check that the end_session_endpoint is available
   - Ensure proper sign-out sequence (OIDC first, then Cookies)

## References

- [Microsoft Identity - OpenID Connect](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/openidconnect)
- [IdentityModel Documentation](https://identitymodel.readthedocs.io/)
- [OpenID Connect Specification](https://openid.net/specs/openid-connect-core-1_0.html)

## Testing Checklist

- [ ] Login flow redirects to OIDC provider
- [ ] Successful authentication redirects back to application
- [ ] User claims are properly populated
- [ ] Protected pages require authentication
- [ ] Logout clears authentication state
- [ ] Tokens are automatically refreshed before expiration
- [ ] Refresh token is revoked on logout
- [ ] All configured scopes are granted
- [ ] Application works correctly in production mode

## Notes

- This implementation uses the **Hybrid Flow** (code id_token) which is recommended for server-side applications
- Token refresh is handled automatically by the `AutomaticTokenManagementCookieEvents` class
- The implementation is thread-safe and prevents concurrent refresh requests
- All TokenManagement classes are marked with `[ExcludeFromCodeCoverage]` as they're infrastructure code
