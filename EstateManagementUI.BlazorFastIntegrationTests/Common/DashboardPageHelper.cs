using System.Runtime.CompilerServices;
using System.Globalization;
using Microsoft.Playwright;
using Shouldly;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;
using Shared.IntegrationTesting;
using SimpleResults;
using TransactionProcessor.DataTransferObjects.Requests.MerchantSchedule;

namespace EstateManagementUI.IntegrationTests.Common;

public sealed class DashboardPageHelper
{
    private const string EstateManagementHeading = "Estate Management";
    private const string BackToListText = "Back to List";
    private const string LoginUsernameSelector = "#loginUsername";
    private const string LoginPasswordSelector = "#loginPassword";
    private const string SubmitLoginButtonSelector = "#submitLoginButton";
    private const string LoginButtonSelector = "#loginButton";
    private const string SelectSelector = "select";
    private const string SelectOptionSelector = "select option";
    private const string ValueAttributeName = "value";
    private const string MerchantRowSelector = "div.flex.items-center.justify-between.p-3.bg-gray-50.rounded-lg";
    private const string MerchantsPath = "/merchants";
    private const string DateFormat = "yyyy-MM-dd";
    private const string SettlementScheduleSelector = "select[name='SettlementSchedule']";
    private const string OperatorNameInputSelector = "input[placeholder='Enter operator name']";

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
            await _page.GotoAsync(ResolveBaseUrl() + "/entry");
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await _page.GetByRole(AriaRole.Heading, new() { Name = EstateManagementHeading }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(NavigateToAppAddressAsync));
    }

    public async Task NavigateToEntryScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GotoAsync(ResolveBaseUrl() + "/entry");
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(NavigateToEntryScreenAsync));
    }

    public async Task AssertEntryScreenVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByRole(AriaRole.Heading, new() { Name = EstateManagementHeading }).IsVisibleAsync()).ShouldBeTrue();
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
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenEstateInfoFromEntryAsync));
    }

    public async Task AssertEstateInfoPageVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Heading, new() { Name = EstateManagementHeading }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.GetByText("Comprehensive estate management and configuration").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator(LoginButtonSelector).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await _page.GetByRole(AriaRole.Heading, new() { Name = EstateManagementHeading }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByText("Comprehensive estate management and configuration").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator(LoginButtonSelector).IsVisibleAsync()).ShouldBeTrue();
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

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
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
            (await _page.GetByRole(AriaRole.Button, new() { Name = BackToListText }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertOperatorViewVisibleAsync));
    }

    public async Task BackToOperatorListFromViewAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = BackToListText }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
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
            (await _page.Locator(OperatorNameInputSelector).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Update Operator" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertOperatorEditVisibleAsync));
    }

    public async Task CancelOperatorEditAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Cancel" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(CancelOperatorEditAsync));
    }

    public async Task OpenNewOperatorScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#newOperatorButton").ClickAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Create New Operator" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(OpenNewOperatorScreenAsync));
    }

    public async Task AssertNewOperatorScreenVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await WaitForAnyVisibleAsync(
                "h1:has-text('Create New Operator')",
                OperatorNameInputSelector,
                "#createOperatorButton")).ShouldBeTrue();

            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Create New Operator" });
            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator(OperatorNameInputSelector).IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#createOperatorButton").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertNewOperatorScreenVisibleAsync));
    }

    public async Task CreateOperatorAsync(string operatorName, bool requireCustomMerchantNumber, bool requireCustomTerminalNumber)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator(OperatorNameInputSelector).FillAsync(operatorName);
            await _page.Locator("input[type='checkbox']").Nth(0).SetCheckedAsync(requireCustomMerchantNumber);
            await _page.Locator("input[type='checkbox']").Nth(1).SetCheckedAsync(requireCustomTerminalNumber);
            await _page.Locator("#createOperatorButton").ClickAsync();
        }, nameof(CreateOperatorAsync));
    }

    public async Task ClickSignInButtonAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var entrySignInButton = _page.Locator(LoginButtonSelector);

            Console.WriteLine($"Sign in before click: {_page.Url}");

            if (await entrySignInButton.IsVisibleAsync())
            {
                await entrySignInButton.ClickAsync();
            }
            else
            {
                (await _page.Locator("a[href='/login']").IsVisibleAsync()).ShouldBeTrue();
                await _page.Locator("a[href='/login']").ClickAsync();
            }

            await WaitForLoginScreenAsync();
            Console.WriteLine($"Sign in after click: {_page.Url}");
            Console.WriteLine($"Sign in title after click: {await _page.TitleAsync()}");
            Console.WriteLine($"Sign in body after click: {await _page.Locator("body").InnerTextAsync()}");
        }, nameof(ClickSignInButtonAsync));
    }

    public async Task AssertLoginScreenVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Sign in" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator(LoginUsernameSelector).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator(LoginPasswordSelector).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator(SubmitLoginButtonSelector).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(AssertLoginScreenVisibleAsync));
    }

    public async Task LoginAsync(string username, string password)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            if (!_page.Url.Contains("/login", StringComparison.OrdinalIgnoreCase) ||
                !await _page.Locator(LoginUsernameSelector).IsVisibleAsync())
            {
                await ClickSignInButtonAsync();
            }

            await _page.Locator(LoginUsernameSelector).FillAsync(username);
            await _page.Locator(LoginPasswordSelector).FillAsync(password);
            await _page.Locator(SubmitLoginButtonSelector).ClickAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 30000
            });
        }, nameof(LoginAsync));
    }

    public async Task AssertDashboardShellVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Dashboard" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.GetByText("Welcome to Estate Management System").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator("#dashboardLink").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(AssertDashboardShellVisibleAsync));
    }

    public async Task AssertAuthenticatedLandingVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Heading, new() { Name = EstateManagementHeading }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.GetByText("Contracts").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(AssertAuthenticatedLandingVisibleAsync));
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

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenEstateManagementScreenAsync));
    }

    public async Task AssertEstateManagementHeadingVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForEstateOverviewAsync();
            (await _page.GetByRole(AriaRole.Heading, new() { Name = EstateManagementHeading }).IsVisibleAsync()).ShouldBeTrue();
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
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(SwitchToOperatorsTabAsync));
    }

    public async Task AssertAssignedOperatorsSectionVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Assigned Operators" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertAssignedOperatorsSectionVisibleAsync));
    }

    public async Task AddOperatorToEstateAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#addOperatorButton").ClickAsync();

            var option = _page.Locator($"{SelectOptionSelector}:has-text('{operatorName}')");
            await option.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 10000
            });

            (await option.CountAsync()).ShouldBeGreaterThan(0);

            var optionValue = await option.First.GetAttributeAsync(ValueAttributeName);
            optionValue.ShouldNotBeNull();

            await _page.Locator(SelectSelector).SelectOptionAsync(new[] { optionValue! });
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add", Exact = true }).ClickAsync();
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

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
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

    public async Task OpenFileProcessingScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var fileProcessingLink = _page.Locator("#fileProcessingLink");
            if (await fileProcessingLink.CountAsync() > 0 && await fileProcessingLink.First.IsVisibleAsync())
            {
                await fileProcessingLink.First.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            }
            else
            {
                await _page.GotoAsync(ResolveBaseUrl() + "/file-processing");
            }

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenFileProcessingScreenAsync));
    }

    public async Task AssertFileProcessingHeadingVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "File Processing" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Upload File" }).First.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertFileProcessingHeadingVisibleAsync));
    }

    public async Task OpenFileUploadPageAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GotoAsync(ResolveBaseUrl() + "/file-processing/upload");
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenFileUploadPageAsync));
    }

    public async Task AssertFileUploadPageVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Upload File" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#merchantSelect").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#fileProfileSelect").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#uploadFileInput").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#uploadFileButton").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertFileUploadPageVisibleAsync));
    }

    public async Task AssertFileUploadDropdownDefaultsAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.Locator("#merchantSelect").InputValueAsync()).ShouldBe(string.Empty);
            (await _page.Locator("#fileProfileSelect").InputValueAsync()).ShouldBe(string.Empty);
        }, nameof(AssertFileUploadDropdownDefaultsAsync));
    }

    public async Task UploadBatchTopupFileAsync()
    {
        await UploadBatchTopupFileAsync("""
                                  254701000001,500
                                  254701000002,500
                                  254701000003,500
                                  """);
    }

    public async Task UploadBatchTopupFileAsync(string fileContents)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await SelectFirstRealOptionAsync("#merchantSelect");
            await SelectFirstRealOptionAsync("#fileProfileSelect");

            string tempFilePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.csv");
            await File.WriteAllTextAsync(tempFilePath, fileContents);
            string fileName = Path.GetFileName(tempFilePath);

            try
            {
                await _page.Locator("#uploadFileInput").SetInputFilesAsync(tempFilePath);

                await _page.Locator("#uploadFileButton").ClickAsync();
                await _page.GetByText($"File '{fileName}' uploaded successfully.").WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 30000
                });
            }
            finally
            {
                try
                {
                    File.Delete(tempFilePath);
                }
                catch
                {
                    // Best effort only. The browser may still be holding the file briefly.
                }
            }
        }, nameof(UploadBatchTopupFileAsync));
    }

    public async Task AssertFileUploadSuccessMessageVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.GetByText("uploaded successfully").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertFileUploadSuccessMessageVisibleAsync));
    }

    public async Task CreateContractAsync(string contractDescription, string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GotoAsync(ResolveBaseUrl() + "/contracts/new");
            await _page.Locator("input[placeholder='Enter contract description']").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator("#createContractButton").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            var operatorOption = _page.Locator(SelectOptionSelector).Filter(new() { HasText = operatorName });
            await operatorOption.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 10000
            });

            var operatorValue = await operatorOption.First.GetAttributeAsync(ValueAttributeName);
            operatorValue.ShouldNotBeNull();

            await _page.Locator(SelectSelector).SelectOptionAsync(new[] { operatorValue! });
            await _page.Locator("input[placeholder='Enter contract description']").FillAsync(contractDescription);
            await _page.Locator("#createContractButton").ClickAsync();
            await _page.Locator("#newContractButton").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
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
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
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
            (await _page.GetByRole(AriaRole.Button, new() { Name = BackToListText }).IsVisibleAsync()).ShouldBeTrue();
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
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

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

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
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

    public async Task AddProductToContractAsync(string productName, string displayText, bool isVariableValue, decimal? value)
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
            await modal.Locator("input[type='checkbox']").SetCheckedAsync(isVariableValue);
            if (isVariableValue == false)
            {
                await modal.Locator("input[placeholder='Enter value']").FillAsync(value?.ToString(CultureInfo.InvariantCulture) ?? throw new InvalidOperationException("Value is required when the product is not variable."));
            }
            await modal.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

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

    public async Task AddFeeToContractAsync(string feeDescription, string calculationType, string feeType, decimal feeValue)
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
            await modal.Locator(SelectSelector).Nth(0).SelectOptionAsync(ResolveCalculationTypeOptionValue(calculationType));
            await modal.Locator(SelectSelector).Nth(1).SelectOptionAsync(ResolveFeeTypeOptionValue(feeType));
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
            await _page.GetByRole(AriaRole.Button, new() { Name = BackToListText }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(BackToContractListAsync));
    }

    public async Task OpenMerchantManagementScreenAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var merchantsLink = _page.Locator("#merchantsLink");
            if (await merchantsLink.CountAsync() > 0 && await merchantsLink.First.IsVisibleAsync())
            {
                await merchantsLink.First.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            }
            else
            {
                await _page.GotoAsync(ResolveBaseUrl() + MerchantsPath);
            }

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenMerchantManagementScreenAsync));
    }

    public async Task AssertMerchantManagementHeadingVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Merchant Management" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantManagementHeadingVisibleAsync));
    }

    public async Task CreateMerchantAsync(string merchantName)
    {
        await CreateMerchantAsync(new MerchantCreationInput(
            merchantName,
            "Immediate",
            "1 Integration Road",
            "Suite 100",
            "Test Town",
            "Test Region",
            "TE1 1ST",
            "United Kingdom",
            "Test Contact",
            "test.contact@example.com",
            "01234567890"));
    }

    public async Task CreateMerchantAsync(MerchantCreationInput merchant)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var newMerchantButton = _page.Locator("#newMerchantButton");
            if (await newMerchantButton.CountAsync() > 0 && await newMerchantButton.First.IsVisibleAsync())
            {
                await newMerchantButton.First.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
            }
            else
            {
                await _page.GotoAsync(ResolveBaseUrl() + "/merchants/new");
            }

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            await _page.Locator("input[placeholder='Enter merchant name']").FillAsync(merchant.MerchantName);
            await _page.Locator(SettlementScheduleSelector).SelectOptionAsync(merchant.SettlementSchedule);
            await _page.Locator("input[placeholder='Enter address line 1']").FillAsync(merchant.AddressLine1);
            await _page.Locator("input[placeholder='Enter address line 2 (optional)']").FillAsync(merchant.AddressLine2 ?? string.Empty);
            await _page.Locator("input[placeholder='Enter town']").FillAsync(merchant.Town);
            await _page.Locator("input[placeholder='Enter region']").FillAsync(merchant.Region);
            await _page.Locator("input[placeholder='Enter postcode']").FillAsync(merchant.PostCode);

            await _page.GetByRole(AriaRole.Button, new() { Name = "Select country" }).ClickAsync();
            await _page.GetByRole(AriaRole.Button, new() { Name = merchant.Country }).ClickAsync();

            await _page.Locator("input[placeholder='Enter contact name']").FillAsync(merchant.ContactName);
            await _page.Locator("input[placeholder='Enter email address']").FillAsync(merchant.EmailAddress);
            await _page.Locator("input[placeholder='Enter phone number']").FillAsync(merchant.PhoneNumber);

            await _page.Locator("#createMerchantButton").ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            await _page.GetByRole(AriaRole.Heading, new() { Name = "Merchant Management" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(CreateMerchantAsync));
    }

    public async Task AssertMerchantListContainsAsync(string merchantName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var row = GetMerchantRow(merchantName);
            await row.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await row.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantListContainsAsync));
    }

    public async Task OpenMerchantViewAsync(string merchantName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var row = GetMerchantRow(merchantName);
            await row.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await row.GetByRole(AriaRole.Button, new() { Name = "View" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenMerchantViewAsync));
    }

    public async Task AssertMerchantViewVisibleAsync(string merchantName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = $"View Merchant: {merchantName}" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "View Schedule" }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = BackToListText }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantViewVisibleAsync));
    }

    public async Task SwitchMerchantTabAsync(string tabName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = tabName }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(SwitchMerchantTabAsync));
    }

    public async Task AssertMerchantPageTextVisibleAsync(string expectedText)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var text = _page.GetByText(expectedText);
            await text.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await text.First.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantPageTextVisibleAsync));
    }

    public async Task OpenMerchantScheduleFromViewAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "View Schedule" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenMerchantScheduleFromViewAsync));
    }

    public async Task AssertMerchantReadOnlyScheduleVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Selected Year Schedule" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            _page.Url.ShouldContain("readOnly=true");
            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Selected Year Schedule" }).IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#saveScheduleButton").CountAsync()).ShouldBe(0);
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Back to Merchant" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantReadOnlyScheduleVisibleAsync));
    }

    public async Task BackToMerchantFromViewScheduleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Back to Merchant" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(BackToMerchantFromViewScheduleAsync));
    }

    public async Task OpenMerchantEditAsync(string merchantName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var merchantId = ExtractMerchantIdFromUrl(_page.Url);
            if (merchantId == Guid.Empty)
            {
                await _page.GotoAsync(ResolveBaseUrl() + MerchantsPath);
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

                var row = GetMerchantRow(merchantName);
                await row.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 10000
                });

                await row.GetByRole(AriaRole.Button, new() { Name = "Edit" }).ClickAsync();
            }
            else
            {
                await _page.GotoAsync($"{ResolveBaseUrl()}{MerchantsPath}/{merchantId}/edit");
            }

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenMerchantEditAsync));
    }

    public async Task AssertMerchantEditVisibleAsync(string merchantName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = $"Edit Merchant: {merchantName}" });
            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await _page.GetByRole(AriaRole.Button, new() { Name = "Edit Schedule" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantEditVisibleAsync));
    }

    public async Task AssertMerchantEditOpeningHoursVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByText("Enter merchant opening and closing times in HHmm format").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await _page.Locator("#saveOpeningHoursButton").ScrollIntoViewIfNeededAsync();
            (await _page.Locator("#saveOpeningHoursButton").IsVisibleAsync()).ShouldBeTrue();
            (await _page.Locator("#mondayOpening").IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantEditOpeningHoursVisibleAsync));
    }

    public async Task SaveMerchantOpeningHoursAsync(MerchantOpeningHoursInput openingHours)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#mondayOpening").FillAsync(openingHours.MondayOpening);
            await _page.Locator("#mondayClosing").FillAsync(openingHours.MondayClosing);
            await _page.Locator("#tuesdayOpening").FillAsync(openingHours.TuesdayOpening);
            await _page.Locator("#tuesdayClosing").FillAsync(openingHours.TuesdayClosing);
            await _page.Locator("#wednesdayOpening").FillAsync(openingHours.WednesdayOpening);
            await _page.Locator("#wednesdayClosing").FillAsync(openingHours.WednesdayClosing);
            await _page.Locator("#thursdayOpening").FillAsync(openingHours.ThursdayOpening);
            await _page.Locator("#thursdayClosing").FillAsync(openingHours.ThursdayClosing);
            await _page.Locator("#fridayOpening").FillAsync(openingHours.FridayOpening);
            await _page.Locator("#fridayClosing").FillAsync(openingHours.FridayClosing);
            await _page.Locator("#saturdayOpening").FillAsync(openingHours.SaturdayOpening);
            await _page.Locator("#saturdayClosing").FillAsync(openingHours.SaturdayClosing);
            await _page.Locator("#sundayOpening").FillAsync(openingHours.SundayOpening);
            await _page.Locator("#sundayClosing").FillAsync(openingHours.SundayClosing);

            await _page.Locator("#saveOpeningHoursButton").ClickAsync();
            await _page.GetByText("Merchant opening hours updated successfully").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(SaveMerchantOpeningHoursAsync));
    }

    public async Task AddMerchantOperatorAsync(string operatorName, string merchantNumber, string terminalNumber)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#addOperatorButton").ClickAsync();

            var option = _page.Locator(SelectOptionSelector).Filter(new() { HasText = operatorName });
            await option.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Attached,
                Timeout = 10000
            });

            var optionValue = await option.First.GetAttributeAsync(ValueAttributeName);
            optionValue.ShouldNotBeNull();

            await _page.Locator(SelectSelector).SelectOptionAsync(new[] { optionValue! });

            var merchantNumberInput = _page.GetByPlaceholder("Enter merchant number");
            await merchantNumberInput.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await merchantNumberInput.FillAsync(merchantNumber);

            var terminalNumberInput = _page.GetByPlaceholder("Enter terminal number");
            await terminalNumberInput.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await terminalNumberInput.FillAsync(terminalNumber);

            await _page.GetByRole(AriaRole.Button, new() { Name = "Add", Exact = true }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(AddMerchantOperatorAsync));
    }

    public async Task AssertMerchantOperatorVisibleAsync(string operatorName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var row = _page.Locator(MerchantRowSelector)
                .Filter(new() { HasText = operatorName });
            await row.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await row.First.GetByRole(AriaRole.Button, new() { Name = "Remove" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantOperatorVisibleAsync));
    }

    public async Task AssertMerchantContractsTabVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var text = _page.GetByText("No contracts assigned");
            await text.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await text.First.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantContractsTabVisibleAsync));
    }

    public async Task AddMerchantDeviceAsync(string deviceIdentifier)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#addDeviceButton").ClickAsync();
            var deviceInput = _page.GetByPlaceholder("Enter device identifier");
            await deviceInput.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await deviceInput.FillAsync(deviceIdentifier);
            await deviceInput.PressAsync("Tab");
            await _page.GetByRole(AriaRole.Button, new() { Name = "Add", Exact = true }).ClickAsync();

            var row = _page.Locator(MerchantRowSelector)
                .Filter(new() { HasText = deviceIdentifier });
            await row.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(AddMerchantDeviceAsync));
    }

    public async Task AssertMerchantDeviceVisibleAsync(string deviceIdentifier)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var row = _page.Locator(MerchantRowSelector)
                .Filter(new() { HasText = deviceIdentifier });
            await row.First.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await row.First.GetByRole(AriaRole.Button, new() { Name = "Swap" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantDeviceVisibleAsync));
    }

    public async Task OpenMerchantScheduleFromEditAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var merchantId = ExtractMerchantIdFromUrl(_page.Url);
            if (merchantId == Guid.Empty)
            {
                await _page.Locator("#editScheduleButton").ClickAsync();
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                return;
            }

            await _page.GotoAsync($"{ResolveBaseUrl()}{MerchantsPath}/{merchantId}/schedule");
        }, nameof(OpenMerchantScheduleFromEditAsync));
    }

    public async Task AssertMerchantEditableScheduleVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var deadline = DateTime.UtcNow.AddSeconds(120);
            while (DateTime.UtcNow < deadline)
            {
                bool saveVisible = await _page.Locator("#saveScheduleButton").IsVisibleAsync();
                bool monthVisible = await _page.Locator("#month-1-closed-days").IsVisibleAsync();
                bool backVisible = await _page.GetByRole(AriaRole.Button, new() { Name = "Back to Edit Merchant" }).IsVisibleAsync();

                if (saveVisible && monthVisible && backVisible)
                {
                    return;
                }

                await _page.WaitForTimeoutAsync(500);
            }

            throw new TimeoutException("Editable merchant schedule did not become visible within 120 seconds.");
        }, nameof(AssertMerchantEditableScheduleVisibleAsync));
    }

    public async Task SaveMerchantScheduleAsync(int year, string closedDays)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#selectedYear").SelectOptionAsync(year.ToString(CultureInfo.InvariantCulture));
            await _page.Locator("#loadYearButton").ClickAsync();
            await _page.Locator("#month-1-closed-days").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.Locator("#month-1-closed-days").FillAsync(closedDays);
            await _page.Locator("#saveScheduleButton").ClickAsync();
            await _page.GetByText($"Schedule saved for {year}.").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
        }, nameof(SaveMerchantScheduleAsync));
    }

    private async Task EnsureMerchantScheduleExistsAsync(int year, string closedDays)
    {
        var merchantId = ExtractMerchantIdFromUrl(_page.Url);
        if (merchantId == Guid.Empty || this.TestingContext.Estates.Count != 1)
        {
            return;
        }

        var estateId = this.TestingContext.GetAllEstateIds().Single();
        var accessToken = this.TestingContext.AccessToken;
        var client = this.TestingContext.DockerHelper.TransactionProcessorClient;
        List<int> closedDayList = closedDays.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(int.Parse)
            .ToList();

        var scheduleRequest = new CreateMerchantScheduleRequest
        {
            Year = year,
            Months =
            [
                new MerchantScheduleMonthRequest
                {
                    Month = 1,
                    ClosedDays = closedDayList
                }
            ]
        };

        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (DateTime.UtcNow < deadline)
        {
            var existing = await client.GetMerchantSchedule(accessToken, estateId, merchantId, year, CancellationToken.None);
            if (existing.IsSuccess)
            {
                return;
            }

            if (existing.Status != ResultStatus.NotFound)
            {
                throw new InvalidOperationException(existing.Errors.FirstOrDefault() ?? existing.Message ?? "Failed to check merchant schedule state.");
            }

            var createResult = await client.CreateMerchantSchedule(accessToken, estateId, merchantId, scheduleRequest, CancellationToken.None);
            if (createResult.IsFailed && createResult.Status != ResultStatus.Conflict)
            {
                throw new InvalidOperationException(createResult.Errors.FirstOrDefault() ?? createResult.Message ?? "Failed to create merchant schedule.");
            }

            await _page.WaitForTimeoutAsync(1000);
        }

        throw new TimeoutException($"Merchant schedule for {year} did not become available.");
    }

    public async Task BackToMerchantFromEditScheduleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GetByRole(AriaRole.Button, new() { Name = "Back to Edit Merchant" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(BackToMerchantFromEditScheduleAsync));
    }

    public async Task OpenMerchantDepositAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var merchantId = ExtractMerchantIdFromUrl(_page.Url);
            if (merchantId == Guid.Empty)
            {
                await _page.GotoAsync(ResolveBaseUrl() + MerchantsPath);
                await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

                var row = _page.Locator("tbody tr").First;
                await row.WaitForAsync(new LocatorWaitForOptions
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = 10000
                });

                await row.GetByRole(AriaRole.Button, new() { Name = "Make Deposit" }).ClickAsync();
            }
            else
            {
                await _page.GotoAsync($"{ResolveBaseUrl()}{MerchantsPath}/{merchantId}/deposit");
            }

            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenMerchantDepositAsync));
    }

    public async Task OpenMerchantDepositAsync(string merchantName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.GotoAsync(ResolveBaseUrl() + MerchantsPath);
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);

            var row = GetMerchantRow(merchantName);
            await row.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await row.GetByRole(AriaRole.Button, new() { Name = "Make Deposit" }).ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(OpenMerchantDepositAsync));
    }

    public async Task AssertMerchantDepositVisibleAsync(string merchantName)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            var heading = _page.GetByRole(AriaRole.Heading, new() { Name = "Make Merchant Deposit" });
            var merchantText = _page.GetByText($"For merchant: {merchantName}");
            var depositAmount = _page.Locator("#depositAmount");
            var depositDate = _page.Locator("#depositDate");
            var depositReference = _page.Locator("#depositReference");

            await heading.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await merchantText.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await depositAmount.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await depositDate.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            await depositReference.WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });

            (await heading.IsVisibleAsync()).ShouldBeTrue();
            (await merchantText.IsVisibleAsync()).ShouldBeTrue();
            (await depositAmount.IsVisibleAsync()).ShouldBeTrue();
            (await depositDate.IsVisibleAsync()).ShouldBeTrue();
            (await depositReference.IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertMerchantDepositVisibleAsync));
    }

    public async Task SubmitMerchantDepositAsync(decimal amount, DateTime date, string reference)
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await _page.Locator("#depositAmount").FillAsync(amount.ToString(CultureInfo.InvariantCulture));
            await _page.Locator("#depositDate").FillAsync(date.ToString(DateFormat, CultureInfo.InvariantCulture));
            await _page.Locator("#depositReference").FillAsync(reference);
            await _page.Locator("#makeDepositButton").ClickAsync();
            await _page.GetByText("Deposit recorded successfully").WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 10000
            });
            await _page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
        }, nameof(SubmitMerchantDepositAsync));
    }

    private static string ResolveCalculationTypeOptionValue(string calculationType)
    {
        return calculationType.Trim().ToLowerInvariant() switch
        {
            "fixed" or "0" => "0",
            "percentage" or "1" => "1",
            _ => throw new InvalidOperationException($"Unsupported calculation type '{calculationType}'.")
        };
    }

    private static string ResolveFeeTypeOptionValue(string feeType)
    {
        return feeType.Trim().ToLowerInvariant() switch
        {
            "merchant" or "0" => "0",
            "serviceprovider" or "service provider" or "1" => "1",
            _ => throw new InvalidOperationException($"Unsupported fee type '{feeType}'.")
        };
    }

    public async Task AssertHomePageVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            (await _page.TitleAsync()).ShouldBe("Welcome - Estate Management");
            (await _page.Locator(LoginButtonSelector).IsVisibleAsync()).ShouldBeTrue();
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
            await WaitForDashboardContentAsync();
            await AssertDashboardShellVisibleAsync();
            await _page.GetByRole(AriaRole.Heading, new() { Name = "Welcome, Administrator" }).WaitForAsync(new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible,
                Timeout = 30000
            });

            (await _page.GetByRole(AriaRole.Heading, new() { Name = "Welcome, Administrator" }).IsVisibleAsync()).ShouldBeTrue();
        }, nameof(AssertAdministratorDashboardVisibleAsync));
    }

    public async Task AssertComparisonDateSelectorVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForDashboardContentAsync();
            var selector = _page.Locator("#comparisonDateSelector");
            var deadline = DateTime.UtcNow.AddSeconds(30);

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
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForDashboardContentAsync();
            await AssertInfoBoxVisibleAsync("Merchants with Sales (Last Hour)", "45");
            await AssertInfoBoxVisibleAsync("Merchants with No Sales Today", "12");
            await AssertInfoBoxVisibleAsync("Merchants with No Sales (7 Days)", "5");
        }, nameof(AssertMerchantKpiSummaryCardsVisibleAsync));
    }

    public async Task AssertSalesComparisonCardsVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForDashboardContentAsync();
            await AssertCardVisibleAsync("Today's Sales", "523 transactions", new Regex(@"[£$¤]\s?145,000\.00"));
            await AssertCardVisibleAsync("Failed Sales (Low Credit)", "15 transactions", new Regex(@"[£$¤]\s?850\.00"));
        }, nameof(AssertSalesComparisonCardsVisibleAsync));
    }

    public async Task AssertRecentMerchantsSectionVisibleAsync()
    {
        await RunWithFailureArtifactsAsync(async () =>
        {
            await WaitForDashboardContentAsync();
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
            Timeout = 30000
        });
    }

    private async Task WaitForDashboardContentAsync()
    {
        await WaitForEstateOverviewAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
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
        return _page.Locator(MerchantRowSelector)
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
        var deadline = DateTime.UtcNow.AddSeconds(30);

        while (DateTime.UtcNow < deadline)
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

            await _page.WaitForTimeoutAsync(250);
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

    private async Task SelectFirstRealOptionAsync(string selectSelector)
    {
        var options = _page.Locator($"{selectSelector} option");
        var optionCount = await options.CountAsync();
        for (var index = 0; index < optionCount; index++)
        {
            var option = options.Nth(index);
            var value = await option.GetAttributeAsync(ValueAttributeName);
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            await _page.Locator(selectSelector).SelectOptionAsync(new[] { value! });
            return;
        }

        throw new InvalidOperationException($"Could not find a non-placeholder option for {selectSelector}");
    }

    private ILocator GetMerchantRow(string merchantName)
    {
        return _page.Locator("tbody tr").Filter(new() { HasText = merchantName });
    }

    private static Guid ExtractMerchantIdFromUrl(string url)
    {
        var match = Regex.Match(url, @"/merchants/(?<id>[0-9a-fA-F-]+)(?:/.*)?(?:\?.*)?$", RegexOptions.IgnoreCase);
        return match.Success && Guid.TryParse(match.Groups["id"].Value, out var merchantId)
            ? merchantId
            : Guid.Empty;
    }

    private async Task WaitForLoginScreenAsync()
    {
        var deadline = DateTime.UtcNow.AddSeconds(30);
        while (DateTime.UtcNow < deadline)
        {
            if (_page.Url.Contains("/login", StringComparison.OrdinalIgnoreCase) ||
                await _page.Locator(LoginUsernameSelector).IsVisibleAsync())
            {
                return;
            }

            await _page.WaitForTimeoutAsync(250);
        }

        throw new TimeoutException("The login screen did not appear in time.");
    }

    private string ResolveBaseUrl()
    {
        var hostPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
        return $"http://127.0.0.1:{hostPort}";
    }

    private string ResolveEstateManagementBaseUrl()
    {
        var hostPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
        return $"http://127.0.0.1:{hostPort}";
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

    public sealed record MerchantOpeningHoursInput(
        string MondayOpening,
        string MondayClosing,
        string TuesdayOpening,
        string TuesdayClosing,
        string WednesdayOpening,
        string WednesdayClosing,
        string ThursdayOpening,
        string ThursdayClosing,
        string FridayOpening,
        string FridayClosing,
        string SaturdayOpening,
        string SaturdayClosing,
        string SundayOpening,
        string SundayClosing);

    public sealed record MerchantCreationInput(
        string MerchantName,
        string SettlementSchedule,
        string AddressLine1,
        string? AddressLine2,
        string Town,
        string Region,
        string PostCode,
        string Country,
        string ContactName,
        string EmailAddress,
        string PhoneNumber);
}
