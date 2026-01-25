using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;

namespace EstateManagementUI.BusinessLogic.Client {
    public partial interface IApiClient {
        Task<Result<List<RecentContractModel>>> GetRecentContracts(ContractQueries.GetRecentContractsQuery request,
                                                                   CancellationToken cancellationToken);

        Task<Result<List<ContractDropDownModel>>> GetContracts(ContractQueries.GetContractsForDropDownQuery request,
                                                               CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        public async Task<Result<List<RecentContractModel>>> GetRecentContracts(ContractQueries.GetRecentContractsQuery request,
                                                                                CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Contract>> apiResult = await this.EstateReportingApiClient.GetRecentContracts(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<RecentContractModel> recentContractModels = apiResult.Data.ToRecentContracts();

            return Result.Success(recentContractModels);
        }

        public async Task<Result<List<ContractDropDownModel>>> GetContracts(ContractQueries.GetContractsForDropDownQuery request,
                                                                    CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Contract>> apiResult = await this.EstateReportingApiClient.GetContracts(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<ContractDropDownModel> contractModels = apiResult.Data.ToContractDropDown();

            return Result.Success(contractModels);
        }
    }
}
