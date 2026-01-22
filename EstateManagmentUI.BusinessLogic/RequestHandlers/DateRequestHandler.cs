using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using SimpleResults;

namespace EstateManagementUI.BusinessLogic.RequestHandlers
{
    public class DateRequestHandler : IRequestHandler<Queries.GetComparisonDatesQuery, Result<List<ComparisonDateModel>>>
    {
        private readonly IApiClient ApiClient;
        
        public DateRequestHandler(IApiClient apiClient) {
            this.ApiClient = apiClient;
        }
        public async Task<Result<List<ComparisonDateModel>>> Handle(Queries.GetComparisonDatesQuery request,
                                                                    CancellationToken cancellationToken) {
            return await this.ApiClient.GetComparisonDates(request, cancellationToken);
        }
    }

    public class EstateRequestHandler : IRequestHandler<Queries.GetEstateQuery, Result<EstateModel>>,
        IRequestHandler<Commands.AddOperatorToEstateCommand, Result>,
        IRequestHandler<Commands.RemoveOperatorFromEstateCommand, Result>,
        IRequestHandler<Queries.GetAssignedOperatorsQuery, Result<List<OperatorModel>>> {
        private readonly IApiClient ApiClient;

        public EstateRequestHandler(IApiClient apiClient) {
            this.ApiClient = apiClient;
        }

        public async Task<Result<EstateModel>> Handle(Queries.GetEstateQuery request,
                                                      CancellationToken cancellationToken) {
            return await this.ApiClient.GetEstate(request, cancellationToken);
        }

        public async Task<Result> Handle(Commands.AddOperatorToEstateCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.AddEstateOperator(request, cancellationToken);
        }

        public async Task<Result> Handle(Commands.RemoveOperatorFromEstateCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.RemoveEstateOperator(request, cancellationToken);
        }

        public async Task<Result<List<OperatorModel>>> Handle(Queries.GetAssignedOperatorsQuery request,
                                                              CancellationToken cancellationToken) {
            return await this.ApiClient.GetEstateAssignedOperators(request, cancellationToken);
        }
    }

    public class MerchantRequestHandler : IRequestHandler<Queries.GetMerchantsQuery, Result<List<MerchantListModel>>>,
                                        IRequestHandler<Queries.GetMerchantQuery, Result<MerchantModel>>,
                                        IRequestHandler<Commands.AddMerchantDeviceCommand, Result>,
                                        IRequestHandler<Commands.AddOperatorToMerchantCommand, Result>,
                                        IRequestHandler<Commands.CreateMerchantCommand, Result>,
                                        IRequestHandler<Commands.MakeMerchantDepositCommand, Result>,
                                        IRequestHandler<Commands.RemoveContractFromMerchantCommand, Result>,
                                        IRequestHandler<Commands.RemoveOperatorFromMerchantCommand, Result>,
                                        IRequestHandler<Commands.SetMerchantSettlementScheduleCommand, Result>,
                                        IRequestHandler<Commands.SwapMerchantDeviceCommand, Result>,
                                        IRequestHandler<Commands.UpdateMerchantAddressCommand, Result>,
                                        IRequestHandler<Commands.UpdateMerchantCommand, Result>,
                                        IRequestHandler<Commands.UpdateMerchantContactCommand, Result>,
                                        IRequestHandler<Commands.AssignContractToMerchantCommand, Result>,
                                        IRequestHandler<Queries.GetRecentMerchantsQuery, Result<List<RecentMerchantsModel>>>,
                                        IRequestHandler<Queries.GetMerchantsForDropDownQuery, Result<List<MerchantDropDownModel>>> {
        private readonly IApiClient ApiClient;

        public MerchantRequestHandler(IApiClient apiClient)
        {
            this.ApiClient = apiClient;
        }

        public async Task<Result<List<MerchantListModel>>> Handle(Queries.GetMerchantsQuery request,
                                                                  CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchants(request, cancellationToken);
        }

        public async Task<Result> Handle(Commands.AddMerchantDeviceCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.AddOperatorToMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.CreateMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.MakeMerchantDepositCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.RemoveContractFromMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.RemoveOperatorFromMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.SetMerchantSettlementScheduleCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.SwapMerchantDeviceCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.UpdateMerchantAddressCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.UpdateMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.UpdateMerchantContactCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result<MerchantModel>> Handle(Queries.GetMerchantQuery request,
                                                        CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockMerchant());
        }

        public async Task<Result> Handle(Commands.AssignContractToMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result<List<RecentMerchantsModel>>> Handle(Queries.GetRecentMerchantsQuery request,
                                                        CancellationToken cancellationToken) {
            return await this.ApiClient.GetRecentMerchants(request, cancellationToken);
        }

        public async Task<Result<List<MerchantDropDownModel>>> Handle(Queries.GetMerchantsForDropDownQuery request,
                                                                      CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }
    }

    public class ContractRequestHandler : IRequestHandler<Queries.GetContractsQuery, Result<List<ContractModel>>>,
                                            IRequestHandler<Queries.GetContractQuery, Result<ContractModel>>,
                                            IRequestHandler<Commands.CreateContractCommand, Result>,
                                            IRequestHandler<Commands.AddProductToContractCommand, Result>,
                                            IRequestHandler<Commands.AddTransactionFeeForProductToContractCommand, Result>,
    IRequestHandler<Queries.GetRecentContractsQuery, Result<List<RecentContractModel>>> {

        private readonly IApiClient ApiClient;

        public ContractRequestHandler(IApiClient apiClient)
        {
            this.ApiClient = apiClient;
        }

        public async Task<Result<List<ContractModel>>> Handle(Queries.GetContractsQuery request,
                                                              CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockContracts());
        }

        public async Task<Result<ContractModel>> Handle(Queries.GetContractQuery request,
                                                        CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockContract());
        }

        public async Task<Result> Handle(Commands.CreateContractCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.AddProductToContractCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result> Handle(Commands.AddTransactionFeeForProductToContractCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }

        public async Task<Result<List<RecentContractModel>>> Handle(Queries.GetRecentContractsQuery request,
                                                                    CancellationToken cancellationToken) {
            return await this.ApiClient.GetRecentContracts(request, cancellationToken);
        }
    }

    public class OperatorRequestHandler : IRequestHandler<Queries.GetOperatorsQuery, Result<List<OperatorModel>>>,
                                            IRequestHandler<Queries.GetOperatorQuery, Result<OperatorModel>>,
                                            IRequestHandler<Commands.CreateOperatorCommand, Result>,
                                            IRequestHandler<Commands.UpdateOperatorCommand, Result>
    {
        private readonly IApiClient ApiClient;

        public OperatorRequestHandler(IApiClient apiClient)
        {
            this.ApiClient = apiClient;
        }

        public async Task<Result<List<OperatorModel>>> Handle(Queries.GetOperatorsQuery request,
                                                              CancellationToken cancellationToken) {
            return await this.ApiClient.GetOperators(request, cancellationToken);
        }
        public async Task<Result<OperatorModel>> Handle(Queries.GetOperatorQuery request,
                                                        CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockOperator());
        }
        public async Task<Result> Handle(Commands.CreateOperatorCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }
        public async Task<Result> Handle(Commands.UpdateOperatorCommand request,
                                         CancellationToken cancellationToken) {
            return Result.Success();
        }
    }

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

    public class DashboardRequestHandler : IRequestHandler<Queries.GetTodaysSalesQuery, Result<TodaysSalesModel>>,
        IRequestHandler<Queries.GetTodaysSettlementQuery, Result<TodaysSettlementModel>>,
        IRequestHandler<Queries.GetTodaysSalesCountByHourQuery, Result<List<TodaysSalesCountByHourModel>>>,
        IRequestHandler<Queries.GetTodaysSalesValueByHourQuery, Result<List<TodaysSalesValueByHourModel>>>,
        IRequestHandler<Queries.GetMerchantKpiQuery, Result<MerchantKpiModel>>,
        IRequestHandler<Queries.GetTodaysFailedSalesQuery, Result<TodaysSalesModel>>,
        IRequestHandler<Queries.GetTopProductDataQuery, Result<List<TopBottomProductDataModel>>>,
        IRequestHandler<Queries.GetBottomProductDataQuery, Result<List<TopBottomProductDataModel>>>,
        IRequestHandler<Queries.GetTopMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
        IRequestHandler<Queries.GetBottomMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
        IRequestHandler<Queries.GetTopOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
        IRequestHandler<Queries.GetBottomOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
        IRequestHandler<Queries.GetLastSettlementQuery, Result<LastSettlementModel>>,
        IRequestHandler<Queries.GetTransactionDetailQuery, Result<List<TransactionDetailModel>>>
    {
        private readonly IApiClient ApiClient;

        public DashboardRequestHandler(IApiClient apiClient) {
            this.ApiClient = apiClient;
            
        }

        // Implementations similar to above handlers returning stub data
        public async Task<Result<TodaysSalesModel>> Handle(Queries.GetTodaysSalesQuery request,
                                                           CancellationToken cancellationToken) {
            return await this.ApiClient.GetTodaysSales(request, cancellationToken);
        }

        public async Task<Result<TodaysSettlementModel>> Handle(Queries.GetTodaysSettlementQuery request,
                                                                CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockTodaysSettlement());
        }

        public async Task<Result<List<TodaysSalesCountByHourModel>>> Handle(Queries.GetTodaysSalesCountByHourQuery request,
                                                                            CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockSalesCountByHour());
        }

        public async Task<Result<List<TodaysSalesValueByHourModel>>> Handle(Queries.GetTodaysSalesValueByHourQuery request,
                                                                            CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockSalesValueByHour());
        }

        public async Task<Result<MerchantKpiModel>> Handle(Queries.GetMerchantKpiQuery request,
                                                           CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchantKpi(request, cancellationToken);
        }

        public async Task<Result<TodaysSalesModel>> Handle(Queries.GetTodaysFailedSalesQuery request,
                                                           CancellationToken cancellationToken) {
            return await this.ApiClient.GetTodaysFailedSales(request, cancellationToken);
        }

        public async Task<Result<List<TopBottomProductDataModel>>> Handle(Queries.GetTopProductDataQuery request,
                                                                          CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockTopProducts());
        }

        public async Task<Result<List<TopBottomProductDataModel>>> Handle(Queries.GetBottomProductDataQuery request,
                                                                          CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockBottomProducts());
        }

        public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(Queries.GetTopMerchantDataQuery request,
                                                                           CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockTopMerchants());
        }

        public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(Queries.GetBottomMerchantDataQuery request,
                                                                           CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockBottomMerchants());
        }

        public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(Queries.GetTopOperatorDataQuery request,
                                                                           CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockTopOperators());
        }

        public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(Queries.GetBottomOperatorDataQuery request,
                                                                           CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockBottomOperators());
        }

        public async Task<Result<LastSettlementModel>> Handle(Queries.GetLastSettlementQuery request,
                                                              CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockLastSettlement());
        }

        public async Task<Result<List<TransactionDetailModel>>> Handle(Queries.GetTransactionDetailQuery request,
                                                                       CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockTransactionDetails(request));
        }
    }
}
