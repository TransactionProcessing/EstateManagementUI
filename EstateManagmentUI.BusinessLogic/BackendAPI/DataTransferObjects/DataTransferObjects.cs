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

    public class Merchant
    {
        #region Properties

        [JsonProperty("created_date_time")]
        public DateTime CreatedDateTime { get; set; }
        [JsonProperty("estate_reporting_id")]
        public Int32 EstateReportingId { get; set; }
        [JsonProperty("last_sale")]
        public DateTime LastSale { get; set; }
        [JsonProperty("last_sale_date_time")]
        public DateTime LastSaleDateTime { get; set; }
        [JsonProperty("last_statement")]
        public DateTime LastStatement { get; set; }
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }
        [JsonProperty("merchant_reporting_id")]
        public Int32 MerchantReportingId { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("post_code")]
        public String PostCode { get; set; }
        [JsonProperty("reference")]
        public String Reference { get; set; }
        [JsonProperty("region")]
        public String Region { get; set; }
        [JsonProperty("town")]
        public String Town { get; set; }

        #endregion
    }
}
