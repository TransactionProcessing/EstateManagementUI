using EstateManagementUI.BusinessLogic.Models;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Clients
{
    public interface IApiClient {
        Task<Result<List<FileImportLogModel>>> GetFileImportLogList(String accessToken,
                                                                    Guid actionId,
                                                                    Guid estateId,
                                                                    Guid merchantId,
                                                                    DateTime startDate,
                                                                    DateTime endDate,
                                                                    CancellationToken cancellationToken);

        Task<EstateModel> GetEstate(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    CancellationToken cancellationToken);

        Task<List<MerchantModel>> GetMerchants(String accessToken,
                                               Guid actionId,
                                               Guid estateId,
                                               CancellationToken cancellationToken);

        Task<Result<List<OperatorModel>>> GetOperators(String accessToken,
                                                       Guid actionId,
                                                       Guid estateId,
                                                       CancellationToken cancellationToken);

        Task<Result<OperatorModel>> GetOperator(String accessToken,
                                                       Guid actionId,
                                                       Guid estateId,
                                                       Guid operatorId,
                                                       CancellationToken cancellationToken);

        Task<List<ContractModel>> GetContracts(String accessToken,
                                               Guid actionId,
                                               Guid estateId,
                                               CancellationToken cancellationToken);

        Task<Result<ContractModel>> GetContract(String accessToken,
                                                        Guid actionId,
                                                        Guid estateId,
                                                        Guid contractId,
                                                        CancellationToken cancellationToken);

        Task<Result> CreateOperator(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    CreateOperatorModel createOperatorModel,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateOperator(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    UpdateOperatorModel updateOperatorModel,
                                    CancellationToken cancellationToken);

        Task<Result<FileImportLogModel>> GetFileImportLog(String accessToken,
                                                                    Guid actionId,
                                                                    Guid estateId,
                                                                    Guid merchantId,
                                                                    Guid fileImportLogId,
                                                                    CancellationToken cancellationToken);

        Task<Result<FileDetailsModel>> GetFileDetails(String accessToken,
                                                      Guid actionId,
                                                      Guid estateId,
                                                      Guid fileId,
                                                      CancellationToken cancellationToken);
    }
}
