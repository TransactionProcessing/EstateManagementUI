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
public class EditRoleUsers : StandardHydroComponent
{
    private readonly IPermissionsRepository PermissionsRepository;
    public int Id { get; set; }
    public string Name { get; set; }
    public List<String> UsersList { get; set; }

    public EditRoleUsers(IPermissionsRepository permissionsRepository)
    {
        PermissionsRepository = permissionsRepository;
    }

    public override async Task MountAsync()
    {
        Result<BusinessLogic.PermissionService.Database.Entities.Role> role = await PermissionsRepository.GetRole(Id, CancellationToken.None);
        Result<List<UserRole>> userRoles = await this.PermissionsRepository.GetRoleUsers(this.Id, CancellationToken.None);        //Result<List<(ApplicationSection, Function, bool)>> permissionsList = await PermissionsRepository.GetRolePermissions(Id, CancellationToken.None);
        
        Name = role.Data.Name;
        this.UsersList = userRoles.Data.Select(ur => ur.UserName).ToList();

    }

    public async Task Save()
    {
        //if (!Validate())
        //{
        //    return;
        //}

        //List<(int, int, bool)> newPermissions = Permissions.Select(p => (p.Item1, p.Item3, p.Item5)).ToList();
        //Result result = await PermissionsRepository.UpdateRolePermissions(Id, newPermissions, CancellationToken.None);
        //if (result.IsSuccess) {
        //    Back();
        //    Dispatch(new ShowMessage($"Role {this.Name} has been updated successfully"), Global);
        //}
        //else {
        //    Dispatch(new ShowMessage($"Error updating role {this.Name}", ToastType.Error),Global);
        //}
    }

    [SkipOutput]
    public void Back() => Location(Url.Page("/PermissionManagement/Roles/Index"));

    [SkipOutput]
    public void Add(Int32 id) => Location(Url.Page("/PermissionManagement/Roles/AddUser", new { id }));

    [SkipOutput]
    public async Task Remove(Int32 id, String userName) {
        var result = await this.PermissionsRepository.RemoveUserFromRole(id, userName, CancellationToken.None);
        if (result.IsSuccess) {
            Back();
            Dispatch(new ShowMessage($"User {userName} removed from Role {this.Name} successfully"), Global);
        }
        else {
            Dispatch(new ShowMessage($"Error removing User {userName} removed from Role {this.Name}"), Global);
        }
    }

    //public async Task Toggle(int applicationSectionId, int functionId)
    //{
    //    //var p = this.Permissions.SingleOrDefault(i =>
    //    //    i.Item1 == applicationSectionId && i.Item3 == functionId);
    //    int indexToUpdate = 0;
    //    for (int i = 0; i < Permissions.Count; i++)
    //    {
    //        if (Permissions[i].Item1 == applicationSectionId && Permissions[i].Item3 == functionId)
    //        {
    //            indexToUpdate = i;
    //            break;
    //        }
    //    }
    //    var p = Permissions[indexToUpdate];

    //    var checkValue = p.Item5 switch
    //    {
    //        true => false,
    //        _ => true
    //    };

    //    var updatedItem = (p.Item1, p.Item2, p.Item3, p.Item4, checkValue);

    //    Permissions[indexToUpdate] = updatedItem;
    //}
}
