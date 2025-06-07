using SimpleResults;
using System.Diagnostics.CodeAnalysis;
using EstateManagementUI.BusinessLogic.Common;
using Microsoft.Identity.Client;

namespace EstateManagementUI.BusinessLogic.PermissionService;

public class PermissionsService : IPermissionsService {
    private readonly IPermissionsRepository PermissionsRepository;
    private readonly IConfigurationService ConfigurationService;

    public async Task<Result> DoIHavePermissions(String userName,
                                                 String sectionName,
                                                 String function) {
        Boolean permissionsBypass = this.ConfigurationService.GetPermissionsBypass();
        if (permissionsBypass)
            return Result.Success();

        if (String.IsNullOrEmpty(userName))
            return Result.Forbidden("User name is required");

        if (String.IsNullOrEmpty(sectionName))
            return Result.Forbidden("Section name is required");

        if (String.IsNullOrEmpty(function))
            return Result.Forbidden("Function is required");
        
        await this.LoadPermissionsData();

        (String userName, String role) userRole = this.UserRoles.SingleOrDefault(u => u.userName == userName);

        if (userRole == default)
            return Result.Forbidden($"User name {userName} has no roles assigned");

        List<(String role, String section, String function)> roleFunctions = this.RoleFunctions.Where(r => r.role == userRole.role).ToList();

        if (roleFunctions.Any() == false)
            return Result.Forbidden($"Users role {userRole.role} has no functions assigned");

        List<(String role, String section, String function)>? permissionCheck = roleFunctions.Where(r => r.section == sectionName && r.function == function).ToList();

        if (permissionCheck.Any() == false)
            return Result.Forbidden($"User {userName} in role {userRole} does not have access to {sectionName}-{function}");

        return Result.Success($"User {userName} in role {userRole} has access to {sectionName}-{function}");
    }

    public async Task<Result> DoIHavePermissions(String userName,
                                                 String sectionName) {

        Boolean permissionsBypass = this.ConfigurationService.GetPermissionsBypass();

        if (permissionsBypass)
            return Result.Success();

        if (String.IsNullOrEmpty(userName))
            return Result.Forbidden("User name is required");

        if (String.IsNullOrEmpty(sectionName))
            return Result.Forbidden("Section name is required");
        
        await this.LoadPermissionsData();

        (String userName, String role) userRole = this.UserRoles.SingleOrDefault(u => u.userName == userName);

        if (userRole == default)
            return Result.Forbidden($"User name {userName} has no roles assigned");

        List<(String role, String section, String function)> roleFunctions = this.RoleFunctions.Where(r => r.role == userRole.role).ToList();

        if (roleFunctions.Any() == false)
            return Result.Forbidden($"Users role {userRole.role} has no functions assigned");

        List<(String role, String section, String function)>? permissionCheck = roleFunctions.Where(r => r.section == sectionName).ToList();

        if (permissionCheck.Any() == false)
            return Result.Forbidden($"User {userName} in role {userRole} does not have access to {sectionName}");

        return Result.Success($"User {userName} in role {userRole} has access to {sectionName}");
    }

    public PermissionsService(IPermissionsRepository permissionsRepository, IConfigurationService configurationService) {
        this.PermissionsRepository = permissionsRepository;
        this.ConfigurationService = configurationService;
    }

    private async Task LoadPermissionsData() {
        // TODO: this will be cached and probably refreshed periodically
        var roleResult = await this.PermissionsRepository.GetRolesFunctions();
        if (roleResult.IsFailed) {
            // TODO : Handle error properly, e.g., log it or throw an exception
        }
        var userResult = await this.PermissionsRepository.GetUsers(CancellationToken.None);
        if (userResult.IsFailed)
        {
            // TODO : Handle error properly, e.g., log it or throw an exception
        }

        this.RoleFunctions = roleResult.Data;
        this.UserRoles= userResult.Data;
    }

    private List<(String role, String section, String function)> RoleFunctions = new();
    private List<(String userName, String role)> UserRoles = new();
}