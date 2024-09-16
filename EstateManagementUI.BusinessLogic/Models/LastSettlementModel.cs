using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class LastSettlementModel
{
    public DateTime SettlementDate { get; set; }
    public Decimal SalesValue { get; set; }
    public Decimal FeesValue { get; set; }
    public Int32 SalesCount { get; set; }
}