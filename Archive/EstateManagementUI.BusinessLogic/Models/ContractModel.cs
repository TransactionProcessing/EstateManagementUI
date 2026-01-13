using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public record ContractModel {
    public Guid ContractId { get; set; }

    public String Description { get; set; }
    public Guid OperatorId { get; set; }
    public String OperatorName { get; set; }
    public Int32 NumberOfProducts { get; set; }

    public List<ContractProductModel> ContractProducts {get; set; }
}