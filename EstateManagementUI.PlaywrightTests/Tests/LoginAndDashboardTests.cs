using EstateManagementUI.PlaywrightTests.Infrastructure;
using Microsoft.Playwright;
using NUnit.Framework;
using Shouldly;

namespace EstateManagementUI.PlaywrightTests.Tests;

[TestFixture]
public class LoginAndDashboardTests : PlaywrightTestBase
{
    private const string TestUsername = "estateuser@testestate1.co.uk";
    private const string TestPassword = "123456";

    [Test]
    [Order(1)]
    public async Task Test_01_HomePage_Should_Display_SignIn_Button()
    {
        // Arrange
        var appUrl = $"https://localhost:{DockerHelper!.EstateManagementUiPort}";

        // Act
        await Page!.GotoAsync(appUrl);

        // Assert
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        var pageTitle = await Page.TitleAsync();
        pageTitle.ShouldContain("Estate Management");

        // Check for Sign In button
        var signInButton = Page.Locator("#loginButton");
        await signInButton.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        var isVisible = await signInButton.IsVisibleAsync();
        isVisible.ShouldBeTrue();
    }

    [Test]
    [Order(2)]
    public async Task Test_02_Login_Flow_Should_Redirect_To_Identity_Server()
    {
        // Arrange
        var appUrl = $"https://localhost:{DockerHelper!.EstateManagementUiPort}";
        await Page!.GotoAsync(appUrl);

        // Act
        var signInButton = Page.Locator("#loginButton");
        await signInButton.ClickAsync();

        // Wait for redirect to identity server
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000); // Give time for navigation

        // Assert
        var currentUrl = Page.Url;
        currentUrl.ShouldContain("identity-server", Case.Insensitive);
        
        // Check for login form elements
        var usernameField = Page.Locator("input[name='Input.Username'], input[type='text']").First;
        var passwordField = Page.Locator("input[name='Input.Password'], input[type='password']").First;
        var loginButton = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Login" });

        await usernameField.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });
        (await usernameField.IsVisibleAsync()).ShouldBeTrue();
        (await passwordField.IsVisibleAsync()).ShouldBeTrue();
        (await loginButton.IsVisibleAsync()).ShouldBeTrue();
    }

    [Test]
    [Order(3)]
    public async Task Test_03_Successful_Login_Should_Navigate_To_Dashboard()
    {
        // Arrange
        var appUrl = $"https://localhost:{DockerHelper!.EstateManagementUiPort}";
        await Page!.GotoAsync(appUrl);

        // Navigate to login
        var signInButton = Page.Locator("#loginButton");
        await signInButton.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000);

        // Act - Perform login
        var usernameField = Page.Locator("input[name='Input.Username'], input[type='text']").First;
        var passwordField = Page.Locator("input[name='Input.Password'], input[type='password']").First;
        var loginButton = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Login" });

        await usernameField.FillAsync(TestUsername);
        await passwordField.FillAsync(TestPassword);
        await loginButton.ClickAsync();

        // Wait for redirect back to app
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(5000); // Give time for authentication process

        // Assert - Should be on dashboard
        var pageTitle = await Page.TitleAsync();
        pageTitle.ShouldContain("Dashboard");

        var currentUrl = Page.Url;
        currentUrl.ShouldContain("localhost");
        currentUrl.ShouldContain(DockerHelper.EstateManagementUiPort.ToString());
    }

    [Test]
    [Order(4)]
    public async Task Test_04_Dashboard_Should_Display_Navigation_Menu()
    {
        // Arrange - Login first
        await PerformLogin().ConfigureAwait(false);

        // Assert - Check for navigation elements
        var merchantsLink = Page.Locator("#merchantsLink");
        var operatorsLink = Page.Locator("#operatorsLink");
        var contractsLink = Page.Locator("#contractsLink");
        var estateDetailsLink = Page.Locator("#estateDetailsLink");

        // Wait for one of them to ensure page is loaded
        await merchantsLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 10000 });

        (await merchantsLink.IsVisibleAsync()).ShouldBeTrue();
        (await operatorsLink.IsVisibleAsync()).ShouldBeTrue();
        (await contractsLink.IsVisibleAsync()).ShouldBeTrue();
        (await estateDetailsLink.IsVisibleAsync()).ShouldBeTrue();
    }

    [Test]
    [Order(5)]
    public async Task Test_05_Dashboard_Should_Allow_Navigation_To_Merchants()
    {
        // Arrange - Login first
        await PerformLogin().ConfigureAwait(false);

        // Act
        var merchantsLink = Page!.Locator("#merchantsLink");
        await merchantsLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await merchantsLink.ClickAsync();
        
        // Wait for navigation
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000);

        // Assert
        var pageTitle = await Page.TitleAsync();
        pageTitle.ShouldContain("Merchants", Case.Insensitive);
    }

    [Test]
    [Order(6)]
    public async Task Test_06_Dashboard_Should_Allow_Navigation_To_Operators()
    {
        // Arrange - Login first
        await PerformLogin().ConfigureAwait(false);

        // Act
        var operatorsLink = Page!.Locator("#operatorsLink");
        await operatorsLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await operatorsLink.ClickAsync();
        
        // Wait for navigation
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000);

        // Assert
        var pageTitle = await Page.TitleAsync();
        pageTitle.ShouldContain("Operators", Case.Insensitive);
    }

    [Test]
    [Order(7)]
    public async Task Test_07_Dashboard_Should_Allow_Navigation_To_Contracts()
    {
        // Arrange - Login first
        await PerformLogin().ConfigureAwait(false);

        // Act
        var contractsLink = Page!.Locator("#contractsLink");
        await contractsLink.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        await contractsLink.ClickAsync();
        
        // Wait for navigation
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000);

        // Assert
        var pageTitle = await Page.TitleAsync();
        pageTitle.ShouldContain("Contracts", Case.Insensitive);
    }

    private async Task PerformLogin()
    {
        var appUrl = $"https://localhost:{DockerHelper!.EstateManagementUiPort}";
        await Page!.GotoAsync(appUrl);

        // Click Sign In
        var signInButton = Page.Locator("#loginButton");
        await signInButton.ClickAsync();
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(2000);

        // Fill in credentials
        var usernameField = Page.Locator("input[name='Input.Username'], input[type='text']").First;
        var passwordField = Page.Locator("input[name='Input.Password'], input[type='password']").First;
        var loginButton = Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Login" });

        await usernameField.FillAsync(TestUsername);
        await passwordField.FillAsync(TestPassword);
        await loginButton.ClickAsync();

        // Wait for redirect
        await Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Page.WaitForTimeoutAsync(5000);
    }
}
