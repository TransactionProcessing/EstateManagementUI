using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class ContractProduct
{
    [JsonProperty("contract_id")]
    public Guid ContractId { get; set; }
    [JsonProperty("product_id")]
    public Guid ProductId { get; set; }
    [JsonProperty("product_name")]
    public String ProductName { get; set; }
    [JsonProperty("display_text")]
    public String DisplayText { get; set; }
    [JsonProperty("product_type")]
    public Int32 ProductType { get; set; }
    [JsonProperty("value")]
    public Decimal? Value { get; set; }
    [JsonProperty("transaction_fees")]
    public List<ContractProductTransactionFee> TransactionFees { get; set; }
}