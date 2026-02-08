using EstateManagementUI.BusinessLogic.BackendAPI;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using SecurityService.Client;
using SecurityService.DataTransferObjects.Responses;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Shared.General;
using TransactionProcessor.Client;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient {
        Task<Result<List<ComparisonDateModel>>> GetComparisonDates(Queries.GetComparisonDatesQuery request,
                                                                   CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        private readonly IEstateReportingApiClient EstateReportingApiClient;
        private readonly ISecurityServiceClient SecurityServiceClient;
        private readonly ITransactionProcessorClient TransactionProcessorClient;

        public ApiClient(IEstateReportingApiClient estateReportingApiClient, 
                         ISecurityServiceClient securityServiceClient,
                         ITransactionProcessorClient transactionProcessorClient) {
            this.EstateReportingApiClient = estateReportingApiClient;
            this.SecurityServiceClient = securityServiceClient;
            this.TransactionProcessorClient = transactionProcessorClient;
        }
        public async Task<Result<List<ComparisonDateModel>>> GetComparisonDates(Queries.GetComparisonDatesQuery request,
                                                                                CancellationToken cancellationToken) {

            // Get a token here 
            var token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);
            Result<List<ComparisonDate>> apiResult = await this.EstateReportingApiClient.GetComparisonDates(token.Data, request.EstateId, cancellationToken);

            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<ComparisonDateModel> comparisonDates = APIModelFactory.ConvertFrom(apiResult.Data);

            return Result.Success(comparisonDates);
        }

        private async Task<Result<String>> GetToken(CancellationToken cancellationToken) {
            // Get a token here 
            // TODO: Add caching
            var clientId = ConfigurationReader.GetValueOrDefault("AppSettings", "ClientId", "");
            var clientSecret = ConfigurationReader.GetValueOrDefault("AppSettings", "ClientSecret", "");
            Result<TokenResponse>? token = await this.SecurityServiceClient.GetToken(clientId, clientSecret, cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);
            return Result.Success<String>(token.Data.AccessToken);
        }
    }
}
