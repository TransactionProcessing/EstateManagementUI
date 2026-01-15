using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects
{
    public class ComparisonDate
    {
        [JsonProperty("order_value")]
        public Int32 OrderValue { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("description")]
        public String Description { get; set; }
    }
}
