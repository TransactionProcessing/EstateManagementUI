namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class MerchantOpeningHour
{
    public Guid MerchantId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public String OpeningTime { get; set; }
    public String ClosingTime { get; set; }
}