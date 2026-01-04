using Microsoft.Playwright;
using Shared.IntegrationTesting;
using Shouldly;

namespace EstateManagementUI.IntegrationTests.Common;

public class PlaywrightEstateManagementUiHelpers
{
    private readonly IPage Page;
    private readonly Int32 EstateManagementUiPort;

    public PlaywrightEstateManagementUiHelpers(IPage page, Int32 estateManagementUiPort)
    {
        this.Page = page;
        this.EstateManagementUiPort = estateManagementUiPort;
    }

    private async Task VerifyPageTitle(String expectedTitle)
    {
        var title = await this.Page.TitleAsync();
        title.ShouldBe($"{expectedTitle} - Estate Management");
    }

    public async Task NavigateToHomePage()
    {
        await this.Page.GotoAsync($"https://localhost:{this.EstateManagementUiPort}");
        await this.Page.SetViewportSizeAsync(1920, 1080);
        await this.VerifyPageTitle("Welcome");
    }

    public async Task ClickContractsSidebarOption()
    {
        await this.Page.ClickAsync("#contractsLink");
    }

    public async Task ClickMyEstateSidebarOption()
    {
        await this.Page.ClickAsync("#estateDetailsLink");
    }

    public async Task ClickMyMerchantsSidebarOption()
    {
        await this.Page.ClickAsync("#merchantsLink");
    }

    public async Task ClickMyOperatorsSidebarOption()
    {
        await this.Page.ClickAsync("#operatorsLink");
    }

    public async Task ClickOnTheSignInButton()
    {
        await this.Page.ClickAsync("#loginButton");
    }

    public async Task VerifyOnTheLoginScreen()
    {
        await Retry.For(async () =>
        {
            var loginButton = this.Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Login" });
            await loginButton.WaitForAsync(new LocatorWaitForOptions { Timeout = 120000 });
            loginButton.ShouldNotBeNull();
        });
    }

    public async Task LoginWithUsernameAndPassword(String username, String password)
    {
        await Retry.For(async () =>
        {
            var usernameField = this.Page.Locator("input[name='Input.Username'], input[type='text']").First;
            var passwordField = this.Page.Locator("input[name='Input.Password'], input[type='password']").First;
            var loginButton = this.Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Login" });

            await usernameField.FillAsync(username);
            await passwordField.FillAsync(password);
            await loginButton.ClickAsync();
        }, TimeSpan.FromMinutes(2));
    }

    public async Task VerifyOnTheDashboard()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("Dashboard");
        }, TimeSpan.FromMinutes(2));
    }

    public async Task VerifyOnTheMerchantsListScreen()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("Merchants List");
        });
    }

    public async Task VerifyOnTheOperatorsListScreen()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("Operators");
        });
    }

    public async Task VerifyOnTheContractsListScreen()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("Contracts");
        });
    }

    public async Task VerifyOnTheViewEstatePage()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("View Estate");
        });
    }

    public async Task VerifyDashboardNavigationMenuIsDisplayed()
    {
        await Retry.For(async () =>
        {
            var merchantsLink = this.Page.Locator("#merchantsLink");
            var operatorsLink = this.Page.Locator("#operatorsLink");
            var contractsLink = this.Page.Locator("#contractsLink");
            var estateDetailsLink = this.Page.Locator("#estateDetailsLink");

            merchantsLink.ShouldNotBeNull();
            operatorsLink.ShouldNotBeNull();
            contractsLink.ShouldNotBeNull();
            estateDetailsLink.ShouldNotBeNull();

            (await merchantsLink.IsVisibleAsync()).ShouldBeTrue();
            (await operatorsLink.IsVisibleAsync()).ShouldBeTrue();
            (await contractsLink.IsVisibleAsync()).ShouldBeTrue();
            (await estateDetailsLink.IsVisibleAsync()).ShouldBeTrue();
        }, TimeSpan.FromSeconds(30));
    }

    public async Task VerifyDashboardEstateInformationIsDisplayed()
    {
        await Retry.For(async () =>
        {
            // Verify the dashboard page is displayed
            await this.VerifyPageTitle("Dashboard");
            
            // Check that we're on the dashboard page
            String currentUrl = this.Page.Url;
            currentUrl.ShouldContain("/Dashboard");
        }, TimeSpan.FromSeconds(30));
    }

    public async Task NavigateToDashboard()
    {
        // Navigate to dashboard by URL
        await this.Page.GotoAsync($"https://localhost:{this.EstateManagementUiPort}/Dashboard");
        
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("Dashboard");
        }, TimeSpan.FromSeconds(30));
    }
}
