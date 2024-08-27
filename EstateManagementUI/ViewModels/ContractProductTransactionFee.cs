using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public record ContractProductTransactionFee {
    public Guid ContractProductTransactionFeeId { get; set; }
    public String Description { get; set; }
    public String FeeType { get; set; }
    public String CalculationType { get; set; }
    public Decimal Value { get; set; }
}