using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using SecurityService.DataTransferObjects.Responses;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<MerchantKpiModel>> GetMerchantKpi(Queries.GetMerchantKpiQuery request, CancellationToken cancellationToken);
        Task<Result<List<RecentMerchantsModel>>> GetRecentMerchants(Queries.GetRecentMerchantsQuery request, CancellationToken cancellationToken);
        Task<Result<List<RecentContractModel>>> GetRecentContracts(Queries.GetRecentContractsQuery request, CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        public async Task<Result<MerchantKpiModel>> GetMerchantKpi(Queries.GetMerchantKpiQuery request,
                                                                   CancellationToken cancellationToken) {

            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetMerchantKpi(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            MerchantKpiModel merchantKpiModel = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(merchantKpiModel);
        }

        public async Task<Result<List<RecentMerchantsModel>>> GetRecentMerchants(Queries.GetRecentMerchantsQuery request,
                                                                             CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Merchant>> apiResult = await this.EstateReportingApiClient.GetRecentMerchants(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<RecentMerchantsModel> recentMerchantsModels = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(recentMerchantsModels);
        }

        public async Task<Result<List<RecentContractModel>>> GetRecentContracts(Queries.GetRecentContractsQuery request,
                                                                                CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Contract>> apiResult = await this.EstateReportingApiClient.GetRecentContracts(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<RecentContractModel> recentContractModels = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(recentContractModels);
        }
    }
}
