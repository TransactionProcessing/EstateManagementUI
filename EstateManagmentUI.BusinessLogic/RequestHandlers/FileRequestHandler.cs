using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using FileProcessor.DataTransferObjects.Responses;
using MediatR;
using Shared.Results;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class FileRequestHandler : IRequestHandler<FileProcessingQueries.GetFileImportLogsListQuery, Result<List<FileProcessingModels.FileImportLogDetailsModel>>>,
    IRequestHandler<FileProcessingQueries.GetFileImportLogQuery, Result<FileProcessingModels.FileImportLogDetailsModel>>
    {

    private readonly IApiClient ApiClient;

    public FileRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }
    
    public async Task<Result<List<FileProcessingModels.FileImportLogDetailsModel>>> Handle(FileProcessingQueries.GetFileImportLogsListQuery request,
                                                                                           CancellationToken cancellationToken) {
        return await this.ApiClient.GetFileImportLogsList(request, cancellationToken);
    }

    public async Task<Result<FileProcessingModels.FileImportLogDetailsModel>> Handle(FileProcessingQueries.GetFileImportLogQuery request,
                                                                                     CancellationToken cancellationToken) {
        return await this.ApiClient.GetFileImportLog(request, cancellationToken);
    }
}