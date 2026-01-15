using EstateManagementUI.BusinessLogic.Models;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using Shared.Results;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient {
        Task<Result<List<ComparisonDateModel>>> GetComparisonDates(String accessToken,
                                                                   Guid actionId,
                                                                   Guid estateId,
                                                                   CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        private readonly IEstateReportingApiClient EstateReportingApiClient;

        public ApiClient(IEstateReportingApiClient estateReportingApiClient) {
            this.EstateReportingApiClient = estateReportingApiClient;
        }
        public async Task<Result<List<ComparisonDateModel>>> GetComparisonDates(String accessToken,
                                                                                Guid actionId,
                                                                                Guid estateId,
                                                                                CancellationToken cancellationToken) {
            Result<List<ComparisonDate>> apiResult = await this.EstateReportingApiClient.GetComparisonDates(accessToken, estateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<ComparisonDateModel> comparisonDates = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(comparisonDates);
        }
    }
}
