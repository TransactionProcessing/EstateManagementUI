using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using SimpleResults;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService;

public interface IPermissionsRepository {
    Task<Result<List<(String role, String section, String function)>>> GetRolesFunctions();
    Task<Result<List<(String userName, String role)>>> GetUsers(CancellationToken cancellationToken);
    Task<Result> MigrateDatabase(CancellationToken cancellationToken);
    Task<Result> SeedDatabase(CancellationToken cancellationToken);
    Task<Result<List<Role>>> GetRoles(CancellationToken cancellationToken);
    Task<Result<Role>> GetRole(Int32 roleId, CancellationToken cancellationToken);
    Task<Result<List<(ApplicationSection, Function, Boolean)>>> GetRolePermissions(Int32 roleId, CancellationToken cancellationToken);
    Task<Result> UpdateRolePermissions(Int32 roleId, List<(Int32, Int32, Boolean)> newPermissions, CancellationToken cancellationToken);
    Task<Result<List<UserRole>>> GetRoleUsers(Int32 roleId, CancellationToken cancellationToken);
    Task<Result> AddUserToRole(Int32 roleId, String userName, CancellationToken cancellationToken);
    Task<Result> RemoveUserFromRole(Int32 roleId, String userName, CancellationToken cancellationToken);
    Task<Result> AddRole(String roleName, CancellationToken cancellationToken);
}