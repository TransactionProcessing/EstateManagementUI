using EstateManagementUI.BusinessLogic.PermissionService;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Clients;

public class PermissionsService : IPermissionsService {
    private readonly IPermissionsRepository PermissionsRepository;

    public async Task<Result> DoIHavePermissions(String userName,
                                                 String sectionName,
                                                 String function) {

        if (this.RoleFunctions.Any() == false && this.UserRoles.Any() == false)
            await this.LoadPermissionsData();

        (String userName, String role) userRole = this.UserRoles.SingleOrDefault(u => u.userName == userName);

        if (userRole == default)
            return Result.Forbidden($"User name {userName} has no roles assigned");

        List<(String role, String section, String function)> roleFunctions = this.RoleFunctions.Where(r => r.role == userRole.role).ToList();

        if (roleFunctions.Any() == false)
            return Result.Forbidden($"Users role {userRole.role} has no functions assigned");

        (String role, String section, String function) permissionCheck = roleFunctions.SingleOrDefault(r => r.section == sectionName && r.function == function);

        if (permissionCheck == default)
            return Result.Forbidden(
                $"User {userName} in role {userRole} does not have access to {sectionName}-{function}");

        return Result.Success($"User {userName} in role {userRole} has access to {sectionName}-{function}");
    }

    public async Task<Result> DoIHavePermissions(String userName,
                                                 String sectionName) {
        if (this.RoleFunctions.Any() == false && this.UserRoles.Any() == false)
            await this.LoadPermissionsData();

        (String userName, String role) userRole = this.UserRoles.SingleOrDefault(u => u.userName == userName);

        if (userRole == default)
            return Result.Forbidden($"User name {userName} has no roles assigned");

        List<(String role, String section, String function)> roleFunctions = this.RoleFunctions.Where(r => r.role == userRole.role).ToList();

        if (roleFunctions.Any() == false)
            return Result.Forbidden($"Users role {userRole.role} has no functions assigned");

        List<(String role, String section, String function)>? permissionCheck = roleFunctions.Where(r => r.section == sectionName).ToList();

        if (permissionCheck.Any() == false)
            return Result.Forbidden(
                $"User {userName} in role {userRole} does not have access to {sectionName}");

        return Result.Success($"User {userName} in role {userRole} has access to {sectionName}");
    }

    public PermissionsService(IPermissionsRepository permissionsRepository) {
        this.PermissionsRepository = permissionsRepository;
    }

    public async Task<Result> LoadPermissionsData() {
        // TODO: this will be cached and probably refershed periodically
        this.RoleFunctions = await this.PermissionsRepository.GetRolesFunctions();
        this.UserRoles= await this.PermissionsRepository.GetUsers(CancellationToken.None);

        return Result.Success();
    }

    private List<(String role, String section, String function)> RoleFunctions = new();
    private List<(String userName, String role)> UserRoles = new();
}