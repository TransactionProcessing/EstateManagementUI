namespace EstateManagementUI.Pages.Merchant.MerchantDetails;

public class MerchantPageEvents
{
    public record MerchantCreatedEvent;
    public record MerchantUpdatedEvent;
    public record ShowAddOperatorDialog;
    public record HideAddOperatorDialog;
    public record ShowEditOperatorDialog(Guid OperatorId);
    public record HideEditOperatorDialog;

    public record OperatorAssignedToMerchantEvent;
    public record OperatorRemovedFromMerchantEvent;
}