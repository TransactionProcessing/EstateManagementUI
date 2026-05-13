namespace EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
public class ContractProductTransactionFee
{
    public Guid TransactionFeeId { get; set; }
    public string? Description { get; set; }
    public Int32 CalculationType { get; set; }
    public Int32 FeeType { get; set; }
    public Decimal Value { get; set; }
    public Int32 ContractProductTransactionFeeReportingId { get; set; }
}