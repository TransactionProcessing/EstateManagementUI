namespace EstateManagementUI.BlazorServer.Permissions;

/// <summary>
/// Defines the functions/actions that can be performed within each section
/// </summary>
public enum PermissionFunction
{
    View,
    Create,
    Edit,
    Delete,
    List,

    // Specific functions for certain sections
    // Merchant
    MakeDeposit,

    // Reporting
    TransactionDetailReport,
    TransactionMerchantSummaryReport,
    TransactionOperatorSummaryReport
}
