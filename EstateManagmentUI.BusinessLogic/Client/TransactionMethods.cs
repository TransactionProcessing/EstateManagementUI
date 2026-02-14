using EstateManagementUI.BusinessLogic.Models;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Requests;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<TodaysSalesModel>> GetTodaysSales(TransactionQueries.GetTodaysSalesQuery request, CancellationToken cancellationToken);
        Task<Result<TodaysSalesModel>> GetTodaysFailedSales(TransactionQueries.GetTodaysFailedSalesQuery request, CancellationToken cancellationToken);
        Task<Result<TransactionModels.TransactionDetailReportResponse>> GetTransactionDetailReport(TransactionQueries.GetTransactionDetailQuery request, CancellationToken cancellationToken);
        Task<Result<TransactionModels.TransactionSummaryByMerchantResponse>> GetMerchantTransactionSummary(TransactionQueries.GetMerchantTransactionSummaryQuery request, CancellationToken cancellationToken);
        Task<Result<TransactionModels.TransactionSummaryByOperatorResponse>> GetOperatorTransactionSummary(TransactionQueries.GetOperatorTransactionSummaryQuery request, CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        public async Task<Result<TodaysSalesModel>> GetTodaysSales(TransactionQueries.GetTodaysSalesQuery request,
                                                                   CancellationToken cancellationToken) {
            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetTodaysSales(token.Data, request.EstateId, 0, 0, request.ComparisonDate, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            TodaysSalesModel todaysSalesModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(todaysSalesModel);
        }

        public async Task<Result<TodaysSalesModel>> GetTodaysFailedSales(TransactionQueries.GetTodaysFailedSalesQuery request,
                                                                         CancellationToken cancellationToken)
        {
            // Get a token here 
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<TodaysSales> apiResult = await this.EstateReportingApiClient.GetTodaysFailedSales(token.Data, request.EstateId, 0, 0, request.ResponseCode, request.ComparisonDate, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            TodaysSalesModel todaysSalesModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(todaysSalesModel);
        }

        public async Task<Result<TransactionModels.TransactionDetailReportResponse>> GetTransactionDetailReport(TransactionQueries.GetTransactionDetailQuery request,
                                                                                                                CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            TransactionDetailReportRequest apiRequest = new TransactionDetailReportRequest {
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                Merchants = request.MerchantIds,
                Operators = request.OperatorIds,
                Products = request.ProductIds
            };

            Result<TransactionDetailReportResponse> apiResult = await this.EstateReportingApiClient.GetTransactionDetailReport(token.Data, request.EstateId, apiRequest, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            TransactionModels.TransactionDetailReportResponse transactionDetailReportResponseModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(transactionDetailReportResponseModel);
        }

        public async Task<Result<TransactionModels.TransactionSummaryByMerchantResponse>> GetMerchantTransactionSummary(TransactionQueries.GetMerchantTransactionSummaryQuery request,
                                                                                                                   CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            TransactionSummaryByMerchantRequest apiRequest = new TransactionSummaryByMerchantRequest
            {
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                Merchants = request.MerchantId.HasValue switch {
                    true => [request.MerchantId.Value],
                    _ => null
                },
                Operators = request.OperatorId.HasValue switch
                {
                    true => [request.OperatorId.Value],
                    _ => null
                }
            };

            Result<TransactionSummaryByMerchantResponse> apiResult = await this.EstateReportingApiClient.GetMerchantTransactionSummary(token.Data, request.EstateId, apiRequest, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            TransactionModels.TransactionSummaryByMerchantResponse transactionSummaryByMerchantResponseModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(transactionSummaryByMerchantResponseModel);
        }

        public async Task<Result<TransactionModels.TransactionSummaryByOperatorResponse>> GetOperatorTransactionSummary(TransactionQueries.GetOperatorTransactionSummaryQuery request,
                                                                                                                        CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            TransactionSummaryByOperatorRequest apiRequest = new TransactionSummaryByOperatorRequest
            {
                EndDate = request.EndDate,
                StartDate = request.StartDate,
                Merchants = request.MerchantId.HasValue switch
                {
                    true => [request.MerchantId.Value],
                    _ => null
                },
                Operators = request.OperatorId.HasValue switch
                {
                    true => [request.OperatorId.Value],
                    _ => null
                }
            };

            Result<TransactionSummaryByOperatorResponse> apiResult = await this.EstateReportingApiClient.GetOperatorTransactionSummary(token.Data, request.EstateId, apiRequest, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            TransactionModels.TransactionSummaryByOperatorResponse transactionSummaryByOperatorResponseModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(transactionSummaryByOperatorResponseModel);
        }
    }
}
