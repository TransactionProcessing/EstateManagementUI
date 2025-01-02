using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;

namespace EstateManagementUI.Pages.Dashboard
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class IndexModel : SecurePageModel
    {
        public IndexModel(IPermissionsService permissionsService) : base(permissionsService, 
            ApplicationSections.Dashboard, DashboardFunctions.Dashboard) {
        }
    }
}
