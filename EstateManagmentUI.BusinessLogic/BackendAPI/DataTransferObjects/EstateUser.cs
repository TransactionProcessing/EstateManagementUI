using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class EstateUser
{
    [JsonProperty("user_id")]
    public Guid UserId { get; set; }
    [JsonProperty("email_address")]
    public string? EmailAddress { get; set; }
    [JsonProperty("created_date_time")]
    public DateTime CreatedDateTime { get; set; }
}