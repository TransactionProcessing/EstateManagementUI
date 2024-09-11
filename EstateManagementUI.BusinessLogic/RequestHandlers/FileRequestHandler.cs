using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class FileRequestHandler : IRequestHandler<Queries.GetFileImportLogsListQuery, Result<List<FileImportLogModel>>>,
                                  IRequestHandler<Queries.GetFileImportLogQuery, Result<FileImportLogModel>>,
                                  IRequestHandler<Queries.GetFileDetailsQuery, Result<FileDetailsModel>> {

    private readonly IApiClient ApiClient;
    public FileRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<FileImportLogModel>>> Handle(Queries.GetFileImportLogsListQuery request,
                                                               CancellationToken cancellationToken) {
        Result<List<FileImportLogModel>> models = await this.ApiClient.GetFileImportLogList(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.StartDate.Date, request.EndDate.Date, cancellationToken);
        return models;
    }

    public async Task<Result<FileImportLogModel>> Handle(Queries.GetFileImportLogQuery request, CancellationToken cancellationToken) {
        Result<FileImportLogModel> model = await this.ApiClient.GetFileImportLog(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.FileImportLogId, cancellationToken);
        return model;
    }

    public async Task<Result<FileDetailsModel>> Handle(Queries.GetFileDetailsQuery request,
                                                       CancellationToken cancellationToken) {
        Result<FileDetailsModel> model = await this.ApiClient.GetFileDetails(request.AccessToken, Guid.Empty, request.EstateId,
            request.FileId, cancellationToken);
        return model;
    }
}