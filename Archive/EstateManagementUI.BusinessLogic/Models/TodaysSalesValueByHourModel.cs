using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class TodaysSalesValueByHourModel
{
    public int Hour { get; set; }

    public Decimal TodaysSalesValue { get; set; }

    public Decimal ComparisonSalesValue { get; set; }
}