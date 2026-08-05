using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Client;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using EstateManagementUI.BusinessLogic.Services;
using FileProcessor.Client;
using Shared.Results;
using SimpleResults;
using FileImportLogDetailsModel = EstateManagementUI.BusinessLogic.Models.FileProcessingModels.FileImportLogDetailsModel;

namespace EstateManagementUI.BlazorServer.Testing;

public sealed class TestApiClient : IApiClient
{
    private readonly TestMediator _mediator;
    private readonly TestSupportState _supportState;
    private readonly List<FileProcessor.Models.FileProfile> _fileProfiles = new();

    public TestApiClient(TestMediator mediator, TestSupportState supportState)
    {
        _mediator = mediator;
        _supportState = supportState;
        Reset();
    }

    public void Reset()
    {
        _fileProfiles.Clear();
        _fileProfiles.Add(new FileProcessor.Models.FileProfile(
            Guid.Parse("88888888-8888-8888-8888-888888888888"),
            "Default File Profile",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty));

        _supportState.SetFileImportLog(new FileImportLogDetailsModel
        {
            FileImportLogId = Guid.Parse("99999999-9999-9999-9999-999999999999"),
            ImportLogDate = DateTime.UtcNow,
            Files =
            [
                new FileProcessingModels.FileProcessingFileModel
                {
                    FileId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                    FileName = "default.csv",
                    FileProfile = "Default File Profile",
                    DateTimeUploaded = DateTime.UtcNow,
                    UploadedBy = "Test User",
                    UploadedById = Guid.Empty,
                    MerchantId = Guid.Empty,
                    MerchantName = "Test Merchant 1",
                    FileLines =
                    [
                        new FileProcessingModels.FileProcessingLineModel
                        {
                            LineNumber = 1,
                            LineContents = "ok",
                            LineStatus = FileProcessingLineStatus.Successful
                        }
                    ]
                }
            ]
        });
    }

    public Task<Result<EstateModels.EstateModel>> GetEstate(EstateQueries.GetEstateQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result<List<OperatorModels.OperatorModel>>> GetEstateAssignedOperators(EstateQueries.GetAssignedOperatorsQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result> RemoveEstateOperator(EstateCommands.RemoveOperatorFromEstateCommand request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result> AddEstateOperator(EstateCommands.AddOperatorToEstateCommand request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);

    public Task<Result<List<FileProcessingModels.FileImportLogDetailsModel>>> GetFileImportLogsList(FileProcessingQueries.GetFileImportLogsListQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<FileProcessingModels.FileImportLogDetailsModel>> GetFileImportLog(FileProcessingQueries.GetFileImportLogQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);

    public Task<Result<TodaysSalesModel>> GetTodaysSales(TransactionQueries.GetTodaysSalesQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result<TodaysSalesModel>> GetTodaysFailedSales(TransactionQueries.GetTodaysFailedSalesQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result<List<TransactionModels.TodaysSalesByHourModel>>> GetTodaysSalesByHour(TransactionQueries.GetTodaysSalesByHourQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result<TransactionModels.TransactionDetailReportResponse>> GetTransactionDetailReport(TransactionQueries.GetTransactionDetailQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result<TransactionModels.TransactionSummaryByMerchantResponse>> GetMerchantTransactionSummary(TransactionQueries.GetMerchantTransactionSummaryQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result<TransactionModels.TransactionSummaryByOperatorResponse>> GetOperatorTransactionSummary(TransactionQueries.GetOperatorTransactionSummaryQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);
    public Task<Result<TransactionModels.ProductPerformanceResponse>> GetProductPerformance(TransactionQueries.GetProductPerformanceQuery request, CancellationToken cancellationToken) => _mediator.Send(request, cancellationToken);

    public Task<Result<List<FileProcessor.Models.FileProfile>>> GetFileProfiles(CancellationToken cancellationToken = default) => Task.FromResult(Result.Success(_fileProfiles.ToList()));
    public Task<Result<Guid>> UploadFileAsync(Guid estateId, Guid merchantId, Guid userId, Guid fileProfileId, Stream fileStream, string fileName, CancellationToken cancellationToken = default)
        => Task.FromResult(Result.Success(Guid.NewGuid()));

    public Task<Result<List<MerchantModels.MerchantListModel>>> GetMerchants(MerchantQueries.GetMerchantsQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<MerchantModels.MerchantDropDownModel>>> GetMerchants(MerchantQueries.GetMerchantsForDropDownQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<MerchantModels.RecentMerchantsModel>>> GetRecentMerchants(MerchantQueries.GetRecentMerchantsQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<MerchantModels.MerchantKpiModel>> GetMerchantKpi(MerchantQueries.GetMerchantKpiQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<MerchantModels.MerchantModel>> GetMerchant(MerchantQueries.GetMerchantQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<MerchantModels.MerchantScheduleModel>> GetMerchantSchedule(MerchantQueries.GetMerchantScheduleQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<MerchantModels.MerchantOperatorModel>>> GetMerchantOperators(MerchantQueries.GetMerchantOperatorsQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<MerchantModels.MerchantContractModel>>> GetMerchantContracts(MerchantQueries.GetMerchantContractsQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<MerchantModels.MerchantDeviceModel>>> GetMerchantDevices(MerchantQueries.GetMerchantDevicesQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<MerchantModels.MerchantOpeningHoursModel>> GetMerchantOpeningHours(MerchantQueries.GetMerchantOpeningHoursQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result> UpdateMerchant(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> UpdateMerchantAddress(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> UpdateMerchantContact(MerchantCommands.UpdateMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> CreateMerchantSchedule(MerchantCommands.CreateMerchantScheduleCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> UpdateMerchantOpeningHours(MerchantCommands.UpdateMerchantOpeningHoursCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> RemoveOperatorFromMerchant(MerchantCommands.RemoveOperatorFromMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> AddOperatorToMerchant(MerchantCommands.AddOperatorToMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> RemoveContractFromMerchant(MerchantCommands.RemoveContractFromMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> AddContractToMerchant(MerchantCommands.AssignContractToMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> AddDeviceToMerchant(MerchantCommands.AddMerchantDeviceCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> SwapMerchantDevice(MerchantCommands.SwapMerchantDeviceCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> MakeMerchantDeposit(MerchantCommands.MakeMerchantDepositCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> CreateMerchant(MerchantCommands.CreateMerchantCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> UpdateMerchantSchedule(MerchantCommands.UpdateMerchantScheduleCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);

    public Task<Result<List<OperatorModels.OperatorModel>>> GetOperators(OperatorQueries.GetOperatorsQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<OperatorModels.OperatorDropDownModel>>> GetOperators(OperatorQueries.GetOperatorsForDropDownQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<OperatorModels.OperatorModel>> GetOperator(OperatorQueries.GetOperatorQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result> UpdateOperator(OperatorCommands.UpdateOperatorCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> CreateOperator(OperatorCommands.CreateOperatorCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);

    public Task<Result<List<ContractModels.RecentContractModel>>> GetRecentContracts(ContractQueries.GetRecentContractsQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<ContractModels.ContractDropDownModel>>> GetContracts(ContractQueries.GetContractsForDropDownQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<List<ContractModels.ContractModel>>> GetContracts(ContractQueries.GetContractsQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<ContractModels.ContractModel>> GetContract(ContractQueries.GetContractQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result> CreateContract(ContractCommands.CreateContractCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> AddProductToContract(ContractCommands.AddProductToContractCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> AddTransactionFeeToProduct(ContractCommands.AddTransactionFeeToProductCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);
    public Task<Result> RemoveTransactionFeeFromProduct(ContractCommands.RemoveTransactionFeeFromProductCommand command, CancellationToken cancellationToken) => _mediator.Send(command, cancellationToken);

    public Task<Result<List<ComparisonDateModel>>> GetComparisonDates(DateQueries.GetComparisonDatesQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
    public Task<Result<TodaysSettlementModel>> GetTodaysSettlement(SettlementQueries.GetTodaysSettlementQuery query, CancellationToken cancellationToken) => _mediator.Send(query, cancellationToken);
}
