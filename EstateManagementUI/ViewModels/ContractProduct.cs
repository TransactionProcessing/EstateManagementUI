using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public record ContractProduct {
    public Guid ContractProductId { get; set; }
    public String ProductName { get; set; }
    public String ProductType { get; set; }
    public String DisplayText { get; set; }
    public String Value { get; set; }
    public Int32 NumberOfFees { get; set; }
}