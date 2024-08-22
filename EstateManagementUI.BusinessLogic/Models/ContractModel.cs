using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record ContractModel {
    public Guid ContractId { get; set; }

    public String Description { get; set; }

    public String OperatorName { get; set; }
    public Int32 NumberOfProducts { get; set; }

    public List<ContractProductModel> ContractProducts {get; set; }
}

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

[ExcludeFromCodeCoverage]
public record ContractProductTransactionFeeModel {
    public Guid ContractProductTransactionFeeId { get; set; }
    public String Description { get; set; }
    public String FeeType { get; set; }
    public String CalculationType { get; set; }
    public Decimal Value { get; set; }
}