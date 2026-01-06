using Microsoft.Playwright;
using Reqnroll;
using Shared.IntegrationTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EstateManagementUI.BlazorIntegrationTests.Common;

public class BlazorUiHelpers
{
    private readonly IPage Page;
    private readonly Int32 EstateManagementUiPort;

    public BlazorUiHelpers(IPage page, Int32 estateManagementUiPort)
    {
        this.Page = page;
        this.EstateManagementUiPort = estateManagementUiPort;
    }

    private async Task VerifyPageTitle(String expectedTitle)
    {
        await Retry.For(async () =>
        {
            var title = await this.Page.TitleAsync();
            title.ShouldBe($"{expectedTitle}");
        });
    }

    public async Task NavigateToHomePage()
    {
        await this.Page.GotoAsync($"https://localhost:{this.EstateManagementUiPort}");
        await this.VerifyPageTitle("Welcome - Estate Management");
    }

    public async Task ClickContractsSidebarOption()
    {
        await this.Page.ClickButtonById("contractsLink");
    }

    public async Task ClickMyEstateSidebarOption()
    {
        await this.Page.ClickButtonById("estateDetailsLink");
    }

    public async Task ClickMyMerchantsSidebarOption()
    {
        await this.Page.ClickButtonById("merchantsLink");
    }

    public async Task ClickMyOperatorsSidebarOption()
    {
        await this.Page.ClickButtonById("operatorsLink");
    }

    public async Task ClickOnTheSignInButton()
    {
        await this.Page.ClickButtonById("loginButton");
    }

    public async Task VerifyOnTheMakeMerchantDepositScreen()
    {
        await Retry.For(async () => 
        { 
            var title = await this.Page.TitleAsync();
            title.ShouldBe("Make Merchant Deposit"); 
        });
    }

    public async Task VerifyOnTheTheMerchantDetailsScreen(String merchantName)
    {
        await Retry.For(async () =>
        {
            var value = await this.Page.GetValueById("MerchantName");
            value.ShouldBe(merchantName);
        });
    }

    public async Task VerifyOnTheNewContractScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("New Contract"); });
    }

    public async Task VerifyOnTheNewContractProductScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("New Contract Product"); });
    }

    public async Task VerifyOnTheNewMerchantScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("New Merchant"); });
    }

    public async Task VerifyOnTheEditMerchantScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Edit Merchant"); });
    }

    public async Task VerifyOnTheMakeDepositScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Make Deposit"); });
    }

    public async Task VerifyOnTheViewMerchantScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("View Merchant"); });
    }

    public async Task VerifyOnTheNewOperatorScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("New Operator"); });
    }

    public async Task VerifyOnTheEditOperatorScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Edit Operator"); });
    }

    public async Task VerifyOnTheContractsListScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Contract Management"); });
    }

    public async Task VerifyOnTheContractProductsListScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("View Contract Products"); });
    }

    public async Task VerifyOnTheContractProductsFeesListScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("View Contract Product Fees"); });
    }

    public async Task VerifyOnTheNewTransactionFeeScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("New Transaction Fee"); });
    }

    public async Task VerifyOnTheLoginScreen()
    {
        await Retry.For(async () =>
        {
            var loginButton = await this.Page.FindButtonByText("Login", TimeSpan.FromMinutes(2));
            loginButton.ShouldNotBeNull();
        });
    }

    public async Task ClickOnTheMerchantOperatorsTab()
    {
        await this.ClickTab("nav-operators-tab");
    }

    public async Task ClickOnTheMerchantContractsTab()
    {
        await this.ClickTab("nav-contracts-tab");
    }

    public async Task ClickOnTheMerchantDevicesTab()
    {
        await this.ClickTab("nav-devices-tab");
    }

    private async Task ClickTab(String tabId)
    {
        await Retry.For(async () =>
        {
            var locator = this.Page.Locator($"#{tabId}");
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
            await locator.ClickAsync();
        });
    }

    public async Task VerifyOnMerchantOperatorsTab()
    {
        await Retry.For(async () =>
        {
            var locator = this.Page.Locator("#merchantOperatorList");
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
        });
    }

    public async Task VerifyOnMerchantContractsTab()
    {
        await Retry.For(async () =>
        {
            var locator = this.Page.Locator("#merchantContractList");
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
        });
    }

    public async Task VerifyOnMerchantDevicesTab()
    {
        await Retry.For(async () =>
        {
            var locator = this.Page.Locator("#merchantDeviceList");
            await locator.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 30000 });
        });
    }

    public async Task VerifyOnTheDashboard()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Dashboard"); });
    }

    public async Task VerifyOnTheEstateDetailsScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Estate Management"); });
    }

    public async Task VerifyOnTheMerchantsListScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Merchant Management"); });
    }

    public async Task VerifyOnTheOperatorsListScreen()
    {
        await Retry.For(async () => { await this.VerifyPageTitle("Operator Management"); });
    }

    public async Task VerifyTheCorrectEstateDetailsAreDisplayed(String estateName)
    {
        await Retry.For(async () =>
        {
            var estateNameValue = await this.Page.GetValueById("Estate_Name");
            estateNameValue.ShouldBe(estateName);

            var estateRefValue = await this.Page.GetValueById("Estate_Reference");
            estateRefValue.ShouldNotBeNull();
            estateRefValue.ShouldNotBeEmpty();
        });
    }

    public async Task VerifyTheContractDetailsAreInTheList(List<(String, String, Int32)> contractDescriptions)
    {
        await Retry.For(async () =>
        {
            Int32 foundRowCount = 0;
            var rows = await this.Page.Locator("#contractList tr").AllAsync();

            rows.Count.ShouldBe(contractDescriptions.Count + 1);
            
            foreach ((String description, String operatorName, Int32 products) in contractDescriptions)
            {
                foreach (var row in rows)
                {
                    var headers = await row.Locator("th").AllAsync();
                    if (headers.Any())
                    {
                        // header row so skip
                        continue;
                    }

                    var cells = await row.Locator("td").AllAsync();
                    if (cells.Count > 0)
                    {
                        var cellText = await cells[0].TextContentAsync();
                        if (cellText == description)
                        {
                            // Compare other fields
                            var operatorText = await cells[1].TextContentAsync();
                            var productsText = await cells[2].TextContentAsync();
                            
                            cellText.ShouldBe(description);
                            operatorText.ShouldBe(operatorName);
                            productsText.ShouldBe(products.ToString());

                            foundRowCount++;
                            break;
                        }
                    }
                }
            }

            foundRowCount.ShouldBe(contractDescriptions.Count);
        }, TimeSpan.FromSeconds(120));
    }

    public async Task ClickTheSaveProductButton()
    {
        await this.Page.ClickButtonById("saveProductButton");
    }

    public async Task Login(String username, String password)
    {
        await this.Page.FillIn("Input.Username", username);
        await this.Page.FillIn("Input.Password", password);
        await this.Page.ClickButtonByText("Login");
    }

    public async Task ClickTheNewOperatorButton()
    {
        await this.Page.ClickButtonById("newOperatorButton");
    }

    public async Task ClickTheNewMerchantButton()
    {
        await this.Page.ClickButtonById("newMerchantButton");
    }

    public async Task ClickTheEditOperatorButton(String operatorName)
    {
        await this.ClickElementInTable("operatorList", operatorName, "editOperatorLink");
    }

    public async Task ClickTheEditMerchantButton(String merchantName)
    {
        await this.ClickElementInTable("merchantList", merchantName, "editMerchantLink");
    }

    public async Task ClickTheMakeDepositButtonFor(String merchantName)
    {
        await this.ClickElementInTable("merchantList", merchantName, "makeDepositLink");
    }

    public async Task ClickTheAssignOperatorButton()
    {
        await this.Page.ClickButtonById("assignOperatorButton");
    }

    public async Task ClickTheAddDeviceButton()
    {
        await this.Page.ClickButtonById("addDeviceButton");
    }

    public async Task ClickAddNewContractButton()
    {
        await Retry.For(async () => { await this.Page.ClickButtonById("newContractButton"); });
    }

    public async Task ClickAddNewMerchantButton()
    {
        await Retry.For(async () => { await this.Page.ClickButtonById("newMerchantButton"); });
    }

    public async Task ClickAddNewProductButton()
    {
        await Retry.For(async () => { await this.Page.ClickButtonById("newContractProductButton"); });
    }

    public async Task ClickAddNewTransactionFeeButton()
    {
        await Retry.For(async () => { await this.Page.ClickButtonById("newContractProductTransactionFeeButton"); });
    }

    public async Task ClickTheCreateContractButton()
    {
        await this.Page.ClickButtonById("createContractButton");
    }

    public async Task VerifyMerchantDetailsAreInTheList(List<MerchantDetails> merchantDetailsList)
    {
        await Retry.For(async () =>
        {
            Int32 foundRowCount = 0;
            var rows = await this.Page.Locator("#merchantList tr").AllAsync();

            rows.Count.ShouldBe(merchantDetailsList.Count + 1);

            foreach (var merchantDetails in merchantDetailsList)
            {
                foreach (var row in rows)
                {
                    var headers = await row.Locator("th").AllAsync();
                    if (headers.Any())
                    {
                        continue;
                    }

                    var cells = await row.Locator("td").AllAsync();
                    if (cells.Count > 0)
                    {
                        var cellText = await cells[0].TextContentAsync();
                        // The merchant name cell contains an avatar with the first letter and the full name
                        // So we check if the cell text contains the merchant name
                        if (cellText != null && cellText.Contains(merchantDetails.MerchantName))
                        {
                            cellText.ShouldContain(merchantDetails.MerchantName);
                            var settlementScheduleText = await cells[4].TextContentAsync();
                            settlementScheduleText.ShouldBe(merchantDetails.SettlementSchedule);

                            foundRowCount++;
                            break;
                        }
                    }
                }
            }

            foundRowCount.ShouldBe(merchantDetailsList.Count);
        }, TimeSpan.FromSeconds(120));
    }

    public async Task VerifyOperatorDetailsAreInTheList(String tableId, List<(String, String, String, String)> operatorDetails)
    {
        await Retry.For(async () =>
        {
            Int32 foundRowCount = 0;
            var rows = await this.Page.Locator($"#{tableId} tr").AllAsync();

            rows.Count.ShouldBe(operatorDetails.Count + 1);

            foreach ((String name, String merchantNumber, String terminalNumber, String isDeleted) in operatorDetails)
            {
                foreach (var row in rows)
                {
                    var headers = await row.Locator("th").AllAsync();
                    if (headers.Any())
                    {
                        continue;
                    }

                    var cells = await row.Locator("td").AllAsync();
                    if (cells.Count > 0)
                    {
                        var cellText = await cells[0].TextContentAsync();
                        if (cellText == name)
                        {
                            cellText.ShouldBe(name);
                            var col1Text = await cells[1].TextContentAsync();
                            col1Text.ShouldBe(merchantNumber);
                            var col2Text = await cells[2].TextContentAsync();
                            col2Text.ShouldBe(terminalNumber);
                            
                            if (!String.IsNullOrEmpty(isDeleted))
                            {
                                var col3Text = await cells[3].TextContentAsync();
                                col3Text.ShouldBe(isDeleted);
                            }

                            foundRowCount++;
                            break;
                        }
                    }
                }
            }

            foundRowCount.ShouldBe(operatorDetails.Count);
        }, TimeSpan.FromSeconds(120));
    }

    public async Task VerifyContractDetailsAreInTheList(String tableId, List<(String, String)> contractDetails)
    {
        await Retry.For(async () =>
        {
            Int32 foundRowCount = 0;
            var rows = await this.Page.Locator($"#{tableId} tr").AllAsync();

            rows.Count.ShouldBe(contractDetails.Count + 1);

            foreach ((String contractName, String isDeleted) in contractDetails)
            {
                foreach (var row in rows)
                {
                    var headers = await row.Locator("th").AllAsync();
                    if (headers.Any())
                    {
                        continue;
                    }

                    var cells = await row.Locator("td").AllAsync();
                    if (cells.Count > 0)
                    {
                        var cellText = await cells[0].TextContentAsync();
                        if (cellText == contractName)
                        {
                            cellText.ShouldBe(contractName);
                            var col1Text = await cells[1].TextContentAsync();
                            col1Text.ShouldBe(isDeleted);

                            foundRowCount++;
                            break;
                        }
                    }
                }
            }

            foundRowCount.ShouldBe(contractDetails.Count);
        }, TimeSpan.FromSeconds(120));
    }

    private async Task ClickElementInTable(String tableId, String textToSearchFor, String elementToClickId)
    {
        await Retry.For(async () =>
        {
            var rows = await this.Page.Locator($"#{tableId} tr").AllAsync();
            rows.ShouldNotBeNull();
            rows.Any().ShouldBeTrue();

            foreach (var row in rows)
            {
                var headers = await row.Locator("th").AllAsync();
                if (headers.Any())
                {
                    continue;
                }

                var cells = await row.Locator("td").AllAsync();
                if (cells.Count > 0)
                {
                    var cellText = await cells[0].TextContentAsync();
                    if (cellText == textToSearchFor)
                    {
                        var link = row.Locator($"#{elementToClickId}");
                        await link.ClickAsync();
                        return;
                    }
                }
            }

            throw new Exception($"Could not find row with text '{textToSearchFor}' in table '{tableId}'");
        });
    }
}

public record MerchantDetails
{
    public String MerchantName { get; init; }
    public String SettlementSchedule { get; init; }
    public String ContactName { get; init; }
    public String AddressLine1 { get; init; }
    public String Town { get; init; }

    public MerchantDetails(String merchantName, String settlementSchedule, String contactName, String addressLine1, String town)
    {
        this.MerchantName = merchantName;
        this.SettlementSchedule = settlementSchedule;
        this.ContactName = contactName;
        this.AddressLine1 = addressLine1;
        this.Town = town;
    }
}
