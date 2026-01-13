using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class TodaysSettlementModel
{
    public Decimal TodaysSettlementValue { get; set; }

    public int TodaysSettlementCount { get; set; }

    public Decimal ComparisonSettlementValue { get; set; }

    public int ComparisonSettlementCount { get; set; }
}