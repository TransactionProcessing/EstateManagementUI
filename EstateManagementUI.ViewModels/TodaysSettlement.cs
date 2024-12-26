namespace EstateManagementUI.ViewModels;

public class TodaysSettlement
{
    public Decimal TodaysSettlementValue { get; set; }
    public String ComparisonLabel { get; set; }
    public Decimal ComparisonSettlementValue { get; set; }
    public Decimal Variance { get; set; }
}