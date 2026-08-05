using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class EstateManagementSteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;

    public EstateManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [When("I open the estate management screen")]
    public Task WhenIOpenTheEstateManagementScreen() => GetHelper().OpenEstateManagementScreenAsync();

    [Then("I should see the estate management heading")]
    public Task ThenIShouldSeeTheEstateManagementHeading() => GetHelper().AssertEstateManagementHeadingVisibleAsync();

    [Then("I should see the estate overview summary")]
    public Task ThenIShouldSeeTheEstateOverviewSummary() => GetHelper().AssertEstateOverviewVisibleAsync();

    [When("I switch to the operators tab")]
    public Task WhenISwitchToTheOperatorsTab() => GetHelper().SwitchToOperatorsTabAsync();

    [Then("I should see the assigned operators section")]
    public Task ThenIShouldSeeTheAssignedOperatorsSection() => GetHelper().AssertAssignedOperatorsSectionVisibleAsync();

    [Then("I should see the assigned operator {string}")]
    public Task ThenIShouldSeeTheAssignedOperator(string operatorName) => GetHelper().AssertAssignedOperatorVisibleAsync(operatorName);

    [Then("I should not see the assigned operator {string}")]
    public Task ThenIShouldNotSeeTheAssignedOperator(string operatorName) => GetHelper().AssertAssignedOperatorNotVisibleAsync(operatorName);

    [When("I add the operator {string} to the estate")]
    public Task WhenIAddTheOperatorToTheEstate(string operatorName) => GetHelper().AddOperatorToEstateAsync(operatorName);

    [When("I remove the operator {string} from the estate")]
    public Task WhenIRemoveTheOperatorFromTheEstate(string operatorName) => GetHelper().RemoveOperatorFromEstateAsync(operatorName);

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
