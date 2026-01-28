using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using TransactionProcessor.DataTransferObjects.Requests.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Contract;

namespace EstateManagementUI.BusinessLogic.Client {
    public partial interface IApiClient {
        Task<Result<List<RecentContractModel>>> GetRecentContracts(ContractQueries.GetRecentContractsQuery request,
                                                                   CancellationToken cancellationToken);

        Task<Result<List<ContractDropDownModel>>> GetContracts(ContractQueries.GetContractsForDropDownQuery request,
                                                               CancellationToken cancellationToken);
        Task<Result<List<ContractModel>>> GetContracts(ContractQueries.GetContractsQuery request,
                                                               CancellationToken cancellationToken);
        Task<Result<ContractModel>> GetContract(ContractQueries.GetContractQuery request,
                                                       CancellationToken cancellationToken);

        Task<Result> CreateContract(ContractCommands.CreateContractCommand request,
                                    CancellationToken cancellationToken);
        Task<Result> AddProductToContract(ContractCommands.AddProductToContractCommand request,
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

        public async Task<Result<List<ContractModel>>> GetContracts(ContractQueries.GetContractsQuery request,
                                                                            CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<Contract>> apiResult = await this.EstateReportingApiClient.GetContracts(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<ContractModel> contractModels = apiResult.Data.ToContract();

            return Result.Success(contractModels);
        }

        public async Task<Result<ContractModel>> GetContract(ContractQueries.GetContractQuery request,
                                                                    CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<Contract> apiResult = await this.EstateReportingApiClient.GetContract(token.Data, request.EstateId, request.ContractId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            ContractModel contractModel = apiResult.Data.ToContract();

            return Result.Success(contractModel);
        }

        public async Task<Result> CreateContract(ContractCommands.CreateContractCommand request,
                                                 CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiRequest = new CreateContractRequest() { Description = request.Description, OperatorId = request.OperatorId };

            var apiResult = await this.TransactionProcessorClient.CreateContract(token.Data, request.EstateId, apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> AddProductToContract(ContractCommands.AddProductToContractCommand request,
                                                       CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiRequest = new AddProductToContractRequest() { ProductType = ProductType.NotSet, Value = request.Value, DisplayText = request.DisplayText, ProductName = request.ProductName};

            var apiResult = await this.TransactionProcessorClient.AddProductToContract(token.Data, request.EstateId, request.ContractId, apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }
    }
}
