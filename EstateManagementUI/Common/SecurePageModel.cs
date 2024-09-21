using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleResults;

namespace EstateManagementUI.Common;

public class SecurePageModel : PageModel {
    private readonly IPermissionsService PermissionsService;
    private readonly String SectionName;
    private readonly String PageName;

    public SecurePageModel(IPermissionsService PermissionsService, String sectionName, String pageName) {
        this.PermissionsService = PermissionsService;
        this.SectionName = sectionName;
        this.PageName = pageName;
    }
    public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
                                                           PageHandlerExecutionDelegate next)
    {
        Result result = await this.PermissionsService.DoIHavePermissions(this.User.Identity.Name, this.SectionName, this.PageName);

        if (result.IsFailed)
        {
            Shared.Logger.Logger.LogWarning(result.Message);
            context.Result = new ForbidResult();
        }
        else
        {
            Shared.Logger.Logger.LogWarning($"Permission granted for userName: {this.User.Identity.Name} SectionName: {this.SectionName} PageName: {this.PageName}");
        }
        await base.OnPageHandlerExecutionAsync(context, next);
    }
}