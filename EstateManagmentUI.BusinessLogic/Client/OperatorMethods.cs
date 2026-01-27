using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionProcessor.DataTransferObjects.Requests.Operator;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<List<OperatorModel>>> GetOperators(OperatorQueries.GetOperatorsQuery request,
                                                                     CancellationToken cancellationToken);

        Task<Result<OperatorModel>> GetOperator(OperatorQueries.GetOperatorQuery request,
                                                CancellationToken cancellationToken);

        Task<Result> UpdateOperator(OperatorCommands.UpdateOperatorCommand request,
                                                CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        public async Task<Result<List<OperatorModel>>> GetOperators(OperatorQueries.GetOperatorsQuery request,
                                                                    CancellationToken cancellationToken) {
            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetOperators(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<OperatorModel> operatorModels = apiResult.Data.ToOperator();

            return Result.Success(operatorModels);
        }

        public async Task<Result<OperatorModel>> GetOperator(OperatorQueries.GetOperatorQuery request,
                                                                    CancellationToken cancellationToken)
        {
            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiResult = await this.EstateReportingApiClient.GetOperator(token.Data, request.EstateId, request.OperatorId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            OperatorModel operatorModels = apiResult.Data.ToOperator();

            return Result.Success(operatorModels);
        }

        public async Task<Result> UpdateOperator(OperatorCommands.UpdateOperatorCommand request,
                                                 CancellationToken cancellationToken) {
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            var apiRequest = new UpdateOperatorRequest() { Name= request.Name, RequireCustomMerchantNumber = request.RequireCustomMerchantNumber, RequireCustomTerminalNumber = request.RequireCustomTerminalNumber};

            var apiResult = await this.TransactionProcessorClient.UpdateOperator(token.Data, request.EstateId, request.OperatorId, apiRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }
    }
}
