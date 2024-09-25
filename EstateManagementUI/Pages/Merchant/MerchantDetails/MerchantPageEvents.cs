namespace EstateManagementUI.Pages.Merchant.MerchantDetails;

public class MerchantPageEvents
{
    public record ShowNewMerchantDialog;
    public record HideNewMerchantDialog;
    public record ShowEditMerchantDialog(Guid MerchantId);
    public record HideEditMerchantDialog;
    public record MerchantCreatedEvent;
    public record MerchantUpdatedEvent;
}