using Hydro;
using Microsoft.AspNetCore.Mvc;
using System;

namespace EstateManagementUI.Pages.Shared.Components
{
    public class LoginButton : HydroComponent
    {
        public async Task LogIn()
        {
            Redirect(Url.Page("/Dashboard/Index"));
        }
    }
}
