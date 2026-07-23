using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;
using Shared.IntegrationTesting;
using System.Globalization;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class ContractManagementSteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;
    private string _contractDescription = string.Empty;
    private string _productName = string.Empty;
    private string _feeDescription = string.Empty;

    public ContractManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [When("I open the contract management screen")]
    public Task WhenIOpenTheContractManagementScreen() => GetHelper().OpenContractManagementScreenAsync();

    [Then("I should see the contract management heading")]
    public Task ThenIShouldSeeTheContractManagementHeading() => GetHelper().AssertContractManagementHeadingVisibleAsync();

    [When("I create the following contracts")]
    public async Task WhenICreateTheFollowingContracts(DataTable table)
    {
        DataTableRow row = table.Rows.First();
        _contractDescription = ReqnrollTableHelper.GetStringRowValue(row, "ContractDescription");

        await GetHelper().CreateContractAsync(_contractDescription, ReqnrollTableHelper.GetStringRowValue(row, "OperatorName"));
    }

    [Then("I should see the created contract in the list")]
    public Task ThenIShouldSeeTheCreatedContractInTheList() => GetHelper().AssertContractListContainsAsync(_contractDescription);

    [Then("I should see the contract in the list")]
    public Task ThenIShouldSeeTheContractInTheList() => ThenIShouldSeeTheCreatedContractInTheList();

    [When("I view the contract")]
    public Task WhenIViewTheContract() => GetHelper().OpenContractViewAsync(_contractDescription);

    [Then("I should see the contract details page")]
    public Task ThenIShouldSeeTheContractDetailsPage() => GetHelper().AssertContractViewVisibleAsync(_contractDescription);

    [When("I edit the contract")]
    public Task WhenIEditTheContract() => GetHelper().OpenContractEditAsync(_contractDescription);

    [Then("I should see the contract edit page")]
    public Task ThenIShouldSeeTheContractEditPage() => GetHelper().AssertContractEditVisibleAsync(_contractDescription);

    [When("I add the following products to the contract")]
    public async Task WhenIAddTheFollowingProductsToTheContract(DataTable table)
    {
        DataTableRow row = table.Rows.First();
        _productName = ReqnrollTableHelper.GetStringRowValue(row, "ProductName");

        await GetHelper().AddProductToContractAsync(
            _productName,
            ReqnrollTableHelper.GetStringRowValue(row, "DisplayText"),
            bool.Parse(ReqnrollTableHelper.GetStringRowValue(row, "IsVariableValue")),
            string.IsNullOrWhiteSpace(ReqnrollTableHelper.GetStringRowValue(row, "Value"))
                ? null
                : decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "Value"), CultureInfo.InvariantCulture));
    }

    [Then("I should see the created product in the contract edit view")]
    public Task ThenIShouldSeeTheCreatedProductInTheContractEditView() => GetHelper().AssertContractProductVisibleAsync(_productName);

    [Then("I should see the product in the contract edit view")]
    public Task ThenIShouldSeeTheProductInTheContractEditView() => ThenIShouldSeeTheCreatedProductInTheContractEditView();

    [When("I add the following fees to the contract")]
    public async Task WhenIAddTheFollowingFeesToTheContract(DataTable table)
    {
        DataTableRow row = table.Rows.First();
        _feeDescription = ReqnrollTableHelper.GetStringRowValue(row, "FeeDescription");

        await GetHelper().AddFeeToContractAsync(
            _feeDescription,
            ReqnrollTableHelper.GetStringRowValue(row, "CalculationType"),
            ReqnrollTableHelper.GetStringRowValue(row, "FeeType"),
            decimal.Parse(ReqnrollTableHelper.GetStringRowValue(row, "FeeValue"), CultureInfo.InvariantCulture));
    }

    [Then("I should see the created fee in the contract edit view")]
    public Task ThenIShouldSeeTheCreatedFeeInTheContractEditView() => GetHelper().AssertContractFeeVisibleAsync(_feeDescription);

    [Then("I should see the fee in the contract edit view")]
    public Task ThenIShouldSeeTheFeeInTheContractEditView() => ThenIShouldSeeTheCreatedFeeInTheContractEditView();

    [When("I return to the contract list")]
    public Task WhenIReturnToTheContractList() => GetHelper().BackToContractListAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
