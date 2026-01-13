namespace EstateAdministrationUI.TokenManagement
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// 
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AutomaticTokenManagementOptions {
        #region Properties

        /// <summary>
        /// Gets or sets the refresh before expiration.
        /// </summary>
        /// <value>
        /// The refresh before expiration.
        /// </value>
        public TimeSpan RefreshBeforeExpiration { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        /// Gets or sets a value indicating whether [revoke refresh token on signout].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [revoke refresh token on signout]; otherwise, <c>false</c>.
        /// </value>
        public Boolean RevokeRefreshTokenOnSignout { get; set; } = true;

        /// <summary>
        /// Gets or sets the scheme.
        /// </summary>
        /// <value>
        /// The scheme.
        /// </value>
        public String Scheme { get; set; }

        #endregion
    }
}