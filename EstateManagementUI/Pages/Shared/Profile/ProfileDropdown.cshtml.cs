using Hydro;

//using HydroSales.Authorization;
//using HydroSales.Database;
//using HydroSales.Pages.Start;

namespace EstateManagementUI.Pages.Shared.Profile;

//IAuthorizationService authorizationService, IDatabase database, IMembershipService membershipService
public class ProfileDropdown() : HydroComponent
{
    public override async Task RenderAsync()
    {
        //var user = await database.GetCurrentUser();
        //ViewBag.IsAuthenticated = user != null;
    }

    public void SwitchTheme()
    {
        var theme = this.HttpContext.Request.Cookies.TryGetValue("theme", out var value) ? value : "emerald";
        var newTheme = theme == "emerald" ? "dark" : "emerald";
        this.HttpContext.Response.Cookies.Append("theme", newTheme, new CookieOptions { MaxAge = TimeSpan.FromDays(365) });
        this.Redirect(this.HttpContext.Request.Headers.Referer.ToString());
    }

    public async Task SignUp()
    {
        //await membershipService.SignUp();
        //Redirect(Url.Page("/Invoices/Index"));
    }

    public async Task Logout()
    {
        //await authorizationService.SignOut();
        Redirect("/");
    }
}
