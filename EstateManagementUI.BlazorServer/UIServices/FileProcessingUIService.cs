using EstateManagementUI.BlazorServer.Factories;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Client;
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
        Task<Result<List<FileProfileDropDownModel>>> GetFileProfiles(CancellationToken cancellationToken = default);
        Task<Result<List<FileImportLogDetailsModel>>> GetImportLogList(CorrelationId correlationId,
                                                                       Guid estateId,
                                                                       Guid? merchantId,
                                                                       DateTime startDate,
                                                                       DateTime endDate);
        Task<Result<FileImportLogDetailsModel>> GetImportLog(CorrelationId correlationId,
                                                             Guid estateId,
                                                             Guid? merchantId,
                                                             Guid importLogId);
        Task<Result<Guid>> UploadFileAsync(Guid estateId,
                                           Guid merchantId,
                                           Guid userId,
                                           Guid fileProfileId,
                                           Stream fileStream,
                                           string fileName,
                                           CancellationToken cancellationToken = default);
    }

    public class FileProcessingUIService : IFileProcessingUIService
    {
        private readonly IMediator Mediator;
        private readonly IApiClient ApiClient;

        public FileProcessingUIService(IMediator mediator, IApiClient apiClient)
        {
            this.Mediator = mediator;
            this.ApiClient = apiClient;
        }

        public async Task<Result<List<FileProfileDropDownModel>>> GetFileProfiles(CancellationToken cancellationToken = default)
        {
            Result<List<FileProcessor.Models.FileProfile>> result = await this.ApiClient.GetFileProfiles(cancellationToken);
            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            List<FileProfileDropDownModel> fileProfiles = result.Data
                .Where(profile => !string.IsNullOrWhiteSpace(profile.Name))
                .Select(profile => new FileProfileDropDownModel
                {
                    FileProfileId = profile.FileProfileId,
                    Name = profile.Name
                })
                .DistinctBy(profile => profile.FileProfileId)
                .OrderBy(profile => profile.Name)
                .ToList();

            return Result.Success(fileProfiles);
        }

        public async Task<Result<List<FileImportLogDetailsModel>>> GetImportLogList(CorrelationId correlationId,
                                                                                    Guid estateId,
                                                                                    Guid? merchantId,
                                                                                    DateTime startDate,
                                                                                    DateTime endDate)
        {
            FileProcessingQueries.GetFileImportLogsListQuery query = new(correlationId, estateId, merchantId, startDate, endDate);

            Result<List<FileProcessingModels.FileImportLogDetailsModel>> result = await this.Mediator.Send(query);
            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            List<FileImportLogDetailsModel> fileImportLogList = ModelFactory.ConvertFrom(result.Data);
            return Result.Success(fileImportLogList);
        }

        public async Task<Result<FileImportLogDetailsModel>> GetImportLog(CorrelationId correlationId,
                                                                          Guid estateId,
                                                                          Guid? merchantId,
                                                                          Guid importLogId)
        {
            FileProcessingQueries.GetFileImportLogQuery query = new(correlationId, estateId, merchantId, importLogId);

            Result<FileProcessingModels.FileImportLogDetailsModel> result = await this.Mediator.Send(query);
            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            FileImportLogDetailsModel fileImportLog = ModelFactory.ConvertFrom(result.Data);
            return Result.Success(fileImportLog);
        }

        public async Task<Result<Guid>> UploadFileAsync(Guid estateId,
                                                        Guid merchantId,
                                                        Guid userId,
                                                        Guid fileProfileId,
                                                        Stream fileStream,
                                                        string fileName,
                                                        CancellationToken cancellationToken = default)
        {
            if (fileStream == null)
            {
                return Result.Failure("A file stream is required.");
            }

            Result<Guid> uploadResult = await this.ApiClient.UploadFileAsync(estateId,
                                                                              merchantId,
                                                                              userId,
                                                                              fileProfileId,
                                                                              fileStream,
                                                                              fileName,
                                                                              cancellationToken);
            if (uploadResult.IsFailed)
            {
                return ResultHelpers.CreateFailure(uploadResult);
            }

            return Result.Success(uploadResult.Data);
        }
    }
}
