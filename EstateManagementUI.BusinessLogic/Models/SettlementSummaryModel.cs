using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class SettlementSummaryModel
{
    public DateTime SettlementPeriodStart { get; set; }
    public DateTime SettlementPeriodEnd { get; set; }
    public Guid MerchantId { get; set; }
    public string MerchantName { get; set; }
    public decimal GrossTransactionValue { get; set; }
    public decimal TotalFees { get; set; }
    public decimal NetSettlementAmount { get; set; }
    public string SettlementStatus { get; set; }
}
