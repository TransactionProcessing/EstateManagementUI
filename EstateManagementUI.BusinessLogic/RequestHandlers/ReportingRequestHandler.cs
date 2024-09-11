using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;
using static EstateManagmentUI.BusinessLogic.Requests.Queries;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class ReportingRequestHandler : IRequestHandler<Queries.GetComparisonDatesQuery, Result<List<ComparisonDateModel>>>,
IRequestHandler<GetTodaysSalesQuery, Result<TodaysSalesModel>>,
    IRequestHandler<GetTodaysSettlementQuery, Result<TodaysSettlementModel>>,
IRequestHandler<GetTodaysSalesCountByHourQuery, Result<List<TodaysSalesCountByHourModel>>>,
IRequestHandler<GetTodaysSalesValueByHourQuery, Result<List<TodaysSalesValueByHourModel>>>
{

    private readonly IApiClient ApiClient;
    public ReportingRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<ComparisonDateModel>>> Handle(Queries.GetComparisonDatesQuery request,
                                                                CancellationToken cancellationToken) {
        Result<List<ComparisonDateModel>> model = await this.ApiClient.GetComparisonDates(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSalesModel>> Handle(GetTodaysSalesQuery request,
                                                       CancellationToken cancellationToken) {
        Result<TodaysSalesModel> model = await this.ApiClient.GetTodaysSales(request.AccessToken, request.EstateId, request.EstateId, null,
            null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSettlementModel>> Handle(GetTodaysSettlementQuery request,
                                                            CancellationToken cancellationToken) {
        Result<TodaysSettlementModel> model = await this.ApiClient.GetTodaysSettlement(request.AccessToken, Guid.Empty, request.EstateId, null,
            null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TodaysSalesCountByHourModel>>> Handle(GetTodaysSalesCountByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        Result<List<TodaysSalesCountByHourModel>> model = await this.ApiClient.GetTodaysSalesCountByHour(request.AccessToken, Guid.Empty, request.EstateId,
            null, null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TodaysSalesValueByHourModel>>> Handle(GetTodaysSalesValueByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        Result<List<TodaysSalesValueByHourModel>> model = await this.ApiClient.GetTodaysSalesValueByHour(request.AccessToken, Guid.Empty, request.EstateId,
            null, null, request.ComparisonDate, cancellationToken);
        return model;
    }
}