using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Merchant
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class EditMerchantModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid MerchantId { get; set; }

        public EditMerchantModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Merchant, MerchantFunctions.Edit)
        {
        }
    }
}
