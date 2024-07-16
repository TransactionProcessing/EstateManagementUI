using Hydro;
using Microsoft.AspNetCore.Authentication;

//using HydroSales.Authorization;
//using HydroSales.Database;
//using HydroSales.Pages.Start;

namespace EstateManagementUI.Pages.Shared.Profile;

//IAuthorizationService authorizationService, IDatabase database, IMembershipService membershipService
public class ProfileDropdown : HydroComponent
{
    private readonly IHttpContextAccessor HttpContextAccessor;

    public ProfileDropdown(IHttpContextAccessor httpContextAccessor) {
        this.HttpContextAccessor = httpContextAccessor;
    }

    public async Task SignOut() {
        await this.HttpContextAccessor.HttpContext.SignOutAsync("oidc");
        await this.HttpContextAccessor.HttpContext.SignOutAsync("Cookies");
    }
}
