using EstateManagementUI.BusinessLogic.Models;
using EstateReportingAPI.DataTransferObjects;
using Microsoft.EntityFrameworkCore;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.Clients
{
    public interface IApiClient {

        Task<Result> CreateContract(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    CreateContractModel createContractModel,
                                    CancellationToken cancellationToken);

        Task<Result> AssignOperatorToMerchant(String accessToken,
                                      Guid actionId,
                                      Guid estateId,
                                      Guid merchantId,
                                      AssignOperatorToMerchantModel assignOperatorToMerchantModel,
                                      CancellationToken cancellationToken);

        Task<Result> AssignContractToMerchant(String accessToken,
                                              Guid actionId,
                                              Guid estateId,
                                              Guid merchantId,
                                              AssignContractToMerchantModel assignContractToMerchantModel,
                                              CancellationToken cancellationToken);

        Task<Result> RemoveOperatorFromMerchant(String accessToken,
                                              Guid actionId,
                                              Guid estateId,
                                              Guid merchantId,
                                              Guid operatorId,
                                              CancellationToken cancellationToken);

        Task<Result> RemoveContractFromMerchant(String accessToken,
                                                Guid actionId,
                                                Guid estateId,
                                                Guid merchantId,
                                                Guid contractId,
                                                CancellationToken cancellationToken);


        Task<Result<List<FileImportLogModel>>> GetFileImportLogList(String accessToken,
                                                                    Guid actionId,
                                                                    Guid estateId,
                                                                    Guid merchantId,
                                                                    DateTime startDate,
                                                                    DateTime endDate,
                                                                    CancellationToken cancellationToken);

        Task<Result<EstateModel>> GetEstate(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    CancellationToken cancellationToken);

        Task<Result<List<MerchantModel>>> GetMerchants(String accessToken,
                                               Guid actionId,
                                               Guid estateId,
                                               CancellationToken cancellationToken);

        Task<Result<MerchantModel>> GetMerchant(String accessToken,
                                                       Guid actionId,
                                                       Guid estateId,
                                                       Guid merchantId,
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

        Task<Result<List<ContractModel>>> GetContracts(String accessToken,
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

        Task<Result<List<ComparisonDateModel>>> GetComparisonDates(String accessToken,
                                                                   Guid actionId,
                                                                   Guid estateId,
                                                                   CancellationToken cancellationToken);

        Task<Result<TodaysSalesModel>> GetTodaysSales(String accessToken,
                                                      Guid actionId,
                                                      Guid estateId,
                                                      Int32? merchantReportingId,
                                                      Int32? operatorReportingId,
                                                      DateTime comparisonDate,
                                                      CancellationToken cancellationToken);

        Task<Result<List<TodaysSalesCountByHourModel>>> GetTodaysSalesCountByHour(String accessToken,
            Guid actionId,
            Guid estateId,
            Guid? merchantId,
            Guid? operatorId,
            DateTime comparisonDate,
            CancellationToken cancellationToken);

        Task<Result<List<TodaysSalesValueByHourModel>>> GetTodaysSalesValueByHour(String accessToken,
            Guid actionId,
            Guid estateId,
            Guid? merchantId,
            Guid? operatorId,
            DateTime comparisonDate,
            CancellationToken cancellationToken);

        Task<Result<TodaysSettlementModel>> GetTodaysSettlement(String accessToken,
                                                                Guid actionId,
                                                                Guid estateId,
                                                                Int32? merchantReportingId,
                                                                Int32? operatorReportingId,
                                                                DateTime comparisonDate,
                                                                CancellationToken cancellationToken);

        Task<Result<TodaysSalesModel>> GetTodaysFailedSales(
            string accessToken,
            Guid estateId,
            string responseCode,
            DateTime comparisonDate,
            CancellationToken cancellationToken);

        Task<Result<List<TopBottomOperatorDataModel>>> GetTopBottomOperatorData(
            string accessToken,
            Guid estateId,
            TopBottom topBottom,
            int resultCount,
            CancellationToken cancellationToken);

        Task<Result<List<TopBottomMerchantDataModel>>> GetTopBottomMerchantData(
            string accessToken,
            Guid estateId,
            TopBottom topBottom,
            int resultCount,
            CancellationToken cancellationToken);

        Task<Result<List<TopBottomProductDataModel>>> GetTopBottomProductData(
            string accessToken,
            Guid estateId,
            TopBottom topBottom,
            int resultCount,
            CancellationToken cancellationToken);

        Task<Result<MerchantKpiModel>> GetMerchantKpi(String accessToken, Guid estateId, CancellationToken cancellationToken);

        Task<Result<LastSettlementModel>> GetLastSettlement(
            string accessToken,
            Guid estateId,
            Int32? merchantReportingId,
            Int32? operatorReportingId,
            CancellationToken cancellationToken);

        Task<Result> CreateMerchant(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    CreateMerchantModel createMerchantModel,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateMerchant(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    Guid merchantId,
                                    UpdateMerchantModel updateMerchantModel,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateMerchantAddress(String accessToken,
                                    Guid actionId,
                                    Guid estateId,
                                    Guid merchantId,
                                    AddressModel updateAddressModel,
                                    CancellationToken cancellationToken);

        Task<Result> UpdateMerchantContact(String accessToken,
                                           Guid actionId,
                                           Guid estateId,
                                           Guid merchantId,
                                           ContactModel updateContactModel,
                                           CancellationToken cancellationToken);

        Task<Result> MakeDeposit(String accessToken,
                                     Guid actionId,
                                     Guid estateId,
                                     Guid merchantId,
                                     MakeDepositModel makeDepositModel,
                                     CancellationToken cancellationToken);

        Task<Result> CreateContractProduct(String accessToken,
                                           Guid actionId,
                                           Guid estateId,
                                           Guid contractId,
                                           CreateContractProductModel createContractProductModel,
                                           CancellationToken cancellationToken);

        Task<Result> CreateContractProductTransactionFee(String accessToken,
                                                         Guid actionId,
                                                         Guid estateId,
                                                         Guid contractId,
                                                         Guid productId,
                                                         CreateContractProductTransactionFeeModel createContractProductTransactionFeeModel,
                                                         CancellationToken cancellationToken);

    }
}
