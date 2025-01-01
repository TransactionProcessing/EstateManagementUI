using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.PermissionManagement.Roles
{
    [ExcludeFromCodeCoverage]
    public class EditUsers : PermissionPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Int32 Id { get; set; }
    }
}
