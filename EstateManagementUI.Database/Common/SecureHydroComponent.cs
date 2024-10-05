using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.Pages.Operator;
using Hydro;
using Microsoft.AspNetCore.Authentication;
using SimpleResults;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.PermissionService;
using Hydro.Utils;
using Shared.Middleware;

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

        if (this.User != null) {
            this.EstateId = EstateManagementUI.Pages.Common.Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, 
                EstateManagementUI.Pages.Common.Helpers.EstateIdClaimType);
        }
    }

    public override async Task RenderAsync()
    {
        String userName = this.User.Identity.Name;
        Result permissionsResult = await this.PermissionsService.DoIHavePermissions(userName, this.SectionName, this.PageName);
        if (permissionsResult.IsFailed)
        {
            Shared.Logger.Logger.LogWarning(permissionsResult.Message);
            // TODO: might make this configurable
            this.HttpContext.Response.Redirect("/Error");
            return;
        }
        else {
            Shared.Logger.Logger.LogWarning($"Permission granted for userName: {userName} SectionName: {this.SectionName} PageName: {this.PageName}");
        }
        
        await base.RenderAsync();
    }

    public async Task<Boolean> CanRenderButton(String sectionName, String pageName) {
        String userName = this.User.Identity.Name;
        Result permissionsResult = await this.PermissionsService.DoIHavePermissions(userName, sectionName, pageName);
        return permissionsResult.IsSuccess;
    }
}