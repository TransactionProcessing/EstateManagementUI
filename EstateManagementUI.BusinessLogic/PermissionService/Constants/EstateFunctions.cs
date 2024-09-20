using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record EstateFunctions
{
    public const string View = "View Estate";
    public const string ViewUsersList = "View Estate Users";
    public const string ViewOperatorsList = "View Estate Operators";
    
    //public const string Edit = "Edit Estate";
    //public const string AddOperator = "Add Operator";
    //public const string RemoveOperator = "Remove Operator";
    //public const string AddUser = "Add User";
    //public const string RemoveUser = "Remove User";
}