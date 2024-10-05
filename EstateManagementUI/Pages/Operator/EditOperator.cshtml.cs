using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace EstateManagementUI.Pages.Operator
{
    [Authorize]
    public class EditOperatorModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid OperatorId { get; set; }

        public EditOperatorModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Operator, OperatorFunctions.Edit)
        {
        }
    }
}
