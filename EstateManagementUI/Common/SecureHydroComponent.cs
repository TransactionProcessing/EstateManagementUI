using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.Pages.Operator;
using Hydro;
using Microsoft.AspNetCore.Authentication;
using SimpleResults;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.PermissionService;
using Hydro.Utils;

namespace EstateManagementUI.Common;

public class SecureHydroComponent : HydroComponent {

    private readonly String SectionName;
    private readonly String PageName;
    private readonly IPermissionsService PermissionsService;

    public SecureHydroComponent(String sectionName,
                                String pageName,
                                IPermissionsService permissionsService) : base() {
        this.SectionName = sectionName;
        this.PageName = pageName;
        this.PermissionsService = permissionsService;
        this.SectionName = sectionName;
        this.PageName = pageName;
    }

    protected String AccessToken;
    protected Guid EstateId;

    protected async Task PopulateTokenAndEstateId() {
        if (this.HttpContext != null)
            this.AccessToken = await this.HttpContext.GetTokenAsync("access_token");

        if (this.User != null)
            this.EstateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
    }

    public override async Task RenderAsync()
    {
        String userName = this.User.Identity.Name;
        Result permissionsResult = await this.PermissionsService.DoIHavePermissions(userName, this.SectionName, this.PageName);
        if (permissionsResult.IsFailed)
        {
            // TODO: might make this configurable
            this.HttpContext.Response.Redirect("/Error");
            return;
        }
        
        await base.RenderAsync();
    }
}