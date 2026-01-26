using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Shared.Results;
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

    public class EstateRequestHandler : IRequestHandler<EstateQueries.GetEstateQuery, Result<EstateModel>>,
        IRequestHandler<EstateCommands.AddOperatorToEstateCommand, Result>,
        IRequestHandler<EstateCommands.RemoveOperatorFromEstateCommand, Result>,
        IRequestHandler<EstateQueries.GetAssignedOperatorsQuery, Result<List<OperatorModel>>> {
        private readonly IApiClient ApiClient;

        public EstateRequestHandler(IApiClient apiClient) {
            this.ApiClient = apiClient;
        }

        public async Task<Result<EstateModel>> Handle(EstateQueries.GetEstateQuery request,
                                                      CancellationToken cancellationToken) {
            return await this.ApiClient.GetEstate(request, cancellationToken);
        }

        public async Task<Result> Handle(EstateCommands.AddOperatorToEstateCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.AddEstateOperator(request, cancellationToken);
        }

        public async Task<Result> Handle(EstateCommands.RemoveOperatorFromEstateCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.RemoveEstateOperator(request, cancellationToken);
        }

        public async Task<Result<List<OperatorModel>>> Handle(EstateQueries.GetAssignedOperatorsQuery request,
                                                              CancellationToken cancellationToken) {
            return await this.ApiClient.GetEstateAssignedOperators(request, cancellationToken);
        }
    }

    public class MerchantRequestHandler : IRequestHandler<MerchantQueries.GetMerchantsQuery, Result<List<MerchantListModel>>>,
                                        IRequestHandler<MerchantQueries.GetMerchantQuery, Result<MerchantModel>>,
                                        IRequestHandler<MerchantCommands.AddMerchantDeviceCommand, Result>,
                                        IRequestHandler<MerchantCommands.AddOperatorToMerchantCommand, Result>,
                                        IRequestHandler<MerchantCommands.CreateMerchantCommand, Result>,
                                        IRequestHandler<MerchantCommands.MakeMerchantDepositCommand, Result>,
                                        IRequestHandler<MerchantCommands.RemoveContractFromMerchantCommand, Result>,
                                        IRequestHandler<MerchantCommands.RemoveOperatorFromMerchantCommand, Result>,
                                        IRequestHandler<MerchantCommands.SwapMerchantDeviceCommand, Result>,
                                        IRequestHandler<MerchantCommands.UpdateMerchantCommand, Result>,
                                        IRequestHandler<MerchantCommands.AssignContractToMerchantCommand, Result>,
                                        IRequestHandler<MerchantQueries.GetRecentMerchantsQuery, Result<List<RecentMerchantsModel>>>,
                                        IRequestHandler<MerchantQueries.GetMerchantsForDropDownQuery, Result<List<MerchantDropDownModel>>>,
                                        IRequestHandler<MerchantQueries.GetMerchantContractsQuery, Result<List<MerchantContractModel>>>,
                                        IRequestHandler<MerchantQueries.GetMerchantOperatorsQuery, Result<List<MerchantOperatorModel>>>,
                                        IRequestHandler<MerchantQueries.GetMerchantDevicesQuery, Result<List<MerchantDeviceModel>>>
    {

        private readonly IApiClient ApiClient;

        public MerchantRequestHandler(IApiClient apiClient)
        {
            this.ApiClient = apiClient;
        }

        public async Task<Result<List<MerchantListModel>>> Handle(MerchantQueries.GetMerchantsQuery request,
                                                                  CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchants(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.AddMerchantDeviceCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.AddDeviceToMerchant(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.AddOperatorToMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.AddOperatorToMerchant(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.CreateMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.CreateMerchant(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.MakeMerchantDepositCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.MakeMerchantDeposit(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.RemoveContractFromMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.RemoveContractFromMerchant(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.RemoveOperatorFromMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.RemoveOperatorFromMerchant(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.SwapMerchantDeviceCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.SwapMerchantDevice(request, cancellationToken);
        }


        public async Task<Result> Handle(MerchantCommands.UpdateMerchantCommand request,
                                         CancellationToken cancellationToken) {
            Result updateMerchantResult = await this.ApiClient.UpdateMerchant(request, cancellationToken);
            if (updateMerchantResult.IsFailed)
                return ResultHelpers.CreateFailure(updateMerchantResult);

            Result updateMerchantAddressResult = await this.ApiClient.UpdateMerchantAddress(request, cancellationToken);
            if (updateMerchantAddressResult.IsFailed)
                return ResultHelpers.CreateFailure(updateMerchantAddressResult);
            Result updateMerchantContactResult = await this.ApiClient.UpdateMerchantContact(request, cancellationToken);
            if (updateMerchantContactResult.IsFailed)
                return ResultHelpers.CreateFailure(updateMerchantContactResult);

            return Result.Success();
        }

        public async Task<Result<MerchantModel>> Handle(MerchantQueries.GetMerchantQuery request,
                                                        CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchant(request, cancellationToken);
        }

        public async Task<Result> Handle(MerchantCommands.AssignContractToMerchantCommand request,
                                         CancellationToken cancellationToken) {
            return await this.ApiClient.AddContractToMerchant(request, cancellationToken);
        }

        public async Task<Result<List<RecentMerchantsModel>>> Handle(MerchantQueries.GetRecentMerchantsQuery request,
                                                                     CancellationToken cancellationToken) {
            return await this.ApiClient.GetRecentMerchants(request, cancellationToken);
        }

        public async Task<Result<List<MerchantDropDownModel>>> Handle(MerchantQueries.GetMerchantsForDropDownQuery request,
                                                                      CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchants(request, cancellationToken);
        }

        public async Task<Result<List<MerchantContractModel>>> Handle(MerchantQueries.GetMerchantContractsQuery request,
                                                                      CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchantContracts(request, cancellationToken);
        }

        public async Task<Result<List<MerchantOperatorModel>>> Handle(MerchantQueries.GetMerchantOperatorsQuery request,
                                                                      CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchantOperators(request, cancellationToken);
        }

        public async Task<Result<List<MerchantDeviceModel>>> Handle(MerchantQueries.GetMerchantDevicesQuery request,
                                                                    CancellationToken cancellationToken) {
            return await this.ApiClient.GetMerchantDevices(request, cancellationToken);
        }
    }

    public class ContractRequestHandler : IRequestHandler<ContractQueries.GetContractsQuery, Result<List<ContractModel>>>,
                                            IRequestHandler<ContractQueries.GetContractQuery, Result<ContractModel>>,
                                            IRequestHandler<Commands.CreateContractCommand, Result>,
                                            IRequestHandler<Commands.AddProductToContractCommand, Result>,
                                            IRequestHandler<Commands.AddTransactionFeeForProductToContractCommand, Result>,
    IRequestHandler<ContractQueries.GetRecentContractsQuery, Result<List<RecentContractModel>>>,
    IRequestHandler<ContractQueries.GetContractsForDropDownQuery, Result<List<ContractDropDownModel>>>
    {

        private readonly IApiClient ApiClient;

        public ContractRequestHandler(IApiClient apiClient)
        {
            this.ApiClient = apiClient;
        }

        public async Task<Result<List<ContractModel>>> Handle(ContractQueries.GetContractsQuery request,
                                                              CancellationToken cancellationToken) {
            return Result.Success(StubTestData.GetMockContracts());
        }

        public async Task<Result<ContractModel>> Handle(ContractQueries.GetContractQuery request,
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

        public async Task<Result<List<RecentContractModel>>> Handle(ContractQueries.GetRecentContractsQuery request,
                                                                    CancellationToken cancellationToken) {
            return await this.ApiClient.GetRecentContracts(request, cancellationToken);
        }

        public async Task<Result<List<ContractDropDownModel>>> Handle(ContractQueries.GetContractsForDropDownQuery request,
                                                                      CancellationToken cancellationToken) {
            return await this.ApiClient.GetContracts(request, cancellationToken);
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
        IRequestHandler<MerchantQueries.GetMerchantKpiQuery, Result<MerchantKpiModel>>,
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

        public async Task<Result<MerchantKpiModel>> Handle(MerchantQueries.GetMerchantKpiQuery request,
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
