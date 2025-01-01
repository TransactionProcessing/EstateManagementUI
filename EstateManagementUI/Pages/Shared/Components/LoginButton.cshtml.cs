using Hydro;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Shared.Components
{
    [ExcludeFromCodeCoverage]
    public class LoginButton : HydroComponent
    {
        public async Task LogIn()
        {
            Redirect(Url.Page("/Dashboard/Index"));
        }
    }
}
