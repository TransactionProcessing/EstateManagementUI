using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record ContractProductModel {
    public Guid ContractProductId { get; set; }
    public String ProductName { get; set; }
    public String ProductType { get; set; }
    public String DisplayText { get; set; }
    public String Value { get; set; }
    public Int32 NumberOfFees { get; set; }

    public List<ContractProductTransactionFeeModel> ContractProductTransactionFees { get; set; }
}