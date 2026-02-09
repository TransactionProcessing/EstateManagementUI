using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class ContractProductTransactionFee
{
    [JsonProperty("transaction_fee_id")] 
    public Guid TransactionFeeId { get; set; }
    [JsonProperty("description")] 
    public string? Description { get; set; }
    [JsonProperty("calculation_type")] 
    public Int32 CalculationType { get; set; }
    [JsonProperty("fee_type")] 
    public Int32 FeeType { get; set; }
    [JsonProperty("value")] 
    public Decimal Value { get; set; }
}