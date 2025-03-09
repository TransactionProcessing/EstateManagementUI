using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace EstateManagementUI.Pages.Merchant
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class MakeDepositModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid MerchantId { get; set; }

        public MakeDepositModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Merchant, MerchantFunctions.MakeDeposit)
        {
        }
    }
}
