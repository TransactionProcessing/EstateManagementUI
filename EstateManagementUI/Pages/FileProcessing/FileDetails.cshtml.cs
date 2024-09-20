using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.FileProcessing
{
    [Authorize]
    public class FileDetailsModel : SecurePageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid FileId { get; set; }

        public FileDetailsModel(IPermissionsService permissionsService) : base(permissionsService,
            ApplicationSections.FileProcessing, FileProcessingFunctions.ViewFileDetails)
        {
        }
    }
}
