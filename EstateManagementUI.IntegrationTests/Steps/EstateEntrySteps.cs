using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class EstateEntrySteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;

    public EstateEntrySteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [Given("the user navigates to the entry screen")]
    [Given("I navigate to the entry screen")]
    [When("I navigate to the entry screen")]
    public Task WhenINavigateToTheEntryScreen() => GetHelper().NavigateToEntryScreenAsync();

    [Then("I should see the entry screen")]
    public Task ThenIShouldSeeTheEntryScreen() => GetHelper().AssertEntryScreenVisibleAsync();

    [When("I open the estate information page")]
    public Task WhenIOpenTheEstateInformationPage() => GetHelper().OpenEstateInfoFromEntryAsync();

    [Then("I should see the estate info page")]
    public Task ThenIShouldSeeTheEstateInfoPage() => GetHelper().AssertEstateInfoPageVisibleAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
