using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record ReportingFunctions
{
    public static readonly String TransactionAnalysis = "View Transaction Analysis";
    public static readonly String SettlementAnalysis = "View Settlement Analysis";
}