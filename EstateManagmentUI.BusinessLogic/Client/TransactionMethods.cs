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
        Task<Result<TodaysSalesModel>> GetTodaysSales(Queries.GetTodaysSalesQuery request, CancellationToken cancellationToken);
        Task<Result<TodaysSalesModel>> GetTodaysFailedSales(Queries.GetTodaysFailedSalesQuery request, CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        public async Task<Result<TodaysSalesModel>> GetTodaysSales(Queries.GetTodaysSalesQuery request,
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

        public async Task<Result<TodaysSalesModel>> GetTodaysFailedSales(Queries.GetTodaysFailedSalesQuery request,
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
    }
}
