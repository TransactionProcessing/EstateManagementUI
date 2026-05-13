namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
public class EstateUser
{
    public Guid UserId { get; set; }
    public string? EmailAddress { get; set; }
    public DateTime CreatedDateTime { get; set; }
}