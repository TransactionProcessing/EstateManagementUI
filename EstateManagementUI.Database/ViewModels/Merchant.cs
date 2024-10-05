using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public record Merchant
{
    public string Name { get; set; }
    public string Reference { get; set; }
    public string SettlementSchedule { get; set; }
    public string ContactName { get; set; }
    public string AddressLine1 { get; set; }
    public string Town { get; set; }

    public Guid Id { get; set; }
}