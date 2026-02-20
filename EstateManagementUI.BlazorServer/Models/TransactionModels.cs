using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BlazorServer.Models;

[ExcludeFromCodeCoverage]
public record TransactionModels
{
    public class TransactionDetailReportResponse
    {
        public List<TransactionDetail> Transactions { get; set; }
        public TransactionDetailSummary Summary { get; set; }
    }

    public class TransactionDetailSummary
    {
        public Decimal TotalValue { get; set; }
        public Decimal TotalFees { get; set; }
        public Int32 TransactionCount { get; set; }
    }

    public class TransactionDetail
    {
        public Guid Id { get; set; }
        public DateTime DateTime { get; set; }
        public String Merchant { get; set; }
        public Guid MerchantId { get; set; }
        public Int32 MerchantReportingId { get; set; }
        public String Operator { get; set; }
        public Guid OperatorId { get; set; }
        public Int32 OperatorReportingId { get; set; }
        public String Product { get; set; }
        public Guid ProductId { get; set; }
        public Int32 ProductReportingId { get; set; }
        public String Type { get; set; }
        public String Status { get; set; }
        public Decimal Value { get; set; }
        public Decimal TotalFees { get; set; }
        public Decimal NetAmount { get; set; }
        public String SettlementReference { get; set; }
    }

    public class TransactionSummaryByMerchantResponse
    {
        public List<MerchantDetail> Merchants { get; set; }
        public MerchantDetailSummary Summary { get; set; }
    }

    public class MerchantDetail
    {
        public Guid MerchantId { get; set; }
        public Int32 MerchantReportingId { get; set; }
        public string MerchantName { get; set; }
        public Int32 TotalCount { get; set; }
        public Decimal TotalValue { get; set; }
        public Decimal AverageValue { get; set; }
        public Int32 AuthorisedCount { get; set; }
        public Int32 DeclinedCount { get; set; }
        public Decimal AuthorisedPercentage { get; set; }
    }

    public class MerchantDetailSummary
    {
        public Int32 TotalMerchants { get; set; }
        public Int32 TotalCount { get; set; }
        public Decimal TotalValue { get; set; }
        public Decimal AverageValue { get; set; }
    }

    public class TransactionSummaryByOperatorResponse
    {
        public List<OperatorDetail> Operators { get; set; }
        public OperatorDetailSummary Summary { get; set; }
    }

    public class OperatorDetail
    {
        public Guid OperatorId { get; set; }
        public Int32 OperatorReportingId { get; set; }
        public string OperatorName { get; set; }
        public Int32 TotalCount { get; set; }
        public Decimal TotalValue { get; set; }
        public Decimal AverageValue { get; set; }
        public Int32 AuthorisedCount { get; set; }
        public Int32 DeclinedCount { get; set; }
        public Decimal AuthorisedPercentage { get; set; }
    }

    public class OperatorDetailSummary
    {
        public Int32 TotalOperators { get; set; }
        public Int32 TotalCount { get; set; }
        public Decimal TotalValue { get; set; }
        public Decimal AverageValue { get; set; }
    }

    public class ProductPerformanceResponse
    {
        public List<ProductPerformanceDetail> ProductDetails { get; set; }
        public ProductPerformanceSummary Summary { get; set; }
    }

    public class ProductPerformanceDetail
    {
        public String ProductName { get; set; }
        public Guid ProductId { get; set; }
        public Int32 ProductReportingId { get; set; }
        public Guid ContractId { get; set; }
        public Int32 ContractReportingId { get; set; }
        public Int32 TransactionCount { get; set; }
        public Decimal TransactionValue { get; set; }
        public Decimal PercentageOfTotal { get; set; }
    }

    public class ProductPerformanceSummary
    {
        public Int32 TotalProducts { get; set; }
        public Int32 TotalCount { get; set; }
        public Decimal TotalValue { get; set; }
        public Decimal AveragePerProduct { get; set; }
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

    public class TodaysSalesByHourModel
    {
        public int Hour { get; set; }
        public int TodaysSalesCount { get; set; }
        public int ComparisonSalesCount { get; set; }
        public decimal TodaysSalesValue { get; set; }
        public decimal ComparisonSalesValue { get; set; }
    }

    public class MerchantKpiModel
    {
        public int MerchantsWithNoSaleInLast7Days { get; set; }
        public int MerchantsWithNoSaleToday { get; set; }
        public int MerchantsWithSaleInLastHour { get; set; }
    }

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
}