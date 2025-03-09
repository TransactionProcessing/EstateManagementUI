using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Merchant.MerchantDetails;

[ExcludeFromCodeCoverage]
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
    public record DepositMadeEvent;
}