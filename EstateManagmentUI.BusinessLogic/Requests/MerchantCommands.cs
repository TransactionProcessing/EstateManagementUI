using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class MerchantCommands {
    public record MerchantAddress(Guid AddressId, string AddressLine1, string Town, string Region, string PostalCode, string Country);
    public record MerchantContact(Guid ContactId, string ContactName, string ContactEmail, string ContactPhone);
    public record UpdateMerchantCommand(CorrelationId CorrelationId,Guid EstateId, Guid MerchantId, string Name, String SettlementSchedule, MerchantAddress MerchantAddress, MerchantContact MerchantContact) : IRequest<Result>;
    public record RemoveOperatorFromMerchantCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, Guid OperatorId) : IRequest<Result>;
    public record AddOperatorToMerchantCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, Guid OperatorId, string? MerchantNumber, string? TerminalNumber) : IRequest<Result>;
    public record RemoveContractFromMerchantCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, Guid ContractId) : IRequest<Result>;
    public record AssignContractToMerchantCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, Guid ContractId) : IRequest<Result>;
    public record AddMerchantDeviceCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, string DeviceIdentifier) : IRequest<Result>;
    public record SwapMerchantDeviceCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, string OldDevice, string NewDevice) : IRequest<Result>;
    public record MakeMerchantDepositCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, decimal Amount, DateTime Date, string Reference) : IRequest<Result>;
    public record CreateMerchantCommand(CorrelationId CorrelationId, Guid EstateId, Guid MerchantId, string Name, String SettlementSchedule, MerchantAddress MerchantAddress, MerchantContact MerchantContact) : IRequest<Result>;
}