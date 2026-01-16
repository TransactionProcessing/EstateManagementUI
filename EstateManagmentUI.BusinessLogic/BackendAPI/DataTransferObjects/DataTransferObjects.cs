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

    public class TodaysSales
    {
        [JsonProperty("todays_average_sales_value")]
        public Decimal TodaysAverageSalesValue { get; set; }
        [JsonProperty("todays_sales_value")]
        public Decimal TodaysSalesValue { get; set; }
        [JsonProperty("todays_sales_count")]
        public Int32 TodaysSalesCount { get; set; }
        [JsonProperty("comparison_sales_value")]
        public Decimal ComparisonSalesValue { get; set; }
        [JsonProperty("comparison_sales_count")]
        public Int32 ComparisonSalesCount { get; set; }
        [JsonProperty("comparison_average_sales_value")]
        public Decimal ComparisonAverageSalesValue { get; set; }
    }
}
