using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace EstateManagementUI.Pages.Merchant
{
    [Authorize]
    public class NewMerchantModel : SecurePageModel
    {
        public NewMerchantModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Merchant, MerchantFunctions.ViewList)
        {
        }
    }
}
