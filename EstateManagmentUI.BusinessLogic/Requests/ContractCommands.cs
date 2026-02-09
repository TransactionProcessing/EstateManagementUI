using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class ContractCommands {
    public record CreateContractCommand(CorrelationId CorrelationId, Guid EstateId, string Description, Guid OperatorId) : IRequest<Result>;
    public record AddProductToContractCommand(CorrelationId CorrelationId, Guid EstateId, Guid ContractId, string ProductName, string DisplayText, decimal? Value) : IRequest<Result>;
    public record AddTransactionFeeToProductCommand(CorrelationId CorrelationId, Guid EstateId, Guid ContractId, Guid ProductId, string Description, decimal Value, String CalculationType, String FeeType) : IRequest<Result>;
    public record RemoveTransactionFeeFromProductCommand(CorrelationId CorrelationId, Guid EstateId, Guid ContractId, Guid ProductId, Guid FeeId) : IRequest<Result>;
}