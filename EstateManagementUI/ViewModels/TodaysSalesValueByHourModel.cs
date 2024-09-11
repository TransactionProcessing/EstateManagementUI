namespace EstateManagementUI.ViewModels;

public class TodaysSalesValueByHourModel
{
    public int Hour { get; set; }

    public Decimal TodaysSalesValue { get; set; }

    public Decimal ComparisonSalesValue { get; set; }
}