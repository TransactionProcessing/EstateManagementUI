using Hydro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Logger;

namespace EstateManagementUI.Pages.PermissionManagement.Components
{
    public class OptionSelect : HydroComponent
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
