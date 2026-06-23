# Merchant Management Reqnroll Tests Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a single end-to-end Reqnroll scenario that walks through the merchant UI from list to create, view, edit, schedule, and deposit pages.

**Architecture:** Extend the shared Playwright helper with merchant-specific navigation and assertion methods so the feature file stays readable. Keep the scenario focused on route transitions and stable UI markers rather than duplicating the full page-test matrix.

**Tech Stack:** Reqnroll, Playwright, NUnit, C#, Blazor Server UI

---

### Task 1: Add merchant journey helpers to the shared integration helper

**Files:**
- Modify: `EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs`

- [ ] **Step 1: Add merchant navigation and assertion methods**

Add methods that match the existing contract/operator helper style:

```csharp
public async Task OpenMerchantManagementScreenAsync()
public async Task AssertMerchantManagementHeadingVisibleAsync()
public async Task CreateMerchantAsync(string merchantName)
public async Task AssertMerchantListContainsAsync(string merchantName)
public async Task OpenMerchantViewAsync(string merchantName)
public async Task AssertMerchantViewVisibleAsync(string merchantName)
public async Task OpenMerchantEditAsync(string merchantName)
public async Task AssertMerchantEditVisibleAsync(string merchantName)
public async Task OpenMerchantScheduleFromViewAsync()
public async Task AssertMerchantScheduleVisibleAsync()
public async Task OpenMerchantScheduleFromEditAsync()
public async Task OpenMerchantDepositAsync(string merchantName)
public async Task AssertMerchantDepositVisibleAsync(string merchantName)
public async Task AddMerchantOpeningHoursAsync()
public async Task SubmitMerchantDepositAsync(decimal amount, string reference)
public async Task BackToMerchantListAsync()
```

Use the existing merchant route IDs and labels already present in the Blazor pages:

```text
#createMerchantButton
#viewMerchantLink
#editMerchantLink
#viewScheduleButton
#editScheduleButton
#makeDepositLink
```

- [ ] **Step 2: Reuse the same failure-artifact wrapper and selectors**

Keep the helper methods wrapped in `RunWithFailureArtifactsAsync(...)`, and follow the current pattern of waiting for `LoadState.NetworkIdle` after navigation/clicks.

- [ ] **Step 3: Verify the helper compiles cleanly**

Run:

```powershell
dotnet build EstateManagementUI.IntegrationTests\EstateManagementUI.IntegrationTests.csproj
```

Expected: build succeeds with the new helper methods referenced by the step definitions.

### Task 2: Add the merchant feature and step definitions

**Files:**
- Create: `EstateManagementUI.IntegrationTests/Features/MerchantManagement.feature`
- Create: `EstateManagementUI.IntegrationTests/Steps/MerchantManagementSteps.cs`

- [ ] **Step 1: Write the failing Reqnroll scenario**

Add one scenario that exercises the merchant journey end to end:

```gherkin
@base @background @estate
Feature: Merchant Management
  As an authenticated estate user
  I want to move through the merchant management screens
  So that I can create, inspect, edit, schedule, and deposit against a merchant from one journey

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
    And I have created the following contracts
      | EstateName  | OperatorName  | ContractName       |
      | Test Estate | Test Operator | Test Merchant Contract |
    And I have created the following security users
      | EmailAddress                 | Password | GivenName  | FamilyName | EstateName  |
      | estateuser@testestate1.co.uk | 123456   | TestEstate | User1      | Test Estate |

  Scenario: Estate users can complete the full merchant screen flow
    Given the user navigates to the app address
    And I click on the Sign In Button
    Then I am presented with a login screen
    When I login with the username 'estateuser@testestate1.co.uk' and password '123456'
    Then I should see the dashboard heading
    When I open the merchant management screen
    Then I should see the merchant management heading
    When I create a merchant
    Then I should see the merchant in the list
    When I view the merchant
    Then I should see the merchant details page
    When I open the merchant schedule from the view page
    Then I should see the merchant schedule page
    When I return to the merchant view page
    Then I should see the merchant details page
    When I edit the merchant
    Then I should see the merchant edit page
    When I open the merchant schedule from the edit page
    Then I should see the merchant schedule page
    When I return to the merchant edit page
    Then I should see the merchant edit page
    When I make a merchant deposit
    Then I should see the merchant deposit page
    When I submit the merchant deposit
    Then I should be back on the merchant list
```

- [ ] **Step 2: Bind the steps to the shared helper**

Create a merchant-specific binding class that calls the helper methods and keeps the scenario text short and readable.

- [ ] **Step 3: Generate and compile the new feature**

Run:

```powershell
dotnet build EstateManagementUI.IntegrationTests\EstateManagementUI.IntegrationTests.csproj
```

Expected: Reqnroll generates the feature code-behind and the new step class compiles.

### Task 3: Run the integration tests and tighten any selectors

**Files:**
- Modify: `EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs`
- Modify: `EstateManagementUI.IntegrationTests/Steps/MerchantManagementSteps.cs`

- [ ] **Step 1: Run the merchant integration tests**

Run:

```powershell
dotnet test EstateManagementUI.IntegrationTests\EstateManagementUI.IntegrationTests.csproj --filter MerchantManagement
```

Expected: the new scenario passes end to end.

- [ ] **Step 2: Fix any route or selector drift**

If a page renders slightly different text or uses a different button label, update the helper method rather than the scenario text so the feature stays readable.

- [ ] **Step 3: Re-run the targeted test and the integration suite**

Run:

```powershell
dotnet test EstateManagementUI.IntegrationTests\EstateManagementUI.IntegrationTests.csproj --filter MerchantManagement
dotnet test EstateManagementUI.IntegrationTests\EstateManagementUI.IntegrationTests.csproj
```

Expected: the merchant scenario passes and the broader integration suite stays green.
