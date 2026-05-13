namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

public class MerchantScheduleResponse
{
    public int Year { get; set; }
    public List<MerchantScheduleMonthResponse> Months { get; set; }
}