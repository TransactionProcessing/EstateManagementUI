using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public record Contract {
    public Guid ContractId { get; set; }
    public string Description { get; set; }
    public string OperatorName { get; set; }
    public Int32 NumberOfProducts { get; set; }

}