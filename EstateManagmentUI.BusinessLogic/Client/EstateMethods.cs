using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using Shared.Results;
using TransactionProcessor.DataTransferObjects.Responses.Estate;

namespace EstateManagementUI.BusinessLogic.Client {
    public partial interface IApiClient {
        Task<Result<EstateModel>> GetEstate(Queries.GetEstateQuery request,
                                            CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {

        public async Task<Result<EstateModel>> GetEstate(Queries.GetEstateQuery request,
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
    }
}
