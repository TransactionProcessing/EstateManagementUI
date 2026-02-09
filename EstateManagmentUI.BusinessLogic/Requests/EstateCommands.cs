using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class EstateCommands {
    public record AddOperatorToEstateCommand(CorrelationId CorrelationId, Guid EstateId, Guid OperatorId) : IRequest<Result>;
    public record RemoveOperatorFromEstateCommand(CorrelationId CorrelationId, Guid EstateId, Guid OperatorId) : IRequest<Result>;
}