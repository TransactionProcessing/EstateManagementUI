using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class TransactionQueries {
    public record GetTodaysSalesQuery(CorrelationId CorrelationId, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
    public record GetTodaysFailedSalesQuery(CorrelationId CorrelationId, Guid EstateId, string ResponseCode, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
    public record GetTransactionDetailQuery(CorrelationId CorrelationId, Guid EstateId, DateTime StartDate, DateTime EndDate, List<Int32>? MerchantIds = null, List<Int32>? OperatorIds = null, List<Int32>? ProductIds = null) : IRequest<Result<TransactionModels.TransactionDetailReportResponse>>;
    public record GetMerchantTransactionSummaryQuery(CorrelationId CorrelationId, Guid EstateId, DateTime StartDate, DateTime EndDate, Int32? MerchantId = null, Int32? OperatorId = null) : IRequest<Result<TransactionModels.TransactionSummaryByMerchantResponse>>;
    public record GetOperatorTransactionSummaryQuery(CorrelationId CorrelationId,Guid EstateId, DateTime StartDate, DateTime EndDate, Int32? MerchantId = null, Int32? OperatorId = null) : IRequest<Result<TransactionModels.TransactionSummaryByOperatorResponse>>;
    public record GetProductPerformanceQuery(CorrelationId CorrelationId, Guid EstateId, DateTime StartDate, DateTime EndDate) : IRequest<Result<TransactionModels.ProductPerformanceResponse>>;
    public record GetTodaysSalesByHourQuery(CorrelationId CorrelationId, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TransactionModels.TodaysSalesByHourModel>>>;
}