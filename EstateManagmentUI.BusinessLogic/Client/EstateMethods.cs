using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using Shared.Results;
using TransactionProcessor.DataTransferObjects.Requests.Estate;
using TransactionProcessor.DataTransferObjects.Responses.Estate;

namespace EstateManagementUI.BusinessLogic.Client {
    public partial interface IApiClient {
        Task<Result<EstateModel>> GetEstate(EstateQueries.GetEstateQuery request,
                                            CancellationToken cancellationToken);

        Task<Result<List<OperatorModel>>> GetEstateAssignedOperators(Queries.GetAssignedOperatorsQuery request,
                                                                           CancellationToken cancellationToken);

        Task<Result> RemoveEstateOperator(EstateCommands.RemoveOperatorFromEstateCommand request,
                                            CancellationToken cancellationToken);
        Task<Result> AddEstateOperator(EstateCommands.AddOperatorToEstateCommand request,
                                       CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {

        public async Task<Result<EstateModel>> GetEstate(EstateQueries.GetEstateQuery request,
                                                         CancellationToken cancellationToken) {
            // Get a token here 
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<Estate>? apiResult = await this.EstateReportingApiClient.GetEstate(token.Data, request.EstateId, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            EstateModel estate = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(estate);
        }

        public async Task<Result<List<OperatorModel>>> GetEstateAssignedOperators(Queries.GetAssignedOperatorsQuery request,
                                                                                        CancellationToken cancellationToken) {
            // Get a token here 
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<EstateOperator>>? apiResult = await this.EstateReportingApiClient.GetEstateAssignedOperators(token.Data, request.EstateId, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<OperatorModel> estateOperators = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(estateOperators);
        }

        public async Task<Result> RemoveEstateOperator(EstateCommands.RemoveOperatorFromEstateCommand request,
                                                       CancellationToken cancellationToken) {
            // Get a token here 
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result? apiResult = await this.TransactionProcessorClient.RemoveOperatorFromEstate(token.Data, request.EstateId, request.OperatorId, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }

        public async Task<Result> AddEstateOperator(EstateCommands.AddOperatorToEstateCommand request,
                                                    CancellationToken cancellationToken)
        {
            // Get a token here 
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            AssignOperatorRequest assignOperatorRequest = new AssignOperatorRequest
            {
                OperatorId = request.OperatorId
            };

            Result? apiResult = await this.TransactionProcessorClient.AssignOperatorToEstate(token.Data, request.EstateId, assignOperatorRequest, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            return Result.Success();
        }
    }
}
