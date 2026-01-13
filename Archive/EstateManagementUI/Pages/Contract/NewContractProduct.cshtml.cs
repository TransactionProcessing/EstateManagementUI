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
    public class NewContractProductModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid ContractId { get; set; }

        public NewContractProductModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Contract, ContractFunctions.AddProduct)
        {
        }
    }
}
