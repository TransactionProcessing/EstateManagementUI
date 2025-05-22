using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Contract
{
    [ExcludeFromCodeCoverage]
    public class ContractPageEvents
    {
        public record ContractCreatedEvent;
        public record ContractUpdatedEvent;
        public record ContractProductCreatedEvent;
        public record ContractProductUpdatedEvent;
        public record ContractProductTransactionFeeCreatedEvent;
    }
}
