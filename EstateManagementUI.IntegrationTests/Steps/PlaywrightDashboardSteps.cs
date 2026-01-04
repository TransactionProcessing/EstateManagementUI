using Reqnroll;
using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll.BoDi;

namespace EstateManagementUI.IntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "playwright")]
    public class PlaywrightDashboardSteps
    {
        private readonly TestingContext TestingContext;
        private readonly PlaywrightEstateManagementUiHelpers UiHelpers;

        public PlaywrightDashboardSteps(ScenarioContext scenarioContext, TestingContext testingContext, IObjectContainer container)
        {
            var page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));

            this.TestingContext = testingContext;
            this.UiHelpers = new PlaywrightEstateManagementUiHelpers(page, this.TestingContext.DockerHelper.EstateManagementUiPort);
        }

        [Given(@"I am on the application home page")]
        public async Task GivenIAmOnTheApplicationHomePage()
        {
            await this.UiHelpers.NavigateToHomePage();
        }

        [Given(@"I click on the My Contracts sidebar option")]
        [When(@"I click on the My Contracts sidebar option")]
        public async Task WhenIClickOnTheMyContractsSidebarOption()
        {
            await this.UiHelpers.ClickContractsSidebarOption();
        }

        [Given(@"I click on the My Estate sidebar option")]
        [When(@"I click on the My Estate sidebar option")]
        public async Task WhenIClickOnTheMyEstateSidebarOption()
        {
            await this.UiHelpers.ClickMyEstateSidebarOption();
        }

        [Given(@"I click on the My Merchants sidebar option")]
        [When(@"I click on the My Merchants sidebar option")]
        public async Task WhenIClickOnTheMyMerchantsSidebarOption()
        {
            await this.UiHelpers.ClickMyMerchantsSidebarOption();
        }

        [Given(@"I click on the My Operators sidebar option")]
        [When(@"I click on the My Operators sidebar option")]
        public async Task WhenIClickOnTheMyOperatorsSidebarOption()
        {
            await this.UiHelpers.ClickMyOperatorsSidebarOption();
        }

        [Given(@"I click on the Sign In Button")]
        [When(@"I click on the Sign In Button")]
        public async Task WhenIClickOnTheSignInButton()
        {
            await this.UiHelpers.ClickOnTheSignInButton();
        }

        [Then(@"I am presented with a login screen")]
        public async Task ThenIAmPresentedWithALoginScreen()
        {
            await this.UiHelpers.VerifyOnTheLoginScreen();
        }

        [When(@"I login with the username '(.*)' and password '(.*)'")]
        public async Task WhenILoginWithTheUsernameAndPassword(String username, String password)
        {
            await this.UiHelpers.LoginWithUsernameAndPassword(username, password);
        }

        [Then(@"I am presented with the Estate Administrator Dashboard")]
        public async Task ThenIAmPresentedWithTheEstateAdministratorDashboard()
        {
            await this.UiHelpers.VerifyOnTheDashboard();
        }

        [Then(@"I am presented with the Merchants List Screen")]
        public async Task ThenIAmPresentedWithTheMerchantsListScreen()
        {
            await this.UiHelpers.VerifyOnTheMerchantsListScreen();
        }

        [Then(@"I am presented with the Operators List Screen")]
        public async Task ThenIAmPresentedWithTheOperatorsListScreen()
        {
            await this.UiHelpers.VerifyOnTheOperatorsListScreen();
        }

        [Then(@"I am presented with the Contracts List Screen")]
        public async Task ThenIAmPresentedWithTheContractsListScreen()
        {
            await this.UiHelpers.VerifyOnTheContractsListScreen();
        }

        [Then(@"I am presented with the View Estate Page")]
        public async Task ThenIAmPresentedWithTheViewEstatePage()
        {
            await this.UiHelpers.VerifyOnTheViewEstatePage();
        }

        [Then(@"the dashboard displays the navigation menu")]
        public async Task ThenTheDashboardDisplaysTheNavigationMenu()
        {
            await this.UiHelpers.VerifyDashboardNavigationMenuIsDisplayed();
        }

        [Then(@"the dashboard shows estate information")]
        public async Task ThenTheDashboardShowsEstateInformation()
        {
            await this.UiHelpers.VerifyDashboardEstateInformationIsDisplayed();
        }

        [When(@"I navigate back to the dashboard")]
        public async Task WhenINavigateBackToTheDashboard()
        {
            await this.UiHelpers.NavigateToDashboard();
        }
    }
}
