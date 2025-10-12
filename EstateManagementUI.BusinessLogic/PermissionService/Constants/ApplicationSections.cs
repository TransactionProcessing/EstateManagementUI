using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.PermissionService.Constants;

[ExcludeFromCodeCoverage]
public record ApplicationSections
{
    public static readonly string Dashboard = "Dashboard";
    public static readonly string Estate = "Estate";
    public static readonly string Merchant = "Merchant";
    public static readonly string Contract = "Contract";
    public static readonly string Operator = "Operator";
    public static readonly string FileProcessing = "FileProcessing";
    public static readonly string Reporting = "Reporting";
}