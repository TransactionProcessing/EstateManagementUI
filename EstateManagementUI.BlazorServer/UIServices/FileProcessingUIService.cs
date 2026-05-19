using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.UIServices
{
    public interface IFileProcessingUIService
    {
        // Get Import Log List - start date and end date filter
        // GetImportLog - ImportLogId and filename ?
        // Upload file for processing - file and filename
        Task<Result<List<FileImportLogDetailsModel>>> GetImportLogList(CorrelationId correlationId,
                                                                       Guid estateId,
                                                                       Guid? merchantId,
                                                                       DateTime startDate,
                                                                       DateTime endDate);
        Task<Result<FileImportLogDetailsModel>> GetImportLog(CorrelationId correlationId,
                                                             Guid estateId,
                                                             Guid? merchantId,
                                                             Guid importLogId);
        Task<Result> UploadFileAsync(Stream fileStream, String fileName);
    }


    public class FileProcessingUIService : IFileProcessingUIService
    {
        private readonly IMediator Mediator;

        public FileProcessingUIService(IMediator mediator) {
            this.Mediator = mediator;
        }
        
        public async Task<Result<List<FileImportLogDetailsModel>>> GetImportLogList(CorrelationId correlationId,
                                                                                    Guid estateId,
                                                                                    Guid? merchantId, 
                                                                                    DateTime startDate,
                                                                                   DateTime endDate) {
            FileProcessingQueries.GetFileImportLogsListQuery query = new(correlationId, estateId, merchantId, startDate, endDate);

            Result<List<FileProcessingModels.FileImportLogDetailsModel>> result = await this.Mediator.Send(query);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            List<FileImportLogDetailsModel> fileImportLogList = ModelFactory.ConvertFrom(result.Data);
            return Result.Success(fileImportLogList);
        }


        public async Task<Result<FileImportLogDetailsModel>> GetImportLog(CorrelationId correlationId,
                                                                          Guid estateId,
                                                                          Guid? merchantId, 
                                                                          Guid importLogId) {
            FileProcessingQueries.GetFileImportLogQuery query = new(correlationId, estateId, merchantId, importLogId);

            Result<FileProcessingModels.FileImportLogDetailsModel> result = await this.Mediator.Send(query);
            if (result.IsFailed)
                return ResultHelpers.CreateFailure(result);
            FileImportLogDetailsModel fileImportLog = ModelFactory.ConvertFrom(result.Data);
            return Result.Success(fileImportLog);
        }

        public async Task<Result> UploadFileAsync(Stream fileStream,
                                          String fileName) {
            throw new NotImplementedException();
        }
    }
}
