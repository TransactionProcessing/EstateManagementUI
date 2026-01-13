using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Contract
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class NewContractProductTransactionFeeModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid ContractId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid ProductId { get; set; }

        public NewContractProductTransactionFeeModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Contract, ContractFunctions.AddTransactionFee)
        {
        }
    }
}
