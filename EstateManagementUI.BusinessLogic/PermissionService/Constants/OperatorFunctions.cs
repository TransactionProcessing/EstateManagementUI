using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public class OperatorFunctions
{
    public const string ViewList = "View Operators List";
    public const string View = "View Single Operator";
    public const string Edit = "Edit Operator";
    public const string New = "New Operator";
    public const string Remove = "Remove Operator";
}

[ExcludeFromCodeCoverage]
public record FileProcessingFunctions {
    public const String ViewImportLogList = "View Import Log List";
    public const String ViewImportLog = "View Import Log";
    public const String ViewFileDetails = "View File Details";
}

[ExcludeFromCodeCoverage]
public record ReportingFunctions
{
    public const String TransactionAnalysis = "View Transaction Analysis";
    public const String SettlementAnalysis = "View Settlement Analysis";
}