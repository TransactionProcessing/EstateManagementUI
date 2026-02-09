using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class FileRequestHandler : IRequestHandler<Queries.GetFileImportLogsListQuery, Result<List<FileImportLogModel>>>, 
    IRequestHandler<Queries.GetFileImportLogQuery, Result<FileImportLogModel>>, 
    IRequestHandler<Queries.GetFileDetailsQuery, Result<FileDetailsModel>> {

    private readonly IApiClient ApiClient;

    public FileRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<FileImportLogModel>>> Handle(Queries.GetFileImportLogsListQuery request,
                                                               CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockFileImportLogs());
    }

    public async Task<Result<FileImportLogModel>> Handle(Queries.GetFileImportLogQuery request,
                                                         CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockFileImportLog());
    }

    public async Task<Result<FileDetailsModel>> Handle(Queries.GetFileDetailsQuery request,
                                                       CancellationToken cancellationToken) {
        return Result.Success(StubTestData.GetMockFileDetails());
    }
}