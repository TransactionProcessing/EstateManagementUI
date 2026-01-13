using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.PermissionManagement.Roles
{
    [ExcludeFromCodeCoverage]
    public class AddUser : PermissionPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Int32 Id { get; set; }
    }
}
