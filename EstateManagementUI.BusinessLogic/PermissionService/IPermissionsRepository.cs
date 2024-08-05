using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.PermissionService;

public interface IPermissionsRepository {
    Task<Result<List<(String role, String section, String function)>>> GetRolesX();
    Task<Result<List<(String userName, String role)>>> GetUsers();


    //Task<Result<List<Role>>> GetAllRoles(CancellationToken cancellationToken);
    Task<Result> SeedDatabase(CancellationToken cancellationToken);
    Task<Result<List<Role>>> GetRoles(CancellationToken cancellationToken);
    Task<Result<Role>> GetRole(Int32 roleId, CancellationToken cancellationToken);
    Task<Result<List<(ApplicationSection, Function, Boolean)>>> GetRolePermissions(Int32 roleId, CancellationToken cancellationToken);
}