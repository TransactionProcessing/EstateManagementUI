using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class FileLineModel
{
    [JsonProperty("line_data")]
    public string LineData { get; set; }

    [JsonProperty("line_number")]
    public int LineNumber { get; set; }

    [JsonProperty("processing_result")]
    public FileLineProcessingResult ProcessingResult { get; set; }

    [JsonProperty("transaction_id")]
    public Guid TransactionId { get; set; }

    [JsonProperty("rejection_reason")]
    public string RejectionReason { get; set; }
}