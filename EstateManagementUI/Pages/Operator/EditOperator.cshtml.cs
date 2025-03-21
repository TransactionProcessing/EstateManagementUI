using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Operator
{
    [ExcludeFromCodeCoverage]
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
