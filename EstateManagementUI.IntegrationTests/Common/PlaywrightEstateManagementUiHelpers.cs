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

    // Merchant-specific methods
    public async Task ClickNewMerchantButton()
    {
        await this.Page.ClickAsync("#newMerchantButton");
    }

    public async Task VerifyAddNewMerchantScreenIsDisplayed()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("New Merchant");
        });
    }

    public async Task VerifyMerchantListContainsAtLeast(int count)
    {
        await Retry.For(async () =>
        {
            var merchantTable = this.Page.Locator("#merchantList");
            var rows = await merchantTable.Locator("tr").AllAsync();
            rows.Count.ShouldBeGreaterThanOrEqualTo(count + 1); // +1 for header row
        }, TimeSpan.FromSeconds(30));
    }

    public async Task VerifyMerchantListDisplaysMerchantNames()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#merchantList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.ShouldContain("Name", Case.Insensitive);
        });
    }

    public async Task VerifyMerchantListDisplaysSettlementSchedules()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#merchantList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.ShouldContain("Settlement Schedule", Case.Insensitive);
        });
    }

    public async Task VerifyMerchantListDisplaysContactInformation()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#merchantList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.Any(h => h.Contains("Contact", StringComparison.InvariantCultureIgnoreCase)).ShouldBeTrue();
        });
    }

    public async Task ClickViewMerchantButton(string merchantName)
    {
        await Retry.For(async () =>
        {
            var rows = await this.Page.Locator("#merchantList tr").AllAsync();
            foreach (var row in rows)
            {
                var cellText = await row.TextContentAsync();
                if (cellText?.Contains(merchantName) == true)
                {
                    var viewButton = row.Locator("button, a").Filter(new LocatorFilterOptions { HasText = "View" });
                    if (await viewButton.CountAsync() > 0)
                    {
                        await viewButton.First.ClickAsync();
                        return;
                    }
                }
            }
        }, TimeSpan.FromMinutes(1));
    }

    public async Task VerifyViewMerchantScreenIsDisplayed()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("View Merchant");
        });
    }

    public async Task VerifyMerchantDetailsSectionIsVisible()
    {
        await Retry.For(async () =>
        {
            var detailsSection = this.Page.Locator("#merchantDetails, .merchant-details");
            (await detailsSection.IsVisibleAsync()).ShouldBeTrue();
        });
    }

    public async Task VerifyMerchantOperatorsSectionIsVisible()
    {
        await Retry.For(async () =>
        {
            var operatorsSection = this.Page.Locator("#merchantOperatorList");
            (await operatorsSection.IsVisibleAsync()).ShouldBeTrue();
        });
    }

    public async Task VerifyMerchantContractsSectionIsVisible()
    {
        await Retry.For(async () =>
        {
            var contractsSection = this.Page.Locator("#merchantContractList");
            (await contractsSection.IsVisibleAsync()).ShouldBeTrue();
        });
    }

    public async Task VerifyMerchantDevicesSectionIsVisible()
    {
        await Retry.For(async () =>
        {
            var devicesSection = this.Page.Locator("#merchantDeviceList");
            (await devicesSection.IsVisibleAsync()).ShouldBeTrue();
        });
    }

    // Operator-specific methods
    public async Task ClickNewOperatorButton()
    {
        await this.Page.ClickAsync("#newOperatorButton");
    }

    public async Task VerifyAddNewOperatorScreenIsDisplayed()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("New Operator");
        });
    }

    public async Task VerifyOperatorListContainsAtLeast(int count)
    {
        await Retry.For(async () =>
        {
            var operatorTable = this.Page.Locator("#operatorsList");
            var rows = await operatorTable.Locator("tr").AllAsync();
            rows.Count.ShouldBeGreaterThanOrEqualTo(count + 1); // +1 for header row
        }, TimeSpan.FromSeconds(30));
    }

    public async Task VerifyOperatorListDisplaysOperatorNames()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#operatorsList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.ShouldContain("Name", Case.Insensitive);
        });
    }

    public async Task VerifyOperatorListDisplaysCustomMerchantNumberRequirements()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#operatorsList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.Any(h => h.Contains("Merchant Number", StringComparison.InvariantCultureIgnoreCase)).ShouldBeTrue();
        });
    }

    public async Task VerifyOperatorListDisplaysCustomTerminalNumberRequirements()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#operatorsList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.Any(h => h.Contains("Terminal Number", StringComparison.InvariantCultureIgnoreCase)).ShouldBeTrue();
        });
    }

    public async Task VerifyOperatorConfigurationDetailsInList()
    {
        await Retry.For(async () =>
        {
            var operatorTable = this.Page.Locator("#operatorsList");
            (await operatorTable.IsVisibleAsync()).ShouldBeTrue();
            var rows = await operatorTable.Locator("tr").AllAsync();
            rows.Count.ShouldBeGreaterThan(1); // At least header + 1 data row
        });
    }

    // Contract-specific methods
    public async Task ClickNewContractButton()
    {
        await this.Page.ClickAsync("#newContractButton");
    }

    public async Task VerifyAddNewContractScreenIsDisplayed()
    {
        await Retry.For(async () =>
        {
            await this.VerifyPageTitle("New Contract");
        });
    }

    public async Task VerifyContractListContainsAtLeast(int count)
    {
        await Retry.For(async () =>
        {
            var contractTable = this.Page.Locator("#contractList");
            var rows = await contractTable.Locator("tr").AllAsync();
            rows.Count.ShouldBeGreaterThanOrEqualTo(count + 1); // +1 for header row
        }, TimeSpan.FromSeconds(30));
    }

    public async Task VerifyContractListDisplaysDescriptions()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#contractList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.ShouldContain("Description", Case.Insensitive);
        });
    }

    public async Task VerifyContractListDisplaysOperatorInformation()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#contractList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.ShouldContain("Operator", Case.Insensitive);
        });
    }

    public async Task ClickViewProductsButtonForContract(string contractName)
    {
        await Retry.For(async () =>
        {
            var rows = await this.Page.Locator("#contractList tr").AllAsync();
            foreach (var row in rows)
            {
                var cellText = await row.TextContentAsync();
                if (cellText?.Contains(contractName) == true)
                {
                    var viewButton = row.Locator("button, a").Filter(new LocatorFilterOptions { HasText = "View Products" });
                    if (await viewButton.CountAsync() > 0)
                    {
                        await viewButton.First.ClickAsync();
                        return;
                    }
                }
            }
        }, TimeSpan.FromMinutes(1));
    }

    public async Task VerifyContractProductsScreenIsDisplayed()
    {
        await Retry.For(async () =>
        {
            var title = await this.Page.TitleAsync();
            title.ShouldContain("Products for Contract");
        });
    }

    public async Task VerifyProductListContainsAtLeast(int count)
    {
        await Retry.For(async () =>
        {
            var productTable = this.Page.Locator("#contractProductList");
            var rows = await productTable.Locator("tr").AllAsync();
            rows.Count.ShouldBeGreaterThanOrEqualTo(count + 1); // +1 for header row
        }, TimeSpan.FromSeconds(30));
    }

    public async Task VerifyProductListDisplaysProductNames()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#contractProductList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.Any(h => h.Contains("Product", StringComparison.InvariantCultureIgnoreCase) || 
                                h.Contains("Name", StringComparison.InvariantCultureIgnoreCase)).ShouldBeTrue();
        });
    }

    public async Task VerifyProductListDisplaysProductValues()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#contractProductList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.Any(h => h.Contains("Value", StringComparison.InvariantCultureIgnoreCase) || 
                                h.Contains("Price", StringComparison.InvariantCultureIgnoreCase)).ShouldBeTrue();
        });
    }

    // Estate-specific methods
    public async Task VerifyEstateNameIsDisplayed()
    {
        await Retry.For(async () =>
        {
            var estateName = this.Page.Locator("#Estate_Name");
            (await estateName.IsVisibleAsync()).ShouldBeTrue();
        });
    }

    public async Task VerifyEstateReferenceIsDisplayed()
    {
        await Retry.For(async () =>
        {
            var estateReference = this.Page.Locator("#Estate_Reference");
            (await estateReference.IsVisibleAsync()).ShouldBeTrue();
        });
    }

    public async Task VerifyEstateOperatorsListContainsAtLeast(int count)
    {
        await Retry.For(async () =>
        {
            var operatorTable = this.Page.Locator("#estateOperatorsList");
            var rows = await operatorTable.Locator("tr").AllAsync();
            rows.Count.ShouldBeGreaterThanOrEqualTo(count + 1); // +1 for header row
        }, TimeSpan.FromSeconds(30));
    }

    public async Task VerifyEstateOperatorsListDisplaysOperatorNames()
    {
        await Retry.For(async () =>
        {
            var headers = await this.Page.Locator("#estateOperatorsList th").AllAsync();
            var headerTexts = new List<string>();
            foreach (var header in headers)
            {
                headerTexts.Add(await header.TextContentAsync() ?? "");
            }
            headerTexts.ShouldContain("Name", Case.Insensitive);
        });
    }
}
