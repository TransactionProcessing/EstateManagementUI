using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using TransactionProcessing.UnionResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class SettlementQueries {
    public record GetTodaysSettlementQuery(CorrelationId CorrelationId, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSettlementModel>>;
}