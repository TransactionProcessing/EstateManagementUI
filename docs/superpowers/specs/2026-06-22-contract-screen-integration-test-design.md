# Contract Screen Integration Test Design

> **For agentic workers:** this design is the contract-screen counterpart to the existing estate/operator integration coverage. Implement it as one ReqNroll scenario first, then add the minimum helper and step surface needed to make the flow readable.

**Goal:** Add a single ReqNroll integration scenario that walks through the contract screens end-to-end: contract list, create, view, edit, and the edit-screen product and fee subflows.

**Why this shape:** The contract UI is already split into a few distinct screens, and a single user journey is the clearest way to prove those pages still connect correctly without multiplying test maintenance. The scenario should exercise the main navigation and the important edit capabilities in one pass.

**Architecture:** Reuse the existing Docker-backed integration harness and the same Playwright/ReqNroll pattern used by the dashboard and operator tests. Add one contract-focused feature file, a small step-definition class, and contract-specific helper methods in the shared page helper. Keep the scenario self-contained by creating its own contract through the UI instead of depending on pre-seeded contract data.

**Tech Stack:** ReqNroll, NUnit, Playwright, Docker-backed integration containers, shared integration test helpers.

---

### Scenario Shape

The test should follow one continuous journey:

1. Open the app and sign in with the existing estate test user.
2. Navigate to the contract list page and confirm the list screen renders.
3. Start contract creation, enter a contract description, choose an operator, and create it.
4. Confirm the new contract appears in the list.
5. Open the contract view screen and confirm the detail page renders.
6. Open the edit screen and confirm the edit page renders.
7. Add one product from the edit screen and confirm it appears in the edit view.
8. Add one fee from the edit screen and confirm it appears in the edit view.
9. Return to the list screen to prove the navigation loop closes cleanly.

The scenario should avoid relying on contract-description persistence if the current backend does not support updating that field cleanly. The important thing is that the edit screen loads and its product/fee flows work.

---

### Files To Add Or Update

- Create: `EstateManagementUI.IntegrationTests/Features/ContractManagement.feature`
- Create: `EstateManagementUI.IntegrationTests/Steps/ContractManagementSteps.cs`
- Modify: `EstateManagementUI.IntegrationTests/Common/DashboardPageHelper.cs`

---

### Feature Design

Use one scenario, tagged the same way as the other integration journeys so it runs inside the existing seeded estate context.

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

The scenario should use a single contract identifier generated during the run, so later steps can target the same record without extra state plumbing.

---

### Helper Design

Add helper methods for the following behaviors in `DashboardPageHelper.cs`:

- open the contract management list screen
- assert the list screen heading and initial content
- create a contract from the new-contract form
- verify the created contract appears in the list
- open the contract view screen for the created contract
- assert the contract detail heading and details content
- open the contract edit screen for the created contract
- assert the edit page renders
- add a product using the edit-screen modal or inline form
- assert the new product is visible in the edit view
- add a fee using the edit-screen modal or inline form
- assert the new fee is visible in the edit view
- navigate back to the list screen

Prefer existing stable selectors such as explicit buttons, role-based headings, and the current contract page ids/names. Only fall back to CSS selectors where the current UI does not expose a stable accessible target.

Suggested data for the run:

- Contract description: `Integration Contract`
- Product name: `Integration Product`
- Product display text: `Integration Product Display`
- Fee description: `Integration Fee`
- Fee value: `1.50`

If the edit screen requires a product value or calculation type, use the smallest valid values that let the add flow succeed.

---

### Step Design

Create one contract-specific step class scoped to `@estate` that delegates into the helper. Keep the step text close to the user journey so the feature remains readable.

The step file should cover:

- open contract management
- create contract
- view contract
- edit contract
- add product
- add fee
- return to the list

The step class should not duplicate page logic. It should only translate readable Gherkin into helper calls.

---

### Verification Plan

1. First run should fail with missing step definitions or missing helper methods.
2. After adding the step file and helper methods, the scenario should compile and reach browser assertions.
3. If selectors are off, tighten the helper locators rather than broadening the feature.
4. Final state should be one passing scenario in the contract feature file.

Recommended verification command:

```powershell
dotnet test EstateManagementUI.IntegrationTests/EstateManagementUI.IntegrationTests.csproj --filter FullyQualifiedName~EstateManagementUI.IntegrationTests.Features.ContractManagement
```

---

### Acceptance Criteria

- One ReqNroll scenario covers the full contract journey.
- The scenario starts from login and ends back on the contract list.
- Contract creation is verified through the list page.
- Contract view and edit pages are both exercised.
- Product and fee creation flows are both exercised on the edit page.
- The implementation stays inside the existing integration test harness and does not require production code changes unless a selector proves unstable.
