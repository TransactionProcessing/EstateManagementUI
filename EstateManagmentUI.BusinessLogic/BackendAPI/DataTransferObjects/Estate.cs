using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class Estate
{
    [JsonProperty("estate_id")]
    public Guid EstateId { get; set; }
    [JsonProperty("estate_name")]
    public string? EstateName { get; set; }
    [JsonProperty("reference")]
    public string? Reference { get; set; }
    [JsonProperty("operators")]
    public List<EstateOperator>? Operators { get; set; }
    [JsonProperty("merchants")]
    public List<EstateMerchant>? Merchants { get; set; }
    [JsonProperty("contracts")]
    public List<EstateContract>? Contracts { get; set; }
    [JsonProperty("users")]
    public List<EstateUser>? Users { get; set; }
}