namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class MerchantScheduleMonthResponse
{
    public int Month { get; set; }
    public List<int> ClosedDays { get; set; }
}