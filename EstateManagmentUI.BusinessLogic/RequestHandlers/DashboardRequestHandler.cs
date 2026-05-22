using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class DashboardRequestHandler : 
    IRequestHandler<Queries.GetTopProductDataQuery, Result<List<TopBottomProductDataModel>>>,
    IRequestHandler<Queries.GetBottomProductDataQuery, Result<List<TopBottomProductDataModel>>>,
    IRequestHandler<Queries.GetTopMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
    IRequestHandler<Queries.GetBottomMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
    IRequestHandler<Queries.GetTopOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
    IRequestHandler<Queries.GetBottomOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
    IRequestHandler<Queries.GetLastSettlementQuery, Result<LastSettlementModel>>
{
    private readonly IApiClient ApiClient;

    public DashboardRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
            
    }
    
    public async Task<Result<List<TopBottomProductDataModel>>> Handle(Queries.GetTopProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTopProducts());
    }

    public async Task<Result<List<TopBottomProductDataModel>>> Handle(Queries.GetBottomProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockBottomProducts());
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(Queries.GetTopMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTopMerchants());
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(Queries.GetBottomMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockBottomMerchants());
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(Queries.GetTopOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockTopOperators());
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(Queries.GetBottomOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockBottomOperators());
    }

    public async Task<Result<LastSettlementModel>> Handle(Queries.GetLastSettlementQuery request,
                                                          CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockLastSettlement());
    }
}