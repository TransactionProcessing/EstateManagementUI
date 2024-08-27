using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public class EstateFunctions
{
    public const string View = "View Estate";
    public const string Edit = "Edit Estate";
    public const string ViewEstateUsers = "View Estate Users";
    public const string ViewEstateOperators = "View Estate Operators";
    public const string AddOperator = "Add Operator";
    public const string RemoveOperator = "Remove Operator";
    public const string AddUser = "Add User";
    public const string RemoveUser = "Remove User";
}