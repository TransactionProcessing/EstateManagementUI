using Hydro;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Logger;

namespace EstateManagementUI.Pages.PermissionManagement
{
    public class HomeModel : HydroComponent
    {
        public String UserCode { get; set; }

        public HomeModel() {
        }

        public void Submit1()
        {
            // TODO: Validate the entered code
            
        }

        public void Submit2()
        {
            // TODO: Validate the entered code

        }
    }
}
