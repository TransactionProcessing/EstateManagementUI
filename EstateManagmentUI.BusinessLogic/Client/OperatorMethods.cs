using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<List<OperatorModel>>> GetOperators(OperatorQueries.GetOperatorsQuery request,
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

            List<OperatorModel> operatorModels = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(operatorModels);
        }
    }
}
