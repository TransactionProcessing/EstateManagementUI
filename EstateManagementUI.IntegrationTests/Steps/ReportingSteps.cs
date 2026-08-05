using System.Globalization;
using System.Reflection;
using System.Text.Json;
using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;
using Shared.IntegrationTesting;
using Shouldly;
using TransactionProcessor.Client;
using TransactionProcessor.DataTransferObjects;
using TransactionProcessor.DataTransferObjects.Requests.Contract;
using TransactionProcessor.DataTransferObjects.Requests.Estate;
using TransactionProcessor.DataTransferObjects.Requests.Merchant;
using TransactionProcessor.DataTransferObjects.Requests.Operator;
using TransactionProcessor.DataTransferObjects.Responses.Contract;
using TransactionProcessor.DataTransferObjects.Responses.Merchant;
using TransactionProcessor.DataTransferObjects.Responses.Operator;
using EstateAssignOperatorRequest = TransactionProcessor.DataTransferObjects.Requests.Estate.AssignOperatorRequest;
using MerchantAssignOperatorRequest = TransactionProcessor.DataTransferObjects.Requests.Merchant.AssignOperatorRequest;
using SettlementSchedule = TransactionProcessor.DataTransferObjects.Responses.Merchant.SettlementSchedule;
using CalculationType = TransactionProcessor.DataTransferObjects.Responses.Contract.CalculationType;
using FeeType = TransactionProcessor.DataTransferObjects.Responses.Contract.FeeType;
using ProductType = TransactionProcessor.DataTransferObjects.Responses.Contract.ProductType;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "reporting")]
public sealed class ReportingSteps
{
    private const string ReportingDashboardPath = "/reporting";
    private const string TransactionDetailPath = "/reporting/transaction-detail";
    private const string TransactionSummaryMerchantPath = "/reporting/transaction-summary-merchant";
    private const string TransactionSummaryOperatorPath = "/reporting/transaction-summary-operator";
    private const string ProductPerformancePath = "/reporting/product-performance";
    private const string SettlementSummaryPath = "/reporting/settlement-summary";
    private const string MerchantSettlementHistoryPath = "/reporting/merchant-settlement-history";
    private const string SettlementReconciliationPath = "/reporting/settlement-reconciliation";
    private const string AnalyticalChartsPath = "/reporting/analytical-charts";

    private readonly IPage _page;
    private readonly TestingContext _testingContext;
    private readonly Dictionary<string, Guid> _operatorIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Guid> _merchantIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Guid> _contractIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Guid> _productIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, Guid> _productContractIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly ReportingStoredProcedureExecutor _storedProcedureExecutor;
    private Guid _estateId;

    public ReportingSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
        _storedProcedureExecutor = new ReportingStoredProcedureExecutor();
    }

    [Given("I have created the following reporting operators")]
    [When("I have created the following reporting operators")]
    public async Task GivenIHaveCreatedTheFollowingReportingOperators(DataTable table)
    {
        ITransactionProcessorClient client = _testingContext.DockerHelper.TransactionProcessorClient;
        var token = _testingContext.AccessToken;
        _estateId = _testingContext.Estates.Single().EstateId;

        foreach (var row in table.Rows)
        {
            var operatorName = ReqnrollTableHelper.GetStringRowValue(row, "Operator Name");
            var requireCustomMerchantNumber = bool.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Require Custom Merchant Number"));
            var requireCustomTerminalNumber = bool.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Require Custom Terminal Number"));
            var operatorId = await EnsureOperatorExistsAsync(client, token, operatorName, requireCustomMerchantNumber, requireCustomTerminalNumber);
            _operatorIds[operatorName] = operatorId;
        }
    }

    [When("I open the reporting dashboard")]
    public Task WhenIOpenTheReportingDashboard() => NavigateAsync(ReportingDashboardPath);

    [Then("I should see the reporting dashboard heading")]
    public Task ThenIShouldSeeTheReportingDashboardHeading() => AssertHeadingAsync("Reporting Dashboard");

    [Then("I should see the reporting dashboard links")]
    public Task ThenIShouldSeeTheReportingDashboardLinks() => AssertTextsVisibleAsync(
        "Transaction Detail Report",
        "Transaction Summary by Merchant",
        "Transaction Summary by Operator",
        "Product Performance Report",
        "Settlement Summary Report",
        "Merchant Settlement History",
        "Settlement vs Transaction Reconciliation Report",
        "Analytical Charts (Volume & Value)");

    [When("I open the transaction detail report")]
    public Task WhenIOpenTheTransactionDetailReport() => NavigateAsync(TransactionDetailPath);

    [Then("I should see the transaction detail report heading")]
    public Task ThenIShouldSeeTheTransactionDetailReportHeading() => AssertHeadingAsync("Transaction Detail Report");

    [Then("the transaction detail report should show these summary values")]
    public Task ThenTheTransactionDetailReportShouldShowTheseSummaryValues(DataTable table) => AssertSummaryValuesAsync(table);

    [Then("the transaction detail report should list these transactions")]
    public Task ThenTheTransactionDetailReportShouldListTheseTransactions(DataTable table) => AssertTransactionDetailRowsAsync(table);

    [When("I open the transaction summary by merchant report")]
    public Task WhenIOpenTheTransactionSummaryByMerchantReport() => NavigateAsync(TransactionSummaryMerchantPath);

    [Then("I should see the transaction summary by merchant report heading")]
    public Task ThenIShouldSeeTheTransactionSummaryByMerchantReportHeading() => AssertHeadingAsync("Transaction Summary by Merchant");

    [Then("the merchant summary report should show these summary values")]
    public Task ThenTheMerchantSummaryReportShouldShowTheseSummaryValues(DataTable table) => AssertSummaryValuesAsync(table);

    [Then("the merchant summary report should list these merchant rows")]
    public Task ThenTheMerchantSummaryReportShouldListTheseMerchantRows(DataTable table) => AssertMerchantSummaryRowsAsync(table);

    [When("I open the transaction summary by operator report")]
    public Task WhenIOpenTheTransactionSummaryByOperatorReport() => NavigateAsync(TransactionSummaryOperatorPath);

    [Then("I should see the transaction summary by operator report heading")]
    public Task ThenIShouldSeeTheTransactionSummaryByOperatorReportHeading() => AssertHeadingAsync("Transaction Summary by Operator");

    [Then("the operator summary report should show these summary values")]
    public Task ThenTheOperatorSummaryReportShouldShowTheseSummaryValues(DataTable table) => AssertSummaryValuesAsync(table);

    [Then("the operator summary report should list these operator rows")]
    public Task ThenTheOperatorSummaryReportShouldListTheseOperatorRows(DataTable table) => AssertOperatorSummaryRowsAsync(table);

    [When("I open the product performance report")]
    public Task WhenIOpenTheProductPerformanceReport() => NavigateAsync(ProductPerformancePath);

    [Then("I should see the product performance report heading")]
    public Task ThenIShouldSeeTheProductPerformanceReportHeading() => AssertHeadingAsync("Product Performance Report");

    [Then("the product performance report should show these summary values")]
    public Task ThenTheProductPerformanceReportShouldShowTheseSummaryValues(DataTable table) => AssertSummaryValuesAsync(table);

    [Then("the product performance report should list these product rows")]
    public Task ThenTheProductPerformanceReportShouldListTheseProductRows(DataTable table) => AssertProductPerformanceRowsAsync(table);

    [Then("the product performance report should validate the percentage split")]
    public async Task ThenTheProductPerformanceReportShouldValidateThePercentageSplit(DataTable table)
    {
        var row = table.Rows.Single();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Chart View" }).ClickAsync();
        await AssertTextVisibleAsync(ReqnrollTableHelper.GetStringRowValue(row, "Validation Message"));
    }

    [When("I open the settlement summary report")]
    public Task WhenIOpenTheSettlementSummaryReport() => NavigateAsync(SettlementSummaryPath);

    [Then("I should see the settlement summary report heading")]
    public Task ThenIShouldSeeTheSettlementSummaryReportHeading() => AssertHeadingAsync("Settlement Summary Report");

    [Then("the settlement summary report should show these summary values")]
    public Task ThenTheSettlementSummaryReportShouldShowTheseSummaryValues(DataTable table) => AssertSummaryValuesAsync(table);

    [Then("the settlement summary report should list these settlement rows")]
    public Task ThenTheSettlementSummaryReportShouldListTheseSettlementRows(DataTable table) => AssertSettlementSummaryRowsAsync(table);

    [Then("I should see the settlement summary placeholder")]
    public async Task ThenIShouldSeeTheSettlementSummaryPlaceholder()
    {
        await WaitForReportSummaryReadyAsync();
        await AssertTextVisibleAsync("No settlement data available for the selected period");
    }

    [When("I open the merchant settlement history report")]
    public Task WhenIOpenTheMerchantSettlementHistoryReport() => NavigateAsync(MerchantSettlementHistoryPath);

    [Then("I should see the merchant settlement history report heading")]
    public Task ThenIShouldSeeTheMerchantSettlementHistoryReportHeading() => AssertHeadingAsync("Merchant Settlement History");

    [Then("I should see the merchant settlement history placeholder")]
    public Task ThenIShouldSeeTheMerchantSettlementHistoryPlaceholder() => AssertTextVisibleAsync("No settlement history found for the selected criteria");

    [When("I run the todays summary stored procedures for {string}")]
    public async Task WhenIRunTheTodaysSummaryStoredProceduresFor(string dateValue) {
        await Task.Delay(500);
        await CreateStoredProcedures(CancellationToken.None);

        await Task.Delay(500);
        await RunSummaryStoredProcedureAsync("dbo.spBuildTodaysTransactions", dateValue);

        if (string.Equals(dateValue, "Today", StringComparison.OrdinalIgnoreCase))
        {
            await AssertExpectedSummaryTableRowsAsync();
        }
    }

    [When("I run the historic summary stored procedures for {string}")]
    public async Task WhenIRunTheHistoricSummaryStoredProceduresFor(string dateValue)
    {
        await Task.Delay(500);
        await RunSummaryStoredProcedureAsync("dbo.spBuildHistoricTransactions", dateValue);
    }

    [When("the summary tables should contain these row counts")]
    [Then("the summary tables should contain these row counts")]
    public Task ThenTheSummaryTablesShouldContainTheseRowCounts(DataTable table) => AssertSummaryTableRowsAsync(table);

    [When("I open the settlement reconciliation report")]
    public Task WhenIOpenTheSettlementReconciliationReport() => NavigateAsync(SettlementReconciliationPath);

    [Then("I should see the settlement reconciliation report heading")]
    public Task ThenIShouldSeeTheSettlementReconciliationReportHeading() => AssertHeadingAsync("Settlement vs Transaction Reconciliation Report");

    [Then("I should see the settlement reconciliation placeholder")]
    public Task ThenIShouldSeeTheSettlementReconciliationPlaceholder() => AssertTextVisibleAsync("Settlement vs transaction reconciliation report functionality will be implemented here.");

    [When("I open the analytical charts report")]
    public Task WhenIOpenTheAnalyticalChartsReport() => NavigateAsync(AnalyticalChartsPath);

    [Then("I should see the analytical charts report heading")]
    public Task ThenIShouldSeeTheAnalyticalChartsReportHeading() => AssertHeadingAsync("Analytical Charts (Volume & Value)");

    [Then("the analytical charts report should show these summary values")]
    public Task ThenTheAnalyticalChartsReportShouldShowTheseSummaryValues(DataTable table) => AssertSummaryValuesWithRefreshRetryAsync(table);

    [Then("the analytical charts report should use this comparison date")]
    public async Task ThenTheAnalyticalChartsReportShouldUseThisComparisonDate(DataTable table)
    {
        await ExecuteWithFailureScreenshotAsync("analytical-charts-comparison-date", async () =>
        {
            var row = table.Rows.Single();
            var dateToSelect = String.Empty;
            if (!DateOnly.TryParseExact(ReqnrollTableHelper.GetStringRowValue(row, "Comparison Date"), "yyyy-MM-dd", out DateOnly comparisonDate))
            {
                // try using the date to string helper
                var date = ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(row, "Comparison Date"), DateTime.Now);
                dateToSelect = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            else
            {
                dateToSelect = comparisonDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            var expectedLabel = DateTime.ParseExact(dateToSelect, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("MMM dd", CultureInfo.InvariantCulture);
            await _page.Locator("#comparisonDateSelector").SelectOptionAsync(new SelectOptionValue { Value = dateToSelect });
            await WaitForAnalyticalChartsReadyAsync();
            await _page.WaitForFunctionAsync($@"() => {{
                const chart = Chart.getChart('volumeChart');
                if (!chart || !chart.data || !chart.data.datasets || chart.data.datasets.length < 2) return false;
                const label = chart.data.datasets[1].label ?? '';
                return label.includes('{expectedLabel}');
            }}", new FrameWaitForFunctionOptions { Timeout = 120000 });
        });
    }

    [Then("the analytical charts report should compare these chart totals")]
    public async Task ThenTheAnalyticalChartsReportShouldCompareTheseChartTotals(DataTable table)
    {
        await ExecuteWithFailureScreenshotAsync("analytical-charts-summary-values", async () =>
        {
            foreach (var row in table.Rows)
            {
                var chart = ReqnrollTableHelper.GetStringRowValue(row, "Chart");
                var today = ReqnrollTableHelper.GetStringRowValue(row, "Today");
                var comparison = ReqnrollTableHelper.GetStringRowValue(row, "Comparison");

                if (chart.Equals("Volume", StringComparison.OrdinalIgnoreCase))
                {
                    var volume = await WaitForChartTotalsAsync(
                        "volumeChart",
                        int.Parse(today, CultureInfo.InvariantCulture),
                        int.Parse(comparison, CultureInfo.InvariantCulture),
                        0m,
                        0m);
                    volume.totalTodayCount.ShouldBe(int.Parse(today, CultureInfo.InvariantCulture));
                    volume.totalComparisonCount.ShouldBe(int.Parse(comparison, CultureInfo.InvariantCulture));
                }
                else if (chart.Equals("Value", StringComparison.OrdinalIgnoreCase))
                {
                    var value = await WaitForChartTotalsAsync(
                        "valueChart",
                        0,
                        0,
                        decimal.Parse(today, CultureInfo.InvariantCulture),
                        decimal.Parse(comparison, CultureInfo.InvariantCulture));
                    value.totalTodayValue.ShouldBe(decimal.Parse(today, CultureInfo.InvariantCulture));
                    value.totalComparisonValue.ShouldBe(decimal.Parse(comparison, CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(table), $"Unsupported chart '{chart}'.");
                }
            }
        });
    }

    [Then("the analytical charts report should compare these hourly chart points")]
    public Task ThenTheAnalyticalChartsReportShouldCompareTheseHourlyChartPoints(DataTable table) =>
        AssertAnalyticalChartsHourlyPointsAsync(table);

    [Given("I have created the following reporting merchant setups")]
    [When("I have created the following reporting merchant setups")]
    public async Task GivenIHaveCreatedTheFollowingReportingMerchantSetups(DataTable table)
    {
        ITransactionProcessorClient client = _testingContext.DockerHelper.TransactionProcessorClient;
        var token = _testingContext.AccessToken;
        _estateId = _testingContext.Estates.Single().EstateId;

        foreach (var row in table.Rows)
        {
            var merchantName = ReqnrollTableHelper.GetStringRowValue(row, "Merchant Name");
            var settlementSchedule = Enum.Parse<SettlementSchedule>(ReqnrollTableHelper.GetStringRowValue(row, "Settlement Schedule"), true);
            var operatorName = ReqnrollTableHelper.GetStringRowValue(row, "Operator Name");
            var merchantNumber = ReqnrollTableHelper.GetStringRowValue(row, "Merchant Number");
            var terminalNumber = ReqnrollTableHelper.GetStringRowValue(row, "Terminal Number");
            var deviceIdentifier = ReqnrollTableHelper.GetStringRowValue(row, "Device Identifier");
            var contractDescription = ReqnrollTableHelper.GetStringRowValue(row, "Contract Description");
            var productName = ReqnrollTableHelper.GetStringRowValue(row, "Product Name");
            var productDisplayText = ReqnrollTableHelper.GetStringRowValue(row, "Product Display Text");
            var productType = Enum.Parse<ProductType>(ReqnrollTableHelper.GetStringRowValue(row, "Product Type"), true);
            var productValue = decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Product Value"), CultureInfo.InvariantCulture);
            var feeDescription = ReqnrollTableHelper.GetStringRowValue(row, "Fee Description");
            var feeType = Enum.Parse<FeeType>(ReqnrollTableHelper.GetStringRowValue(row, "Fee Type"), true);
            var calculationType = Enum.Parse<CalculationType>(ReqnrollTableHelper.GetStringRowValue(row, "Calculation Type"), true);
            var feeValue = decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Fee Value"), CultureInfo.InvariantCulture);

            var operatorId = await ResolveOperatorIdAsync(client, token, operatorName);
            var merchantId = await EnsureMerchantAsync(client, token, merchantName, settlementSchedule);
            var (contractId, productId) = await EnsureContractAndProductAsync(client, token, contractDescription, operatorId, productName, productDisplayText, productType, productValue, feeDescription, feeType, calculationType, feeValue);

            _merchantIds[merchantName] = merchantId;
            _contractIds[contractDescription] = contractId;
            _productIds[productName] = productId;
            _productContractIds[productName] = contractId;

            await AssignMerchantAndDeviceAsync(client, token, merchantId, contractId, operatorId, merchantNumber, terminalNumber, deviceIdentifier);
        }
    }

    [Given("I have made the following reporting merchant deposits")]
    [When("I have made the following reporting merchant deposits")]
    public async Task GivenIHaveMadeTheFollowingReportingMerchantDeposits(DataTable table)
    {
        var helper = GetHelper();

        foreach (var row in table.Rows)
        {
            var merchantName = ReqnrollTableHelper.GetStringRowValue(row, "Merchant Name");
            var amount = decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Amount"), CultureInfo.InvariantCulture);
            var date = ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(row, "Date"), DateTime.Now);
            var reference = ReqnrollTableHelper.GetStringRowValue(row, "Reference");

            await Task.Delay(500);
            await helper.OpenMerchantDepositAsync(merchantName);
            await Task.Delay(500);
            await helper.AssertMerchantDepositVisibleAsync(merchantName);
            await Task.Delay(500);
            await helper.SubmitMerchantDepositAsync(amount, date, reference);
            await Task.Delay(500);
        }
    }

    [Given("I have seeded the following reporting sales")]
    [When("I have seeded the following reporting sales")]
    public async Task GivenIHaveSeededTheFollowingReportingSales(DataTable table)
    {
        ITransactionProcessorClient client = _testingContext.DockerHelper.TransactionProcessorClient;
        var token = _testingContext.AccessToken;
        _estateId = _testingContext.Estates.Single().EstateId;

        foreach (var row in table.Rows)
        {
            var transactionNumber = ReqnrollTableHelper.GetStringRowValue(row, "Transaction Number");
            var merchantName = ReqnrollTableHelper.GetStringRowValue(row, "Merchant Name");
            var productName = ReqnrollTableHelper.GetStringRowValue(row, "Product Name");
            var operatorName = ReqnrollTableHelper.GetStringRowValue(row, "Operator Name");
            var deviceIdentifier = ReqnrollTableHelper.GetStringRowValue(row, "Device Identifier");
            //var transactionDateTime = DateTime.ParseExact(ReqnrollTableHelper.GetStringRowValue(row, "Transaction Date Time"), "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            var transactionDateTime= ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(row, "Transaction Date Time"), DateTime.Now);
            transactionDateTime= transactionDateTime.AddHours(ReqnrollTableHelper.GetIntValue(row, "Hours"));
            
            var transactionType = ReqnrollTableHelper.GetStringRowValue(row, "Transaction Type");
            var amount = ReqnrollTableHelper.GetDecimalValue(row, "Amount");
            var accountNumber = ReqnrollTableHelper.GetStringRowValue(row, "Account Number");

            await PerformTransactionAsync(client, token, merchantName, productName, operatorName, deviceIdentifier, transactionDateTime, transactionNumber, transactionType, amount, accountNumber);
        }
    }

    [Given("I have processed the following reporting settlements")]
    [When("I have processed the following reporting settlements")]
    public async Task GivenIHaveProcessedTheFollowingReportingSettlements(DataTable table)
    {
        ITransactionProcessorClient client = _testingContext.DockerHelper.TransactionProcessorClient;
        var token = _testingContext.AccessToken;
        _estateId = _testingContext.Estates.Single().EstateId;

        foreach (var row in table.Rows)
        {
            var merchantName = ReqnrollTableHelper.GetStringRowValue(row, "Merchant Name");
            var settlementDate = ReqnrollTableHelper.GetDateForDateString(ReqnrollTableHelper.GetStringRowValue(row, "Settlement Date"), DateTime.Now);
            var merchantId = await ResolveMerchantIdAsync(client, token, merchantName);

            var result = await client.ProcessSettlement(token, settlementDate, _estateId, merchantId, CancellationToken.None);
            result.IsSuccess.ShouldBeTrue();
        }
    }

    private async Task<Guid> EnsureOperatorExistsAsync(ITransactionProcessorClient client, string token, string operatorName, bool requireCustomMerchantNumber, bool requireCustomTerminalNumber)
    {
        var operators = await GetOperatorsAsync(client, token);
        var existing = operators.SingleOrDefault(o => o.Name == operatorName);
        if (existing != null)
        {
            return existing.OperatorId;
        }

        var createResult = await client.CreateOperator(token, _estateId, new CreateOperatorRequest
        {
            Name = operatorName,
            RequireCustomMerchantNumber = requireCustomMerchantNumber,
            RequireCustomTerminalNumber = requireCustomTerminalNumber
        }, CancellationToken.None);

        createResult.IsSuccess.ShouldBeTrue(createResult.Message);

        var createdOperator = await WaitForOperatorAsync(client, token, operatorName);
        var assignResult = await client.AssignOperatorToEstate(token, _estateId, new EstateAssignOperatorRequest
        {
            OperatorId = createdOperator.OperatorId
        }, CancellationToken.None);

        assignResult.IsSuccess.ShouldBeTrue(assignResult.Message);
        return createdOperator.OperatorId;
    }

    private async Task<Guid> ResolveOperatorIdAsync(ITransactionProcessorClient client, string token, string operatorName)
    {
        if (_operatorIds.TryGetValue(operatorName, out var operatorId))
        {
            return operatorId;
        }

        operatorId = (await WaitForOperatorAsync(client, token, operatorName)).OperatorId;
        _operatorIds[operatorName] = operatorId;
        return operatorId;
    }

    private async Task<Guid> ResolveMerchantIdAsync(ITransactionProcessorClient client, string token, string merchantName)
    {
        if (_merchantIds.TryGetValue(merchantName, out var merchantId))
        {
            return merchantId;
        }

        merchantId = (await WaitForMerchantAsync(client, token, merchantName)).MerchantId;
        _merchantIds[merchantName] = merchantId;
        return merchantId;
    }

    private async Task<Guid> EnsureMerchantAsync(ITransactionProcessorClient client, string token, string merchantName, SettlementSchedule settlementSchedule)
    {
        if (_merchantIds.TryGetValue(merchantName, out var merchantId))
        {
            return merchantId;
        }

        var merchant = await WaitForMerchantAsync(client, token, merchantName, allowMissing: true);
        if (merchant != null)
        {
            _merchantIds[merchantName] = merchant.MerchantId;
            return merchant.MerchantId;
        }

        var createMerchantResult = await client.CreateMerchant(token, _estateId, new CreateMerchantRequest
        {
            Name = merchantName,
            SettlementSchedule = settlementSchedule,
            Address = new TransactionProcessor.DataTransferObjects.Requests.Merchant.Address
            {
                AddressLine1 = $"1 {merchantName} Road",
                Town = "Integration Town",
                Region = "Integration Region",
                PostalCode = "INT 1NG",
                Country = "United Kingdom"
            },
            Contact = new TransactionProcessor.DataTransferObjects.Requests.Merchant.Contact
            {
                ContactName = $"{merchantName} Contact",
                EmailAddress = $"{merchantName.Replace(' ', '.').ToLowerInvariant()}@example.com",
                PhoneNumber = "01234567890"
            }
        }, CancellationToken.None);

        createMerchantResult.IsSuccess.ShouldBeTrue(createMerchantResult.Message);

        merchant = await WaitForMerchantAsync(client, token, merchantName);
        _merchantIds[merchantName] = merchant.MerchantId;
        return merchant.MerchantId;
    }

    private async Task<(Guid contractId, Guid productId)> EnsureContractAndProductAsync(ITransactionProcessorClient client, string token, string contractDescription, Guid operatorId, string productName, string productDisplayText, ProductType productType, decimal productValue, string feeDescription, FeeType feeType, CalculationType calculationType, decimal feeValue)
    {
        var contracts = await GetContractsAsync(client, token);
        var contract = contracts.SingleOrDefault(c => c.Description == contractDescription);
        if (contract == null)
        {
            var createContractResult = await client.CreateContract(token, _estateId, new CreateContractRequest
            {
                OperatorId = operatorId,
                Description = contractDescription
            }, CancellationToken.None);

            createContractResult.IsSuccess.ShouldBeTrue(createContractResult.Message);
            contract = await WaitForContractAsync(client, token, contractDescription);
        }

        var products = contract.Products ?? [];
        var product = products.SingleOrDefault(p => p.Name == productName);
        if (product == null)
        {
            var productResult = await client.AddProductToContract(token, _estateId, contract.ContractId, new AddProductToContractRequest
            {
                DisplayText = productDisplayText,
                ProductName = productName,
                ProductType = productType,
                Value = productValue
            }, CancellationToken.None);

            productResult.IsSuccess.ShouldBeTrue(productResult.Message);

            contract = await WaitForContractAsync(client, token, contractDescription, productName);
            products = contract.Products ?? [];
            product = products.Single(p => p.Name == productName);

            var feeResult = await client.AddTransactionFeeForProductToContract(token, _estateId, contract.ContractId, product.ProductId, new AddTransactionFeeForProductToContractRequest
            {
                Description = feeDescription,
                CalculationType = calculationType,
                FeeType = feeType,
                Value = feeValue
            }, CancellationToken.None);

            feeResult.IsSuccess.ShouldBeTrue(feeResult.Message);
        }

        _contractIds[contractDescription] = contract.ContractId;
        _productIds[productName] = product.ProductId;
        return (contract.ContractId, product.ProductId);
    }

    private async Task AssignMerchantAndDeviceAsync(ITransactionProcessorClient client, string token, Guid merchantId, Guid contractId, Guid operatorId, string merchantNumber, string terminalNumber, string deviceIdentifier)
    {
        var merchantContractResult = await client.AddContractToMerchant(token, _estateId, merchantId, new AddMerchantContractRequest
        {
            ContractId = contractId
        }, CancellationToken.None);

        merchantContractResult.IsSuccess.ShouldBeTrue(merchantContractResult.Message);

        var operatorResult = await client.AssignOperatorToMerchant(token, _estateId, merchantId, new MerchantAssignOperatorRequest
        {
            MerchantNumber = merchantNumber,
            TerminalNumber = terminalNumber,
            OperatorId = operatorId
        }, CancellationToken.None);

        operatorResult.IsSuccess.ShouldBeTrue(operatorResult.Message);

        var deviceResult = await client.AddDeviceToMerchant(token, _estateId, merchantId, new AddMerchantDeviceRequest
        {
            DeviceIdentifier = deviceIdentifier
        }, CancellationToken.None);

        deviceResult.IsSuccess.ShouldBeTrue(deviceResult.Message);
    }

    private async Task PerformTransactionAsync(ITransactionProcessorClient client, string token, string merchantName, string productName, string operatorName, string deviceIdentifier, DateTime when, string transactionNumber, string transactionType, decimal amount, string accountNumber)
    {
        var merchantId = await ResolveMerchantIdAsync(client, token, merchantName);
        var productId = _productIds.TryGetValue(productName, out var resolvedProductId)
            ? resolvedProductId
            : (await GetContractsAsync(client, token))
                .SelectMany(c => c.Products ?? [])
                .Single(p => p.Name == productName)
                .ProductId;
        var contractId = _productContractIds.TryGetValue(productName, out var mappedContractId)
            ? mappedContractId
            : (await GetContractsAsync(client, token))
                .Single(c => (c.Products ?? []).Any(p => p.Name == productName))
                .ContractId;
        var operatorId = await ResolveOperatorIdAsync(client, token, operatorName);


        var request = new SaleTransactionRequest {
            EstateId = _estateId,
            MerchantId = merchantId,
            ContractId = contractId,
            ProductId = productId,
            OperatorId = operatorId,
            DeviceIdentifier = deviceIdentifier,
            TransactionDateTime = when,
            TransactionNumber = transactionNumber,
            TransactionType = transactionType,
            CustomerEmailAddress = "customer@example.com",
            AdditionalTransactionMetadata = new Dictionary<string, string>() { { "Amount", amount.ToString() }, }
        };

        if (operatorName == "Safaricom")
        {
            request.AdditionalTransactionMetadata.Add("CustomerAccountNumber", accountNumber);
        }

        if (operatorName == "Voucher") {
            request.AdditionalTransactionMetadata.Add("RecipientMobile", accountNumber);
        }


        const int attempts = 5;
        Exception? lastFailure = null;

        for (var attempt = 1; attempt <= attempts; attempt++)
        {
            var result = await client.PerformTransaction(token, request, CancellationToken.None);

            try
            {
                result.IsSuccess.ShouldBeTrue(result.Message);
                result.Data.ResponseCode.ShouldBe("0000");
                return;
            }
            catch (Exception exception) when (result.Data?.ResponseCode == "1008" && attempt < attempts)
            {
                lastFailure = exception;
                await Task.Delay(500);
            }
        }

        throw lastFailure ?? new InvalidOperationException("Transaction did not complete successfully.");
    }

    private async Task<List<OperatorResponse>> GetOperatorsAsync(ITransactionProcessorClient client, string token)
    {
        var result = await client.GetOperators(token, _estateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue(result.Message);
        return result.Data ?? new List<OperatorResponse>();
    }

    private async Task<OperatorResponse> WaitForOperatorAsync(ITransactionProcessorClient client, string token, string operatorName)
    {
        const int attempts = 20;

        for (var attempt = 0; attempt < attempts; attempt++)
        {
            var operatorResult = (await GetOperatorsAsync(client, token)).SingleOrDefault(o => o.Name == operatorName);
            if (operatorResult != null)
            {
                return operatorResult;
            }

            await Task.Delay(500);
        }

        throw new InvalidOperationException($"Operator '{operatorName}' was not visible after create/assign.");
    }

    private async Task<List<MerchantResponse>> GetMerchantsAsync(ITransactionProcessorClient client, string token)
    {
        var result = await client.GetMerchants(token, _estateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue(result.Message);
        return result.Data ?? new List<MerchantResponse>();
    }

    private async Task<MerchantResponse> WaitForMerchantAsync(ITransactionProcessorClient client, string token, string merchantName, bool allowMissing = false)
    {
        const int attempts = 20;

        for (var attempt = 0; attempt < attempts; attempt++)
        {
            var result = await client.GetMerchants(token, _estateId, CancellationToken.None);
            if (result.IsSuccess)
            {
                var merchants = result.Data ?? new List<MerchantResponse>();
                var merchant = merchants.SingleOrDefault(m => m.MerchantName == merchantName);
                if (merchant != null)
                {
                    return merchant;
                }
            }

            await Task.Delay(500);
        }

        if (allowMissing)
        {
            return null!;
        }

        throw new InvalidOperationException($"Merchant '{merchantName}' was not visible after create.");
    }

    private async Task<List<ContractResponse>> GetContractsAsync(ITransactionProcessorClient client, string token)
    {
        var result = await client.GetContracts(token, _estateId, CancellationToken.None);
        result.IsSuccess.ShouldBeTrue(result.Message);
        return result.Data ?? new List<ContractResponse>();
    }

    private async Task<ContractResponse> WaitForContractAsync(ITransactionProcessorClient client, string token, string contractDescription, string? productName = null)
    {
        const int attempts = 20;

        for (var attempt = 0; attempt < attempts; attempt++)
        {
            var result = await client.GetContracts(token, _estateId, CancellationToken.None);
            if (result.IsSuccess)
            {
                var contracts = result.Data ?? new List<ContractResponse>();
                var contract = contracts.SingleOrDefault(c => c.Description == contractDescription);
                if (contract != null && (productName == null || (contract.Products ?? []).Any(p => p.Name == productName)))
                {
                    return contract;
                }
            }

            await Task.Delay(500);
        }

        throw new InvalidOperationException($"Contract '{contractDescription}' was not visible after create.");
    }

    private async Task NavigateAsync(string path)
    {
        await _page.GotoAsync(
            $"{ResolveBaseUrl()}{path}",
            new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 120000
            });

        await _page.WaitForLoadStateAsync(
            LoadState.DOMContentLoaded,
            new PageWaitForLoadStateOptions { Timeout = 120000 });
    }

    private string ResolveBaseUrl()
    {
        var hostPort = _testingContext.DockerHelper.GetHostPort(Shared.IntegrationTesting.ContainerType.EstateManagementUI);
        return $"https://127.0.0.1:{hostPort}";
    }

    private async Task AssertHeadingAsync(string heading)
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = heading }).IsVisibleAsync()).ShouldBeTrue();
    }

    private async Task AssertTextsVisibleAsync(params string[] texts)
    {
        foreach (var text in texts)
        {
            await AssertTextVisibleAsync(text);
        }
    }

    private async Task AssertTextVisibleAsync(string text)
    {
        (await _page.GetByText(text).First.IsVisibleAsync()).ShouldBeTrue();
    }

    private async Task AssertElementVisibleAsync(string selector)
    {
        (await _page.Locator(selector).IsVisibleAsync()).ShouldBeTrue();
    }

    private async Task AssertInfoBoxValueAsync(string label, int expectedValue)
    {
        var infoBox = _page.Locator(".info-box").Filter(new() { Has = _page.GetByText(label, new() { Exact = true }) });
        var text = await WaitForInfoBoxValueAsync(infoBox, expectedValue.ToString(CultureInfo.CurrentCulture));
        int.Parse(text!, NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.CurrentCulture).ShouldBe(expectedValue);
    }

    private async Task AssertMoneyInfoBoxValueAsync(string label, decimal expectedValue)
    {
        var infoBox = _page.Locator(".info-box").Filter(new() { Has = _page.GetByText(label, new() { Exact = true }) });
        var text = await WaitForInfoBoxValueAsync(infoBox, expectedValue.ToString("C", CultureInfo.CurrentCulture));
        ParseMoney(text!).ShouldBe(expectedValue);
    }

    private static async Task<string?> WaitForInfoBoxValueAsync(ILocator infoBox, string expectedValue)
    {
        const int attempts = 40;

        for (var attempt = 0; attempt < attempts; attempt++)
        {
            var text = await infoBox.Locator(".info-box-number").First.TextContentAsync();
            if (string.Equals(text?.Trim(), expectedValue, StringComparison.CurrentCulture))
            {
                return text;
            }

            await Task.Delay(500);
        }

        return await infoBox.Locator(".info-box-number").First.TextContentAsync();
    }

    private async Task AssertSummaryValuesWithRefreshRetryAsync(DataTable table)
    {
        Exception? lastFailure = null;

        for (var attempt = 1; attempt <= 3; attempt++)
        {
            try
            {
                if (attempt > 1)
                {
                    await _page.ReloadAsync();
                    await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                }

                await AssertSummaryValuesAsync(table);
                return;
            }
            catch (Exception exception)
            {
                lastFailure = exception;
            }
        }

        throw lastFailure ?? new InvalidOperationException("Unable to verify analytical charts summary values.");
    }

    private async Task AssertSummaryValuesAsync(DataTable table)
    {
        await ExecuteWithFailureScreenshotAsync("reporting-summary-values", async () =>
        {
            await WaitForReportSummaryReadyAsync();

            foreach (var row in table.Rows)
            {
                var label = ReqnrollTableHelper.GetStringRowValue(row, "Label");
                var value = ReqnrollTableHelper.GetStringRowValue(row, "Value");

                if (IsMoneyLabel(label))
                {
                    await AssertMoneyInfoBoxValueAsync(label, decimal.Parse(value, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
                }
                else
                {
                    await AssertInfoBoxValueAsync(label, int.Parse(value, NumberStyles.Integer | NumberStyles.AllowThousands, CultureInfo.InvariantCulture));
                }
            }
        });
    }

    private async Task WaitForReportSummaryReadyAsync()
    {
        var spinner = _page.Locator(".animate-spin");
        if (await spinner.CountAsync() > 0)
        {
            await spinner.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Detached,
                Timeout = 120000
            });
        }

        await _page.Locator(".info-box").First.WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 120000
        });
    }

    private async Task AssertSummaryTableRowsAsync(DataTable table)
    {
        foreach (var row in table.Rows)
        {
            var summaryTable = ReqnrollTableHelper.GetStringRowValue(row, "Table");
            var dateValue = ReqnrollTableHelper.GetStringRowValue(row, "Date");
            var expectedCount = ReqnrollTableHelper.GetIntValue(row, "Expected Count");
            await AssertSummaryTableRowCountAsync(summaryTable, dateValue, expectedCount);
        }
    }

    private async Task AssertExpectedSummaryTableRowsAsync()
    {
        await AssertSummaryTableRowCountAsync("TodayTransactions", "Today", 3);
        await AssertSummaryTableRowCountAsync("TransactionHistory", "Yesterday", 2);
    }

    private async Task AssertSummaryTableRowCountAsync(string summaryTable, string dateValue, int expectedCount)
    {
        var connectionString = this.GetLocalConnectionString("TransactionProcessorReadModel");
        var dateParam = ReqnrollTableHelper.GetDateForDateString(dateValue, DateTime.Now).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

        var sql = summaryTable switch
        {
            "TodayTransactions" => "select count(*) as [RowCount] from TodayTransactions where TransactionDate = @Date",
            "TransactionHistory" => "select count(*) as [RowCount] from TransactionHistory where TransactionDate = @Date",
            _ => throw new ArgumentOutOfRangeException(nameof(summaryTable), $"Unsupported summary table '{summaryTable}'.")
        };

        var result = await _storedProcedureExecutor.ExecuteTextAsync(
            connectionString,
            sql,
            new Dictionary<string, object?> { ["@Date"] = dateParam },
            CancellationToken.None);

        result.Rows.Count.ShouldBe(1);
        Convert.ToInt32(result.Rows[0]["RowCount"], CultureInfo.InvariantCulture).ShouldBe(expectedCount);
    }

    private async Task WaitForAnalyticalChartsReadyAsync()
    {
        await _page.Locator(".animate-spin").WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Detached,
            Timeout = 60000
        });

        await _page.WaitForFunctionAsync(@"() => {
            const volumeChart = Chart.getChart('volumeChart');
            const valueChart = Chart.getChart('valueChart');
            return !!volumeChart && !!valueChart && volumeChart.data.datasets.length >= 2 && valueChart.data.datasets.length >= 2;
        }", new FrameWaitForFunctionOptions { Timeout = 120000 });
    }

    private async Task<(int totalTodayCount, int totalComparisonCount, decimal totalTodayValue, decimal totalComparisonValue)> WaitForChartTotalsAsync(
        string chartId,
        int expectedTodayCount,
        int expectedComparisonCount,
        decimal expectedTodayValue,
        decimal expectedComparisonValue)
    {
        Exception? lastError = null;
        (int totalTodayCount, int totalComparisonCount, decimal totalTodayValue, decimal totalComparisonValue)? lastObservedChart = null;

        for (var attempt = 0; attempt < 300; attempt++)
        {
            try
            {
                var chart = await ReadChartAsync(chartId);
                lastObservedChart = chart;
                var totalsMatch =
                    chart.totalTodayCount == expectedTodayCount &&
                    chart.totalComparisonCount == expectedComparisonCount &&
                    chart.totalTodayValue == expectedTodayValue &&
                    chart.totalComparisonValue == expectedComparisonValue;

                if (totalsMatch)
                {
                    return chart;
                }
            }
            catch (Exception exception)
            {
                lastError = exception;
            }

            await Task.Delay(2000);
        }

        if (lastError != null)
        {
            throw new TimeoutException(
                $"Timed out waiting for chart '{chartId}' to reach the expected totals. " +
                $"Last observed values: today count={lastObservedChart?.totalTodayCount}, comparison count={lastObservedChart?.totalComparisonCount}, " +
                $"today value={lastObservedChart?.totalTodayValue}, comparison value={lastObservedChart?.totalComparisonValue}.",
                lastError);
        }

        throw new TimeoutException(
            $"Timed out waiting for chart '{chartId}' to reach the expected totals. " +
            $"Last observed values: today count={lastObservedChart?.totalTodayCount}, comparison count={lastObservedChart?.totalComparisonCount}, " +
            $"today value={lastObservedChart?.totalTodayValue}, comparison value={lastObservedChart?.totalComparisonValue}.");
    }

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);

    public String GetLocalConnectionString(String databaseName)
    {
        String dbName = $"{databaseName}-{this._estateId}";
        var dockerHelper = this._testingContext.DockerHelper;
        Int32? databaseHostPort = dockerHelper.GetHostPort(ContainerType.SqlServer);

        return $"server=localhost,{databaseHostPort};database={dbName};user id={this._testingContext.DockerHelper.SqlCredentials.usename};password={this._testingContext.DockerHelper.SqlCredentials.password};Encrypt=false";
    }

    private async Task CreateStoredProcedures(CancellationToken cancellationToken)
    {
        String executingAssemblyLocation = Assembly.GetExecutingAssembly().Location;
        String executingAssemblyFolder = Path.GetDirectoryName(executingAssemblyLocation);

        String scriptsFolder = $@"{executingAssemblyFolder}/StoredProcedures";

        String[] directiories = Directory.GetDirectories(scriptsFolder);
        if (directiories.Length == 0)
        {
            var list = new List<string> { scriptsFolder };
            directiories = list.ToArray();
        }
        directiories = directiories.OrderBy(d => d).ToArray();

        foreach (String directiory in directiories)
        {
            String[] sqlFiles = Directory.GetFiles(directiory, "*.sql");
            foreach (String sqlFile in sqlFiles.OrderBy(x => x))
            {
                String sql = System.IO.File.ReadAllText(sqlFile);

                // Check here is we need to replace a Database Name
                if (sql.Contains("{DatabaseName}"))
                {
                    sql = sql.Replace("{DatabaseName}",
                         $"TransactionProcessorReadModel-{this._estateId}");
                }

                // Create the new view using the original sql from file
                await this._storedProcedureExecutor.RunSqlAsync(this.GetLocalConnectionString("TransactionProcessorReadModel"),
                    sql, cancellationToken);
            }
        }
    }

    private async Task ExecuteWithFailureScreenshotAsync(string name, Func<Task> action)
    {
        try
        {
            await action();
        }
        catch
        {
            await CaptureFailureScreenshotAsync(name);
            throw;
        }
    }

    private async Task CaptureFailureScreenshotAsync(string name)
    {
        try
        {
            var screenshotDirectory = Path.Combine(Environment.CurrentDirectory, "TestResults", "Screenshots", "Reporting");
            Directory.CreateDirectory(screenshotDirectory);

            var screenshotPath = Path.Combine(
                screenshotDirectory,
                $"screenshot-{SanitizeFileNameSegment(name)}-{DateTime.Now:yyyyMMddHHmmss}.png");

            await _page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = screenshotPath,
                FullPage = true
            });

            Console.WriteLine($"Screenshot saved to: {screenshotPath}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($"Failed to save screenshot for '{name}': {exception.Message}");
        }
    }

    private static string SanitizeFileNameSegment(string value)
    {
        var invalidCharacters = Path.GetInvalidFileNameChars();
        var sanitized = new string(value.Select(character => invalidCharacters.Contains(character) ? '_' : character).ToArray());
        return sanitized.Replace(' ', '_');
    }

    private async Task RunSummaryStoredProcedureAsync(string storedProcedureName, string dateValue) {
        await Task.Delay(500);
        String dateParam = ReqnrollTableHelper.GetDateForDateString(dateValue, DateTime.Now).ToString("yyyy-MM-dd");

        await _storedProcedureExecutor.ExecuteAsync(this.GetLocalConnectionString("TransactionProcessorReadModel"),
            storedProcedureName,
            new Dictionary<string, object?> { ["@Date"] = dateParam },
            CancellationToken.None);
    }

    private async Task AssertTransactionDetailRowsAsync(DataTable table)
    {
        (await _page.Locator("table tbody tr").CountAsync()).ShouldBe(table.Rows.Count);

        foreach (var row in table.Rows)
        {
            var transactionNumber = ReqnrollTableHelper.GetStringRowValue(row, "Transaction Number");
            var merchant = ReqnrollTableHelper.GetStringRowValue(row, "Merchant");
            var product = ReqnrollTableHelper.GetStringRowValue(row, "Product");
            var type = ReqnrollTableHelper.GetStringRowValue(row, "Type");
            var status = ReqnrollTableHelper.GetStringRowValue(row, "Status");

            var matchedRow = _page.Locator("table tbody tr").Filter(new() { HasText = transactionNumber });
            (await matchedRow.IsVisibleAsync()).ShouldBeTrue();
            var rowText = await matchedRow.InnerTextAsync();
            rowText.ShouldContain(transactionNumber);
            rowText.ShouldContain(merchant);
            rowText.ShouldContain(product);
            rowText.ShouldContain(type);
            rowText.ShouldContain(status);
        }
    }

    private async Task AssertMerchantSummaryRowsAsync(DataTable table)
    {
        (await _page.Locator("table tbody tr").CountAsync()).ShouldBe(table.Rows.Count);

        foreach (var row in table.Rows)
        {
            var merchantName = ReqnrollTableHelper.GetStringRowValue(row, "Merchant");
            await AssertSummaryRowAsync(
                merchantName,
                ReqnrollTableHelper.GetStringRowValue(row, "Total Count"),
                ReqnrollTableHelper.GetStringRowValue(row, "Total Value"),
                ReqnrollTableHelper.GetStringRowValue(row, "Average Value"),
                ReqnrollTableHelper.GetStringRowValue(row, "Successful"),
                ReqnrollTableHelper.GetStringRowValue(row, "Failed"),
                ReqnrollTableHelper.GetStringRowValue(row, "Success Rate"));
        }
    }

    private async Task AssertOperatorSummaryRowsAsync(DataTable table)
    {
        (await _page.Locator("table tbody tr").CountAsync()).ShouldBe(table.Rows.Count);

        foreach (var row in table.Rows)
        {
            var operatorName = ReqnrollTableHelper.GetStringRowValue(row, "Operator");
            await AssertSummaryRowAsync(
                operatorName,
                ReqnrollTableHelper.GetStringRowValue(row, "Transaction Count"),
                ReqnrollTableHelper.GetStringRowValue(row, "Transaction Value"),
                ReqnrollTableHelper.GetStringRowValue(row, "Average Value"),
                ReqnrollTableHelper.GetStringRowValue(row, "Successful"),
                ReqnrollTableHelper.GetStringRowValue(row, "Failed"),
                ReqnrollTableHelper.GetStringRowValue(row, "Success Rate"));
        }
    }

    private async Task AssertProductPerformanceRowsAsync(DataTable table)
    {
        (await _page.Locator("table tbody tr").CountAsync()).ShouldBe(table.Rows.Count);

        foreach (var row in table.Rows)
        {
            var productName = ReqnrollTableHelper.GetStringRowValue(row, "Product");
            var transactionCount = ReqnrollTableHelper.GetStringRowValue(row, "Transaction Count");
            var transactionValue = ReqnrollTableHelper.GetStringRowValue(row, "Transaction Value");
            var percentage = ReqnrollTableHelper.GetStringRowValue(row, "Percentage");

            var matchedRow = _page.Locator("table tbody tr").Filter(new() { HasText = productName });
            (await matchedRow.IsVisibleAsync()).ShouldBeTrue();
            var rowText = await matchedRow.InnerTextAsync();
            rowText.ShouldContain(productName);
            rowText.ShouldContain(transactionCount);
            rowText.ShouldContain($"{percentage}%");

            var cells = matchedRow.Locator("td");
            ParseMoney(await cells.Nth(2).InnerTextAsync()).ShouldBe(decimal.Parse(transactionValue, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
        }
    }

    private async Task AssertSettlementSummaryRowsAsync(DataTable table)
    {
        (await _page.Locator("table tbody tr").CountAsync()).ShouldBe(table.Rows.Count);

        foreach (var row in table.Rows)
        {
            var merchantName = ReqnrollTableHelper.GetStringRowValue(row, "Merchant");
            var settlementStatus = ReqnrollTableHelper.GetStringRowValue(row, "Settlement Status");
            var grossValue = ReqnrollTableHelper.GetStringRowValue(row, "Gross Value");
            var totalFees = ReqnrollTableHelper.GetStringRowValue(row, "Total Fees");
            var netSettlement = ReqnrollTableHelper.GetStringRowValue(row, "Net Settlement");

            var matchedRow = _page.Locator("table tbody tr").Filter(new() { HasText = merchantName });
            (await matchedRow.IsVisibleAsync()).ShouldBeTrue();
            var rowText = await matchedRow.InnerTextAsync();
            rowText.ShouldContain(merchantName);
            rowText.ShouldContain(settlementStatus);
            rowText.ShouldContain(FormatCurrency(grossValue));
            rowText.ShouldContain($"-{FormatCurrency(totalFees)}");
            rowText.ShouldContain(FormatCurrency(netSettlement));
        }
    }

    private async Task AssertSummaryRowAsync(string name, string transactionCount, string totalValue, string averageValue, string successful, string failed, string successRate)
    {
        var row = _page.Locator("table tbody tr").Filter(new() { HasText = name });
        (await row.IsVisibleAsync()).ShouldBeTrue();

        var cells = row.Locator("td");
        (await cells.CountAsync()).ShouldBeGreaterThanOrEqualTo(7);

        (await cells.Nth(0).InnerTextAsync()).ShouldBe(name);
        (await cells.Nth(1).InnerTextAsync()).ShouldBe(transactionCount);
        ParseMoney(await cells.Nth(2).InnerTextAsync()).ShouldBe(decimal.Parse(totalValue, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
        ParseMoney(await cells.Nth(3).InnerTextAsync()).ShouldBe(decimal.Parse(averageValue, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture));
        (await cells.Nth(4).InnerTextAsync()).ShouldBe(successful);
        (await cells.Nth(5).InnerTextAsync()).ShouldBe(failed);
        (await cells.Nth(6).InnerTextAsync()).ShouldContain(successRate);
    }

    private static bool IsMoneyLabel(string label) =>
        label.Contains("Value", StringComparison.OrdinalIgnoreCase) ||
        label.Contains("Amount", StringComparison.OrdinalIgnoreCase) ||
        label.Contains("Fee", StringComparison.OrdinalIgnoreCase) ||
        label.Contains("Settlement", StringComparison.OrdinalIgnoreCase) ||
        label.Contains("Average", StringComparison.OrdinalIgnoreCase);

    private static string FormatCurrency(string value)
    {
        if (decimal.TryParse(value, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var parsed))
        {
            return parsed.ToString("C", CultureInfo.CurrentCulture);
        }

        return value;
    }

    private static decimal ParseMoney(string value)
    {
        if (decimal.TryParse(value, NumberStyles.Currency | NumberStyles.Number, CultureInfo.CurrentCulture, out var parsed))
        {
            return parsed;
        }

        if (decimal.TryParse(value, NumberStyles.Currency | NumberStyles.Number, CultureInfo.InvariantCulture, out parsed))
        {
            return parsed;
        }

        var normalized = new string(value.Where(character =>
            char.IsDigit(character) ||
            character == '.' ||
            character == ',' ||
            character == '-').ToArray());

        if (decimal.TryParse(normalized, NumberStyles.Number | NumberStyles.AllowThousands | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out parsed))
        {
            return parsed;
        }

        throw new FormatException($"Unable to parse money value '{value}'.");
    }

    private async Task AssertAnalyticalChartsHourlyPointsAsync(DataTable table)
    {
        await ExecuteWithFailureScreenshotAsync("analytical-charts-hourly-points", async () =>
        {
            var volumeChart = await ReadChartSeriesAsync("volumeChart");
            var valueChart = await ReadChartSeriesAsync("valueChart");

            foreach (var row in table.Rows)
            {
                var chart = ReqnrollTableHelper.GetStringRowValue(row, "Chart");
                var hour = ReqnrollTableHelper.GetStringRowValue(row, "Hour");
                var today = ReqnrollTableHelper.GetStringRowValue(row, "Today");
                var comparison = ReqnrollTableHelper.GetStringRowValue(row, "Comparison");

                if (chart.Equals("Volume", StringComparison.OrdinalIgnoreCase))
                {
                    AssertChartSeriesPoint(volumeChart, hour, decimal.Parse(today, CultureInfo.InvariantCulture), decimal.Parse(comparison, CultureInfo.InvariantCulture));
                }
                else if (chart.Equals("Value", StringComparison.OrdinalIgnoreCase))
                {
                    AssertChartSeriesPoint(valueChart, hour, decimal.Parse(today, CultureInfo.InvariantCulture), decimal.Parse(comparison, CultureInfo.InvariantCulture));
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(table), $"Unsupported chart '{chart}'.");
                }
            }
        });
    }

    private static void AssertChartSeriesPoint(ChartSeriesSnapshot chart, string hour, decimal expectedToday, decimal expectedComparison)
    {
        var index = Array.FindIndex(chart.Labels, label => string.Equals(label, hour, StringComparison.OrdinalIgnoreCase));
        index.ShouldBeGreaterThanOrEqualTo(0, $"Could not find hourly bucket '{hour}'.");
        chart.TodaySeries[index].ShouldBe(expectedToday);
        chart.ComparisonSeries[index].ShouldBe(expectedComparison);
    }

    private async Task<(int totalTodayCount, int totalComparisonCount, decimal totalTodayValue, decimal totalComparisonValue)> ReadChartAsync(string chartId)
    {
        var chart = await ReadChartSeriesAsync(chartId);

        if (chartId == "volumeChart")
        {
            var todayCount = chart.TodaySeries.Select(Convert.ToInt32).Sum();
            var comparisonCount = chart.ComparisonSeries.Select(Convert.ToInt32).Sum();
            return (todayCount, comparisonCount, 0m, 0m);
        }

        var todayValue = chart.TodaySeries.Sum();
        var comparisonValue = chart.ComparisonSeries.Sum();
        return (0, 0, todayValue, comparisonValue);
    }

    private async Task<ChartSeriesSnapshot> ReadChartSeriesAsync(string chartId)
    {
        var json = await _page.EvaluateAsync<string>($@"() => {{
            const chart = Chart.getChart('{chartId}');
            if (!chart) return '';
            return JSON.stringify({{
                labels: Array.from(chart.data.labels ?? []),
                datasets: chart.data.datasets.map(d => Array.from(d.data))
            }});
        }}");

        json.ShouldNotBeNullOrWhiteSpace();

        using var document = JsonDocument.Parse(json);
        var labels = document.RootElement.GetProperty("labels").EnumerateArray().Select(x => x.GetString() ?? string.Empty).ToArray();
        var datasets = document.RootElement.GetProperty("datasets");
        var todaySeries = datasets[0].EnumerateArray().Select(x => x.GetDecimal()).ToArray();
        var comparisonSeries = datasets[1].EnumerateArray().Select(x => x.GetDecimal()).ToArray();

        return new ChartSeriesSnapshot(labels, todaySeries, comparisonSeries);
    }

    private sealed record ChartSeriesSnapshot(string[] Labels, decimal[] TodaySeries, decimal[] ComparisonSeries);
}
