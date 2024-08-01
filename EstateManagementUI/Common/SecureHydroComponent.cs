using EstateManagementUI.BusinessLogic.Clients;
using Hydro;
using SimpleResults;

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