using System.Runtime.CompilerServices;
using System.Globalization;
using Microsoft.Playwright;
using Shouldly;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
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
        await RunWithFailureArtifactsAsync(async () =>
        {
            var baseUrl = ResolveBaseUrl();
            await _page.GotoAsync(baseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(NavigateToAppAddressAsync));
    }

    public async Task NavigateToEntryScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GotoAsync(ResolveBaseUrl() + "/entry");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(NavigateToEntryScreenAsync));
    }

    public async Task AssertEntryScreenVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Estate Management" }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Manage estate details").IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Manage estate users").IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Operator Management").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertEntryScreenVisibleAsync));
    }

    public async Task OpenEstateInfoFromEntryAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("a[href='/estate-info']").ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            await _page.WaitForURLAsync(new Regex(@".*/estate-info.*", RegexOptions.IgnoreCase));
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(OpenEstateInfoFromEntryAsync));
    }

    public async Task AssertEstateInfoPageVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Estate Management" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.GetByText("Comprehensive estate management and configuration").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator("#loginButton").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Estate Management" }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Comprehensive estate management and configuration").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#loginButton").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertEstateInfoPageVisibleAsync));
    }

    public async Task OpenOperatorManagementScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var operatorLink = _page.Locator("#operatorsLink");
            if (await operatorLink.CountAsync() > 0 && await operatorLink.First.IsVisibleAsync())
            {
                await operatorLink.First.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            }
            else
            {
                await _page.GotoAsync(ResolveEstateManagementBaseUrl() + "/operators");
            }

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(OpenOperatorManagementScreenAsync));
    }

    public async Task AssertOperatorManagementHeadingVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Operator Management" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertOperatorManagementHeadingVisibleAsync));
    }

    public async Task AssertOperatorListContainsAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var operatorRow = GetOperatorRow(operatorName);
            await operatorRow.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await operatorRow.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertOperatorListContainsAsync));
    }

    public async Task OpenOperatorViewAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var operatorRow = GetOperatorRow(operatorName);
            await operatorRow.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await operatorRow.ClickAsync();
        }, nameof(OpenOperatorViewAsync));
    }

    public async Task AssertOperatorViewVisibleAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = $"View Operator: {operatorName}" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Operator Details" }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Back to List" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertOperatorViewVisibleAsync));
    }

    public async Task BackToOperatorListFromViewAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Back to List" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(BackToOperatorListFromViewAsync));
    }

    public async Task OpenOperatorEditAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var editButton = GetOperatorRow(operatorName).GetByRole(AriaRole.Button, new() { Name = "Edit" });
            await editButton.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await editButton.ClickAsync();
        }, nameof(OpenOperatorEditAsync));
    }

    public async Task AssertOperatorEditVisibleAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = $"Edit Operator: {operatorName}" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("input[placeholder='Enter operator name']").IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Update Operator" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertOperatorEditVisibleAsync));
    }

    public async Task CancelOperatorEditAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(CancelOperatorEditAsync));
    }

    public async Task OpenNewOperatorScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#newOperatorButton").ClickAsync();
            await _page.WaitForURLAsync(new Regex(@".*/operators/new.*", RegexOptions.IgnoreCase));
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(OpenNewOperatorScreenAsync));
    }

    public async Task AssertNewOperatorScreenVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await WaitForAnyVisibleAsync(
                "h1:has-text('Create New Operator')",
                "input[placeholder='Enter operator name']",
                "#createOperatorButton")).ShouldBeTrue();

            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Create New Operator" });
            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("input[placeholder='Enter operator name']").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#createOperatorButton").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertNewOperatorScreenVisibleAsync));
    }

    public async Task CreateOperatorAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("input[placeholder='Enter operator name']").FillAsync(operatorName);
            await _page.Locator("#createOperatorButton").ClickAsync();
        }, nameof(CreateOperatorAsync));
    }

    public async Task ClickSignInButtonAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var signInButton = _page.Locator("#loginButton");

            (await signInButton.IsVisibleAsync()).ShouldBeTrue();
            Console.WriteLine($"Sign in before click: {_page.Url}");

            await signInButton.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            await WaitForAuthenticationNavigationAsync();
            Console.WriteLine($"Sign in after click: {_page.Url}");
            Console.WriteLine($"Sign in title after click: {await _page.TitleAsync()}");
            Console.WriteLine($"Sign in body after click: {await _page.Locator("body").InnerTextAsync()}");
        }, nameof(ClickSignInButtonAsync));
    }

    public async Task AssertLoginScreenVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForAuthenticationNavigationAsync();

            (await WaitForAnyVisibleAsync(
                "#Input_Username",
                "input[name='Input.Username']",
                "#Input_UserName",
                "input[name='Input.UserName']",
                "#Username",
                "input[name='Username']",
                "input[name='username']",
                "input[name='UserName']",
                "input[name='Email']",
                "input[type='email']",
                "input[type='text']",
                "input[autocomplete='username']")).ShouldBeTrue();

            (await WaitForAnyVisibleAsync(
                "#Input_Password",
                "input[name='Input.Password']",
                "#Input_PasswordInput",
                "#Password",
                "input[name='Password']",
                "input[name='password']",
                "input[name='current-password']",
                "input[type='password']",
                "input[autocomplete='current-password']")).ShouldBeTrue();
        }, nameof(AssertLoginScreenVisibleAsync));
    }

    public async Task LoginAsync(string username, string password)
    {
        await RunWithFailureArtifactsAsync(async () =>
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
        }, nameof(LoginAsync));
    }

    public async Task AssertDashboardShellVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Welcome to Estate Management System").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#dashboardLink").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertDashboardShellVisibleAsync));
    }

    public async Task OpenEstateManagementScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var estateLink = _page.Locator("#estateDetailsLink");
            if (await estateLink.CountAsync() > 0 && await estateLink.First.IsVisibleAsync())
            {
                await estateLink.First.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            }
            else
            {
                await _page.GotoAsync(ResolveEstateManagementBaseUrl() + "/estate");
            }

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(OpenEstateManagementScreenAsync));
    }

    public async Task AssertEstateManagementHeadingVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForEstateOverviewAsync();
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Estate Management" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertEstateManagementHeadingVisibleAsync));
    }

    public async Task AssertEstateOverviewVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForEstateOverviewAsync();

            (await _page.GetByText("Total Merchants").IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Total Operators").IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Total Contracts").IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Total Users").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertEstateOverviewVisibleAsync));
    }

    public async Task SwitchToOperatorsTabAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Operators" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(SwitchToOperatorsTabAsync));
    }

    public async Task AssertAssignedOperatorsSectionVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Assigned Operators" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertAssignedOperatorsSectionVisibleAsync));
    }

    public async Task AddOperatorToEstateAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#addOperatorButton").ClickAsync();

            var option = _page.Locator($"select option:has-text('{operatorName}')");
            await option.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 10000
            });

            (await option.CountAsync()).ShouldBeGreaterThan(0);

            var optionValue = await option.First.GetAttributeAsync("value");
            optionValue.ShouldNotBeNull();

            await _page.Locator("select").SelectOptionAsync(new[] { optionValue! });
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add" }).ClickAsync();
        }, nameof(AddOperatorToEstateAsync));
    }

    public async Task RemoveOperatorFromEstateAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var operatorRow = GetAssignedOperatorRow(operatorName);
            await operatorRow.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await operatorRow.GetByRole(AriaRole.Button, new() { Name = "Remove" }).ClickAsync();
        }, nameof(RemoveOperatorFromEstateAsync));
    }

    public async Task AssertAssignedOperatorVisibleAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var operatorRow = GetAssignedOperatorRow(operatorName);
            await operatorRow.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await operatorRow.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertAssignedOperatorVisibleAsync));
    }

    public async Task AssertAssignedOperatorNotVisibleAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var operatorRow = GetAssignedOperatorRow(operatorName);
            await operatorRow.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });

            (await operatorRow.CountAsync()).ShouldBe(0);
        }, nameof(AssertAssignedOperatorNotVisibleAsync));
    }

    public async Task OpenContractManagementScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var contractsLink = _page.Locator("#contractsLink");
            if (await contractsLink.CountAsync() > 0 && await contractsLink.First.IsVisibleAsync())
            {
                await contractsLink.First.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            }
            else
            {
                await _page.GotoAsync(ResolveBaseUrl() + "/contracts");
            }

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(OpenContractManagementScreenAsync));
    }

    public async Task AssertContractManagementHeadingVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Contract Management" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertContractManagementHeadingVisibleAsync));
    }

    public async Task CreateContractAsync(string contractDescription, string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#newContractButton").ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            await _page.WaitForURLAsync(new Regex(@".*/contracts/new.*", RegexOptions.IgnoreCase));
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.Locator("input[placeholder='Enter contract description']").FillAsync(contractDescription);

            var operatorOption = _page.Locator("select option").Filter(new() { HasText = operatorName });
            await operatorOption.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 10000
            });

            var operatorValue = await operatorOption.First.GetAttributeAsync("value");
            operatorValue.ShouldNotBeNull();

            await _page.Locator("select").SelectOptionAsync(new[] { operatorValue! });
            await _page.Locator("#createContractButton").ClickAsync();
            await _page.WaitForURLAsync(new Regex(@".*/contracts.*", RegexOptions.IgnoreCase));
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(CreateContractAsync));
    }

    public async Task AssertContractListContainsAsync(string contractDescription)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var contractCard = GetContractCard(contractDescription);
            await contractCard.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await contractCard.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertContractListContainsAsync));
    }

    public async Task OpenContractViewAsync(string contractDescription)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var contractCard = GetContractCard(contractDescription);
            await contractCard.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await contractCard.GetByRole(AriaRole.Button, new() { Name = "View" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(OpenContractViewAsync));
    }

    public async Task AssertContractViewVisibleAsync(string contractDescription)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = $"View Contract: {contractDescription}" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Contract Details" }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Back to List" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertContractViewVisibleAsync));
    }

    public async Task OpenContractEditAsync(string contractDescription)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var currentContractId = ExtractContractIdFromUrl(_page.Url);
            if (currentContractId == Guid.Empty)
            {
                await _page.GotoAsync(ResolveBaseUrl() + "/contracts");
                await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                var contractCard = GetContractCard(contractDescription);
                await contractCard.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 10000
                });

                await contractCard.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
            }
            else
            {
                await _page.GotoAsync($"{ResolveBaseUrl()}/contracts/{currentContractId}/edit");
            }

            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(OpenContractEditAsync));
    }

    public async Task AssertContractEditVisibleAsync(string contractDescription)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = $"Edit Contract: {contractDescription}" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).First.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertContractEditVisibleAsync));
    }

    public async Task AddProductToContractAsync(string productName, string displayText, decimal value)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).First.ClickAsync();

            var modal = _page.Locator("div.fixed.inset-0").Filter(new() { HasText = "Add New Product" });
            await modal.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await modal.Locator("input[placeholder='Enter product name']").FillAsync(productName);
            await modal.Locator("input[placeholder='Enter display text']").FillAsync(displayText);
            await modal.Locator("input[placeholder='Enter value']").FillAsync(value.ToString(CultureInfo.InvariantCulture));
            await modal.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var productCard = GetContractProductCard(productName);
            await productCard.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(AddProductToContractAsync));
    }

    public async Task AssertContractProductVisibleAsync(string productName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var productCard = GetContractProductCard(productName);
            await productCard.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await productCard.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertContractProductVisibleAsync));
    }

    public async Task AddFeeToContractAsync(string feeDescription, decimal feeValue)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add Fee" }).First.ClickAsync();

            var modal = _page.Locator("div.fixed.inset-0").Filter(new() { HasText = "Add Transaction Fee" });
            await modal.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await modal.Locator("input[placeholder='Enter fee description']").FillAsync(feeDescription);
            await modal.Locator("select").Nth(0).SelectOptionAsync("0");
            await modal.Locator("select").Nth(1).SelectOptionAsync("0");
            await modal.Locator("input[placeholder='Enter fee value']").FillAsync(feeValue.ToString(CultureInfo.InvariantCulture));
            await modal.GetByRole(AriaRole.Button, new() { Name = "Add Fee" }).ClickAsync();

            await _page.GetByText(feeDescription).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(AddFeeToContractAsync));
    }

    public async Task AssertContractFeeVisibleAsync(string feeDescription)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByText(feeDescription).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertContractFeeVisibleAsync));
    }

    public async Task BackToContractListAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Back to List" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }, nameof(BackToContractListAsync));
    }

    public async Task AssertHomePageVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.TitleAsync()).ShouldBe("Welcome - Estate Management");
            (await _page.Locator("#loginButton").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertHomePageVisibleAsync));
    }

    public async Task AssertDashboardWelcomeMessageVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByText("Welcome to Estate Management System").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertDashboardWelcomeMessageVisibleAsync));
    }

    public async Task AssertEstateDashboardVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await AssertDashboardShellVisibleAsync();
            await AssertComparisonDateSelectorVisibleAsync();
            await AssertMerchantKpiSummaryCardsVisibleAsync();
            await AssertSalesComparisonCardsVisibleAsync();
            await AssertRecentMerchantsSectionVisibleAsync();
        }, nameof(AssertEstateDashboardVisibleAsync));
    }

    public async Task AssertAdministratorDashboardVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await AssertDashboardShellVisibleAsync();
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Welcome, Administrator" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertAdministratorDashboardVisibleAsync));
    }

    public async Task AssertComparisonDateSelectorVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
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
        }, nameof(AssertComparisonDateSelectorVisibleAsync));
    }

    public async Task AssertMerchantKpiSummaryCardsVisibleAsync()
    {
        //await AssertInfoBoxVisibleAsync("Merchants with Sales (Last Hour)", "45");
        //await AssertInfoBoxVisibleAsync("Merchants with No Sales Today", "12");
        //await AssertInfoBoxVisibleAsync("Merchants with No Sales (7 Days)", "5");
        await RunWithFailureArtifactsAsync(async () =>
        {
            await AssertInfoBoxVisibleAsync("Merchants with Sales (Last Hour)", "0");
            await AssertInfoBoxVisibleAsync("Merchants with No Sales Today", "0");
            await AssertInfoBoxVisibleAsync("Merchants with No Sales (7 Days)", "0");
        }, nameof(AssertMerchantKpiSummaryCardsVisibleAsync));
    }

    public async Task AssertSalesComparisonCardsVisibleAsync()
    {
        //await AssertCardVisibleAsync("Today's Sales", "523 transactions", new Regex(@"[£$]145,000\.00"));
        //await AssertCardVisibleAsync("Failed Sales (Low Credit)", "15 transactions", new Regex(@"[£$]850\.00"));
        await RunWithFailureArtifactsAsync(async () =>
        {
            await AssertCardVisibleAsync("Today's Sales", "0 transactions", new Regex(@"[£$¤]\s?0(?:,000)?\.00"));
            await AssertCardVisibleAsync("Failed Sales (Low Credit)", "0 transactions", new Regex(@"[£$¤]\s?0(?:,000)?\.00"));
        }, nameof(AssertSalesComparisonCardsVisibleAsync));
    }

    public async Task AssertRecentMerchantsSectionVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Recently Created Merchants" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertRecentMerchantsSectionVisibleAsync));
    }

    public async Task AssertMerchantKpiSummaryCardsNotVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await AssertInfoBoxAbsentAsync("Merchants with Sales (Last Hour)");
            await AssertInfoBoxAbsentAsync("Merchants with No Sales Today");
            await AssertInfoBoxAbsentAsync("Merchants with No Sales (7 Days)");
        }, nameof(AssertMerchantKpiSummaryCardsNotVisibleAsync));
    }

    public async Task AssertSalesComparisonCardsNotVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await AssertCardAbsentAsync("Today's Sales");
            await AssertCardAbsentAsync("Failed Sales (Low Credit)");
        }, nameof(AssertSalesComparisonCardsNotVisibleAsync));
    }

    public async Task AssertRecentMerchantsSectionNotVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Recently Created Merchants" }).CountAsync()).ShouldBe(0);
        }, nameof(AssertRecentMerchantsSectionNotVisibleAsync));
    }

    public async Task AssertDashboardNavigationLinkVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.Locator("#dashboardLink").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertDashboardNavigationLinkVisibleAsync));
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

    private async Task WaitForEstateOverviewAsync()
    {
        await _page.Locator(".animate-spin").WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Hidden,
            Timeout = 10000
        });
    }

    private async Task WaitForOperatorManagementAsync()
    {
        var spinner = _page.Locator(".animate-spin");
        if (await spinner.CountAsync() > 0)
        {
            await spinner.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Hidden,
                Timeout = 10000
            });
        }
    }

    private ILocator GetAssignedOperatorRow(string operatorName)
    {
        return _page.Locator("div.flex.items-center.justify-between.p-3.bg-gray-50.rounded-lg")
            .Filter(new() { HasText = operatorName });
    }

    private ILocator GetOperatorRow(string operatorName)
    {
        return _page.Locator("tbody tr").Filter(new() { HasText = operatorName });
    }

    private ILocator GetContractCard(string contractDescription)
    {
        return _page.Locator("div.bg-white.rounded-lg.shadow-md.p-6").Filter(new()
        {
            Has = _page.GetByRole(AriaRole.Heading, new() { Name = contractDescription })
        });
    }

    private ILocator GetContractProductCard(string productName)
    {
        return _page.Locator("div.border.border-gray-200.rounded-lg.p-4").Filter(new()
        {
            Has = _page.GetByRole(AriaRole.Heading, new() { Name = productName })
        });
    }

    private static Guid ExtractContractIdFromUrl(string url)
    {
        var match = Regex.Match(url, @"/contracts/(?<id>[0-9a-fA-F-]+)(?:/edit)?(?:\?.*)?$", RegexOptions.IgnoreCase);
        return match.Success && Guid.TryParse(match.Groups["id"].Value, out var contractId)
            ? contractId
            : Guid.Empty;
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
                    await first.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
                    return;
                }
            }
        }

        throw new InvalidOperationException($"Could not find a visible clickable element for selectors: {string.Join(", ", selectors)}");
    }

    private async Task WaitForAuthenticationNavigationAsync()
    {
        try
        {
            await _page.WaitForURLAsync(
                new Regex(@".*/(login|connect/authorize).*", RegexOptions.IgnoreCase),
                new PageWaitForURLOptions
                {
                    Timeout = 60000
                });
        }
        catch
        {
            // If the URL is already where we need it, keep going and let the selector wait decide.
        }
    }

    private string ResolveBaseUrl()
    {
        var hostPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
        return $"https://127.0.0.1:{hostPort}";
    }

    private string ResolveEstateManagementBaseUrl()
    {
        var hostPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
        return $"https://127.0.0.1:{hostPort}";
    }

    private async Task RunWithFailureArtifactsAsync(Func<Task> action, string context)
    {
        try
        {
            await action();
        }
        catch (Exception ex)
        {
            await CaptureDebugArtifactsAsync(context, ex);
            throw;
        }
    }

    private async Task CaptureDebugArtifactsAsync(string context, Exception exception)
    {
        try
        {
            var outputDirectory = Path.Combine(Environment.CurrentDirectory, "TestResults", "Diagnostics");
            Directory.CreateDirectory(outputDirectory);

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var safeContext = context.Replace(" ", "_");
            var artifactPath = Path.Combine(outputDirectory, $"failure-{safeContext}-{timestamp}.txt");

            var bodyText = string.Empty;
            try
            {
                bodyText = await _page.Locator("body").InnerTextAsync();
            }
            catch
            {
                bodyText = "<unable to read body text>";
            }

            var html = string.Empty;
            try
            {
                html = await _page.ContentAsync();
            }
            catch
            {
                html = "<unable to read html>";
            }

            var content = new StringBuilder();
            content.AppendLine($"Context: {context}");
            content.AppendLine($"Exception: {exception.GetType().FullName}");
            content.AppendLine($"Message: {exception.Message}");
            content.AppendLine($"Url: {_page.Url}");
            content.AppendLine($"Title: {await _page.TitleAsync()}");
            content.AppendLine();
            content.AppendLine("Body:");
            content.AppendLine(bodyText);
            content.AppendLine();
            content.AppendLine("Html:");
            content.AppendLine(html);

            await File.WriteAllTextAsync(artifactPath, content.ToString());
            Console.WriteLine($"Failure diagnostics saved to: {artifactPath}");
        }
        catch (Exception captureException)
        {
            Console.WriteLine($"Failed to capture debug artifacts: {captureException.Message}");
        }
    }
}
