using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class ContractManagementSteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;
    private readonly string _contractDescription;

    public ContractManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
        _contractDescription = $"Integration Contract {Guid.NewGuid():N}";
    }

    [When("I open the contract management screen")]
    public Task WhenIOpenTheContractManagementScreen() => GetHelper().OpenContractManagementScreenAsync();

    [Then("I should see the contract management heading")]
    public Task ThenIShouldSeeTheContractManagementHeading() => GetHelper().AssertContractManagementHeadingVisibleAsync();

    [When("I create a contract")]
    public Task WhenICreateAContract() => GetHelper().CreateContractAsync(_contractDescription, "Test Operator");

    [Then("I should see the contract in the list")]
    public Task ThenIShouldSeeTheContractInTheList() => GetHelper().AssertContractListContainsAsync(_contractDescription);

    [When("I view the contract")]
    public Task WhenIViewTheContract() => GetHelper().OpenContractViewAsync(_contractDescription);

    [Then("I should see the contract details page")]
    public Task ThenIShouldSeeTheContractDetailsPage() => GetHelper().AssertContractViewVisibleAsync(_contractDescription);

    [When("I edit the contract")]
    public Task WhenIEditTheContract() => GetHelper().OpenContractEditAsync(_contractDescription);

    [Then("I should see the contract edit page")]
    public Task ThenIShouldSeeTheContractEditPage() => GetHelper().AssertContractEditVisibleAsync(_contractDescription);

    [When("I add a product to the contract")]
    public Task WhenIAddAProductToTheContract() => GetHelper().AddProductToContractAsync("Integration Product", "Integration Product Display", 10m);

    [Then("I should see the product in the contract edit view")]
    public Task ThenIShouldSeeTheProductInTheContractEditView() => GetHelper().AssertContractProductVisibleAsync("Integration Product");

    [When("I add a fee to the contract")]
    public Task WhenIAddAFeeToTheContract() => GetHelper().AddFeeToContractAsync("Integration Fee", 1.50m);

    [Then("I should see the fee in the contract edit view")]
    public Task ThenIShouldSeeTheFeeInTheContractEditView() => GetHelper().AssertContractFeeVisibleAsync("Integration Fee");

    [When("I return to the contract list")]
    public Task WhenIReturnToTheContractList() => GetHelper().BackToContractListAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
