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
    public class ViewContractModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid ContractId { get; set; }

        public ViewContractModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Contract, ContractFunctions.View)
        {
        }
    }
}
