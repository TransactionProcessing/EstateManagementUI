using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.FileProcessing
{
    [ExcludeFromCodeCoverage]
    [Authorize]
    public class FileImportLogModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid FileImportLogId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid MerchantId { get; set; }

        public FileImportLogModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.FileProcessing, FileProcessingFunctions.ViewImportLog)
        {
        }
    }
}
