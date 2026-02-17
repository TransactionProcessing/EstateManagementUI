using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<TodaysSettlementModel>> GetTodaysSettlement(SettlementQueries.GetTodaysSettlementQuery request, CancellationToken cancellationToken);
    }

    public partial class ApiClient {
        public async Task<Result<TodaysSettlementModel>> GetTodaysSettlement(SettlementQueries.GetTodaysSettlementQuery request,
                                                                             CancellationToken cancellationToken) {
            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetTodaysSettlement(token.Data, request.EstateId, request.ComparisonDate, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            TodaysSettlementModel todaysSettlementModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(todaysSettlementModel);
        }
    }
}
