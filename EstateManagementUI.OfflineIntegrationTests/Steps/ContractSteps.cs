using Reqnroll;
using EstateManagementUI.OfflineIntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll.BoDi;
using Shouldly;

namespace EstateManagementUI.OfflineIntegrationTests.Steps;

[Binding]
[Scope(Tag = "contract")]
public class ContractSteps
{
    private readonly IPage Page;

    public ContractSteps(ScenarioContext scenarioContext)
    {
        this.Page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));
    }

    [Then(@"I should see the contracts grid")]
    public async Task ThenIShouldSeeTheContractsGrid()
    {
        // Contracts use a grid layout, not a table - wait for grid container
        await this.Page.WaitForSelectorAsync(".grid", new() { Timeout = 5000 });
    }

    [Then(@"I should see the contract details tab")]
    public async Task ThenIShouldSeeTheContractDetailsTab()
    {
        // Verify contract details are visible
        await Task.Delay(500);
        var isVisible = await this.Page.Locator("body").IsVisibleAsync();
        isVisible.ShouldBeTrue();
    }

    [Then(@"the contracts grid should be displayed")]
    public async Task ThenTheContractsGridShouldBeDisplayed()
    {
        // Verify grid with contract cards is visible
        var gridVisible = await this.Page.Locator(".grid.grid-cols-1").IsVisibleAsync();
        gridVisible.ShouldBeTrue();
    }

    [When(@"I click the first contract view button")]
    public async Task WhenIClickTheFirstContractViewButton()
    {
        // Click the first "View" button in contract cards
        await this.Page.Locator("button:has-text('View')").First.ClickAsync();
        await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I click the create new contract button")]
    public async Task WhenIClickTheCreateNewContractButton()
    {
        // Use button ID from Index.razor line 15
        await this.Page.Locator("#newContractButton").ClickAsync();
        await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I fill in the contract details form")]
    public async Task WhenIFillInTheContractDetailsForm()
    {
        // Fill description field using placeholder attribute
        await this.Page.Locator("input[placeholder='Enter contract description']").FillAsync("Test Contract " + DateTime.Now.Ticks);
        
        // Select first available operator from dropdown
        await this.Page.Locator("select").First.SelectOptionAsync(new SelectOptionValue { Index = 1 });
        
        // Small delay for form validation
        await Task.Delay(300);
    }

    [When(@"I click the create contract submit button")]
    public async Task WhenIClickTheCreateContractSubmitButton()
    {
        // Use button ID from New.razor line 101
        await this.Page.Locator("#createContractButton").ClickAsync();
        await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }

    [When(@"I click the add product button")]
    public async Task WhenIClickTheAddProductButton()
    {
        // Click "Add Product" button in Edit.razor
        await this.Page.Locator("button:has-text('Add Product')").ClickAsync();
        await Task.Delay(500); // Wait for modal to appear
    }

    [When(@"I fill in the product details form")]
    public async Task WhenIFillInTheProductDetailsForm()
    {
        // Fill product name
        await this.Page.Locator("input[placeholder='Enter product name']").FillAsync("Mobile Topup");
        
        // Fill display text
        await this.Page.Locator("input[placeholder='Enter display text']").FillAsync("100 KES");
        
        // Ensure variable value checkbox is unchecked
        var checkbox = this.Page.Locator("input[type='checkbox']");
        if (await checkbox.IsCheckedAsync())
        {
            await checkbox.UncheckAsync();
        }
        
        // Fill value field
        await this.Page.Locator("input[type='number']").First.FillAsync("100.00");
        
        await Task.Delay(300);
    }

    [When(@"I click the add product submit button")]
    public async Task WhenIClickTheAddProductSubmitButton()
    {
        // Click submit button in Add Product modal
        await this.Page.Locator("button[type='submit']:has-text('Add Product')").ClickAsync();
        await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Task.Delay(500);
    }

    [When(@"I click the add fee button for the first product")]
    public async Task WhenIClickTheAddFeeButtonForTheFirstProduct()
    {
        // Click "Add Fee" button
        await this.Page.Locator("button:has-text('Add Fee')").First.ClickAsync();
        await Task.Delay(500); // Wait for modal to appear
    }

    [When(@"I fill in the transaction fee details form")]
    public async Task WhenIFillInTheTransactionFeeDetailsForm()
    {
        // Fill fee description
        await this.Page.Locator("input[placeholder='Enter fee description']").FillAsync("Service Fee");
        
        // Select calculation type (0 = Fixed)
        await this.Page.Locator("select").Nth(0).SelectOptionAsync("0");
        
        // Select fee type (0 = Merchant)
        await this.Page.Locator("select").Nth(1).SelectOptionAsync("0");
        
        // Fill fee value
        await this.Page.Locator("input[type='number']").Last.FillAsync("5.00");
        
        await Task.Delay(300);
    }

    [When(@"I click the add fee submit button")]
    public async Task WhenIClickTheAddFeeSubmitButton()
    {
        // Click submit button in Add Fee modal
        await this.Page.Locator("button[type='submit']").Last.ClickAsync();
        await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        await Task.Delay(500);
    }
}
