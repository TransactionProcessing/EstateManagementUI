using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record FileProcessingFunctions {
    public static readonly String ViewImportLogList = "View Import Log List";
    public static readonly String ViewImportLog = "View Import Log";
    public static readonly String ViewFileDetails = "View File Details";
}