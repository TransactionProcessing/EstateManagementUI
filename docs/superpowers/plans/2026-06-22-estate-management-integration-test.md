# Estate Management Integration Test Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add one ReqNroll integration scenario that covers the estate management flow end-to-end, including entry/info navigation, estate index rendering, and operator add/remove actions.

**Architecture:** Reuse the existing Docker-backed integration test harness, background seeding, and Playwright helpers already used by the dashboard feature. Add one estate-specific feature file and a small helper surface so the scenario reads as a single user journey while keeping navigation and operator assertions centralized in page helper methods.

**Tech Stack:** ReqNroll, NUnit, Playwright, Docker-backed integration containers, shared integration test helpers.

---

### Task 1: Add an estate management feature scenario

**Files:**
- Create: `EstateManagementUI.IntegrationTests/Features/EstateManagement.feature`

- [ ] **Step 1: Write the failing feature**

```gherkin
@base @background @estate
Feature: Estate Management
  As an authenticated estate user
  I want to move through the estate management screens
  So that I can manage estate operators from one end-to-end journey

  Background:
    Given I create the following roles
      | Role Name     |
      | Administrator |
      | Estate        |

    Given I create the following api scopes
      | Name                 | DisplayName                      | Description                          |
      | transactionProcessor | Transaction Processor REST Scope | Scope for Transaction Processor REST |
      | fileProcessor        | File Processor REST Scope        | Scope for File Processor REST        |
      | estateReporting      | Estate Reporting REST Scope      | Scope for Estate Reporting REST      |

    Given I create the following api resources
      | Name                 | DisplayName                | Secret  | Scopes               | UserClaims               |
      | transactionProcessor | Transaction Processor REST | Secret1 | transactionProcessor | merchantId,estateId,role |
      | fileProcessor        | File Processor REST        | Secret1 | fileProcessor        | merchantId,estateId,role |
      | estateReporting      | Estate Reporting REST      | Secret1 | estateReporting      | merchantId,estateId,role |

    Given I create the following identity resources
      | Name    | DisplayName          | Description                                                 | UserClaims                                                             |
      | openid  | Your user identifier |                                                             | sub                                                                    |
      | profile | User profile         | Your user profile information (first name, last name, etc.) | name,role,email,given_name,middle_name,family_name,estateId,merchantId |
      | email   | Email                | Email and Email Verified Flags                              | email_verified,email                                                   |

    Given I create the following clients
      | ClientId       | Name            | Secret  | Scopes                                                                  | GrantTypes         | RedirectUris                         | PostLogoutRedirectUris                | RequireConsent | AllowOfflineAccess | ClientUri            |
      | serviceClient  | Service Client  | Secret1 | transactionProcessor,fileProcessor,estateReporting                      | client_credentials |                                      |                                       |                |                    |                      |
      | estateUIClient | Merchant Client | Secret1 | fileProcessor,transactionProcessor,estateReporting,openid,email,profile | hybrid             | https://127.0.0.1:[port]/signin-oidc | https://127.0.0.1:[port]/signout-oidc | false          | true               | https://127.0.0.1:[port] |

    Given I create the following users
      | Email Address             | Phone Number | Given Name | Middle Name | Family Name | Claims | Roles         | Password |
      | administrator@admin.co.uk |    123456789 | Test       |             | User 1      |        | Administrator | 123456   |

    Given I have a token to access the transaction Processor resource
      | ClientId      |
      | serviceClient |

    Given I have created the following estates
      | EstateName  |
      | Test Estate |

    And I have created the following operators
      | EstateName  | OperatorName  | RequireCustomMerchantNumber | RequireCustomTerminalNumber |
      | Test Estate | Test Operator | True                        | True                        |

    And I have assigned the following operators to the estates
      | EstateName  | OperatorName  |
      | Test Estate | Test Operator |

    And I have created the following security users
      | EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
      | estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

  Scenario: Estate users can navigate the estate screens and manage operators
    Given the user navigates to the app address
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'
    Then I should see the estate info page
    When I open the estate management screen
    Then I should see the estate management heading
    And I should see the estate overview summary
    When I switch to the operators tab
    Then I should see the assigned operators section
    When I add an operator to the estate
    Then I should see the operator added confirmation
    When I remove an operator from the estate
    Then I should see the operator removed confirmation
```

- [ ] **Step 2: Run the feature generation path and confirm it fails**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.EstateManagement`

Expected: fail with missing step definitions for the new estate feature.

- [ ] **Step 3: Verify the feature is scoped cleanly**

The file should remain a single scenario and reuse the existing `@base @background` setup so it stays aligned with the docker-backed estate bootstrap used by the dashboard feature.

### Task 2: Add estate-specific Playwright helper methods

**Files:**
- Modify: `EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs`

- [ ] **Step 1: Add the failing helper calls**

```csharp
public async Task AssertEstateInfoPageVisibleAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = "Estate Management" }).IsVisibleAsync()).ShouldBeTrue();
        (await _page.GetByText("Comprehensive estate management and configuration").IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertEstateInfoPageVisibleAsync));
}

public async Task OpenEstateManagementScreenAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        await _page.GotoAsync(ResolveEstateManagementBaseUrl() + "/estate");
        await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
    }, nameof(OpenEstateManagementScreenAsync));
}

public async Task AssertEstateManagementHeadingVisibleAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByRole(AriaRole.Heading, new() { Name = "Estate Management" }).IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertEstateManagementHeadingVisibleAsync));
}

public async Task AssertEstateOverviewVisibleAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
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

public async Task AddOperatorToEstateAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        await _page.Locator("#addOperatorButton").ClickAsync();
        await _page.Locator("select").SelectOptionAsync(new[] { new SelectOptionValue { Index = 1 } });
        await _page.GetByRole(AriaRole.Button, new() { Name = "Add" }).ClickAsync();
    }, nameof(AddOperatorToEstateAsync));
}

public async Task RemoveOperatorFromEstateAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        await _page.GetByRole(AriaRole.Button, new() { Name = "Remove" }).ClickAsync();
    }, nameof(RemoveOperatorFromEstateAsync));
}

public async Task AssertOperatorAddedConfirmationVisibleAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByText("Operator added successfully").IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertOperatorAddedConfirmationVisibleAsync));
}

public async Task AssertOperatorRemovedConfirmationVisibleAsync()
{
    await RunWithFailureArtifactsAsync(async () =>
    {
        (await _page.GetByText("Operator removed successfully").IsVisibleAsync()).ShouldBeTrue();
    }, nameof(AssertOperatorRemovedConfirmationVisibleAsync));
}

private string ResolveEstateManagementBaseUrl()
{
    var hostPort = this.TestingContext.DockerHelper.GetHostPort(ContainerType.EstateManagementUI);
    return $"https://127.0.0.1:{hostPort}";
}
```

- [ ] **Step 2: Run the new helper methods through the feature and confirm the missing-step failure changes to selector-level failures if needed**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.EstateManagement`

Expected: step binding errors should disappear once the step file is added in Task 3.

### Task 3: Add estate step definitions

**Files:**
- Create: `EstateManagementUI.IntegrationTests/Steps/EstateManagementSteps.cs`

- [ ] **Step 1: Add the step definition file**

```csharp
using EstateManagementUI.IntegrationTests.Common;
using Microsoft.Playwright;
using Reqnroll;

namespace EstateManagementUI.IntegrationTests.Steps;

[Binding]
[Scope(Tag = "estate")]
public sealed class EstateManagementSteps
{
    private readonly IPage _page;
    private readonly TestingContext _testingContext;

    public EstateManagementSteps(IPage page, TestingContext testingContext)
    {
        _page = page;
        _testingContext = testingContext;
    }

    [Then("I should see the estate info page")]
    public Task ThenIShouldSeeTheEstateInfoPage() => GetHelper().AssertEstateInfoPageVisibleAsync();

    [When("I open the estate management screen")]
    public Task WhenIOpenTheEstateManagementScreen() => GetHelper().OpenEstateManagementScreenAsync();

    [Then("I should see the estate management heading")]
    public Task ThenIShouldSeeTheEstateManagementHeading() => GetHelper().AssertEstateManagementHeadingVisibleAsync();

    [Then("I should see the estate overview summary")]
    public Task ThenIShouldSeeTheEstateOverviewSummary() => GetHelper().AssertEstateOverviewVisibleAsync();

    [When("I switch to the operators tab")]
    public Task WhenISwitchToTheOperatorsTab() => GetHelper().SwitchToOperatorsTabAsync();

    [Then("I should see the assigned operators section")]
    public Task ThenIShouldSeeTheAssignedOperatorsSection() => GetHelper().AssertAssignedOperatorsSectionVisibleAsync();

    [When("I add an operator to the estate")]
    public Task WhenIAddAnOperatorToTheEstate() => GetHelper().AddOperatorToEstateAsync();

    [Then("I should see the operator added confirmation")]
    public Task ThenIShouldSeeTheOperatorAddedConfirmation() => GetHelper().AssertOperatorAddedConfirmationVisibleAsync();

    [When("I remove an operator from the estate")]
    public Task WhenIRemoveAnOperatorFromTheEstate() => GetHelper().RemoveOperatorFromEstateAsync();

    [Then("I should see the operator removed confirmation")]
    public Task ThenIShouldSeeTheOperatorRemovedConfirmation() => GetHelper().AssertOperatorRemovedConfirmationVisibleAsync();

    private DashboardPageHelper GetHelper() => new(_page, _testingContext);
}
```

- [ ] **Step 2: Run the estate feature again**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.EstateManagement`

Expected: scenario now binds and moves into browser assertions.

- [ ] **Step 3: Adjust selectors if the estate page uses different accessible names**

If the browser assertions fail, update the helper selectors to match the actual page text and button labels instead of broadening the test surface.

### Task 4: Make the Docker background data reusable for the estate scenario

**Files:**
- Modify: `EstateManagementUI.IntegrationTests/Steps/BackgroundSteps.cs`
- Modify: `EstateManagementUI.IntegrationTests/Common/DockerHelper.cs` only if the estate scenario needs an additional seeded invariant that is not already covered

- [ ] **Step 1: Reuse the existing background seeding**

Keep the current `I have created the following estates`, `I have created the following operators`, and `I have assigned the following operators to the estates` steps, because they already create the estate and operator data the new scenario needs.

- [ ] **Step 2: Verify the login user is enough for the estate flow**

The seeded `estateuser@testestate1.co.uk` account should be used for the scenario because it already exists in the current dashboard background and is tied to `Test Estate`.

- [ ] **Step 3: Only add Docker helper changes if the estate screen requires a new seed invariant**

If operator add/remove requires an extra permission or read-model refresh, add the smallest targeted helper change in `DockerHelper.cs` rather than expanding the background broadly.

### Task 5: Verify the full estate scenario and keep the output clean

**Files:**
- Modify: none unless a selector or seed fix is required

- [ ] **Step 1: Run the single estate integration scenario**

Run:
`dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.EstateManagement`

Expected: pass with one scenario executed successfully.

- [ ] **Step 2: Inspect trace/screenshot artifacts if the scenario fails**

Use the existing `TestResults/Diagnostics`, `TestResults/Screenshots`, and `TestResults/Traces` outputs generated by `BrowserHooks.cs` to identify any selector or timing issue.

- [ ] **Step 3: Commit the integration test changes**

```bash
git add EstateManagementUI.IntegrationTests/Features/EstateManagement.feature EstateManagementUI.IntegrationTests/Steps/EstateManagementSteps.cs EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs docs/superpowers/plans/2026-06-22-estate-management-integration-test.md
git commit -m "test: add estate management integration scenario"
```
