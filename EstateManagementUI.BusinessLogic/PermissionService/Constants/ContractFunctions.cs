using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record ContractFunctions
{
    public const string ViewList = "View Contracts List";
    public const string ViewProductsList = "View Contract Products List";
    public const string ViewProductFeesList = "View Contract Product Fees List";
    public const string View = "View Contract";
    public const string Edit = "Edit Contract";
    public const string New = "New Contract";
    //public const string Remove = "Remove Contract";
    public const string AddProduct = "Add Product";
    //public const string RemoveProduct = "Remove Product";
    public const string AddTransactionFee = "Add Transaction Fee";
    //public const string RemoveTransactionFee = "Remove Transaction Fee";
}