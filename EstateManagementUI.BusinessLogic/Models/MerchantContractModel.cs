using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class MerchantContractModel {
    public Guid ContractId { get; set; }
    public String Name { get; set; }
    public Boolean IsDeleted { get; set; }
}