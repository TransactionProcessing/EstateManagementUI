using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record EstateFunctions
{
    public static readonly string View = "View Estate";
    public static readonly string ViewUsersList = "View Estate Users";
    public static readonly string ViewOperatorsList = "View Estate Operators";
    public static readonly string AddOperator = "Add Operator";
    public static readonly string RemoveOperator = "Remove Operator";

    //public static readonly string Edit = "Edit Estate";
    //public static readonly string AddUser = "Add User";
    //public static readonly string RemoveUser = "Remove User";
}