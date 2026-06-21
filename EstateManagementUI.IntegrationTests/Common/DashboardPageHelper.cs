using System.Runtime.CompilerServices;
using Microsoft.Playwright;
using Shouldly;
using System.Text.Json;
using System.Text.RegularExpressions;
using Shared.IntegrationTesting;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class DashboardPageHelper
{
    private readonly IPage _page;
    private readonly TestingContext TestingContext;

    public DashboardPageHelper(IPage page, TestingContext testingContext) {
        _page = page;
        this.TestingContext = testingContext;
    }

    public async Task NavigateToAppAddressAsync()
    {
        var baseUrl = ResolveBaseUrl();
        await _page.GotoAsync(baseUrl);
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task ClickSignInButtonAsync()
    {
        var signInButton = _page.Locator("#loginButton");

        (await signInButton.IsVisibleAsync()).ShouldBeTrue();
        Console.WriteLine($"Sign in before click: {_page.Url}");

        await signInButton.ClickAsync();
        await _page.WaitForTimeoutAsync(2000);
        Console.WriteLine($"Sign in after click: {_page.Url}");
        Console.WriteLine($"Sign in title after click: {await _page.TitleAsync()}");
        Console.WriteLine($"Sign in body after click: {await _page.Locator("body").InnerTextAsync()}");
    }

    public async Task AssertLoginScreenVisibleAsync()
    {
        await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

        (await WaitForAnyVisibleAsync(
            "#Input_Username",
            "input[name='Input.Username']",
            "#Username",
            "input[name='Username']",
            "input[name='username']",
            "input[type='email']",
            "input[type='text']",
            "input[autocomplete='username']")).ShouldBeTrue();

        (await WaitForAnyVisibleAsync(
            "#Input_Password",
            "input[name='Input.Password']",
            "#Password",
            "input[name='Password']",
            "input[name='password']",
            "input[type='password']",
            "input[autocomplete='current-password']")).ShouldBeTrue();
    }

    public async Task LoginAsync(string username, string password)
    {
        await FillFirstVisibleAsync(
            username,
            "#Input_Username",
            "input[name='Input.Username']",
            "#Username",
            "input[name='Username']",
            "input[name='username']",
            "input[type='email']",
            "input[type='text']",
            "input[autocomplete='username']");

        await FillFirstVisibleAsync(
            password,
            "#Input_Password",
            "input[name='Input.Password']",
            "#Password",
            "input[name='Password']",
            "input[name='password']",
            "input[type='password']",
            "input[autocomplete='current-password']");

        await ClickFirstVisibleAsync(
            "button[type='submit']",
            "input[type='submit']",
            "button:has-text('Sign In')",
            "button:has-text('Login')");

        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    public async Task AssertDashboardShellVisibleAsync()
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" }).IsVisibleAsync()).ShouldBeTrue();
        (await _page.GetByText("Welcome to Estate Management System").IsVisibleAsync()).ShouldBeTrue();
        (await _page.Locator("#dashboardLink").IsVisibleAsync()).ShouldBeTrue();
    }

    public async Task AssertHomePageVisibleAsync()
    {
        (await _page.TitleAsync()).ShouldBe("Welcome - Estate Management");
        (await _page.Locator("#loginButton").IsVisibleAsync()).ShouldBeTrue();
    }

    public async Task AssertDashboardWelcomeMessageVisibleAsync()
    {
        (await _page.GetByText("Welcome to Estate Management System").IsVisibleAsync()).ShouldBeTrue();
    }

    public async Task AssertEstateDashboardVisibleAsync()
    {
        await AssertDashboardShellVisibleAsync();
        await AssertComparisonDateSelectorVisibleAsync();
        await AssertMerchantKpiSummaryCardsVisibleAsync();
        await AssertSalesComparisonCardsVisibleAsync();
        await AssertRecentMerchantsSectionVisibleAsync();
    }

    public async Task AssertAdministratorDashboardVisibleAsync()
    {
        await AssertDashboardShellVisibleAsync();
        (await _page.GetByRole(AriaRole.Heading, new() { Name = "Welcome, Administrator" }).IsVisibleAsync()).ShouldBeTrue();
    }

    public async Task AssertComparisonDateSelectorVisibleAsync()
    {
        var selector = _page.Locator("#comparisonDateSelector");
        var deadline = DateTime.UtcNow.AddSeconds(10);

        while (DateTime.UtcNow < deadline)
        {
            if (await selector.IsVisibleAsync())
            {
                return;
            }

            await _page.WaitForTimeoutAsync(250);
        }

        (await _page.Locator("#comparisonDateSelector").IsVisibleAsync()).ShouldBeTrue();
    }

    public async Task AssertMerchantKpiSummaryCardsVisibleAsync()
    {
        //await AssertInfoBoxVisibleAsync("Merchants with Sales (Last Hour)", "45");
        //await AssertInfoBoxVisibleAsync("Merchants with No Sales Today", "12");
        //await AssertInfoBoxVisibleAsync("Merchants with No Sales (7 Days)", "5");
        await AssertInfoBoxVisibleAsync("Merchants with Sales (Last Hour)", "0");
        await AssertInfoBoxVisibleAsync("Merchants with No Sales Today", "0");
        await AssertInfoBoxVisibleAsync("Merchants with No Sales (7 Days)", "0");
    }

    public async Task AssertSalesComparisonCardsVisibleAsync()
    {
        //await AssertCardVisibleAsync("Today's Sales", "523 transactions", new Regex(@"[£$]145,000\.00"));
        //await AssertCardVisibleAsync("Failed Sales (Low Credit)", "15 transactions", new Regex(@"[£$]850\.00"));
        await AssertCardVisibleAsync("Today's Sales", "0 transactions", new Regex(@"[£$¤]\s?0(?:,000)?\.00"));
        await AssertCardVisibleAsync("Failed Sales (Low Credit)", "0 transactions", new Regex(@"[£$¤]\s?0(?:,000)?\.00"));
    }

    public async Task AssertRecentMerchantsSectionVisibleAsync()
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = "Recently Created Merchants" }).IsVisibleAsync()).ShouldBeTrue();
    }

    public async Task AssertMerchantKpiSummaryCardsNotVisibleAsync()
    {
        await AssertInfoBoxAbsentAsync("Merchants with Sales (Last Hour)");
        await AssertInfoBoxAbsentAsync("Merchants with No Sales Today");
        await AssertInfoBoxAbsentAsync("Merchants with No Sales (7 Days)");
    }

    public async Task AssertSalesComparisonCardsNotVisibleAsync()
    {
        await AssertCardAbsentAsync("Today's Sales");
        await AssertCardAbsentAsync("Failed Sales (Low Credit)");
    }

    public async Task AssertRecentMerchantsSectionNotVisibleAsync()
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = "Recently Created Merchants" }).CountAsync()).ShouldBe(0);
    }

    public async Task AssertDashboardNavigationLinkVisibleAsync()
    {
        (await _page.Locator("#dashboardLink").IsVisibleAsync()).ShouldBeTrue();
    }

    private async Task AssertInfoBoxVisibleAsync(string label, string expectedValue)
    {
        var card = _page.Locator(".info-box").Filter(new() { HasText = label });

        (await card.IsVisibleAsync()).ShouldBeTrue();
        (await card.Locator(".info-box-number").InnerTextAsync()).ShouldBe(expectedValue);
    }

    private async Task AssertInfoBoxAbsentAsync(string label)
    {
        var card = _page.Locator(".info-box").Filter(new() { HasText = label });
        (await card.CountAsync()).ShouldBe(0);
    }

    private async Task AssertCardVisibleAsync(string heading, params object[] expectedTexts)
    {
        var card = _page.Locator("div.card").Filter(new()
        {
            Has = _page.GetByRole(AriaRole.Heading, new() { Name = heading })
        });

        (await card.IsVisibleAsync()).ShouldBeTrue();

        foreach (var expectedText in expectedTexts)
        {
            var locator = expectedText is Regex regex
                ? card.GetByText(regex).First
                : card.GetByText(expectedText.ToString()!).First;
            (await locator.IsVisibleAsync()).ShouldBeTrue();
        }
    }

    private async Task AssertCardAbsentAsync(string heading)
    {
        var card = _page.Locator("div.card").Filter(new()
        {
            Has = _page.GetByRole(AriaRole.Heading, new() { Name = heading })
        });
        (await card.CountAsync()).ShouldBe(0);
    }

    private async Task<bool> WaitForAnyVisibleAsync(params string[] selectors)
    {
        var deadline = DateTime.UtcNow.AddSeconds(30);

        while (DateTime.UtcNow < deadline)
        {
            if (await IsAnyVisibleAsync(selectors))
            {
                return true;
            }

            await _page.WaitForTimeoutAsync(250);
        }

        return false;
    }

    private async Task<bool> IsAnyVisibleAsync(params string[] selectors)
    {
        foreach (var selector in selectors)
        {
            var locator = _page.Locator(selector);
            if (await locator.CountAsync() > 0 && await locator.First.IsVisibleAsync())
            {
                return true;
            }
        }

        return false;
    }

    private async Task FillFirstVisibleAsync(string value, params string[] selectors)
    {
        foreach (var selector in selectors)
        {
            var locator = _page.Locator(selector);
            if (await locator.CountAsync() > 0)
            {
                var first = locator.First;
                if (await first.IsVisibleAsync())
                {
                    await first.FillAsync(value);
                    return;
                }
            }
        }

        throw new InvalidOperationException($"Could not find a visible input for selectors: {string.Join(", ", selectors)}");
    }

    private async Task ClickFirstVisibleAsync(params string[] selectors)
    {
        foreach (var selector in selectors)
        {
            var locator = _page.Locator(selector);
            if (await locator.CountAsync() > 0)
            {
                var first = locator.First;
                if (await first.IsVisibleAsync())
                {
                    await first.ClickAsync();
                    return;
                }
            }
        }

        throw new InvalidOperationException($"Could not find a visible clickable element for selectors: {string.Join(", ", selectors)}");
    }

    private string ResolveBaseUrl()
    {
        var hostPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
        return $"https://localhost:{hostPort}";
    }
}
