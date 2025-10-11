using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleResults;

namespace EstateManagementUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsRepository PermissionsRepository;

        public PermissionsController(IPermissionsRepository permissionsRepository) {
            this.PermissionsRepository = permissionsRepository;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("createRoles")]
        public async Task<IActionResult> CreateRoles(List<String> rolesList, CancellationToken cancellationToken) {
            List<(Int32 roleId, String roleName)> results = new List<(Int32 roleId, String roleName)>();
            
            Shared.Logger.Logger.LogWarning("In CreateRoles");

            foreach (String role in rolesList) {
                Result<Int32> result = await this.PermissionsRepository.AddRole(role, CancellationToken.None);
                if (result.IsSuccess) {
                    results.Add((result.Data, role));
                }
            }

            Shared.Logger.Logger.LogWarning("Returning 200");

            return Ok(results);
        }

        [HttpPost]
        [Route("addUserToRole")]
        public async Task<IActionResult> AddUserToRole(List<AddUserToRole> userRolesList, CancellationToken cancellationToken)
        {
            List<(Int32 roleId, String roleName)> results = new List<(Int32 roleId, String roleName)>();
            Shared.Logger.Logger.LogWarning("In AddUserToRole");

            foreach (var userRole in userRolesList) {
                Result<Role> role = await this.PermissionsRepository.GetRole(userRole.RoleName, cancellationToken);
                await this.PermissionsRepository.AddUserToRole(role.Data.RoleId, userRole.UserName, cancellationToken);
            }

            Shared.Logger.Logger.LogWarning("Returning 200");

            return Ok(results);
        }

        [HttpPost]
        [Route("addRolePermissions")]
        public async Task<IActionResult> AddRolePermissions(List<RolePermissions> rolePermissionsList, CancellationToken cancellationToken) {

            Shared.Logger.Logger.LogWarning("In AddRolePermissions");
            foreach (RolePermissions rolePermissions in rolePermissionsList) {
                Result<Role> role = await this.PermissionsRepository.GetRole(rolePermissions.RoleName, cancellationToken);
                await this.PermissionsRepository.UpdateRolePermissions(role.Data.RoleId, rolePermissions.NewPermissions, CancellationToken.None);
            }

            Shared.Logger.Logger.LogWarning("Returning 200");
            return this.Ok();
        }

        [HttpGet]
        [Route("getRolePermissions")]
        public async Task<IActionResult> GetRolePermissions(String roleName,
                                                            CancellationToken cancellationToken) {
            Result<Role> role = await this.PermissionsRepository.GetRole(roleName, cancellationToken);
            Result<List<(ApplicationSection, Function, Boolean)>> permissionsList = await this.PermissionsRepository.GetRolePermissions(role.Data.RoleId, cancellationToken);
            List<ApplicationSection> applicationSections = permissionsList.Data.Select(p => p.Item1).Distinct().ToList();

            List<Permission> permissions = new List<Permission>();
            foreach ((ApplicationSection, Function, Boolean) valueTuple in permissionsList.Data) {
                permissions.Add(new Permission {
                    ApplicationSection = valueTuple.Item1,
                    Function = valueTuple.Item2,
                    HasAccess = valueTuple.Item3
                });
            }

            RolePermissionsObject returnObject = new() {
                PermissionsList = permissions,
                ApplicationSections = applicationSections
            };

            return this.Ok(returnObject);
        }
    }

    [ExcludeFromCodeCoverage]
    public record RolePermissionsObject {
        public List<Permission> PermissionsList { get; set; }
        public List<ApplicationSection> ApplicationSections { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public record Permission {
        public ApplicationSection ApplicationSection { get; set; }
        public Function Function { get; set; }
        public Boolean HasAccess { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public record AddUserToRole
    {
        public String RoleName { get; set; }
        public String UserName { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public record RolePermissions {
        public String RoleName { get; set; }
        public List<(int, int, bool)> NewPermissions { get; set; }
    }
}
