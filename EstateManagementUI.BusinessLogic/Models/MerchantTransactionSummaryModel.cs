using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class MerchantTransactionSummaryModel
{
    public Guid MerchantId { get; set; }
    public string MerchantName { get; set; }
    public int TotalTransactionCount { get; set; }
    public decimal TotalTransactionValue { get; set; }
    public decimal AverageTransactionValue { get; set; }
    public int SuccessfulTransactionCount { get; set; }
    public int FailedTransactionCount { get; set; }
}
