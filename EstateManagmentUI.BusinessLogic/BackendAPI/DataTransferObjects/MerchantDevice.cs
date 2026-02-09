using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects
{
    public class MerchantDevice
    {
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }
        [JsonProperty("device_id")]
        public Guid DeviceId { get; set; }
        [JsonProperty("device_identifier")]
        public String DeviceIdentifier { get; set; }
        [JsonProperty("is_deleted")]
        public Boolean IsDeleted { get; set; }
    }
}
