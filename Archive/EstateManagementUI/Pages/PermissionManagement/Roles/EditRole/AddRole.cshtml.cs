using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using EstateManagementUI.Common;
using EstateManagementUI.Pages.Shared.Components;
using Hydro;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using static Hydro.Scope;

namespace EstateManagementUI.Pages.PermissionManagement.Roles.EditRole;

[ExcludeFromCodeCoverage]
public class AddRole : StandardHydroComponent
{
    private readonly IPermissionsRepository PermissionsRepository;
    
    public String RoleName { get; set; }

    public AddRole(IPermissionsRepository permissionsRepository)
    {
        PermissionsRepository = permissionsRepository;
    }
    
    public async Task Save()
    {
        if (!Validate())
        {
            return;
        }

        var result = await this.PermissionsRepository.AddRole(this.RoleName, CancellationToken.None);

        if (result.IsSuccess) {
            Back();
            Dispatch(new ShowMessage($"Role {this.RoleName} added successfully"), Global);
        }
        else {
            Dispatch(new ShowMessage($"Error adding role {this.RoleName}", ToastType.Error),Global);
        }
    }

    [SkipOutput]
    public void Back()
    {
        Location(Url.Page("/PermissionManagement/Roles/Index"));
    }
}
