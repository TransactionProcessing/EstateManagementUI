using System.ComponentModel;
using System.Security.Claims;

namespace EstateManagementUI.Common
{
    public static class Helpers {
        public const String EstateIdClaimType = "estateId";

        public static T GetClaimValue<T>(ClaimsIdentity claimsIdentity,
                                         String claimType) {
            if (!claimsIdentity.HasClaim(x => x.Type.ToLower() == claimType.ToLower())) {
                throw new InvalidOperationException($"User {claimsIdentity.Name} does not have Claim [{claimType}]");
            }

            Claim claim = claimsIdentity.Claims.Single(x => x.Type.ToLower() == claimType.ToLower());
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(claim.Value);
        }
    }
}
