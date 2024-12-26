namespace EstateManagementUI.Pages.Merchant.MerchantDetails;

public class MerchantPageEvents
{
    public record MerchantCreatedEvent;
    public record MerchantUpdatedEvent;
    public record ShowAddOperatorDialog;
    public record HideAddOperatorDialog;
    public record OperatorAssignedToMerchantEvent;
    public record OperatorRemovedFromMerchantEvent;
    public record ShowAddContractDialog;
    public record HideAddContractDialog;
    public record ContractAssignedToMerchantEvent;
    public record ContractRemovedFromMerchantEvent;
}