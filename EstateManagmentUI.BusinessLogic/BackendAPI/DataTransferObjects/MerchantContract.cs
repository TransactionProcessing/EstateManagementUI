namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
public class MerchantContract
{
    public Guid MerchantId { get; set; }
    public Guid ContractId { get; set; }
    public String ContractName { get; set; }
    public Boolean IsDeleted { get; set; }
    public List<MerchantContractProduct> ContractProducts { get; set; }
    public String OperatorName { get; set; }
}