using MediatR;
using EstateManagementUI.BlazorServer.Models;

namespace EstateManagementUI.BlazorServer.Requests;

public record CorrelationId(Guid Value);

public static class CorrelationIdHelper
{
    public static CorrelationId New() => new(Guid.NewGuid());
}

public static class Queries
{
    public record GetEstateQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<EstateModel>>;
    public record GetMerchantsQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<List<MerchantModel>>>;
    public record GetOperatorsQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<List<OperatorModel>>>;
    public record GetContractsQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<List<ContractModel>>>;
    public record GetOperatorQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid OperatorId) : IRequest<Result<OperatorModel>>;
    public record GetContractQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid ContractId) : IRequest<Result<ContractModel>>;
    public record GetFileImportLogsListQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, DateTime StartDate, DateTime EndDate)
        : IRequest<Result<List<FileImportLogModel>>>;
    public record GetFileImportLogQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, Guid FileImportLogId)
        : IRequest<Result<FileImportLogModel>>;
    public record GetFileDetailsQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid FileId) : IRequest<Result<FileDetailsModel>>;
    public record GetComparisonDatesQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<List<ComparisonDateModel>>>;
    public record GetTodaysSalesQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
    public record GetTodaysSettlementQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<TodaysSettlementModel>>;
    public record GetTodaysSalesCountByHourQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TodaysSalesCountByHourModel>>>;
    public record GetTodaysSalesValueByHourQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime ComparisonDate) : IRequest<Result<List<TodaysSalesValueByHourModel>>>;
    public record GetMerchantKpiQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<MerchantKpiModel>>;
    public record GetTodaysFailedSalesQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, string ResponseCode, DateTime ComparisonDate) : IRequest<Result<TodaysSalesModel>>;
    public record GetTopProductDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;
    public record GetBottomProductDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomProductDataModel>>>;
    public record GetTopMerchantDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;
    public record GetBottomMerchantDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomMerchantDataModel>>>;
    public record GetTopOperatorDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;
    public record GetBottomOperatorDataQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, int ResultCount) : IRequest<Result<List<TopBottomOperatorDataModel>>>;
    public record GetLastSettlementQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId) : IRequest<Result<LastSettlementModel>>;
    public record GetMerchantQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId) : IRequest<Result<MerchantModel>>;
    public record GetMerchantTransactionSummaryQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime StartDate, DateTime EndDate, Guid? MerchantId = null, Guid? OperatorId = null, Guid? ProductId = null) : IRequest<Result<List<MerchantTransactionSummaryModel>>>;
    public record GetProductPerformanceQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime StartDate, DateTime EndDate) : IRequest<Result<List<ProductPerformanceModel>>>;
    public record GetOperatorTransactionSummaryQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, DateTime StartDate, DateTime EndDate, Guid? MerchantId = null, Guid? OperatorId = null) : IRequest<Result<List<OperatorTransactionSummaryModel>>>;
    public record GetMerchantSettlementHistoryQuery(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid? MerchantId, DateTime StartDate, DateTime EndDate) : IRequest<Result<List<MerchantSettlementHistoryModel>>>;
}

public static class Commands
{
    public record AddMerchantDeviceCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string DeviceIdentifier) : IRequest<Result>;
    public record AddOperatorToMerchantCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, Guid OperatorId, string? MerchantNumber, string? TerminalNumber) : IRequest<Result>;
    public record AddOperatorToEstateCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid OperatorId) : IRequest<Result>;
    public record AssignContractToMerchantCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, Guid ContractId) : IRequest<Result>;
    public record CreateContractCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, string Description, Guid OperatorId) : IRequest<Result>;
    public record CreateMerchantCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, string Name, string ContactName, string ContactEmail) : IRequest<Result>;
    public record CreateMerchantUserCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string EmailAddress, string Password) : IRequest<Result>;
    public record CreateOperatorCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, string Name, bool RequireCustomMerchantNumber, bool RequireCustomTerminalNumber) : IRequest<Result>;
    public record MakeMerchantDepositCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, decimal Amount, DateTime Date, string Reference) : IRequest<Result>;
    public record RemoveContractFromMerchantCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, Guid ContractId) : IRequest<Result>;
    public record RemoveOperatorFromMerchantCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, Guid OperatorId) : IRequest<Result>;
    public record RemoveOperatorFromEstateCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid OperatorId) : IRequest<Result>;
    public record SetMerchantSettlementScheduleCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string Schedule) : IRequest<Result>;
    public record SwapMerchantDeviceCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string OldDevice, string NewDevice) : IRequest<Result>;
    public record UpdateMerchantAddressCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string AddressLine1, string Town, string Region, string PostalCode, string Country) : IRequest<Result>;
    public record UpdateMerchantCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string Name) : IRequest<Result>;
    public record UpdateMerchantContactCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid MerchantId, string ContactName, string ContactEmail, string ContactPhone) : IRequest<Result>;
    public record UpdateOperatorCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid OperatorId, string Name, bool RequireCustomMerchantNumber, bool RequireCustomTerminalNumber) : IRequest<Result>;
    public record AddProductToContractCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid ContractId, string ProductName, string DisplayText, decimal? Value) : IRequest<Result>;
    public record AddTransactionFeeForProductToContractCommand(CorrelationId CorrelationId, string AccessToken, Guid EstateId, Guid ContractId, Guid ProductId, string Description, decimal Value) : IRequest<Result>;
}
