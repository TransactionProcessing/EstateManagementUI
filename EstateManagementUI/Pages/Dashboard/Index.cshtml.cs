using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Dashboard
{
    [Authorize]
    public class IndexModel : PageModel
    {

    }
}
