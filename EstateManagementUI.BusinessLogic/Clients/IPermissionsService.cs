using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Clients
{
    public interface IPermissionsService {
        Task<Result> DoIHavePermissions(String userName,
                                        String sectionName,
                                        String function);

        Task<Result> DoIHavePermissions(String userName,
                                        String sectionName);

        Task<Result> LoadPermissionsData();
    }

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
            this.RoleFunctions = await this.PermissionsRepository.GetRoles();
            this.UserRoles= await this.PermissionsRepository.GetUsers();

            return Result.Success();
        }

        private List<(String role, String section, String function)> RoleFunctions = new();
        private List<(String userName, String role)> UserRoles = new();
    }

    public interface IPermissionsRepository {
        Task<Result<List<(String role, String section, String function)>>> GetRoles();
        Task<Result<List<(String userName, String role)>>> GetUsers();
    }

    public class PermissionsRepository : IPermissionsRepository {
        public async Task<Result<List<(String role, String section, String function)>>> GetRoles() {

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

    public class ApplicationSections {
        public const String Dashboard = "Dashboard";
        public const String Estate = "Estate";
        public const String Merchant = "Merchant";
        public const String Contract = "Contract";
        public const String Operator = "Operator";
        public const String FileProcessing = "FileProcessing";
        public const String Reporting = "Reporting";
    }

    public class DashboardFunctions {

        public const String Dashboard = "Dashboard";
    }

    public class EstateFunctions {
        public const String View = "View Estate";
        public const String Edit  = "Edit Estate";
        public const String AddOperator = "Add Operator";
        public const String RemoveOperator = "Remove Operator";
        public const String AddUser = "Add User";
        public const String RemoveUser = "Remove User";
    }

    public class MerchantFunctions {
        public const String ViewList = "View Merchant List";
        public const String View = "View Single Merchant";
        public const String Edit = "Edit Merchant";
        public const String New = "New Merchant";
        public const String Remove = "Remove Merchant";
        public const String AddOperator = "Add Operator";
        public const String RemoveOperator = "Remove Operator";
        public const String AddDevice = "Add Device";
        public const String RemoveDevice = "Remove Device";
        public const String EditAddress = "Edit Merchant Address";
        public const String EditContact = "Edit Merchant Contact";
        public const String MakeDeposit = "Make Deposit";
    }

    public class ContractFunctions {
        public const String ViewList = "View Contracts List";
        public const String View = "View Single Contract";
        public const String Edit = "Edit Contract";
        public const String New = "New Contract";
        public const String Remove = "Remove Contract";
        public const String AddProduct = "Add Product";
        public const String RemoveProduct = "Remove Product";
        public const String AddTransactionFee = "Add Transaction Fee";
        public const String RemoveTransactionFee = "Remove Transaction Fee";
    }

    public class OperatorFunctions {
        public const String ViewList = "View Operators List";
        public const String View = "View Single Operator";
        public const String Edit = "Edit Operator";
        public const String New = "New Operator";
        public const String Remove = "Remove Operator";
    }
}
