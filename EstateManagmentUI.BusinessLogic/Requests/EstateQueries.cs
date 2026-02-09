using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class EstateQueries {
    public record GetEstateQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<EstateModels.EstateModel>>;
    public record GetAssignedOperatorsQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<OperatorModels.OperatorModel>>>;
}