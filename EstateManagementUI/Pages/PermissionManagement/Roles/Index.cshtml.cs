using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.General;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.PermissionManagement.Roles {
    [ExcludeFromCodeCoverage]
    public class IndexModel : PermissionPageModel {
    }
    [ExcludeFromCodeCoverage]
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
