using Hydro;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System.Globalization;
namespace EstateManagementUI.Pages.Shared.Profile;

public class ProfileDropdown : HydroComponent {
    private readonly IHttpContextAccessor HttpContextAccessor;
    
    public ProfileDropdown(IHttpContextAccessor httpContextAccessor) {
        this.HttpContextAccessor = httpContextAccessor;

        var givenName = HttpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "given_name")?.Value;
        var familyName = HttpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "family_name")?.Value;
        UserFullName = $"{givenName} {familyName}";

        var registrationDateClaim = HttpContextAccessor.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "registration_date");
        if (registrationDateClaim != null)
        {
            if (DateTime.TryParseExact(registrationDateClaim.Value, "yyyy-MM-dd HH:mm:ss.fff", null, DateTimeStyles.None, out DateTime registrationDate))
            {
                this.RegistrationText = $"Registered on {registrationDate.ToShortDateString()}";
            }
        }
        else
        {
            this.RegistrationText = $"Registered on Unknown";
        }
    }

    public async Task SignOut() {
        await this.HttpContextAccessor.HttpContext.SignOutAsync("oidc");
        await this.HttpContextAccessor.HttpContext.SignOutAsync("Cookies");
    }

    public string UserFullName { get; private set; }
    public string RegistrationText { get; private set; }
}
