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
public class AddRoleUser : StandardHydroComponent
{
    private readonly IPermissionsRepository PermissionsRepository;
    public int Id { get; set; }
    public string Name { get; set; }
    public String UserName { get; set; }

    public AddRoleUser(IPermissionsRepository permissionsRepository)
    {
        PermissionsRepository = permissionsRepository;
    }

    public override async Task MountAsync()
    {
        Result<BusinessLogic.PermissionService.Database.Entities.Role> role = await PermissionsRepository.GetRole(Id, CancellationToken.None);
        Name = role.Data.Name;
    }

    public async Task Save()
    {
        if (!Validate())
        {
            return;
        }

        Result result = await this.PermissionsRepository.AddUserToRole(this.Id, this.UserName, CancellationToken.None);

        if (result.IsSuccess) {
            Back();
            Dispatch(new ShowMessage($"User {this.UserName} added to Role {this.Name} successfully"), Global);
        }
        else {
            Dispatch(new ShowMessage($"Error adding user {this.UserName} to role {this.Name}", ToastType.Error),Global);
        }
    }

    [SkipOutput]
    public void Back()
    {
        Location(Url.Page("/PermissionManagement/Roles/EditUsers", new { id = this.Id}));
    }
}
