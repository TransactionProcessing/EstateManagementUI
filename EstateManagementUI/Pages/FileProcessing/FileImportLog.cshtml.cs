using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.FileProcessing
{
    public class FileImportLogModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid FileImportLogId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid MerchantId { get; set; }
    }
}
