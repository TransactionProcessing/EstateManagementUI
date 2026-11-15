using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using TransactionProcessing.UnionResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class DateQueries
{
    public record GetComparisonDatesQuery(CorrelationId CorrelationId, Guid EstateId) : IRequest<Result<List<ComparisonDateModel>>>;
}