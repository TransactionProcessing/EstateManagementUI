namespace EstateManagementUI.BlazorServer.Models;

public record EstateModel(Guid EstateId, string? EstateName, string? Reference) {
    public Int32 MerchantCount { get; init; }
    public Int32 OperatorCount { get; init; }
    public Int32 ContractCount { get; init; }
    public Int32 UserCount { get; init; }
    public List<RecentMerchantsModel> RecentMerchants { get; init; }
    public List<RecentContractModel> RecentContracts { get; init; }
    public List<OperatorModels.OperatorModel> AssignedOperators { get; init; }
    public List<OperatorModels.OperatorDropDownModel> AllOperators { get; init; }
}


// File Processing Models
public class FileImportLogModel
{
    public Guid FileImportLogId { get; set; }
    public DateTime ImportLogDateTime { get; set; }
    public int FileCount { get; set; }
    public DateTime FileUploadedDateTime { get; set; }
}

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
public class ComparisonDateModel
{
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}

public class TodaysSalesModel
{
    public int ComparisonSalesCount { get; set; }
    public decimal ComparisonSalesValue { get; set; }
    public decimal ComparisonAverageValue { get; set; }
    public int TodaysSalesCount { get; set; }
    public decimal TodaysSalesValue { get; set; }
    public decimal TodaysAverageValue { get; set; }
}

public class TodaysSettlementModel
{
    public int ComparisonSettlementCount { get; set; }
    public decimal ComparisonSettlementValue { get; set; }
    public int TodaysSettlementCount { get; set; }
    public decimal TodaysSettlementValue { get; set; }
    public int ComparisonPendingSettlementCount { get; set; }
    public decimal ComparisonPendingSettlementValue { get; set; }
    public int TodaysPendingSettlementCount { get; set; }
    public decimal TodaysPendingSettlementValue { get; set; }
}

public class TodaysSalesCountByHourModel
{
    public int Hour { get; set; }
    public int TodaysSalesCount { get; set; }
    public int ComparisonSalesCount { get; set; }
}

public class TodaysSalesValueByHourModel
{
    public int Hour { get; set; }
    public decimal TodaysSalesValue { get; set; }
    public decimal ComparisonSalesValue { get; set; }
}

public class MerchantKpiModel
{
    public int MerchantsWithNoSaleInLast7Days { get; set; }
    public int MerchantsWithNoSaleToday { get; set; }
    public int MerchantsWithSaleInLastHour { get; set; }
}

public class TopBottomProductDataModel
{
    public string? ProductName { get; set; }
    public decimal SalesValue { get; set; }
}

// Transaction Summary Models
public class MerchantTransactionSummaryModel
{
    public Guid MerchantId { get; set; }
    public string? MerchantName { get; set; }
    public int TotalTransactionCount { get; set; }
    public decimal TotalTransactionValue { get; set; }
    public decimal AverageTransactionValue { get; set; }
    public int SuccessfulTransactionCount { get; set; }
    public int FailedTransactionCount { get; set; }
}

public class OperatorTransactionSummaryModel
{
    public Guid OperatorId { get; set; }
    public string? OperatorName { get; set; }
    public int TotalTransactionCount { get; set; }
    public decimal TotalTransactionValue { get; set; }
    public decimal AverageTransactionValue { get; set; }
    public int SuccessfulTransactionCount { get; set; }
    public int FailedTransactionCount { get; set; }
    public decimal TotalFeesEarned { get; set; }
}

public class ProductPerformanceModel
{
    public string? ProductName { get; set; }
    public int TransactionCount { get; set; }
    public decimal TransactionValue { get; set; }
    public decimal PercentageContribution { get; set; }
}

// Settlement History Models
public class MerchantSettlementHistoryModel
{
    public DateTime SettlementDate { get; set; }
    public string? SettlementReference { get; set; }
    public int TransactionCount { get; set; }
    public decimal NetAmountPaid { get; set; }
}

// Settlement Summary Models
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

// Transaction Detail Models
public class TransactionDetailModel
{
    public Guid TransactionId { get; set; }
    public DateTime TransactionDateTime { get; set; }
    public string? MerchantName { get; set; }
    public Guid MerchantId { get; set; }
    public string? OperatorName { get; set; }
    public Guid OperatorId { get; set; }
    public string? ProductName { get; set; }
    public Guid ProductId { get; set; }
    public string? TransactionType { get; set; } // sale, refund, reversal
    public string? TransactionStatus { get; set; } // successful, failed, reversed
    public decimal GrossAmount { get; set; }
    public decimal FeesCommission { get; set; }
    public decimal NetAmount { get; set; }
    public string? SettlementReference { get; set; }
    public string? ResponseCode { get; set; }
    public DateTime? SettlementDateTime { get; set; }
}

public class RecentMerchantsModel
{
    public DateTime CreatedDateTime { get; set; }
    public Guid MerchantId { get; set; }
    public String Name { get; set; }
    public String Reference { get; set; }
}

public class RecentContractModel
{
    public Guid ContractId { get; set; }
    public string? Description { get; set; }
    public string? OperatorName { get; set; }
}

