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