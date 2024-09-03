using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.FileProcessing
{
    public class FileDetailsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid FileId { get; set; }
    }
}
