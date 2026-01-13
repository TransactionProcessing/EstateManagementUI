using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Common;
using Hydro;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.PermissionManagement.Roles.RoleList
{
    [ExcludeFromCodeCoverage]
    public class RoleList : StandardHydroComponent
    {
        private readonly IPermissionsRepository PermissionsRepository;

        public RoleList(IPermissionsRepository permissionsRepository)
        {
            PermissionsRepository = permissionsRepository;
            Roles = new List<Role>();
        }

        public List<Role> Roles { get; set; }

        public override async Task MountAsync()
        {
            Result<List<BusinessLogic.PermissionService.Database.Entities.Role>> allRoles = await PermissionsRepository.GetRoles(CancellationToken.None);

            if (allRoles.Status == ResultStatus.NotFound)
            {
                // No roles
                Roles = new List<Role>();
                return;
            }

            if (allRoles.IsFailed)
                // TODO: Some error 
                return;

            foreach (BusinessLogic.PermissionService.Database.Entities.Role role in allRoles.Data)
            {
                Roles.Add(new Role
                {
                    Name = role.Name,
                    Id = role.RoleId
                });
            }
        }

        [SkipOutput]
        public async Task Edit(int id) =>
            Location(Url.Page("/PermissionManagement/Roles/Edit", new { id }));

        [SkipOutput]
        public async Task EditUsers(int id) =>
            Location(Url.Page("/PermissionManagement/Roles/EditUsers", new { id }));

        [SkipOutput]
        public void Add() => Location(Url.Page("/PermissionManagement/Roles/Add"));

        [SkipOutput]
        public void Back()
        {
            Location(Url.Page("/PermissionManagement/Home"));
        }
    }

    [ExcludeFromCodeCoverage]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
