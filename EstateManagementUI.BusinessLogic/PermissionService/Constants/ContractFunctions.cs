using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record ContractFunctions
{
    public static readonly string ViewList = "View Contracts List";
    public static readonly string ViewProductsList = "View Contract Products List";
    public static readonly string ViewProductFeesList = "View Contract Product Fees List";
    public static readonly string View = "View Contract";
    public static readonly string Edit = "Edit Contract";
    public static readonly string New = "New Contract";
    //public static readonly  string Remove = "Remove Contract";
    public static readonly string AddProduct = "Add Product";
    //public static readonly  string RemoveProduct = "Remove Product";
    public static readonly string AddTransactionFee = "Add Transaction Fee";
    //public static readonly  string RemoveTransactionFee = "Remove Transaction Fee";
}