using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class OperatorCommands {
    public record UpdateOperatorCommand(CorrelationId CorrelationId,Guid EstateId, Guid OperatorId, string Name, bool RequireCustomMerchantNumber, bool RequireCustomTerminalNumber) : IRequest<Result>;

    public record CreateOperatorCommand(CorrelationId CorrelationId, Guid EstateId, string Name, bool RequireCustomMerchantNumber, bool RequireCustomTerminalNumber) : IRequest<Result>;
}