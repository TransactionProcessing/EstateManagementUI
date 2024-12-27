using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace EstateManagementUI.Pages.Merchant
{
    [Authorize]
    public class ViewMerchantModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid MerchantId { get; set; }

        public ViewMerchantModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Merchant, MerchantFunctions.View)
        {
        }
    }
}
