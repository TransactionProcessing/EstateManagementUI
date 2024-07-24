using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.General;
using Shared.Logger;

namespace EstateManagementUI.Pages
{
    public class IndexModel : PageModel
    {
        public IndexModel() {
            Logger.LogWarning("In index page");
        }

        public void OnGet()
        {

        }

        public async Task LogIn() {

        }
    }
}
