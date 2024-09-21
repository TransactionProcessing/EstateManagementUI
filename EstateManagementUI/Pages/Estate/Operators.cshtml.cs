using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Estate
{
    [Authorize]
    public class Operators : SecurePageModel
    {
        public Operators(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Estate, EstateFunctions.ViewOperatorsList)
        {
        }
    }
}
