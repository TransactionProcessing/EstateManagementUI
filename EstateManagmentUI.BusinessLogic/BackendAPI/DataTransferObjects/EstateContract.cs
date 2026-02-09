using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class EstateContract
{
    [JsonProperty("operator_id")]
    public Guid OperatorId { get; set; }
    [JsonProperty("contract_id")]
    public Guid ContractId { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("operator_name")]
    public string? OperatorName { get; set; }
}