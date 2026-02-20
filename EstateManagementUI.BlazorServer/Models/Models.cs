using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BlazorServer.Models;

[ExcludeFromCodeCoverage]
public record EstateModel(Guid EstateId, string? EstateName, string? Reference) {
    public Int32 MerchantCount { get; init; }
    public Int32 OperatorCount { get; init; }
    public Int32 ContractCount { get; init; }
    public Int32 UserCount { get; init; }
    public List<MerchantModels.RecentMerchantsModel> RecentMerchants { get; init; }
    public List<ContractModels.RecentContractModel> RecentContracts { get; init; }
    public List<OperatorModels.OperatorModel> AssignedOperators { get; init; }
    public List<OperatorModels.OperatorDropDownModel> AllOperators { get; init; }
}

[ExcludeFromCodeCoverage]
// File Processing Models
public class FileImportLogModel
{
    public Guid FileImportLogId { get; set; }
    public DateTime ImportLogDateTime { get; set; }
    public int FileCount { get; set; }
    public DateTime FileUploadedDateTime { get; set; }
}

[ExcludeFromCodeCoverage]
public class FileDetailsModel
{
    public Guid FileId { get; set; }
    public string? FileLocation { get; set; }
    public string? FileProfileName { get; set; }
    public string? MerchantName { get; set; }
    public Guid UserId { get; set; }
    public DateTime FileUploadedDateTime { get; set; }
    public DateTime ProcessingCompletedDateTime { get; set; }
    public int TotalLines { get; set; }
    public int SuccessfullyProcessedLines { get; set; }
    public int FailedLines { get; set; }
    public int IgnoredLines { get; set; }
}

// Dashboard Models
[ExcludeFromCodeCoverage]
public class ComparisonDateModel
{
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}

// Settlement History Models
[ExcludeFromCodeCoverage]
public class MerchantSettlementHistoryModel
{
    public DateTime SettlementDate { get; set; }
    public string? SettlementReference { get; set; }
    public int TransactionCount { get; set; }
    public decimal NetAmountPaid { get; set; }
}

// Settlement Summary Models
[ExcludeFromCodeCoverage]
public class SettlementSummaryModel
{
    public DateTime SettlementPeriodStart { get; set; }
    public DateTime SettlementPeriodEnd { get; set; }
    public Guid MerchantId { get; set; }
    public string? MerchantName { get; set; }
    public decimal GrossTransactionValue { get; set; }
    public decimal TotalFees { get; set; }
    public decimal NetSettlementAmount { get; set; }
    public string? SettlementStatus { get; set; }
}



