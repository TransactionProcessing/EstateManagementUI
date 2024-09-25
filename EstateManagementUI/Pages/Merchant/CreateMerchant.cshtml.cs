using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Merchant
{
    [Authorize]
    public class CreateMerchantModel : SecurePageModel
    {
        public CreateMerchantModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Merchant, MerchantFunctions.ViewList)
        {
        }
    }
}
