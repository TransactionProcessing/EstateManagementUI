using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class MerchantContract
{
    [JsonProperty("merchant_id")]
    public Guid MerchantId { get; set; }
    [JsonProperty("contract_id")]
    public Guid ContractId { get; set; }
    [JsonProperty("contract_name")]

    public String ContractName { get; set; }
    [JsonProperty("is_deleted")]
    public Boolean IsDeleted { get; set; }
    [JsonProperty("contract_products")]
    public List<MerchantContractProduct> ContractProducts { get; set; }
    [JsonProperty("operator_name")]
    public String OperatorName { get; set; }
}