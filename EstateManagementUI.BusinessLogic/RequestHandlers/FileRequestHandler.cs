using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class FileRequestHandler : IRequestHandler<Queries.GetFileImportLogsList, Result<List<FileImportLogModel>>> {

    private readonly IApiClient ApiClient;
    public FileRequestHandler(IApiClient apiClient) {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<FileImportLogModel>>> Handle(Queries.GetFileImportLogsList request,
                                                               CancellationToken cancellationToken) {
        Result<List<FileImportLogModel>> models = await this.ApiClient.GetFileImportLogList(request.AccessToken, Guid.Empty, request.EstateId,
            request.MerchantId, request.StartDate.Date, request.EndDate.Date, cancellationToken);
        return models;
    }
}