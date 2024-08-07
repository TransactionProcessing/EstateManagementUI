using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.General;

namespace EstateManagementUI.Pages.PermissionManagement.Roles {
    public class IndexModel : PermissionPageModel {
    }

    public class PermissionPageModel : PageModel {
        public IActionResult OnGet()
        {
            String cookieValue = Request.Cookies["PermissionManagementCookie"];
            var expectedCookieValue = ConfigurationReader.GetValue("AppSettings", "PermissionCookieValue");
            if (String.IsNullOrEmpty(cookieValue) || cookieValue != expectedCookieValue)
            {
                return new RedirectToPageResult("/Index");
            }

            return Page();
        }
    }
}
