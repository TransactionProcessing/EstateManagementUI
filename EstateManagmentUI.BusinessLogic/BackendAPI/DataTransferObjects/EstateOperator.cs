namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
public class EstateOperator
{
    public Guid OperatorId { get; set; }
    public string? Name { get; set; }
    public bool RequireCustomMerchantNumber { get; set; }
    public bool RequireCustomTerminalNumber { get; set; }
    public DateTime CreatedDateTime { get; set; }
}