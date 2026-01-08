namespace EstateManagementUI.BlazorServer.Models;

// Estate Models
public class EstateModel
{
    public Guid EstateId { get; set; }
    public string? EstateName { get; set; }
    public string? Reference { get; set; }
    public List<EstateOperatorModel>? Operators { get; set; }
}

public class EstateOperatorModel
{
    public Guid OperatorId { get; set; }
    public string? Name { get; set; }
    public bool RequireCustomMerchantNumber { get; set; }
    public bool RequireCustomTerminalNumber { get; set; }
}

// Merchant Models
public class MerchantModel
{
    public Guid MerchantId { get; set; }
    public string? MerchantName { get; set; }
    public string? MerchantReference { get; set; }
    public decimal? Balance { get; set; }
    public decimal? AvailableBalance { get; set; }
    public string? SettlementSchedule { get; set; }
    public string? AddressLine1 { get; set; }
    public string? AddressLine2 { get; set; }
    public string? Town { get; set; }
    public string? Region { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public string? ContactName { get; set; }
    public string? ContactEmailAddress { get; set; }
    public string? ContactPhoneNumber { get; set; }
}

// Operator Models
public class OperatorModel
{
    public Guid OperatorId { get; set; }
    public string? Name { get; set; }
    public bool RequireCustomMerchantNumber { get; set; }
    public bool RequireCustomTerminalNumber { get; set; }
}

// Contract Models
public class ContractModel
{
    public Guid ContractId { get; set; }
    public string? Description { get; set; }
    public string? OperatorName { get; set; }
    public Guid OperatorId { get; set; }
    public List<ContractProductModel>? Products { get; set; }
}

public class ContractProductModel
{
    public Guid ContractProductId { get; set; }
    public string? ProductName { get; set; }
    public string? DisplayText { get; set; }
    public string? ProductType { get; set; }
    // Changed from decimal? to string? to support displaying "Variable" for variable-value products
    // This aligns with how the backend represents variable vs fixed value products
    public string? Value { get; set; }
    public int NumberOfFees { get; set; }
    public List<ContractProductTransactionFeeModel>? TransactionFees { get; set; }
}

public class ContractProductTransactionFeeModel
{
    public Guid TransactionFeeId { get; set; }
    public string? Description { get; set; }
    public string? CalculationType { get; set; }
    public string? FeeType { get; set; }
    public decimal Value { get; set; }
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

public class TopBottomMerchantDataModel
{
    public string? MerchantName { get; set; }
    public decimal SalesValue { get; set; }
}

public class TopBottomOperatorDataModel
{
    public string? OperatorName { get; set; }
    public decimal SalesValue { get; set; }
}

public class LastSettlementModel
{
    public DateTime SettlementDate { get; set; }
    public decimal FeesValue { get; set; }
    public int SalesCount { get; set; }
    public decimal SalesValue { get; set; }
    public decimal SettlementValue { get; set; }
}

// Result wrapper for consistency with SimpleResults
public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Message { get; }

    private Result(bool isSuccess, T? data, string? message = null)
    {
        IsSuccess = isSuccess;
        Data = data;
        Message = message;
    }

    public static Result<T> Success(T data) => new(true, data);
    public static Result<T> Failure(string message) => new(false, default, message);
}

public class Result
{
    public bool IsSuccess { get; }
    public string? Message { get; }

    private Result(bool isSuccess, string? message = null)
    {
        IsSuccess = isSuccess;
        Message = message;
    }

    public static Result Success() => new(true);
    public static Result Failure(string message) => new(false, message);
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
