using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Reporting
{
    [Authorize]
    public class TransactionAnalysisModel : SecurePageModel
    {
        public TransactionAnalysisModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Reporting, ReportingFunctions.TransactionAnalysis)
        {
        }
    }
}
