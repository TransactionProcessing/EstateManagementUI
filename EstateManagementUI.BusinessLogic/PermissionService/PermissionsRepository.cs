using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;
using EstateManagementUI.BusinessLogic.Common;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService.Database;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using SimpleResults;
using SQLite;

namespace EstateManagementUI.BusinessLogic.PermissionService;

[ExcludeFromCodeCoverage]
public class PermissionsRepository : IPermissionsRepository {
    private readonly IDbContextFactory<PermissionsContext> PermissionsContextFactory;

    public PermissionsRepository(IDbContextFactory<PermissionsContext> permissionsContextFactory) {
        this.PermissionsContextFactory = permissionsContextFactory;
    }

    public async Task<Result<List<(String role, String section, String function)>>> GetRolesFunctions() {

        List<(String role, String section, String function)> result = new();

        Result<List<Role>> allRoles = await this.GetRoles(CancellationToken.None);

        foreach (Role role in allRoles.Data) {
            var permissions = await this.GetRolePermissions(role.RoleId, CancellationToken.None);
            foreach (var permission in permissions.Data) {
                result.Add((role.Name, permission.Item1.Name, permission.Item2.Name));
            }
        }

        return result;
        //result.Add(("View All", ApplicationSections.Dashboard, DashboardFunctions.Dashboard));
        //result.Add(("View Lists", ApplicationSections.Dashboard, DashboardFunctions.Dashboard));
        //result.Add(("Administrator", ApplicationSections.Dashboard, DashboardFunctions.Dashboard));

        //result.Add(("View All", ApplicationSections.Estate, EstateFunctions.View));
        //result.Add(("View All", ApplicationSections.Merchant, MerchantFunctions.ViewList));
        //result.Add(("View All", ApplicationSections.Merchant, MerchantFunctions.View));
        //result.Add(("View All", ApplicationSections.Contract, ContractFunctions.ViewList));
        //result.Add(("View All", ApplicationSections.Contract, ContractFunctions.View));
        //result.Add(("View All", ApplicationSections.Operator, OperatorFunctions.ViewList));
        //result.Add(("View All", ApplicationSections.Operator, OperatorFunctions.View));

        //result.Add(("View Lists", ApplicationSections.Estate, EstateFunctions.View));
        //result.Add(("View Lists", ApplicationSections.Merchant, MerchantFunctions.ViewList));
        //result.Add(("View Lists", ApplicationSections.Contract, ContractFunctions.ViewList));
        //result.Add(("View Lists", ApplicationSections.Operator, OperatorFunctions.ViewList));

        //result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.View));
        //result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.Edit));
        //result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.AddOperator));
        //result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.RemoveOperator));
        //result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.AddUser));
        //result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.RemoveUser));

        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.ViewList));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.View));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.Edit));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.New));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.Remove));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.AddOperator));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.RemoveOperator));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.AddDevice));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.RemoveDevice));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.EditAddress));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.EditContact));
        //result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.MakeDeposit));

        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.ViewList));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.View));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.Edit));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.New));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.Remove));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.AddProduct));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.RemoveProduct));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.AddTransactionFee));
        //result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.RemoveTransactionFee));

        //result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.ViewList));
        //result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.View));
        //result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.Edit));
        //result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.New));
        //result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.Remove));

        //return result;
    }

    public async Task<Result<List<(String userName, String role)>>> GetUsers(CancellationToken cancellationToken) {
        List<(String userName, String role)> result = new List<(String userName, String role)>();
        
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        var userRoles = await context.UserRoles.ToListAsync(cancellationToken);
        var roles = await context.Roles.ToListAsync(cancellationToken);

        foreach (UserRole userRole in userRoles) {
            var role = roles.SingleOrDefault(r => r.RoleId == userRole.RoleId);
            result.Add((userRole.UserName, role.Name));
        }

        return result;
    }

    public async Task<Result> MigrateDatabase(CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        try {
            var conn = context.Database.GetConnectionString();
            await context.Database.MigrateAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception e) {
            return Result.Failure(e.GetCombinedExceptionMessages());
        }
        
    }

    private async Task<Result> SeedApplicationSections(PermissionsContext context,
                                                       List<String> applicationSections,
                                                       CancellationToken cancellationToken) {
        try {
            foreach (String applicationSection in applicationSections) {
                await context.ApplicationSections.AddAsync(new ApplicationSection { Name = applicationSection },
                    cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException sqex) when (sqex.InnerException.Message.Contains("UNIQUE")) {
            context.ChangeTracker.Clear();
            return Result.Success();
        }
        catch (Exception ex) {
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
    }

    private async Task<Result> SeedApplicationFunctions(PermissionsContext context,
                                                        List<(String, String)> applicationFunctions,
                                                        CancellationToken cancellationToken) {
        try {
            foreach ((String, String) applicationFunction in applicationFunctions) {

                ApplicationSection? applicationSectionRecord =
                    await context.ApplicationSections.SingleOrDefaultAsync(a => a.Name == applicationFunction.Item1,
                        cancellationToken: cancellationToken);

                await context.Functions.AddAsync(new Function { ApplicationSectionId = applicationSectionRecord.ApplicationSectionId, Name = applicationFunction.Item2 }, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException sqex) when (sqex.InnerException.Message.Contains("UNIQUE"))
        {
            context.ChangeTracker.Clear();
            return Result.Success();
        }
        catch (Exception ex) {
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
    }
    
    private async Task<Result> SeedRoles(PermissionsContext context,
                                         CancellationToken cancellationToken) {
        try {
            await context.Roles.AddAsync(new Role { Name = "View" }, cancellationToken);
            await context.Roles.AddAsync(new Role { Name = "ViewList" }, cancellationToken);
            await context.Roles.AddAsync(new Role { Name = "Administrator" }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException sqex) when (sqex.InnerException.Message.Contains("UNIQUE"))
        {
            context.ChangeTracker.Clear();
            return Result.Success();
        }
        catch (Exception ex) {
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
    }

    private async Task<Result> SeedUserRoles(PermissionsContext context,
                                             List<(String,String)> usersRoles,
                                         CancellationToken cancellationToken)
    {
        try
        {
            foreach ((String, String) userRole in usersRoles) {
                Role? role = await context.Roles.SingleOrDefaultAsync(r => r.Name == userRole.Item2, cancellationToken);

                await context.UserRoles.AddAsync(new UserRole() { RoleId = role.RoleId, UserName = userRole.Item1}, cancellationToken);
            }
            await context.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (DbUpdateException sqex) when (sqex.InnerException.Message.Contains("UNIQUE"))
        {
            context.ChangeTracker.Clear();
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
    }

    public async Task<Result> SeedDatabase(CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        // Add the application sections to the application sections table
        List<String> applicationSections = this.GetApplicationSections();
        List<(String, String)> applicationFunctions = this.GetApplicationFunctions();

        // TODO: handle failures
        await SeedApplicationSections(context, applicationSections, cancellationToken);
        await SeedApplicationFunctions(context, applicationFunctions, cancellationToken);
        //await SeedRoles(context, cancellationToken);
        //List<(String, String)> userRoles = new List<(String, String)>();
        //userRoles.Add(("adminuser1", "Administrator"));
        //userRoles.Add(("adminuser2", "Administrator"));
        //userRoles.Add(("viewuser", "View"));
        //userRoles.Add(("viewlistuser", "ViewList"));
        //userRoles.Add(("viewalluser", "View"));
        //userRoles.Add(("viewalluser", "ViewList"));
        //await SeedUserRoles(context, userRoles, cancellationToken);

        return Result.Success();
    }


    public async Task<Result<List<Role>>> GetRoles(CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Roles.ToListAsync(cancellationToken);
    }

    public async Task<Result<Role>> GetRole(Int32 roleId,
                                            CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.Roles.SingleOrDefaultAsync(t => t.RoleId == roleId, cancellationToken);
    }

    private async  Task<Result<List<(ApplicationSection, Function)>>> GetRolePermissions(PermissionsContext context, Int32 roleId, CancellationToken cancellationToken)
    {
        List<RolePermission>? rolePermissions = await context.RolePermissions.Where(rp => rp.RoleId == roleId).ToListAsync(cancellationToken);
        List<ApplicationSection>? applicationSections = await context.ApplicationSections.ToListAsync(cancellationToken);
        List<Function>? functions = await context.Functions.ToListAsync(cancellationToken);

        List<(ApplicationSection, Function)> results = new List<(ApplicationSection, Function)>();
        foreach (RolePermission rolePermission in rolePermissions)
        {
            Function function = functions.Single(f => f.FunctionId == rolePermission.PermissionId);
            ApplicationSection applicationSection = applicationSections.Single(a =>
                a.ApplicationSectionId == function.ApplicationSectionId);

            results.Add((applicationSection, function));
        }
        return Result.Success(results);
    }

    private async Task<Result<List<(ApplicationSection, Function)>>> GetAllFunctions(PermissionsContext context, CancellationToken cancellationToken) {
        List<ApplicationSection>? applicationSections = await context.ApplicationSections.ToListAsync(cancellationToken);
        List<Function>? functions = await context.Functions.ToListAsync(cancellationToken);
        List<(ApplicationSection, Function)> result = new List<(ApplicationSection, Function)>();
        foreach (Function function in functions)
        {
            ApplicationSection section = applicationSections.Single(a => a.ApplicationSectionId == function.ApplicationSectionId);
            result.Add((section, function));
        }
        return Result.Success(result);
    }


    public async Task<Result<List<(ApplicationSection, Function, Boolean)>>> GetRolePermissions(Int32 roleId,
                                                                            CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        Result<List<(ApplicationSection, Function)>> allFunctions = await this.GetAllFunctions(context, cancellationToken);
        Result<List<(ApplicationSection, Function)>> rolePermisions = await this.GetRolePermissions(context, roleId, cancellationToken);

        List<(ApplicationSection, Function, Boolean)> result = new();
        foreach ((ApplicationSection, Function) function in allFunctions.Data) {

            Boolean hasPermission =
                rolePermisions.Data.SingleOrDefault(rp => rp.Item1 == function.Item1 && rp.Item2 == function.Item2) !=
                default;
            result.Add((function.Item1, function.Item2, hasPermission));
        }
        return result;
    }

    public async Task<Result> UpdateRolePermissions(Int32 roleId,
                                                    List<(Int32, Int32, Boolean)> newPermissions,
                                                    CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        // Clear down the roles permissions
        List<RolePermission> currentPermissions = await context.RolePermissions.Where(r => r.RoleId == roleId).ToListAsync(cancellationToken);
        context.RemoveRange(currentPermissions);
        
        // Add the new permissions
        foreach ((Int32, Int32, Boolean) newPermission in newPermissions) {
            if (newPermission.Item3 == true) {
                RolePermission rolePermission = new RolePermission {
                    RoleId = roleId, PermissionId = newPermission.Item2
                };
                await context.RolePermissions.AddAsync(rolePermission, cancellationToken);
            }
        }
        // Save the changes
        await context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }

    public async Task<Result<List<UserRole>>> GetRoleUsers(Int32 roleId,
                                                           CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        return await context.UserRoles.Where(ur => ur.RoleId == roleId).ToListAsync(cancellationToken);
    }

    public async Task<Result> AddUserToRole(Int32 roleId,
                                            String userName,
                                            CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        UserRole userRole = new UserRole { UserName = userName, RoleId = roleId };
        await context.UserRoles.AddAsync(userRole, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> RemoveUserFromRole(Int32 roleId,
                                                 String userName,
                                                 CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        UserRole? userRole =
            await context.UserRoles.SingleOrDefaultAsync(ur => ur.RoleId == roleId && ur.UserName == userName);
        context.UserRoles.Remove(userRole);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }

    public async Task<Result> AddRole(String roleName,
                                      CancellationToken cancellationToken) {
        PermissionsContext context = await this.PermissionsContextFactory.CreateDbContextAsync(cancellationToken);
        Role role = new Role { Name = roleName };
        await context.Roles.AddAsync(role, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
    
    public List<String> GetApplicationSections() {
        List<String> applicationSections  = GetActions<ApplicationSections>();

        return applicationSections;
    }

    public List<(String, String)> GetApplicationFunctions()
    {
        List<(String, String)> functions = new List<(String, String)>();
        //DashboardFunctions
        functions.AddRange(GetActions<DashboardFunctions>("Dashboard"));
        //EstateFunctions
        functions.AddRange(GetActions<EstateFunctions>("Estate"));
        //MerchantFunctions
        functions.AddRange(GetActions<MerchantFunctions>("Merchant"));
        //ContractFunctions
        functions.AddRange(GetActions<ContractFunctions>("Contract"));
        //OperatorFunctions
        functions.AddRange(GetActions<OperatorFunctions>("Operator"));

        return functions;
    }

    public static List<(string, string)> GetActions<T>(string category)
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(field => field.IsLiteral && !field.IsInitOnly)
            .Select(field => (category, (string)field.GetValue(null)))
            .ToList();
    }

    public static List<string> GetActions<T>()
    {
        return typeof(T)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(field => field.IsLiteral && !field.IsInitOnly)
            .Select(field => (string)field.GetValue(null))
            .ToList();
    }

}