using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;
using Shared.IntegrationTesting;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class OperatorManagementSteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;
    private string _operatorName = string.Empty;

    public OperatorManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [When("I open the operator management screen")]
    public Task WhenIOpenTheOperatorManagementScreen() => GetHelper().OpenOperatorManagementScreenAsync();

    [Then("I should see the operator management heading")]
    public Task ThenIShouldSeeTheOperatorManagementHeading() => GetHelper().AssertOperatorManagementHeadingVisibleAsync();

    [Then("I should see the operator {string} in the operator list")]
    public Task ThenIShouldSeeTheOperatorInTheOperatorList(string operatorName) => GetHelper().AssertOperatorListContainsAsync(operatorName);

    [When("I open the operator view for {string}")]
    public Task WhenIOpenTheOperatorViewFor(string operatorName) => GetHelper().OpenOperatorViewAsync(operatorName);

    [Then("I should see the operator view page for {string}")]
    public Task ThenIShouldSeeTheOperatorViewPageFor(string operatorName) => GetHelper().AssertOperatorViewVisibleAsync(operatorName);

    [When("I go back to the operator list from the view page")]
    public Task WhenIGoBackToTheOperatorListFromTheViewPage() => GetHelper().BackToOperatorListFromViewAsync();

    [When("I open the operator edit page for {string}")]
    public Task WhenIOpenTheOperatorEditPageFor(string operatorName) => GetHelper().OpenOperatorEditAsync(operatorName);

    [Then("I should see the operator edit page for {string}")]
    public Task ThenIShouldSeeTheOperatorEditPageFor(string operatorName) => GetHelper().AssertOperatorEditVisibleAsync(operatorName);

    [When("I cancel operator editing")]
    public Task WhenICancelOperatorEditing() => GetHelper().CancelOperatorEditAsync();

    [When("I open the new operator screen")]
    public Task WhenIOpenTheNewOperatorScreen() => GetHelper().OpenNewOperatorScreenAsync();

    [Then("I should see the new operator screen")]
    public Task ThenIShouldSeeTheNewOperatorScreen() => GetHelper().AssertNewOperatorScreenVisibleAsync();

    [When("I create the following operators")]
    public async Task WhenICreateTheFollowingOperators(DataTable table)
    {
        DataTableRow row = table.Rows.First();
        _operatorName = ReqnrollTableHelper.GetStringRowValue(row, "OperatorName");

        await GetHelper().CreateOperatorAsync(
            _operatorName,
            bool.Parse(ReqnrollTableHelper.GetStringRowValue(row, "RequireCustomMerchantNumber")),
            bool.Parse(ReqnrollTableHelper.GetStringRowValue(row, "RequireCustomTerminalNumber")));
    }

    [Then("I should see the created operator in the operator list")]
    public Task ThenIShouldSeeTheCreatedOperatorInTheOperatorList() => GetHelper().AssertOperatorListContainsAsync(_operatorName);

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
