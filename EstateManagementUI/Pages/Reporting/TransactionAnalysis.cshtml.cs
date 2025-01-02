using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Reporting
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class TransactionAnalysisModel : SecurePageModel
    {
        public TransactionAnalysisModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.Reporting, ReportingFunctions.TransactionAnalysis)
        {
        }
    }
}
