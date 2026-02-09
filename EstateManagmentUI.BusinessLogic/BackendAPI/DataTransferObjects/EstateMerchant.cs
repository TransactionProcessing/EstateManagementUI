using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class EstateMerchant
{
    [JsonProperty("merchant_id")]
    public Guid MerchantId { get; set; }
    [JsonProperty("name")]
    public string? Name { get; set; }
    [JsonProperty("reference")]
    public string? Reference { get; set; }
}