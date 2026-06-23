# Contract Screen Integration Test Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add one ReqNroll integration scenario that covers the contract list, create, view, edit, product, and fee flows end-to-end.

**Architecture:** Reuse the existing Docker-backed integration harness and the same estate-user background used by the operator scenario. Add one contract feature file, one scoped step-definition class, and a small set of helper methods on the shared `DashboardPageHelper` so the scenario reads like a single user journey while all browser logic stays centralized.

**Tech Stack:** ReqNroll, NUnit, Playwright, Docker-backed integration containers, shared integration test helpers.

---

### Task 1: Add the contract feature scenario

**Files:**
- Create: `EstateManagementUI.IntegrationTests/Features/ContractManagement.feature`

- [ ] **Step 1: Write the failing feature**

```gherkin
@base @background @estate
Feature: Contract Management
  As an authenticated estate user
  I want to move through the contract screens
  So that I can create, inspect, and edit a contract from one journey

  Scenario: Estate users can complete the full contract screen flow
    Given the user navigates to the app address
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'
    Then I should see the estate info page
    When I open the contract management screen
    Then I should see the contract management heading
    When I create a contract
    Then I should see the contract in the list
    When I view the contract
    Then I should see the contract details page
    When I edit the contract
    Then I should see the contract edit page
    When I add a product to the contract
    Then I should see the product in the contract edit view
    When I add a fee to the contract
    Then I should see the fee in the contract edit view
    When I return to the contract list
    Then I should see the contract management heading
```

- [ ] **Step 2: Run the feature to confirm the new scenario does not bind yet**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.ContractManagement`

Expected: fail because the new contract step definitions do not exist yet.

- [ ] **Step 3: Keep the scenario self-contained**

Use one generated contract name during the run so the later list/view/edit steps all target the same record without relying on pre-seeded contract data.

### Task 2: Expand the shared Playwright helper for contract screens

**Files:**
- Modify: `EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs`

- [ ] **Step 1: Add contract navigation and assertion helpers**

```csharp
public async Task OpenContractManagementScreenAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        var contractLink = _page.Locator("#contractsLink");
        if (await contractLink.CountAsync() > 0 && await contractLink.First.IsVisibleAsync())
        {
            await contractLink.First.ClickAsync(new LocatorClickOptions { NoWaitAfter = true });
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
        (await _page.GetByRole(AriaRole.Heading, new() { Name = "Contract Management" }).IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertContractManagementHeadingVisibleAsync));
}

public async Task CreateContractAsync(string contractDescription, string operatorName)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        await _page.Locator("#newContractButton").ClickAsync();
        await _page.WaitForURLAsync(new Regex(@".*/contracts/new.*", RegexOptions.IgnoreCase));
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
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }, nameof(CreateContractAsync));
}

public async Task AssertContractListContainsAsync(string contractDescription)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByText(contractDescription).IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertContractListContainsAsync));
}

public async Task OpenContractViewAsync(string contractDescription)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        await _page.GetByText(contractDescription).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }, nameof(OpenContractViewAsync));
}

public async Task AssertContractViewVisibleAsync(string contractDescription)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = $"View Contract: {contractDescription}" }).IsVisibleAsync()).ShouldBeTrue();
        (await _page.GetByRole(AriaRole.Button, new() { Name = "Back to List" }).IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertContractViewVisibleAsync));
}
```

- [ ] **Step 2: Add contract edit helpers for the product and fee flows**

```csharp
public async Task OpenContractEditAsync(string contractDescription)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        var editButton = _page.GetByRole(AriaRole.Button, new() { Name = "Edit" }).First;
        await editButton.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }, nameof(OpenContractEditAsync));
}

public async Task AssertContractEditVisibleAsync(string contractDescription)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = $"Edit Contract: {contractDescription}" }).IsVisibleAsync()).ShouldBeTrue();
        (await _page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertContractEditVisibleAsync));
}

public async Task AddProductToContractAsync(string productName, string displayText, decimal value)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
        await _page.GetByRole(AriaRole.Heading, new() { Name = "Add New Product" }).WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 10000
        });

        await _page.Locator("input[placeholder='Enter product name']").FillAsync(productName);
        await _page.Locator("input[placeholder='Enter display text']").FillAsync(displayText);
        await _page.Locator("input[placeholder='Enter value']").FillAsync(value.ToString("0.##", CultureInfo.InvariantCulture));
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Product" }).ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }, nameof(AddProductToContractAsync));
}

public async Task AssertContractProductVisibleAsync(string productName)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByText(productName).IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertContractProductVisibleAsync));
}

public async Task AddFeeToContractAsync(string feeDescription, decimal feeValue)
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Fee" }).First.ClickAsync();
        await _page.GetByRole(AriaRole.Heading, new() { Name = "Add Transaction Fee" }).WaitForAsync(new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible,
            Timeout = 10000
        });

        await _page.Locator("input[placeholder='Enter fee description']").FillAsync(feeDescription);
        await _page.Locator("select").Nth(0).SelectOptionAsync("0");
        await _page.Locator("select").Nth(1).SelectOptionAsync("0");
        await _page.Locator("input[placeholder='Enter fee value']").FillAsync(feeValue.ToString("0.##", CultureInfo.InvariantCulture));
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add Fee" }).First.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
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
        await _page.GetByRole(AriaRole.Button, new() { Name = "Back to List" }).First.ClickAsync();
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }, nameof(BackToContractListAsync));
}
```

- [ ] **Step 3: Run the existing helper-backed integration suite to catch selector mismatches early**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.OperatorManagement`

Expected: operator coverage still passes after the shared helper grows.

### Task 3: Add the contract step definitions

**Files:**
- Create: `EstateManagementUI.IntegrationTests/Steps/ContractManagementSteps.cs`

- [ ] **Step 1: Add the contract step file**

```csharp
using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class ContractManagementSteps
{
    private const string ContractDescription = "Integration Contract";
    private const string OperatorName = "Test Operator";
    private const string ProductName = "Integration Product";
    private const string ProductDisplayText = "Integration Product Display";
    private const string FeeDescription = "Integration Fee";

    private readonly IPage _page;
    private readonly TestingContext _testingContext;

    public ContractManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [When("I open the contract management screen")]
    public Task WhenIOpenTheContractManagementScreen() => GetHelper().OpenContractManagementScreenAsync();

    [Then("I should see the contract management heading")]
    public Task ThenIShouldSeeTheContractManagementHeading() => GetHelper().AssertContractManagementHeadingVisibleAsync();

    [When("I create a contract")]
    public Task WhenICreateAContract() => GetHelper().CreateContractAsync(ContractDescription, OperatorName);

    [Then("I should see the contract in the list")]
    public Task ThenIShouldSeeTheContractInTheList() => GetHelper().AssertContractListContainsAsync(ContractDescription);

    [When("I view the contract")]
    public Task WhenIViewTheContract() => GetHelper().OpenContractViewAsync(ContractDescription);

    [Then("I should see the contract details page")]
    public Task ThenIShouldSeeTheContractDetailsPage() => GetHelper().AssertContractViewVisibleAsync(ContractDescription);

    [When("I edit the contract")]
    public Task WhenIEditTheContract() => GetHelper().OpenContractEditAsync(ContractDescription);

    [Then("I should see the contract edit page")]
    public Task ThenIShouldSeeTheContractEditPage() => GetHelper().AssertContractEditVisibleAsync(ContractDescription);

    [When("I add a product to the contract")]
    public Task WhenIAddAProductToTheContract() => GetHelper().AddProductToContractAsync(ProductName, ProductDisplayText, 10m);

    [Then("I should see the product in the contract edit view")]
    public Task ThenIShouldSeeTheProductInTheContractEditView() => GetHelper().AssertContractProductVisibleAsync(ProductName);

    [When("I add a fee to the contract")]
    public Task WhenIAddAFeeToTheContract() => GetHelper().AddFeeToContractAsync(FeeDescription, 1.50m);

    [Then("I should see the fee in the contract edit view")]
    public Task ThenIShouldSeeTheFeeInTheContractEditView() => GetHelper().AssertContractFeeVisibleAsync(FeeDescription);

    [When("I return to the contract list")]
    public Task WhenIReturnToTheContractList() => GetHelper().BackToContractListAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
```

- [ ] **Step 2: Run the contract feature again**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.ContractManagement`

Expected: the scenario binds and reaches the browser interactions.

- [ ] **Step 3: Tighten selectors only if the browser assertions show the UI uses a different accessible target**

Prefer changing the helper to match the current UI instead of broadening the Gherkin or duplicating navigation logic in the step file.

### Task 4: Verify the full contract flow and commit it

**Files:**
- Modify: `EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs`
- Create: `EstateManagementUI.IntegrationTests/Steps/ContractManagementSteps.cs`
- Create: `EstateManagementUI.IntegrationTests/Features/ContractManagement.feature`

- [ ] **Step 1: Run the single contract integration scenario**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.ContractManagement`

Expected: one passing scenario that creates a contract, opens the view page, opens the edit page, adds a product, adds a fee, and returns to the list.

- [ ] **Step 2: Inspect traces and screenshots if the scenario fails**

Use the existing `TestResults/Diagnostics`, `TestResults/Screenshots`, and `TestResults/Traces` outputs generated by `BrowserHooks.cs` to identify whether the failure is a selector issue, a timing issue, or a backend data issue.

- [ ] **Step 3: Commit the integration test changes**

```bash
git add EstateManagementUI.IntegrationTests/Features/ContractManagement.feature EstateManagementUI.IntegrationTests/Steps/ContractManagementSteps.cs EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs docs/superpowers/plans/2026-06-22-contract-screen-integration-test.md
git commit -m "test: add contract screen integration scenario"
```
