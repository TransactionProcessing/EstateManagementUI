using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class FileDetailsModel
{
    [JsonProperty("file_id")]
    public Guid FileId { get; set; }

    [JsonProperty("processing_completed")]
    public bool ProcessingCompleted { get; set; }

    [JsonProperty("estate_id")]
    public Guid EstateId { get; set; }

    [JsonProperty("user_id")]
    public Guid UserId { get; set; }

    [JsonProperty("user_email_address")]
    public string UserEmailAddress { get; set; }

    [JsonProperty("merchant_id")]
    public Guid MerchantId { get; set; }

    [JsonProperty("merchant_name")]
    public string MerchantName { get; set; }

    [JsonProperty("file_profile_id")]
    public Guid FileProfileId { get; set; }

    [JsonProperty("file_profile_name")]
    public string FileProfileName { get; set; }

    [JsonProperty("file_import_log_id")]
    public Guid FileImportLogId { get; set; }

    [JsonProperty("file_location")]
    public string FileLocation { get; set; }

    [JsonProperty("file_lines")]
    public List<FileLineModel> FileLines { get; set; }

    [JsonProperty("processing_summary")]
    public FileProcessingSummaryModel ProcessingSummary { get; set; }
}