using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Contract
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class IndexModel : SecurePageModel
    {
        public IndexModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Contract, ContractFunctions.ViewList)
        {
        }
    }
}
