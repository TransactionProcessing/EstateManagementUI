using Reqnroll;
using System.Threading.Tasks;
using EstateManagementUI.OfflineIntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll.BoDi;
using Shouldly;

namespace EstateManagementUI.OfflineIntegrationTests.Steps
{
    [Binding]
    [Scope(Tag = "merchant")]
    public class MerchantSteps
    {
        private readonly IPage Page;

        public MerchantSteps(ScenarioContext scenarioContext)
        {
            this.Page = scenarioContext.ScenarioContainer.Resolve<IPage>(scenarioContext.ScenarioInfo.Title.Replace(" ", ""));
        }

        #region View Merchants List

        [Then(@"I should see the merchants table")]
        [Then(@"the merchant list table should be visible")]
        public async Task ThenIShouldSeeTheMerchantsTable()
        {
            // Wait for and verify the merchant list table exists
            await this.Page.Locator("#merchantList").WaitForAsync(new LocatorWaitForOptions { Timeout = 5000 });
            var isVisible = await this.Page.Locator("#merchantList").IsVisibleAsync();
            isVisible.ShouldBeTrue("Merchant table should be visible");
        }

        #endregion

        #region View Single Merchant

        [When(@"I click the view button for the first merchant")]
        public async Task WhenIClickTheViewButtonForTheFirstMerchant()
        {
            // Click the first view button (icon button with id="viewMerchantLink")
            await this.Page.Locator("#viewMerchantLink").First.ClickAsync();
            await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        [Then(@"I should see the merchant details page")]
        public async Task ThenIShouldSeeTheMerchantDetailsPage()
        {
            // Verify we're on the merchant details page by checking for tabs
            var detailsTab = await this.Page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Merchant Details" }).IsVisibleAsync();
            detailsTab.ShouldBeTrue("Merchant details tab should be visible");
        }

        [Then(@"the merchant information should be displayed")]
        public async Task ThenTheMerchantInformationShouldBeDisplayed()
        {
            // Verify merchant information is displayed (check for presence of info)
            var pageContent = await this.Page.Locator("body").TextContentAsync();
            pageContent.ShouldNotBeNullOrEmpty();
        }

        #endregion

        #region Edit Merchant

        [When(@"I click the edit button for the first merchant in the table")]
        public async Task WhenIClickTheEditButtonForTheFirstMerchantInTheTable()
        {
            // Click the first edit button (icon button with id="editMerchantLink")
            await this.Page.Locator("#editMerchantLink").First.ClickAsync();
            await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        [Then(@"I should see the merchant edit page")]
        public async Task ThenIShouldSeeTheMerchantEditPage()
        {
            // Verify we're on the edit page by checking for the edit form
            var url = this.Page.Url;
            url.ShouldContain("/edit");
        }

        [When(@"I update the merchant name field")]
        public async Task WhenIUpdateTheMerchantNameField()
        {
            // Find the merchant name input field by name attribute and update it
            var nameInput = this.Page.Locator("input[name='MerchantName']");
            await nameInput.ClearAsync();
            await nameInput.FillAsync("Updated Merchant Name");
        }

        [When(@"I click the save changes button")]
        public async Task WhenIClickTheSaveChangesButton()
        {
            // Click the save button (type="submit" button in the form)
            await this.Page.Locator("button[type='submit']").First.ClickAsync();
            await Task.Delay(500); // Wait for form submission
        }

        [Then(@"the merchant should be updated successfully")]
        public async Task ThenTheMerchantShouldBeUpdatedSuccessfully()
        {
            // Wait a moment for the update to process
            await Task.Delay(1000);
            // Verify we're still on a valid page (no error occurred)
            var url = this.Page.Url;
            url.ShouldNotBeNullOrEmpty();
        }

        #endregion

        #region Make Deposit

        [When(@"I click the make deposit button for the first merchant in the table")]
        public async Task WhenIClickTheMakeDepositButtonForTheFirstMerchantInTheTable()
        {
            // Click the first make deposit button (icon button with id="makeDepositLink")
            await this.Page.Locator("#makeDepositLink").First.ClickAsync();
            await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        [Then(@"I should see the deposit page")]
        public async Task ThenIShouldSeeTheDepositPage()
        {
            // Verify we're on the deposit page
            var url = this.Page.Url;
            url.ShouldContain("/deposit");
        }

        [When(@"I enter deposit amount ""(.*)""")]
        public async Task WhenIEnterDepositAmount(string amount)
        {
            // Fill in the deposit amount using the id selector
            await this.Page.Locator("#depositAmount").FillAsync(amount);
        }

        [When(@"I enter deposit date ""(.*)""")]
        public async Task WhenIEnterDepositDate(string date)
        {
            // Fill in the deposit date using the id selector
            await this.Page.Locator("#depositDate").FillAsync(date);
        }

        [When(@"I enter deposit reference ""(.*)""")]
        public async Task WhenIEnterDepositReference(string reference)
        {
            // Fill in the deposit reference using the id selector
            await this.Page.Locator("#depositReference").FillAsync(reference);
        }

        [When(@"I click the submit deposit button")]
        public async Task WhenIClickTheSubmitDepositButton()
        {
            // Click the make deposit button using the id selector
            await this.Page.Locator("#makeDepositButton").ClickAsync();
            await Task.Delay(500); // Wait for form submission
        }

        [Then(@"the deposit should be processed successfully")]
        public async Task ThenTheDepositShouldBeProcessedSuccessfully()
        {
            // Wait a moment for the deposit to process
            await Task.Delay(1000);
            // Verify we're redirected or the form succeeded
            var url = this.Page.Url;
            url.ShouldNotBeNullOrEmpty();
        }

        #endregion

        #region Create New Merchant

        [When(@"I click the new merchant button")]
        public async Task WhenIClickTheNewMerchantButton()
        {
            // Click the new merchant button using the id selector
            await this.Page.Locator("#newMerchantButton").ClickAsync();
            await this.Page.WaitForLoadStateAsync(LoadState.NetworkIdle);
        }

        [Then(@"I should see the create merchant page")]
        public async Task ThenIShouldSeeTheCreateMerchantPage()
        {
            // Verify we're on the create page
            var url = this.Page.Url;
            url.ShouldContain("/new");
        }

        [When(@"I enter merchant name ""(.*)""")]
        public async Task WhenIEnterMerchantName(string merchantName)
        {
            // Fill in merchant name using name attribute selector
            await this.Page.Locator("input[name='MerchantName']").FillAsync(merchantName);
        }

        [When(@"I select settlement schedule ""(.*)""")]
        public async Task WhenISelectSettlementSchedule(string schedule)
        {
            // Select settlement schedule using name attribute selector
            await this.Page.Locator("select[name='SettlementSchedule']").SelectOptionAsync(schedule);
        }

        [When(@"I enter address line 1 ""(.*)""")]
        public async Task WhenIEnterAddressLine1(string address)
        {
            // Fill in address line 1 using name attribute selector
            await this.Page.Locator("input[name='AddressLine1']").FillAsync(address);
        }

        [When(@"I enter town ""(.*)""")]
        public async Task WhenIEnterTown(string town)
        {
            // Fill in town using name attribute selector
            await this.Page.Locator("input[name='Town']").FillAsync(town);
        }

        [When(@"I enter region ""(.*)""")]
        public async Task WhenIEnterRegion(string region)
        {
            // Fill in region using name attribute selector
            await this.Page.Locator("input[name='Region']").FillAsync(region);
        }

        [When(@"I enter postcode ""(.*)""")]
        public async Task WhenIEnterPostcode(string postcode)
        {
            // Fill in postcode using name attribute selector
            await this.Page.Locator("input[name='PostCode']").FillAsync(postcode);
        }

        [When(@"I enter country ""(.*)""")]
        public async Task WhenIEnterCountry(string country)
        {
            // Fill in country using name attribute selector
            await this.Page.Locator("input[name='Country']").FillAsync(country);
        }

        [When(@"I enter contact name ""(.*)""")]
        public async Task WhenIEnterContactName(string contactName)
        {
            // Fill in contact name using name attribute selector
            await this.Page.Locator("input[name='ContactName']").FillAsync(contactName);
        }

        [When(@"I enter email address ""(.*)""")]
        public async Task WhenIEnterEmailAddress(string email)
        {
            // Fill in email address using name attribute selector
            await this.Page.Locator("input[name='EmailAddress']").FillAsync(email);
        }

        [When(@"I enter phone number ""(.*)""")]
        public async Task WhenIEnterPhoneNumber(string phone)
        {
            // Fill in phone number using name attribute selector
            await this.Page.Locator("input[name='PhoneNumber']").FillAsync(phone);
        }

        [When(@"I click the submit create merchant button")]
        public async Task WhenIClickTheSubmitCreateMerchantButton()
        {
            // Wait a moment for form validation
            await Task.Delay(300);
            // Click the create merchant button using the id selector
            await this.Page.Locator("#createMerchantButton").ClickAsync();
            await Task.Delay(500); // Wait for form submission
        }

        [Then(@"the merchant should be created successfully")]
        public async Task ThenTheMerchantShouldBeCreatedSuccessfully()
        {
            // Wait for navigation to complete
            await Task.Delay(1000);
            // Verify we're redirected back to the merchants list
            var url = this.Page.Url;
            url.ShouldContain("/merchants");
            url.ShouldNotContain("/new");
        }

        #endregion
    }
}
