using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public class OperatorQueries {
    public record GetOperatorsQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<OperatorModels.OperatorModel>>>;
    public record GetOperatorQuery(CorrelationId CorrelationId, Guid EstateId, Guid OperatorId) : IRequest<Result<OperatorModels.OperatorModel>>;
    public record GetOperatorsForDropDownQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<OperatorModels.OperatorDropDownModel>>>;
}