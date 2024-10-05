using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagementUI.Pages.Common
{
    public static class Helpers
    {
        public static Decimal SafeDivision(this Decimal numerator, Decimal denominator)
        {
            return (denominator == 0) ? 0 : numerator / denominator;
        }

        public const string EstateIdClaimType = "estateId";

        public static T GetClaimValue<T>(ClaimsIdentity claimsIdentity,
                                         string claimType)
        {
            if (!claimsIdentity.HasClaim(x => x.Type.ToLower() == claimType.ToLower()))
            {
                throw new InvalidOperationException($"User {claimsIdentity.Name} does not have Claim [{claimType}]");
            }

            Claim claim = claimsIdentity.Claims.Single(x => x.Type.ToLower() == claimType.ToLower());
            return (T)TypeDescriptor.GetConverter(typeof(T)).ConvertFromInvariantString(claim.Value);
        }

        public static async Task<string> RenderKpiCardClass(decimal variance, bool lessIsGood)
        {
            string className = null;
            if (lessIsGood)
            {
                className = variance switch
                {
                    var n when n < 0 => "info-box bg-success",
                    var n when n == 0 => "info-box bg-info",
                    var n when n is > 0 and < 0.2m => "info-box bg-warning",
                    _ => "info-box bg-danger"
                };
            }
            else
            {
                className = variance switch
                {
                    var n when n > 0 => "info-box bg-success",
                    var n when n == 0 => "info-box bg-info",
                    var n when n is < 0 and >= -0.2m => "info-box bg-warning",
                    _ => "info-box bg-danger"
                };
            }

            return className;
        }
    }
}
