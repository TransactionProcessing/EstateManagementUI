using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using FileProcessor.Client;
using FileProcessor.DataTransferObjects;
using Shared.General;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Client
{
    public partial interface IApiClient
    {
        Task<Result<List<FileProcessor.Models.FileProfile>>> GetFileProfiles(CancellationToken cancellationToken = default);

        Task<Result<Guid>> UploadFileAsync(Guid estateId,
                                           Guid merchantId,
                                           Guid userId,
                                           Guid fileProfileId,
                                           Stream fileStream,
                                           string fileName,
                                           CancellationToken cancellationToken = default);
    }

    public partial class ApiClient
    {
        public async Task<Result<List<FileProcessor.Models.FileProfile>>> GetFileProfiles(CancellationToken cancellationToken = default)
        {
            Result<string> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
            {
                return ResultHelpers.CreateFailure(token);
            }

            Result<List<FileProcessor.Models.FileProfile>> result = await this.FileProcessorClient.GetFileProfiles(token.Data, cancellationToken);
            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            return Result.Success(result.Data);
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

            if (string.IsNullOrWhiteSpace(fileName))
            {
                return Result.Failure("A file name is required.");
            }

            Result<string> token = await this.GetToken(cancellationToken);
            if (token.IsFailed)
            {
                return ResultHelpers.CreateFailure(token);
            }

            byte[] fileBytes;
            using (MemoryStream memoryStream = new())
            {
                await fileStream.CopyToAsync(memoryStream, cancellationToken);
                fileBytes = memoryStream.ToArray();
            }

            UploadFileRequest request = new()
            {
                FileId = Guid.NewGuid(),
                EstateId = estateId,
                MerchantId = merchantId,
                UserId = userId,
                FileProfileId = fileProfileId,
                UploadDateTime = DateTime.UtcNow
            };

            Result<Guid> result = await this.FileProcessorClient.UploadFile(token.Data, fileName, fileBytes, request, cancellationToken);
            if (result.IsFailed)
            {
                return ResultHelpers.CreateFailure(result);
            }

            return Result.Success(result.Data);
        }
    }
}
