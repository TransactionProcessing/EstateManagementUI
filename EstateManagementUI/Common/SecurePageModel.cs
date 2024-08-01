using EstateManagementUI.BusinessLogic.Clients;
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
            context.Result = new ForbidResult();
        }
        await base.OnPageHandlerExecutionAsync(context, next);
    }
}