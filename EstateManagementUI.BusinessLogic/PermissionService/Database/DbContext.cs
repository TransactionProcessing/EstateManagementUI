using System.Linq.Expressions;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using SimpleResults;
using SQLite;
using System.Text;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using System.Collections.Generic;
using System.Data;

namespace EstateManagementUI.BusinessLogic.PermissionService.Database;

public class DatabaseContext : IDatabaseContext
{
    private readonly SQLiteAsyncConnection Connection;

    public DatabaseContext(string connectionString)
    {
        Connection = new SQLiteAsyncConnection(connectionString);
    }
    public async Task InitialiseDatabase()
    {
        await Connection.CreateTableAsync<Role>();
        await Connection.CreateTableAsync<ApplicationSection>();
        await Connection.CreateTableAsync<Function>();
        await Connection.CreateTableAsync<RolePermission>();
        await Connection.CreateTableAsync<ApplicationSectionFunction>();
    }

    public async Task<Result<Int32>> AddRole(Role role,
                                              CancellationToken cancellationToken, Boolean ignoreDuplicates=false) {
        try
        {
            await this.Connection.InsertAsync(role);

            return Result.Success(role.RoleId);
        }
        catch(SQLiteException sqex) when (sqex.Message.Contains("UNIQUE") && ignoreDuplicates) {
            return Result.Success(role.RoleId);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
    }

    public async Task<Result<List<Role>>> GetRoles(CancellationToken cancellationToken)
    {
        List<Role>? roles = await Connection.Table<Role>().ToListAsync();
        if (roles.Any() == false)
            return Result.NotFound("No roles have been defined");
        return Result.Success(roles);
    }

    public async Task<Result<Role>> GetRole(Int32 roleId, CancellationToken cancellationToken)
    {
        List<Role>? roles = await Connection.Table<Role>().Where(r => r.RoleId == roleId).ToListAsync();
        Role? role = roles.SingleOrDefault();

        if (role == null)
            return Result.NotFound($"No role found with Id {roleId}");
        return Result.Success(role);
    }

    public async Task<Result<Int32>> AddApplicationSection(ApplicationSection applicationSection,
                                                           CancellationToken cancellationToken, Boolean ignoreDuplicates = false) {
        try {
            await this.Connection.InsertAsync(applicationSection);

            return applicationSection.ApplicationSectionId;
        }
        catch (SQLiteException sqex) when (sqex.Message.Contains("UNIQUE") && ignoreDuplicates)
        {
            return Result.Success(applicationSection.ApplicationSectionId);
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
    }

    public async Task<Result<Int32>> AddFunction(Function function,
                                                 ApplicationSection applicationSection,
                                                 CancellationToken cancellationToken, Boolean ignoreDuplicates = false) {
        try {
            await this.Connection.InsertAsync(function);

            ApplicationSectionFunction applicationSectionFunction = new ApplicationSectionFunction {
                ApplicationSectionId = applicationSection.ApplicationSectionId, FunctionId = function.FunctionId
            };
            await this.Connection.InsertAsync(applicationSectionFunction);

            return Result.Success(function.FunctionId);
        }
        catch (SQLiteException sqex) when (sqex.Message.Contains("UNIQUE") && ignoreDuplicates)
        {
            return Result.Success(function.FunctionId);
        }
        catch (Exception ex) {
            return Result.Failure(ex.GetCombinedExceptionMessages());
        }
    }

    public async Task<Result<List<(ApplicationSection, Function)>>> GetAllFunctions(CancellationToken cancellationToken) {
        List<ApplicationSection>? applicationSections = await this.Connection.Table<ApplicationSection>().ToListAsync();
        List<Function>? functions = await this.Connection.Table<Function>().ToListAsync();
        List<ApplicationSectionFunction>? applicationSectionFunctions = await this.Connection.Table<ApplicationSectionFunction>().ToListAsync();
        List<(ApplicationSection, Function)> result = new List<(ApplicationSection, Function)>();
        foreach (ApplicationSectionFunction applicationSectionFunction in applicationSectionFunctions) {
            ApplicationSection section = applicationSections.Single(a => a.ApplicationSectionId == applicationSectionFunction.ApplicationSectionId);
            Function func = functions.Single(f => f.FunctionId == applicationSectionFunction.FunctionId);

            result.Add((section, func));
        }

        return Result.Success(result);
    }

    public async Task<Result<List<(ApplicationSection, Function)>>> GetRolePermissions(Int32 roleId, CancellationToken cancellationToken) {
        List<RolePermission>? rolePermissions = await this.Connection.Table<RolePermission>().Where(rp => rp.RoleId == roleId).ToListAsync();
        List<ApplicationSection>? applicationSections = await this.Connection.Table<ApplicationSection>().ToListAsync();
        List<ApplicationSectionFunction>? applicationSectionFunctions = await this.Connection.Table<ApplicationSectionFunction>().ToListAsync();
        List<Function>? functions = await this.Connection.Table<Function>().ToListAsync();

        List<(ApplicationSection, Function)> results = new List<(ApplicationSection, Function)>();
        foreach (RolePermission rolePermission in rolePermissions) {
            Function function = functions.Single(f => f.FunctionId == rolePermission.PermissionId);
            ApplicationSectionFunction applicationSectionFunction =
                applicationSectionFunctions.Single(asf => asf.FunctionId == function.FunctionId);
            ApplicationSection applicationSection = applicationSections.Single(a =>
                a.ApplicationSectionId == applicationSectionFunction.ApplicationSectionId);

            results.Add((applicationSection,function));
        }
        return Result.Success(results);
    }
}

public static class ExceptionHelper
{
    public static string GetCombinedExceptionMessages(this Exception ex)
    {
        StringBuilder sb = new StringBuilder();
        AppendExceptionMessages(ex, sb);
        return sb.ToString();
    }

    private static void AppendExceptionMessages(Exception ex, StringBuilder sb)
    {
        if (ex == null) return;

        sb.AppendLine(ex.Message);

        if (ex.InnerException != null)
        {
            AppendExceptionMessages(ex.InnerException, sb);
        }
    }
}