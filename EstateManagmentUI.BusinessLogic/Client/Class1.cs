using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using FileProcessor.DataTransferObjects.Responses;
using Microsoft.AspNetCore.Http;
using Shared.Results;
using SimpleResults;
using System;
using System.Collections.Generic;
using System.Text;

namespace EstateManagementUI.BusinessLogic.Client {
    public partial interface IApiClient {
        Task<Result<List<FileProcessingModels.FileImportLogDetailsModel>>> GetFileImportLogsList(FileProcessingQueries.GetFileImportLogsListQuery query,
                                                                                                 CancellationToken cancellationToken);
         Task<Result<FileProcessingModels.FileImportLogDetailsModel>> GetFileImportLog(FileProcessingQueries.GetFileImportLogQuery query,
                                                                                       CancellationToken cancellationToken);
    }

    public partial class ApiClient : IApiClient {
        public async Task<Result<List<FileProcessingModels.FileImportLogDetailsModel>>> GetFileImportLogsList(FileProcessingQueries.GetFileImportLogsListQuery query,
                                                                                                        CancellationToken cancellationToken) {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<List<BackendAPI.FileImportLog>> apiResult = await this.EstateReportingApiClient.GetFileImportLogsList(token.Data, query.EstateId, query.MerchantId , query.StartDate, query.EndDate, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);

            List<FileProcessingModels.FileImportLogDetailsModel> fileImportLogList = apiResult.Data.ToFileImportLogList();

            return Result.Success(fileImportLogList);

        }

        public async Task<Result<FileProcessingModels.FileImportLogDetailsModel>> GetFileImportLog(FileProcessingQueries.GetFileImportLogQuery query,
                                                                                                   CancellationToken cancellationToken)
        {
            Result<String> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
                return ResultHelpers.CreateFailure(token);

            Result<BackendAPI.FileImportLog> apiResult = await this.EstateReportingApiClient.GetFileImportLog(token.Data, query.EstateId, null, query.FileImportLogId, cancellationToken);
            if (apiResult.IsFailed)
                return ResultHelpers.CreateFailure(apiResult);
            
            FileProcessingModels.FileImportLogDetailsModel fileImportLog = apiResult.Data.ToFileImportLog();

            return Result.Success(fileImportLog);
        }
    }
}
