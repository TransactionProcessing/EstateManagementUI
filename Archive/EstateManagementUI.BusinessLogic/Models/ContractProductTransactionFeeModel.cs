using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record ContractProductTransactionFeeModel {
    public Guid ContractProductTransactionFeeId { get; set; }
    public String Description { get; set; }
    public String FeeType { get; set; }
    public String CalculationType { get; set; }
    public Decimal Value { get; set; }
}