using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record ReportingFunctions
{
    public const String TransactionAnalysis = "View Transaction Analysis";
    public const String SettlementAnalysis = "View Settlement Analysis";
}