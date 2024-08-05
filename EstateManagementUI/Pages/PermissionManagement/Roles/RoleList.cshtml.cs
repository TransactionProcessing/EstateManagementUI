using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.PermissionService;
using Hydro;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;

namespace EstateManagementUI.Pages.PermissionManagement.Roles
{
    public class RoleList : HydroComponent
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

            if (allRoles.Status == ResultStatus.NotFound) {
                // No roles
                this.Roles = new List<Role>();
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

        //[SkipOutput]
        //public void Add() =>
        //Location(Url.Page("/Customers/Add"));

        [SkipOutput]
        public void Edit(Int32 id) =>
            Location(Url.Page("/PermissionManagement/Roles/Edit", new { id }));
    }

    public class Role
    {
        public Int32 Id { get; set; }
        public string Name { get; set; }
    }
}
