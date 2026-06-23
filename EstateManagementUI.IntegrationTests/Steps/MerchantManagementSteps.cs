using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class MerchantManagementSteps
{
    private const string OperatorName = "Test Operator";
    private const string OperatorMerchantNumber = "12345678";
    private const string OperatorTerminalNumber = "87654321";
    private const string DepositReference = "DEP-001";
    private const decimal DepositAmount = 100m;

    private readonly IPage _page;
    private readonly TestingContext _testingContext;
    private readonly string _merchantName = $"Integration Merchant {Guid.NewGuid():N}";
    private readonly string _deviceIdentifier = $"DEVICE-{Guid.NewGuid():N}";

    public MerchantManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [When("I open the merchant management screen")]
    public Task WhenIOpenTheMerchantManagementScreen() => GetHelper().OpenMerchantManagementScreenAsync();

    [Then("I should see the merchant management heading")]
    public Task ThenIShouldSeeTheMerchantManagementHeading() => GetHelper().AssertMerchantManagementHeadingVisibleAsync();

    [When("I create a merchant")]
    public Task WhenICreateAMerchant() => GetHelper().CreateMerchantAsync(_merchantName);

    [Then("I should see the merchant in the list")]
    public Task ThenIShouldSeeTheMerchantInTheList() => GetHelper().AssertMerchantListContainsAsync(_merchantName);

    [When("I view the merchant")]
    public Task WhenIViewTheMerchant() => GetHelper().OpenMerchantViewAsync(_merchantName);

    [Then("I should see the merchant view page")]
    public Task ThenIShouldSeeTheMerchantViewPage() => GetHelper().AssertMerchantViewVisibleAsync(_merchantName);

    [When("I switch to the address tab")]
    public Task WhenISwitchToTheAddressTab() => GetHelper().SwitchMerchantTabAsync("Address Details");

    [Then("I should see the merchant address details")]
    public Task ThenIShouldSeeTheMerchantAddressDetails() => GetHelper().AssertMerchantPageTextVisibleAsync("Address Line 1");

    [When("I switch to the contact tab")]
    public Task WhenISwitchToTheContactTab() => GetHelper().SwitchMerchantTabAsync("Contact Details");

    [Then("I should see the merchant contact details")]
    public Task ThenIShouldSeeTheMerchantContactDetails() => GetHelper().AssertMerchantPageTextVisibleAsync("Contact Name");

    [When("I switch to the opening hours tab")]
    public Task WhenISwitchToTheOpeningHoursTab() => GetHelper().SwitchMerchantTabAsync("Opening Hours");

    [Then("I should see the merchant opening hours details")]
    public Task ThenIShouldSeeTheMerchantOpeningHoursDetails() => GetHelper().AssertMerchantPageTextVisibleAsync("Monday");

    [When("I switch to the merchant operators tab")]
    public Task WhenISwitchToTheOperatorsTab() => GetHelper().SwitchMerchantTabAsync("Assigned Operators");

    [Then("I should see no operators assigned")]
    public Task ThenIShouldSeeNoOperatorsAssigned() => GetHelper().AssertMerchantPageTextVisibleAsync("No operators assigned");

    [When("I switch to the contracts tab")]
    public Task WhenISwitchToTheContractsTab() => GetHelper().SwitchMerchantTabAsync("Assigned Contracts");

    [Then("I should see no contracts assigned")]
    public Task ThenIShouldSeeNoContractsAssigned() => GetHelper().AssertMerchantContractsTabVisibleAsync();

    [When("I switch to the devices tab")]
    public Task WhenISwitchToTheDevicesTab() => GetHelper().SwitchMerchantTabAsync("Assigned Devices");

    [Then("I should see no devices assigned")]
    public Task ThenIShouldSeeNoDevicesAssigned() => GetHelper().AssertMerchantPageTextVisibleAsync("No devices assigned");

    [When("I open the merchant schedule from the view page")]
    public Task WhenIOpenTheMerchantScheduleFromTheViewPage() => GetHelper().OpenMerchantScheduleFromViewAsync();

    [Then("I should see the read-only merchant schedule page")]
    public Task ThenIShouldSeeTheReadOnlyMerchantSchedulePage() => GetHelper().AssertMerchantReadOnlyScheduleVisibleAsync();

    [When("I return to the merchant view page")]
    public Task WhenIReturnToTheMerchantViewPage() => GetHelper().BackToMerchantFromViewScheduleAsync();

    [When("I open the merchant edit page")]
    public Task WhenIOpenTheMerchantEditPage() => GetHelper().OpenMerchantEditAsync(_merchantName);

    [Then("I should see the merchant edit page")]
    public Task ThenIShouldSeeTheMerchantEditPage() => GetHelper().AssertMerchantEditVisibleAsync(_merchantName);

    [When("I switch to the opening hours editor")]
    public Task WhenISwitchToTheOpeningHoursEditor() => GetHelper().SwitchMerchantTabAsync("Opening Hours");

    [Then("I should see the merchant opening hours editor")]
    public Task ThenIShouldSeeTheMerchantOpeningHoursEditor() => GetHelper().AssertMerchantEditOpeningHoursVisibleAsync();

    [When("I save merchant opening hours")]
    public Task WhenISaveMerchantOpeningHours() => GetHelper().SaveMerchantOpeningHoursAsync();

    [Then("I should see merchant opening hours updated successfully")]
    public Task ThenIShouldSeeMerchantOpeningHoursUpdatedSuccessfully() => GetHelper().AssertMerchantPageTextVisibleAsync("Merchant opening hours updated successfully");

    [When("I switch to the merchant operators editor")]
    public Task WhenISwitchToTheOperatorsEditor() => GetHelper().SwitchMerchantTabAsync("Assigned Operators");

    [When("I add the operator to the merchant")]
    public Task WhenIAddTheOperatorToTheMerchant() => GetHelper().AddMerchantOperatorAsync(OperatorName, OperatorMerchantNumber, OperatorTerminalNumber);

    [Then("I should see the operator in the merchant list")]
    public Task ThenIShouldSeeTheOperatorInTheMerchantList() => GetHelper().AssertMerchantOperatorVisibleAsync(OperatorName);

    [When("I switch to the contracts editor")]
    public Task WhenISwitchToTheContractsEditor() => GetHelper().SwitchMerchantTabAsync("Assigned Contracts");

    [When("I switch to the devices editor")]
    public Task WhenISwitchToTheDevicesEditor() => GetHelper().SwitchMerchantTabAsync("Assigned Devices");

    [When("I add the device to the merchant")]
    public Task WhenIAddTheDeviceToTheMerchant() => GetHelper().AddMerchantDeviceAsync(_deviceIdentifier);

    [Then("I should see the device in the merchant list")]
    public Task ThenIShouldSeeTheDeviceInTheMerchantList() => GetHelper().AssertMerchantDeviceVisibleAsync(_deviceIdentifier);

    [When("I open the merchant schedule from the edit page")]
    public Task WhenIOpenTheMerchantScheduleFromTheEditPage() => GetHelper().OpenMerchantScheduleFromEditAsync();

    [Then("I should see the editable merchant schedule page")]
    public Task ThenIShouldSeeTheEditableMerchantSchedulePage() => GetHelper().AssertMerchantEditableScheduleVisibleAsync();

    [When("I save the merchant schedule")]
    public Task WhenISaveTheMerchantSchedule() => GetHelper().SaveMerchantScheduleAsync();

    [Then("I should see schedule saved successfully")]
    public Task ThenIShouldSeeScheduleSavedSuccessfully() => GetHelper().AssertMerchantPageTextVisibleAsync("Schedule saved for");

    [When("I return to the merchant edit page")]
    public Task WhenIReturnToTheMerchantEditPage() => GetHelper().BackToMerchantFromEditScheduleAsync();

    [When("I open the merchant deposit page")]
    public Task WhenIOpenTheMerchantDepositPage() => GetHelper().OpenMerchantDepositAsync();

    [Then("I should see the merchant deposit page")]
    public Task ThenIShouldSeeTheMerchantDepositPage() => GetHelper().AssertMerchantDepositVisibleAsync(_merchantName);

    [When("I submit the merchant deposit")]
    public Task WhenISubmitTheMerchantDeposit() => GetHelper().SubmitMerchantDepositAsync(DepositAmount, DepositReference);

    [Then("I should be back on the merchant list")]
    public Task ThenIShouldBeBackOnTheMerchantList() => GetHelper().AssertMerchantListContainsAsync(_merchantName);

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
