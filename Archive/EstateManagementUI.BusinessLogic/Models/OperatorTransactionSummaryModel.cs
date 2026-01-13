using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class OperatorTransactionSummaryModel
{
    public Guid OperatorId { get; set; }
    public string? OperatorName { get; set; }
    public int TotalTransactionCount { get; set; }
    public decimal TotalTransactionValue { get; set; }
    public decimal AverageTransactionValue { get; set; }
    public int SuccessfulTransactionCount { get; set; }
    public int FailedTransactionCount { get; set; }
    public decimal TotalFeesEarned { get; set; }
}
