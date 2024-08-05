using System.Runtime.CompilerServices;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database;

public interface IDatabaseContext
{
    Task InitialiseDatabase();

    Task<Result<Int32>> AddRole(Role role, CancellationToken cancellationToken, Boolean ignoreDuplicates = false);
    Task<Result<List<Role>>> GetRoles(CancellationToken cancellationToken);
    Task<Result<Role>> GetRole(Int32 roleId, CancellationToken cancellationToken);

    Task<Result<Int32>> AddApplicationSection(ApplicationSection applicationSection, CancellationToken cancellationToken, Boolean ignoreDuplicates = false);
    Task<Result<Int32>> AddFunction(Function function, ApplicationSection applicationSection, CancellationToken cancellationToken, Boolean ignoreDuplicates = false);
    Task<Result<List<(ApplicationSection, Function)>>> GetAllFunctions(CancellationToken cancellationToken);

    Task<Result<List<(ApplicationSection, Function)>>> GetRolePermissions(Int32 roleId,
                                                                          CancellationToken cancellationToken);


}