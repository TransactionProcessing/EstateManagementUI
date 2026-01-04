namespace EstateManagementUI.BlazorServer.TokenManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Options for automatic token management
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AutomaticTokenManagementOptions
    {
        /// <summary>
        /// Gets or sets the refresh before expiration.
        /// </summary>
        public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Gets or sets a value indicating whether to revoke refresh token on signout.
        /// </summary>
        public Boolean RevokeRefreshTokenOnSignout { get; set; } = true;

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        public String Scheme { get; set; }
    }
}
