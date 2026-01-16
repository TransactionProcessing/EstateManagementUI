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

    public class MerchantKpi
    {
        [JsonProperty("merchants_with_sale_in_last_hour")]
        public Int32 MerchantsWithSaleInLastHour { get; set; }
        [JsonProperty("merchants_with_no_sale_today")]
        public Int32 MerchantsWithNoSaleToday { get; set; }
        [JsonProperty("merchants_with_no_sale_in_last7_days")]
        public Int32 MerchantsWithNoSaleInLast7Days { get; set; }
    }
}
