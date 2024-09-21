using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Contract
{
    [Authorize]
    public class ContractProductTransactionFees : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid ContractId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid ContractProductId { get; set; }

        public ContractProductTransactionFees(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Contract, ContractFunctions.ViewProductFeesList)
        {
        }
    }
}
