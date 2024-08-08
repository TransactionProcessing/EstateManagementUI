using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.PermissionManagement.Roles
{
    public class EditUsers : PermissionPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Int32 Id { get; set; }
    }
}
