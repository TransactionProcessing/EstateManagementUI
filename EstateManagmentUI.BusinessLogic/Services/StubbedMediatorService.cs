using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.RequestHandlers;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Services;

/// <summary>
/// Stubbed MediatR service that returns mock data without making remote calls
/// This allows quick development and testing without backend dependencies
/// </summary>
public class StubbedMediatorService : IMediator
{
    private readonly IApiClient ApiClient;

    public StubbedMediatorService(IApiClient apiClient) {
        this.ApiClient = apiClient;
    }
    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        //ateRequestHandler dateHandler = new DateRequestHandler(this.ApiClient);

        return request switch
        {
            // Estate Queries
            //Queries.GetEstateQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockEstate())),
            //Queries.GetMerchantsQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockMerchants())),
            //Queries.GetOperatorsQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockOperators())),
            //Queries.GetContractsQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockContracts())),
            //Queries.GetOperatorQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockOperator())),
            //Queries.GetContractQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockContract())),
            //Queries.GetMerchantQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockMerchant())),
            
            // File Processing Queries
            //Queries.GetFileImportLogsListQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockFileImportLogs())),
            //Queries.GetFileImportLogQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockFileImportLog())),
            //Queries.GetFileDetailsQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockFileDetails())),
            
            // Dashboard Queries
            //Queries.GetComparisonDatesQuery q => await dateHandler.Handle(q, cancellationToken),
            //Queries.GetTodaysSalesQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockTodaysSales())),
            //Queries.GetTodaysSettlementQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockTodaysSettlement())),
            //Queries.GetTodaysSalesCountByHourQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockSalesCountByHour())),
            //Queries.GetTodaysSalesValueByHourQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockSalesValueByHour())),
            //Queries.GetMerchantKpiQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockMerchantKpi())),
            //Queries.GetTodaysFailedSalesQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockTodaysSales())),
            //Queries.GetTopProductDataQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockTopProducts())),
            //Queries.GetBottomProductDataQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockBottomProducts())),
            //Queries.GetTopMerchantDataQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockTopMerchants())),
            //Queries.GetBottomMerchantDataQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockBottomMerchants())),
            //Queries.GetTopOperatorDataQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockTopOperators())),
            //Queries.GetBottomOperatorDataQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockBottomOperators())),
            //Queries.GetLastSettlementQuery => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockLastSettlement())),
            //Queries.GetTransactionDetailQuery q => Task.FromResult((TResponse)(object)Result.Success(StubTestData.GetMockTransactionDetails(q))),
            
            // Commands - just return success
            //Commands.AddMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.AddOperatorToMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.AddOperatorToEstateCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.AssignContractToMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.CreateContractCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.CreateMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.CreateMerchantUserCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.CreateOperatorCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.MakeMerchantDepositCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.RemoveContractFromMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.RemoveOperatorFromMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.RemoveOperatorFromEstateCommand => Task.FromResult((TResponse)(object)Result.Success()),
//            Commands.SetMerchantSettlementScheduleCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.SwapMerchantDeviceCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.UpdateMerchantAddressCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.UpdateMerchantCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.UpdateMerchantContactCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.UpdateOperatorCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.AddProductToContractCommand => Task.FromResult((TResponse)(object)Result.Success()),
            //Commands.AddTransactionFeeForProductToContractCommand => Task.FromResult((TResponse)(object)Result.Success()),
            
            _ => throw new NotImplementedException($"Request type {request.GetType().Name} is not implemented in stubbed mediator")
        };
    }

    public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
    {
        return Task.CompletedTask;
    }

    public Task<object?> Send(object request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<object?>(null);
    }

    public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task Publish(object notification, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
    {
        return Task.CompletedTask;
    }

    // Mock data methods
    
}
