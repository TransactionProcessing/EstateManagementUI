namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
public class EstateContract
{
    public Guid OperatorId { get; set; }
    public Guid ContractId { get; set; }
    public string? Name { get; set; }
    public string? OperatorName { get; set; }
}