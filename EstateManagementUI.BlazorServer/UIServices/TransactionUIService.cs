using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.UIServices;

public interface ITransactionUIService {
    Task<Result<TransactionModels.TodaysSalesModel>> GetTodaysSales(CorrelationId correlationId, Guid estateId, DateTime comparisonDate);
    Task<Result<TransactionModels.TodaysSalesModel>> GetTodaysFailedSales(CorrelationId correlationId, Guid estateId, String responseCode, DateTime comparisonDate);
    Task<Result<List<TransactionModels.TodaysSalesByHourModel>>> GetTodaysSalesByHour(CorrelationId correlationId, Guid estateId, DateTime comparisonDate);
    Task<Result<TransactionModels.TodaysSettlementModel>> GetTodaysSettlement(CorrelationId correlationId, Guid estateId, DateTime comparisonDate);
    Task<Result<TransactionModels.ProductPerformanceResponse>> GetProductPerformance(CorrelationId correlationId, Guid estateId, DateTime startDate, DateTime endDate);
    Task<Result<TransactionModels.TransactionDetailReportResponse>> GetTransactionDetail(CorrelationId correlationId, Guid estateId, DateTime startDate, DateTime endDate,List<Int32>? merchantIds,
                                                                                   List<Int32>? operatorIds, List<Int32>? productIds);

    Task<Result<TransactionModels.TransactionSummaryByMerchantResponse>> GetMerchantTransactionSummary(CorrelationId correlationId,
                                                                                                       Guid estateId,
                                                                                                       DateTime startDate,
                                                                                                       DateTime endDate,
                                                                                                       Int32? merchant,
                                                                                                       Int32? @operator);

    Task<Result<TransactionModels.TransactionSummaryByOperatorResponse>> GetOperatorTransactionSummary(CorrelationId correlationId,
                                                                                                       Guid estateId,
                                                                                                       DateTime startDate,
                                                                                                       DateTime endDate,
                                                                                                       Int32? merchant,
                                                                                                       Int32? @operator);
}

public class TransactionUIService : ITransactionUIService
{
    private readonly IMediator Mediator;

    public TransactionUIService(IMediator mediator) {
        this.Mediator = mediator;
    }

    public async Task<Result<TransactionModels.TodaysSalesModel>> GetTodaysSales(CorrelationId correlationId, Guid estateId, DateTime comparisonDate) {
        var query = new TransactionQueries.GetTodaysSalesQuery(correlationId, estateId, comparisonDate);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var todaysSales = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(todaysSales);
    }

    public async Task<Result<TransactionModels.TodaysSalesModel>> GetTodaysFailedSales(CorrelationId correlationId, Guid estateId, String responseCode, DateTime comparisonDate) {
        var query = new TransactionQueries.GetTodaysFailedSalesQuery(correlationId, estateId, responseCode, comparisonDate);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var todaysFailedSales = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(todaysFailedSales);
    }

    public async Task<Result<List<TransactionModels.TodaysSalesByHourModel>>> GetTodaysSalesByHour(CorrelationId correlationId,
                                                                                                   Guid estateId,
                                                                                                   DateTime comparisonDate) {
        var query = new TransactionQueries.GetTodaysSalesByHourQuery(correlationId, estateId, comparisonDate);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var todaysSalesByHour = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(todaysSalesByHour);
    }

    public async Task<Result<TransactionModels.TodaysSettlementModel>> GetTodaysSettlement(CorrelationId correlationId,
                                                                                           Guid estateId,
                                                                                           DateTime comparisonDate) {
        var query = new SettlementQueries.GetTodaysSettlementQuery(correlationId, estateId, comparisonDate);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var todaysSettlement = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(todaysSettlement);

    }

    public async Task<Result<TransactionModels.ProductPerformanceResponse>> GetProductPerformance(CorrelationId correlationId,
                                                                             Guid estateId,
                                                                             DateTime startDate,
                                                                             DateTime endDate) {
        var query = new TransactionQueries.GetProductPerformanceQuery(correlationId, estateId, startDate, endDate);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var productPerformance = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(productPerformance);
    }

    public async Task<Result<TransactionModels.TransactionDetailReportResponse>> GetTransactionDetail(CorrelationId correlationId,
                                                                                                      Guid estateId,
                                                                                                      DateTime startDate,
                                                                                                      DateTime endDate,
                                                                                                      List<Int32>? merchantIds,
                                                                                                      List<Int32>? operatorIds,
                                                                                                      List<Int32>? productIds) {
        var query = new TransactionQueries.GetTransactionDetailQuery(correlationId, estateId, startDate, endDate, merchantIds, operatorIds, productIds);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var transactionDetail = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(transactionDetail);
    }

    public async Task<Result<TransactionModels.TransactionSummaryByMerchantResponse>> GetMerchantTransactionSummary(CorrelationId correlationId,
                                                                                                                    Guid estateId,
                                                                                                                    DateTime startDate,
                                                                                                                    DateTime endDate,
                                                                                                                    Int32? merchant,
                                                                                                                    Int32? @operator) {
        var query = new TransactionQueries.GetMerchantTransactionSummaryQuery(correlationId, estateId, startDate, endDate, merchant, @operator);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var merchantTransactionSummary = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(merchantTransactionSummary);
    }

    public async Task<Result<TransactionModels.TransactionSummaryByOperatorResponse>> GetOperatorTransactionSummary(CorrelationId correlationId,
                                                                                                                    Guid estateId,
                                                                                                                    DateTime startDate,
                                                                                                                    DateTime endDate,
                                                                                                                    Int32? merchant,
                                                                                                                    Int32? @operator) {
        var query = new TransactionQueries.GetOperatorTransactionSummaryQuery(correlationId, estateId, startDate, endDate, merchant, @operator);
        var result = await this.Mediator.Send(query);
        if (result.IsFailed)
            return ResultHelpers.CreateFailure(result);
        var operatorTransactionSummary = ModelFactory.ConvertFrom(result.Data);
        return Result.Success(operatorTransactionSummary);
    }
}