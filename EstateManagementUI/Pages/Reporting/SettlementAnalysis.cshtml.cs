using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Reporting
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class SettlementAnalysisModel : SecurePageModel
    {
        public SettlementAnalysisModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Reporting, ReportingFunctions.SettlementAnalysis)
        {
        }
    }
}
