using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class TransactionSummaryByMerchantResponse
{
    [JsonProperty("merchants")]
    public List<MerchantDetail> Merchants { get; set; }
    [JsonProperty("summary")]
    public MerchantDetailSummary Summary { get; set; }
}

public class TransactionSummaryByOperatorResponse
{
    [JsonProperty("operators")]
    public List<OperatorDetail> Operators { get; set; }
    [JsonProperty("summary")]
    public OperatorDetailSummary Summary { get; set; }
}

public class OperatorDetail
{
    [JsonProperty("operator_id")]
    public Guid OperatorId { get; set; }
    [JsonProperty("operator_reporting_id")]
    public Int32 OperatorReportingId { get; set; }
    [JsonProperty("operator_name")]
    public string OperatorName { get; set; }
    [JsonProperty("total_count")]
    public Int32 TotalCount { get; set; }
    [JsonProperty("total_value")]
    public Decimal TotalValue { get; set; }
    [JsonProperty("average_value")]
    public Decimal AverageValue { get; set; }
    [JsonProperty("authorised_count")]
    public Int32 AuthorisedCount { get; set; }
    [JsonProperty("declined_count")]
    public Int32 DeclinedCount { get; set; }
    [JsonProperty("authorised_percentage")]
    public Decimal AuthorisedPercentage { get; set; }
}

public class OperatorDetailSummary
{
    [JsonProperty("total_operators")]
    public Int32 TotalOperators { get; set; }
    [JsonProperty("total_count")]
    public Int32 TotalCount { get; set; }
    [JsonProperty("total_value")]
    public Decimal TotalValue { get; set; }
    [JsonProperty("average_value")]
    public Decimal AverageValue { get; set; }
}

public class ProductPerformanceResponse
{
    [JsonProperty("product_details")]
    public List<ProductPerformanceDetail> ProductDetails { get; set; }
    [JsonProperty("summary")]
    public ProductPerformanceSummary Summary { get; set; }
}

public class ProductPerformanceDetail
{
    [JsonProperty("product_name")]
    public String ProductName { get; set; }
    [JsonProperty("product_id")]
    public Guid ProductId { get; set; }
    [JsonProperty("product_reporting_id")]
    public Int32 ProductReportingId { get; set; }
    [JsonProperty("contract_id")]
    public Guid ContractId { get; set; }
    [JsonProperty("contract_reporting_id")]
    public Int32 ContractReportingId { get; set; }
    [JsonProperty("transaction_count")]
    public Int32 TransactionCount { get; set; }
    [JsonProperty("transaction_value")]
    public Decimal TransactionValue { get; set; }
    [JsonProperty("percentage_of_total")]
    public Decimal PercentageOfTotal { get; set; }
}

public class ProductPerformanceSummary
{
    [JsonProperty("total_products")]
    public Int32 TotalProducts { get; set; }
    [JsonProperty("total_count")]
    public Int32 TotalCount { get; set; }
    [JsonProperty("total_value")]
    public Decimal TotalValue { get; set; }
    [JsonProperty("average_per_product")]
    public Decimal AveragePerProduct { get; set; }
}