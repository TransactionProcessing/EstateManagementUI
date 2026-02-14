using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class TransactionSummaryByMerchantResponse
{
    [JsonProperty("merchants")]
    public List<MerchantDetail> Merchants { get; set; }
    [JsonProperty("summary")]
    public MerchantDetailSummary Summary { get; set; }
}