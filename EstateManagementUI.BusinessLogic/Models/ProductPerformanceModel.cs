using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class ProductPerformanceModel
{
    public string ProductName { get; set; }
    public int TransactionCount { get; set; }
    public decimal TransactionValue { get; set; }
    public decimal PercentageContribution { get; set; }
}
