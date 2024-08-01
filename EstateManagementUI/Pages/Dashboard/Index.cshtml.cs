using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;

namespace EstateManagementUI.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : SecurePageModel
    {
        public IndexModel(IPermissionsService permissionsService) : base(permissionsService, 
            ApplicationSections.Dashboard, DashboardFunctions.Dashboard) {
        }
    }
}
