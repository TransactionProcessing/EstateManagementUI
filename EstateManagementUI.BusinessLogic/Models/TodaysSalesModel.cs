using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class TodaysSalesModel
{
    public Decimal TodaysSalesValue { get; set; }

    public int TodaysSalesCount { get; set; }

    public Decimal ComparisonSalesValue { get; set; }

    public int ComparisonSalesCount { get; set; }
}