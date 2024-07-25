using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Logger;

namespace EstateManagementUI.Pages.Estate
{
    public class IndexModel : PageModel
    {
        public IndexModel() {
            Logger.LogWarning("In index page - Estate");
        }
        public void OnGet()
        {
        }
    }
}
