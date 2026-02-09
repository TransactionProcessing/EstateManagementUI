using Newtonsoft.Json;

namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

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

    [JsonProperty("products")]
    public List<ContractProduct> Products { get; set; }

    #endregion
}