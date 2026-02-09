using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class Merchant
{
    [JsonProperty("merchant_id")]
    public Guid MerchantId { get; set; }
    [JsonProperty("merchant_reporting_id")]
    public Int32 MerchantReportingId { get; set; }
    [JsonProperty("name")]
    public String Name { get; set; }
    [JsonProperty("reference")]
    public String Reference { get; set; }
    [JsonProperty("balance")]
    public Decimal Balance { get; set; }
    [JsonProperty("settlement_schedule")]
    public Int32 SettlementSchedule { get; set; }
    [JsonProperty("created_date_time")]
    public DateTime CreatedDateTime { get; set; }

    [JsonProperty("address_id")]
    public Guid AddressId { get; set; }
    [JsonProperty("address_line1")]
    public String AddressLine1 { get; set; }
    [JsonProperty("address_line2")]
    public String AddressLine2 { get; set; }
    [JsonProperty("town")]
    public String Town { get; set; }
    [JsonProperty("region")]
    public String Region { get; set; }
    [JsonProperty("post_code")]
    public String PostCode { get; set; }
    [JsonProperty("country")]
    public String Country { get; set; }

    [JsonProperty("contact_id")]
    public Guid ContactId { get; set; }
    [JsonProperty("contact_name")]
    public String ContactName { get; set; }
    [JsonProperty("contact_email")]
    public String ContactEmail { get; set; }
    [JsonProperty("contact_phone")]
    public String ContactPhone { get; set; }
}