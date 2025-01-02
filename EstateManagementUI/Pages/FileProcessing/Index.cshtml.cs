using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.FileProcessing
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class IndexModel : SecurePageModel
    {
        public IndexModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.FileProcessing, FileProcessingFunctions.ViewImportLogList)
        {
        }
    }
}
