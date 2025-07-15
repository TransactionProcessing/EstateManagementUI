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
public class EditRole : StandardHydroComponent
{
    private readonly IPermissionsRepository PermissionsRepository;
    public int Id { get; set; }
    public string Name { get; set; }

    public List<(int, string, int, string, bool)> Permissions { get; set; }

    public EditRole(IPermissionsRepository permissionsRepository)
    {
        PermissionsRepository = permissionsRepository;
    }

    public override async Task MountAsync()
    {
        Result<BusinessLogic.PermissionService.Database.Entities.Role> role = await PermissionsRepository.GetRole(Id, CancellationToken.None);

        Result<List<(ApplicationSection, Function, bool)>> permissionsList = await PermissionsRepository.GetRolePermissions(Id, CancellationToken.None);
        List<ApplicationSection> applicationSections = permissionsList.Data.Select(p => p.Item1).Distinct().ToList();

        Permissions = new();
        foreach (ApplicationSection applicationSection in applicationSections)
        {
            List<(Function, bool)> functionAccess = permissionsList.Data.Where(p =>
                p.Item1.ApplicationSectionId == applicationSection.ApplicationSectionId).Select(x => (x.Item2, x.Item3)).ToList();

            foreach ((Function, bool) function in functionAccess)
            {
                Permissions.Add((applicationSection.ApplicationSectionId, applicationSection.Name, function.Item1.FunctionId, function.Item1.Name, function.Item2));
            }
        }
        Name = role.Data.Name;

    }

    public async Task Save()
    {
        if (!Validate())
        {
            return;
        }

        List<(int, int, bool)> newPermissions = Permissions.Select(p => (p.Item1, p.Item3, p.Item5)).ToList();
        Result result = await PermissionsRepository.UpdateRolePermissions(Id, newPermissions, CancellationToken.None);
        if (result.IsSuccess) {
            Back();
            Dispatch(new ShowMessage($"Role {this.Name} has been updated successfully"), Global);
        }
        else {
            Dispatch(new ShowMessage($"Error updating role {this.Name}", ToastType.Error),Global);
        }
    }

    [SkipOutput]
    public void Back() => Location(Url.Page("/PermissionManagement/Roles/Index"));
    
    public async Task Toggle(int applicationSectionId, int functionId)
    {
        //var p = this.Permissions.SingleOrDefault(i =>
        //    i.Item1 == applicationSectionId && i.Item3 == functionId);
        int indexToUpdate = 0;
        for (int i = 0; i < Permissions.Count; i++)
        {
            if (Permissions[i].Item1 == applicationSectionId && Permissions[i].Item3 == functionId)
            {
                indexToUpdate = i;
                break;
            }
        }
        var p = Permissions[indexToUpdate];

        var checkValue = p.Item5 switch
        {
            true => false,
            _ => true
        };

        var updatedItem = (p.Item1, p.Item2, p.Item3, p.Item4, checkValue);

        Permissions[indexToUpdate] = updatedItem;
    }
}
