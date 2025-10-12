using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record MerchantFunctions
{
    public static readonly string ViewList = "View Merchant List";
    public static readonly string View = "View Single Merchant";
    public static readonly string Edit = "Edit Merchant";
    public static readonly string New = "New Merchant";
    //public static readonly string Remove = "Remove Merchant";
    public static readonly string AddOperator = "Add Operator";
    public static readonly string RemoveOperator = "Remove Operator";
    public static readonly string AddContract = "Add Contract";
    public static readonly string RemoveContract = "Remove Contract";
    public static readonly string AddDevice = "Add Device";
    //public static readonly string RemoveDevice = "Remove Device";
    //public static readonly string EditAddress = "Edit Merchant Address";
    //public static readonly string EditContact = "Edit Merchant Contact";
    public static readonly string MakeDeposit = "Make Deposit";
}