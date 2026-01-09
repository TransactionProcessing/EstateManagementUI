using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagmentUI.BusinessLogic.Requests;
using EstateReportingAPI.DataTransferObjects;
using MediatR;
using Microsoft.IdentityModel.Abstractions;
using SimpleResults;
using static EstateManagmentUI.BusinessLogic.Requests.Queries;

namespace EstateManagementUI.BusinessLogic.RequestHandlers;

public class ReportingRequestHandler : IRequestHandler<Queries.GetComparisonDatesQuery, Result<List<ComparisonDateModel>>>,
IRequestHandler<GetTodaysSalesQuery, Result<TodaysSalesModel>>,
    IRequestHandler<GetTodaysSettlementQuery, Result<TodaysSettlementModel>>,
IRequestHandler<GetTodaysSalesCountByHourQuery, Result<List<TodaysSalesCountByHourModel>>>,
IRequestHandler<GetTodaysSalesValueByHourQuery, Result<List<TodaysSalesValueByHourModel>>>,
IRequestHandler<GetMerchantKpiQuery, Result<MerchantKpiModel>>,
IRequestHandler<GetTodaysFailedSalesQuery, Result<TodaysSalesModel>>,
IRequestHandler<GetTopProductDataQuery, Result<List<TopBottomProductDataModel>>>,
IRequestHandler<GetBottomProductDataQuery, Result<List<TopBottomProductDataModel>>>,
IRequestHandler<GetTopMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
IRequestHandler<GetBottomMerchantDataQuery, Result<List<TopBottomMerchantDataModel>>>,
IRequestHandler<GetTopOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
IRequestHandler<GetBottomOperatorDataQuery, Result<List<TopBottomOperatorDataModel>>>,
IRequestHandler<GetLastSettlementQuery, Result<LastSettlementModel>>,
IRequestHandler<GetMerchantTransactionSummaryQuery, Result<List<MerchantTransactionSummaryModel>>>,
IRequestHandler<GetProductPerformanceQuery, Result<List<ProductPerformanceModel>>>,
IRequestHandler<GetOperatorTransactionSummaryQuery, Result<List<OperatorTransactionSummaryModel>>> {

    private readonly IApiClient ApiClient;
    public ReportingRequestHandler(IApiClient apiClient)
    {
        this.ApiClient = apiClient;
    }

    public async Task<Result<List<ComparisonDateModel>>> Handle(Queries.GetComparisonDatesQuery request,
                                                                CancellationToken cancellationToken) {
        Result<List<ComparisonDateModel>> model = await this.ApiClient.GetComparisonDates(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSalesModel>> Handle(GetTodaysSalesQuery request,
                                                       CancellationToken cancellationToken) {
        Result<TodaysSalesModel> model = await this.ApiClient.GetTodaysSales(request.AccessToken, request.EstateId, request.EstateId, null,
            null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSettlementModel>> Handle(GetTodaysSettlementQuery request,
                                                            CancellationToken cancellationToken) {
        Result<TodaysSettlementModel> model = await this.ApiClient.GetTodaysSettlement(request.AccessToken, Guid.Empty, request.EstateId, null,
            null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TodaysSalesCountByHourModel>>> Handle(GetTodaysSalesCountByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        Result<List<TodaysSalesCountByHourModel>> model = await this.ApiClient.GetTodaysSalesCountByHour(request.AccessToken, Guid.Empty, request.EstateId,
            null, null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TodaysSalesValueByHourModel>>> Handle(GetTodaysSalesValueByHourQuery request,
                                                                        CancellationToken cancellationToken) {
        Result<List<TodaysSalesValueByHourModel>> model = await this.ApiClient.GetTodaysSalesValueByHour(request.AccessToken, Guid.Empty, request.EstateId,
            null, null, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<MerchantKpiModel>> Handle(GetMerchantKpiQuery request,
                                                       CancellationToken cancellationToken) {
        Result<MerchantKpiModel> model = await this.ApiClient.GetMerchantKpi(request.AccessToken, request.EstateId, cancellationToken);
        return model;
    }

    public async Task<Result<TodaysSalesModel>> Handle(GetTodaysFailedSalesQuery request,
                                                       CancellationToken cancellationToken) {
        Result<TodaysSalesModel> model = await this.ApiClient.GetTodaysFailedSales(request.AccessToken, request.EstateId,
            request.ResponseCode, request.ComparisonDate, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomProductDataModel>>> Handle(GetTopProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        Result<List<TopBottomProductDataModel>> model = await this.ApiClient.GetTopBottomProductData(request.AccessToken, request.EstateId, TopBottom.Top,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomProductDataModel>>> Handle(GetBottomProductDataQuery request,
                                                                      CancellationToken cancellationToken) {
        Result<List<TopBottomProductDataModel>> model = await this.ApiClient.GetTopBottomProductData(request.AccessToken, request.EstateId, TopBottom.Bottom,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(GetTopMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomMerchantDataModel>> model = await this.ApiClient.GetTopBottomMerchantData(request.AccessToken, request.EstateId, TopBottom.Top,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomMerchantDataModel>>> Handle(GetBottomMerchantDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomMerchantDataModel>> model = await this.ApiClient.GetTopBottomMerchantData(request.AccessToken, request.EstateId, TopBottom.Bottom,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(GetTopOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomOperatorDataModel>> model = await this.ApiClient.GetTopBottomOperatorData(request.AccessToken, request.EstateId, TopBottom.Top,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<List<TopBottomOperatorDataModel>>> Handle(GetBottomOperatorDataQuery request,
                                                                       CancellationToken cancellationToken) {
        Result<List<TopBottomOperatorDataModel>> model = await this.ApiClient.GetTopBottomOperatorData(request.AccessToken, request.EstateId, TopBottom.Bottom,
            request.ResultCount, cancellationToken);
        return model;
    }

    public async Task<Result<LastSettlementModel>> Handle(GetLastSettlementQuery request,
                                                          CancellationToken cancellationToken) {
        Result<LastSettlementModel> model = await this.ApiClient.GetLastSettlement(request.AccessToken, request.EstateId, null, null,
            cancellationToken);
        return model;
    }

    public async Task<Result<List<MerchantTransactionSummaryModel>>> Handle(GetMerchantTransactionSummaryQuery request,
                                                                             CancellationToken cancellationToken) {
        // TODO: Replace with actual API call when endpoint is available
        // For now, return mock data for testing
        var merchants = await this.ApiClient.GetMerchants(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        
        if (!merchants.IsSuccess) {
            return Result.Failure<List<MerchantTransactionSummaryModel>>(merchants.Message);
        }

        var summary = new List<MerchantTransactionSummaryModel>();
        var random = new Random(42); // Use seed for consistent test data
        
        const decimal DefaultSuccessRate = 0.85m; // 85% success rate for mock data
        
        foreach (var merchant in merchants.Data) {
            var totalCount = random.Next(100, 1000);
            var successfulCount = (int)(totalCount * DefaultSuccessRate);
            var failedCount = totalCount - successfulCount;
            var totalValue = (decimal)(random.NextDouble() * 50000 + 10000);
            
            summary.Add(new MerchantTransactionSummaryModel {
                MerchantId = merchant.Id,
                MerchantName = merchant.Name,
                TotalTransactionCount = totalCount,
                TotalTransactionValue = Math.Round(totalValue, 2),
                AverageTransactionValue = Math.Round(totalValue / totalCount, 2),
                SuccessfulTransactionCount = successfulCount,
                FailedTransactionCount = failedCount
            });
        }
        
        // Apply filters if specified
        if (request.MerchantId.HasValue) {
            summary = summary.Where(s => s.MerchantId == request.MerchantId.Value).ToList();
        }
        
        return Result.Success(summary);
    }

    public async Task<Result<List<ProductPerformanceModel>>> Handle(GetProductPerformanceQuery request,
                                                                     CancellationToken cancellationToken) {
        // TODO: Replace with actual API call when endpoint is available
        // For now, return mock data for testing
        var contracts = await this.ApiClient.GetContracts(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        
        if (!contracts.IsSuccess) {
            return Result.Failure<List<ProductPerformanceModel>>(contracts.Message);
        }

        var products = new List<ProductPerformanceModel>();
        
        // Calculate days in the date range to vary data based on period
        var daysInRange = (request.EndDate - request.StartDate).Days + 1;
        
        // Use date range as seed for consistent but varying data
        var seed = request.StartDate.GetHashCode() ^ request.EndDate.GetHashCode();
        var random = new Random(seed);
        
        // Collect all unique products from all contracts
        var productNames = contracts.Data
            .SelectMany(c => c.Products ?? new List<ContractProductModel>())
            .Select(p => p.ProductName)
            .Distinct()
            .ToList();
        
        decimal totalValue = 0;
        
        // Generate mock transaction data for each product
        // Scale transaction counts based on the date range length
        var countMultiplier = Math.Max(1, daysInRange / 30.0); // Scale based on 30-day baseline
        
        foreach (var productName in productNames) {
            if (string.IsNullOrEmpty(productName)) continue;
            
            var baseTransactionCount = random.Next(50, 500);
            var transactionCount = (int)(baseTransactionCount * countMultiplier);
            var transactionValue = Math.Round((decimal)(random.NextDouble() * 30000 + 5000) * (decimal)countMultiplier, 2);
            totalValue += transactionValue;
            
            products.Add(new ProductPerformanceModel {
                ProductName = productName,
                TransactionCount = transactionCount,
                TransactionValue = transactionValue,
                PercentageContribution = 0 // Will be calculated after total is known
            });
        }
        
        // Calculate percentage contributions (ensure they sum to 100%)
        if (totalValue > 0) {
            decimal percentageSum = 0;
            for (int i = 0; i < products.Count; i++) {
                if (i == products.Count - 1) {
                    // Last item gets the remainder to ensure exact 100% (protected against negative values)
                    products[i].PercentageContribution = Math.Max(0, Math.Round(100 - percentageSum, 2));
                } else {
                    var percentage = Math.Round((products[i].TransactionValue / totalValue) * 100, 2);
                    products[i].PercentageContribution = percentage;
                    percentageSum += percentage;
                }
            }
        }
        
        // Sort by transaction value descending
        products = products.OrderByDescending(p => p.TransactionValue).ToList();
        
        return Result.Success(products);
    }

    public async Task<Result<List<OperatorTransactionSummaryModel>>> Handle(GetOperatorTransactionSummaryQuery request,
                                                                             CancellationToken cancellationToken) {
        // TODO: Replace with actual API call when endpoint is available
        // For now, return mock data for testing
        var operators = await this.ApiClient.GetOperators(request.AccessToken, Guid.Empty, request.EstateId, cancellationToken);
        
        if (!operators.IsSuccess) {
            return Result.Failure<List<OperatorTransactionSummaryModel>>(operators.Message);
        }

        var summary = new List<OperatorTransactionSummaryModel>();
        var random = new Random(42); // Use seed for consistent test data
        
        const decimal DefaultSuccessRate = 0.92m; // 92% success rate for mock data
        const decimal DefaultFeePercentage = 0.015m; // 1.5% fee rate
        
        foreach (var op in operators.Data) {
            var totalCount = random.Next(500, 5000);
            var successfulCount = (int)(totalCount * DefaultSuccessRate);
            var failedCount = totalCount - successfulCount;
            var totalValue = (decimal)(random.NextDouble() * 500000 + 50000);
            var totalFees = Math.Round(totalValue * DefaultFeePercentage, 2);
            
            summary.Add(new OperatorTransactionSummaryModel {
                OperatorId = op.Id,
                OperatorName = op.Name,
                TotalTransactionCount = totalCount,
                TotalTransactionValue = Math.Round(totalValue, 2),
                AverageTransactionValue = Math.Round(totalValue / totalCount, 2),
                SuccessfulTransactionCount = successfulCount,
                FailedTransactionCount = failedCount,
                TotalFeesEarned = totalFees
            });
        }
        
        // Apply filters if specified
        if (request.OperatorId.HasValue) {
            summary = summary.Where(s => s.OperatorId == request.OperatorId.Value).ToList();
        }
        
        return Result.Success(summary);
    }
}