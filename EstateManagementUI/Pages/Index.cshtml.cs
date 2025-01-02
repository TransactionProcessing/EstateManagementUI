using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.General;
using Shared.Logger;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages
{
    [ExcludeFromCodeCoverage]
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
