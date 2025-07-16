using EstateManagementUI.Common;
using Hydro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Logger;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.PermissionManagement.Components
{
    [ExcludeFromCodeCoverage]
    public class OptionSelect : StandardHydroComponent
    {
        public void Submit1()
        {
            Location(Url.Page("/PermissionManagement/Roles/Index"));

        }

        public void Submit2()
        {
            // TODO: Validate the entered code

        }
    }
}
