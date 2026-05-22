using EstateManagementUI.BusinessLogic.Models;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Requests;

public static class FileProcessingQueries
{
    public record GetFileImportLogsListQuery(CorrelationId CorrelationId, Guid EstateId, Guid? MerchantId, DateTime StartDate, DateTime EndDate)
        : IRequest<Result<List<FileProcessingModels.FileImportLogDetailsModel>>>;
    public record GetFileImportLogQuery(CorrelationId CorrelationId, Guid EstateId, Guid? MerchantId, Guid FileImportLogId)
        : IRequest<Result<FileProcessingModels.FileImportLogDetailsModel>>;
}