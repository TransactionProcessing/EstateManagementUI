using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class EstateOperator
{
    [JsonProperty("operator_id")]
    public Guid OperatorId { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("require_custom_merchant_number")]
    public bool RequireCustomMerchantNumber { get; set; }
    [JsonProperty("require_custom_terminal_number")]
    public bool RequireCustomTerminalNumber { get; set; }
    [JsonProperty("created_date_time")]
    public DateTime CreatedDateTime { get; set; }
}