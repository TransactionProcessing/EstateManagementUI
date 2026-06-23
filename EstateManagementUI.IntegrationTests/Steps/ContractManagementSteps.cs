using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class ContractManagementSteps
{
    private const string ContractDescription = "Integration Contract";
    private const string ProductName = "Integration Product";
    private const string ProductDisplayText = "Integration Product Display";
    private const string FeeDescription = "Integration Fee";

    private readonly IPage _page;
    private readonly TestingContext _testingContext;

    public ContractManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [When("I open the contract management screen")]
    public Task WhenIOpenTheContractManagementScreen() => GetHelper().OpenContractManagementScreenAsync();

    [Then("I should see the contract management heading")]
    public Task ThenIShouldSeeTheContractManagementHeading() => GetHelper().AssertContractManagementHeadingVisibleAsync();

    [When("I create a contract")]
    public Task WhenICreateAContract() => GetHelper().CreateContractAsync(ContractDescription, "Test Operator");

    [Then("I should see the contract in the list")]
    public Task ThenIShouldSeeTheContractInTheList() => GetHelper().AssertContractListContainsAsync(ContractDescription);

    [When("I view the contract")]
    public Task WhenIViewTheContract() => GetHelper().OpenContractViewAsync(ContractDescription);

    [Then("I should see the contract details page")]
    public Task ThenIShouldSeeTheContractDetailsPage() => GetHelper().AssertContractViewVisibleAsync(ContractDescription);

    [When("I edit the contract")]
    public Task WhenIEditTheContract() => GetHelper().OpenContractEditAsync(ContractDescription);

    [Then("I should see the contract edit page")]
    public Task ThenIShouldSeeTheContractEditPage() => GetHelper().AssertContractEditVisibleAsync(ContractDescription);

    [When("I add a product to the contract")]
    public Task WhenIAddAProductToTheContract() => GetHelper().AddProductToContractAsync(ProductName, ProductDisplayText, 10m);

    [Then("I should see the product in the contract edit view")]
    public Task ThenIShouldSeeTheProductInTheContractEditView() => GetHelper().AssertContractProductVisibleAsync(ProductName);

    [When("I add a fee to the contract")]
    public Task WhenIAddAFeeToTheContract() => GetHelper().AddFeeToContractAsync(FeeDescription, 1.50m);

    [Then("I should see the fee in the contract edit view")]
    public Task ThenIShouldSeeTheFeeInTheContractEditView() => GetHelper().AssertContractFeeVisibleAsync(FeeDescription);

    [When("I return to the contract list")]
    public Task WhenIReturnToTheContractList() => GetHelper().BackToContractListAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
