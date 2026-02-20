using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.UIServices;

public interface ICalendarUIService {
    Task<Result<List<ComparisonDateModel>>> GetComparisonDates(CorrelationId correlationId, Guid estateId);
}

public class CalendarUIService : ICalendarUIService {
    private readonly IMediator Mediator;

    public CalendarUIService(IMediator mediator) {
        this.Mediator = mediator;
    }

    public async Task<Result<List<ComparisonDateModel>>> GetComparisonDates(CorrelationId correlationId, Guid estateId) {
        var query = new Queries.GetComparisonDatesQuery(correlationId, estateId);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var comparisonDates = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(comparisonDates);
    }
}