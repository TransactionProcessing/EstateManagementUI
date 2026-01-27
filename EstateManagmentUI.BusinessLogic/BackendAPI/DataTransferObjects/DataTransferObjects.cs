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

    public class Contract
    {
        #region Properties
        [JsonProperty("estate_id")]
        public Guid EstateId { get; set; }
        [JsonProperty("estate_reporting_id")]
        public Int32 EstateReportingId { get; set; }
        [JsonProperty("contract_id")]
        public Guid ContractId { get; set; }
        [JsonProperty("contract_reporting_id")]
        public Int32 ContractReportingId { get; set; }
        [JsonProperty("description")]
        public String Description { get; set; }
        [JsonProperty("operator_name")]
        public String OperatorName { get; set; }
        [JsonProperty("operator_id")]
        public Guid OperatorId { get; set; }
        [JsonProperty("operator_reporting_id")]
        public Int32 OperatorReportingId { get; set; }

        #endregion
    }

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

    public class EstateUser
    {
        [JsonProperty("user_id")]
        public Guid UserId { get; set; }
        [JsonProperty("email_address")]
        public string? EmailAddress { get; set; }
        [JsonProperty("created_date_time")]
        public DateTime CreatedDateTime { get; set; }
    }

    public class EstateOperator
    {
        [JsonProperty("operator_id")]
        public Guid OperatorId { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("require_custom_merchant_number")]
        public bool RequireCustomMerchantNumber { get; set; }
        [JsonProperty("require_custom_terminal_number")]
        public bool RequireCustomTerminalNumber { get; set; }
        [JsonProperty("created_date_time")]
        public DateTime CreatedDateTime { get; set; }
    }

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


    public class EstateMerchant
    {
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }
        [JsonProperty("name")]
        public string? Name { get; set; }
        [JsonProperty("reference")]
        public string? Reference { get; set; }
    }

    public class Operator
    {
        [JsonProperty("estate_reporting_id")]
        public Int32 EstateReportingId { get; set; }
        [JsonProperty("operator_id")]
        public Guid OperatorId { get; set; }
        [JsonProperty("name")]
        public String Name { get; set; }
        [JsonProperty("operator_reporting_id")]
        public Int32 OperatorReportingId { get; set; }
        [JsonProperty("require_custom_merchant_number")]
        public Boolean RequireCustomMerchantNumber { get; set; }
        [JsonProperty("require_custom_terminal_number")]
        public Boolean RequireCustomTerminalNumber { get; set; }
    }

    public class MerchantOperator
    {
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }
        [JsonProperty("operator_id")]
        public Guid OperatorId { get; set; }
        [JsonProperty("operator_name")]
        public String OperatorName { get; set; }
        [JsonProperty("merchant_number")]
        public String MerchantNumber { get; set; }
        [JsonProperty("terminal_number")]
        public String TerminalNumber { get; set; }
        [JsonProperty("is_deleted")]
        public Boolean IsDeleted { get; set; }
    }

    public class MerchantContract
    {
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }
        [JsonProperty("contract_id")]
        public Guid ContractId { get; set; }
        [JsonProperty("contract_name")]

        public String ContractName { get; set; }
        [JsonProperty("is_deleted")]
        public Boolean IsDeleted { get; set; }
        [JsonProperty("contract_products")]
        public List<MerchantContractProduct> ContractProducts { get; set; }
        [JsonProperty("operator_name")]
        public String OperatorName { get; set; }
    }

    public class MerchantContractProduct
    {
        [JsonProperty("merchant_id")]
        public Guid MerchantId { get; set; }
        [JsonProperty("contract_id")]
        public Guid ContractId { get; set; }
        [JsonProperty("product_id")]
        public Guid ProductId { get; set; }
        [JsonProperty("product_name")]
        public String ProductName { get; set; }
        [JsonProperty("display_text")]
        public String DisplayText { get; set; }
        [JsonProperty("product_type")]
        public Int32 ProductType { get; set; }
        [JsonProperty("value")]
        public Decimal? Value { get; set; }
    }

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
