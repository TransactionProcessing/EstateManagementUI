using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class FileProcessingSummaryModel
{
    [JsonProperty("failed_lines")]
    public int FailedLines { get; set; }

    [JsonProperty("ignored_lines")]
    public int IgnoredLines { get; set; }

    [JsonProperty("not_processed_lines")]
    public int NotProcessedLines { get; set; }

    [JsonProperty("rejected_lines")]
    public int RejectedLines { get; set; }

    [JsonProperty("successfully_processed_lines")]
    public int SuccessfullyProcessedLines { get; set; }

    [JsonProperty("total_lines")]
    public int TotalLines { get; set; }
}