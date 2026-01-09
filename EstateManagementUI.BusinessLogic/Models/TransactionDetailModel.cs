namespace EstateManagementUI.BusinessLogic.Models;

public class TransactionDetailModel
{
    public Guid TransactionId { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public String? MerchantName { get; set; }
    public Guid MerchantId { get; set; }
    public String? OperatorName { get; set; }
    public Guid OperatorId { get; set; }
    public String? ProductName { get; set; }
    public String? TransactionType { get; set; }
    public String? TransactionStatus { get; set; }
    public Decimal GrossAmount { get; set; }
    public Decimal FeesCommission { get; set; }
    public Decimal NetAmount { get; set; }
    public String? SettlementReference { get; set; }
}
