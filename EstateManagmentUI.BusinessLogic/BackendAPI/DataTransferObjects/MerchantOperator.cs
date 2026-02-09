using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class MerchantOperator
{
    [JsonProperty("merchant_id")]
    public Guid MerchantId { get; set; }
    [JsonProperty("operator_id")]
    public Guid OperatorId { get; set; }
    [JsonProperty("operator_name")]
    public String OperatorName { get; set; }
    [JsonProperty("merchant_number")]
    public String MerchantNumber { get; set; }
    [JsonProperty("terminal_number")]
    public String TerminalNumber { get; set; }
    [JsonProperty("is_deleted")]
    public Boolean IsDeleted { get; set; }
}