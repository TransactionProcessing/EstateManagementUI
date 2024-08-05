using System.Collections.Generic;
using System.Reflection;
using EstateManagementUI.BusinessLogic.PermissionService.Constants;
using EstateManagementUI.BusinessLogic.PermissionService.Database;
using EstateManagementUI.BusinessLogic.PermissionService.Database.Entities;
using Microsoft.AspNetCore.Components.Forms;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.PermissionService;

public class PermissionsRepository : IPermissionsRepository {
    private readonly IDatabaseContext DatabaseContext;

    public PermissionsRepository(IDatabaseContext databaseContext) {
        this.DatabaseContext = databaseContext;
    }
    
    public async Task<Result<List<(String role, String section, String function)>>> GetRolesX() {

        List<(String role, String section, String function)> result = new();

        result.Add(("View All", ApplicationSections.Dashboard, DashboardFunctions.Dashboard));
        result.Add(("View Lists", ApplicationSections.Dashboard, DashboardFunctions.Dashboard));
        result.Add(("Administrator", ApplicationSections.Dashboard, DashboardFunctions.Dashboard));

        result.Add(("View All", ApplicationSections.Estate, EstateFunctions.View));
        result.Add(("View All", ApplicationSections.Merchant, MerchantFunctions.ViewList));
        result.Add(("View All", ApplicationSections.Merchant, MerchantFunctions.View));
        result.Add(("View All", ApplicationSections.Contract, ContractFunctions.ViewList));
        result.Add(("View All", ApplicationSections.Contract, ContractFunctions.View));
        result.Add(("View All", ApplicationSections.Operator, OperatorFunctions.ViewList));
        result.Add(("View All", ApplicationSections.Operator, OperatorFunctions.View));

        result.Add(("View Lists", ApplicationSections.Estate, EstateFunctions.View));
        result.Add(("View Lists", ApplicationSections.Merchant, MerchantFunctions.ViewList));
        result.Add(("View Lists", ApplicationSections.Contract, ContractFunctions.ViewList));
        result.Add(("View Lists", ApplicationSections.Operator, OperatorFunctions.ViewList));

        result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.View));
        result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.Edit));
        result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.AddOperator));
        result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.RemoveOperator));
        result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.AddUser));
        result.Add(("Administrator", ApplicationSections.Estate, EstateFunctions.RemoveUser));

        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.ViewList));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.View));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.Edit));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.New));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.Remove));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.AddOperator));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.RemoveOperator));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.AddDevice));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.RemoveDevice));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.EditAddress));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.EditContact));
        result.Add(("Administrator", ApplicationSections.Merchant, MerchantFunctions.MakeDeposit));

        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.ViewList));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.View));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.Edit));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.New));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.Remove));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.AddProduct));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.RemoveProduct));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.AddTransactionFee));
        result.Add(("Administrator", ApplicationSections.Contract, ContractFunctions.RemoveTransactionFee));
            
        result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.ViewList));
        result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.View));
        result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.Edit));
        result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.New));
        result.Add(("Administrator", ApplicationSections.Operator, OperatorFunctions.Remove));

        return result;
    }

    public async Task<Result<List<(String userName, String role)>>> GetUsers() {
        List<(String userName, String role)> result = new List<(String userName, String role)>();

        result.Add(("ViewListUser", "View Lists"));
        result.Add(("ViewAllUser", "View All"));
        result.Add(("Administrator", "Administrator"));
        result.Add(("estateuser@productionestate1.co.uk", "Administrator"));
        result.Add(("estateuser@testestate1.co.uk", "Administrator"));
        return result;
    }

    public async Task<Result> SeedDatabase(CancellationToken cancellationToken) {
        // make sure the schema is up to date
        await this.DatabaseContext.InitialiseDatabase();

        // Add the application sections to the application sections table
        List<String> applicationSections = this.GetApplicationSections();
        List<(String, String)> applicationFunctions = this.GetApplicationFunctions();

        foreach (String applicationSection in applicationSections)
        {
            Result<Int32> applicationSectionResult = await this.DatabaseContext.AddApplicationSection(new ApplicationSection { Name = applicationSection },
                cancellationToken, true);

            if (applicationSectionResult.IsFailed)
                return Result.Failure(applicationSectionResult.Message);

            // Now add the functions for this section
            List<(String, String)> functions = applicationFunctions.Where(af => af.Item1 == applicationSection).ToList();

            foreach ((String, String) function in functions)
            {
                var addFunctionResult = await this.DatabaseContext.AddFunction(new Function { Name = function.Item2 },
                    new ApplicationSection
                    {
                        Name = applicationSection,
                        ApplicationSectionId = applicationSectionResult.Data
                    }, cancellationToken, true);

                if (addFunctionResult.IsFailed)
                    return Result.Failure(addFunctionResult.Message);
            }
        }

        // Add some default roles
        await this.DatabaseContext.AddRole(new Role { Name = "View" }, cancellationToken, true);
        await this.DatabaseContext.AddRole(new Role { Name = "ViewList" }, cancellationToken, true);
        await this.DatabaseContext.AddRole(new Role { Name = "Administrator" }, cancellationToken, true);

        return Result.Success();






    }

    public async Task<Result<List<Role>>> GetRoles(CancellationToken cancellationToken) {
        return await this.DatabaseContext.GetRoles(cancellationToken);
    }

    public async Task<Result<Role>> GetRole(Int32 roleId,
                                            CancellationToken cancellationToken) {
        return await this.DatabaseContext.GetRole(roleId, cancellationToken);
    }

    public async Task<Result<List<(ApplicationSection, Function, Boolean)>>> GetRolePermissions(Int32 roleId,
                                                                            CancellationToken cancellationToken) {
        Result<List<(ApplicationSection, Function)>> allFunctions = await this.DatabaseContext.GetAllFunctions(cancellationToken);
        Result<List<(ApplicationSection, Function)>> rolePermisions = await this.DatabaseContext.GetRolePermissions(roleId, cancellationToken);

        List<(ApplicationSection, Function, Boolean)> result = new();
        foreach ((ApplicationSection, Function) function in allFunctions.Data) {

            Boolean hasPermission =
                rolePermisions.Data.SingleOrDefault(rp => rp.Item1 == function.Item1 && rp.Item2 == function.Item2) !=
                default;
            result.Add((function.Item1, function.Item2, hasPermission));
        }
        return result;
    }

    public List<Role> Roles;

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