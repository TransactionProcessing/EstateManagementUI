using EstateManagementUI.BusinessLogic.Models;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<MerchantKpiModel>> GetMerchantKpi(String accessToken, Guid actionId, Guid estateId, CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        public async Task<Result<MerchantKpiModel>> GetMerchantKpi(String accessToken,
                                                                   Guid actionId,
                                                                   Guid estateId,
                                                                   CancellationToken cancellationToken) {
            var apiResult = await this.EstateReportingApiClient.GetMerchantKpi(accessToken, estateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            MerchantKpiModel merchantKpiModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(merchantKpiModel);
        }
    }
}
