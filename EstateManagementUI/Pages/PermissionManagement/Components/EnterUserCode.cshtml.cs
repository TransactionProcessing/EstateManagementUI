using Hydro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.Logger;

namespace EstateManagementUI.Pages.PermissionManagement.Components
{
    public class EnterUserCode : HydroComponent
    {
        public string UserCode { get; set; }

        public EnterUserCode()
        {
        }

        public void Submit()
        {
            // TODO: Validate the entered code
            //Redirect("/PermissionManagement/Home");
            Location(Url.Page("/PermissionManagement/Home"));
        }
    }
}
