# Fast UI Test Project Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a separate fast UI test project that reuses the existing `.feature` files but runs without Docker by starting the Blazor app in test mode with in-memory data and test-only control endpoints.

**Architecture:** Keep the current Docker-backed integration project untouched. Add a second Playwright/Reqnroll test project that launches the Blazor app locally, points it at test-mode registrations, and drives it through the same browser flows. The app gets a test-mode branch that swaps in the in-memory data store and test auth, plus a small test-support surface for reset/seed operations so the existing scenarios can stay single-sourced.

**Tech Stack:** .NET 10, Blazor Server, Playwright, Reqnroll, NUnit, MediatR, existing `TestDataStore`, test-only HTTP control endpoints, local `dotnet run` app launch.

## Global Constraints

- Keep the existing `EstateManagementUI.IntegrationTests` project in place for Docker-backed end-to-end coverage.
- Reuse the existing `.feature` files; do not duplicate scenario text into a new source of truth.
- The new fast suite must not require Docker to start the UI or its supporting services.
- Test mode must remain opt-in and should not change production behavior when disabled.
- Preserve the current browser automation stack, including Playwright and Reqnroll.

---

### Task 1: Add app test-mode wiring and test control endpoints

**Files:**
- Modify: `EstateManagementUI.BlazorServer/Program.cs`
- Modify: `EstateManagementUI.BlazorServer/Common/BoostrapperExtensions.cs`
- Create: `EstateManagementUI.BlazorServer/Testing/TestSupportEndpoints.cs`
- Create: `EstateManagementUI.BlazorServer/Testing/TestMediator.cs`
- Create: `EstateManagementUI.BlazorServer/Testing/TestApiClient.cs`
- Modify: `EstateManagementUI.BlazorServer/appsettings.Test.json`

**Interfaces:**
- Consumes: `ITestDataStore`, `TestDataStore`, existing UI service interfaces, `IMediator`, `IApiClient`
- Produces: a working `BackedByTestDataStore` test mode and HTTP endpoints for resetting/seeding test data

- [ ] **Step 1: Add a failing app-startup test for the new mode**

Create or extend the existing Blazor Server tests so `AppSettings:TestMode=BackedByTestDataStore` can be parsed and the app can be built without touching Docker-only configuration.

- [ ] **Step 2: Wire the test registrations**

Register `ITestDataStore` and test-mode replacements for the mediator-backed UI services so the app can serve UI pages from in-memory state.

- [ ] **Step 3: Add test-support endpoints**

Expose a small internal surface for `reset`, `seed-defaults`, and reporting-data seeding so the fast test project can prepare scenario state over HTTP.

- [ ] **Step 4: Run the app tests**

Run the existing Blazor Server unit tests plus a targeted app boot check for the new test mode.

### Task 2: Scaffold the new fast UI test project

**Files:**
- Create: `EstateManagementUI.BlazorFastIntegrationTests/EstateManagementUI.BlazorFastIntegrationTests.csproj`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/appsettings.json`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Hooks/BrowserHooks.cs`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Hooks/AppHostHooks.cs`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Common/AppHost.cs`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Common/TestSupportClient.cs`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Features/*.feature` as linked items

**Interfaces:**
- Consumes: the Blazor app test-mode URL, Playwright, Reqnroll, NUnit
- Produces: a new test project that starts the UI locally and reuses the existing feature files

- [ ] **Step 1: Write the new project file**

Reference the same test packages as the current integration project, add a project reference to the Blazor app, and link the existing `.feature` files into the new project.

- [ ] **Step 2: Add the local app host helper**

Launch the Blazor app with `AppSettings__TestMode=BackedByTestDataStore` and a predictable local base URL, then wait for a health or entry page response before tests begin.

- [ ] **Step 3: Add the Playwright hooks**

Create a browser hook that reads the base URL from the app host instead of Docker-derived ports and preserves screenshots/traces on failure.

- [ ] **Step 4: Verify the project compiles**

Build the new project and confirm Reqnroll generates the linked feature code-behind successfully.

### Task 3: Port the scenario plumbing to the fast project

**Files:**
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Common/FastTestingContext.cs`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Common/FastDashboardPageHelper.cs`
- Create: `EstateManagementUI.BlazorFastIntegrationTests/Steps/*.cs`
- Optionally modify: linked helper files if they can be shared cleanly without Docker

**Interfaces:**
- Consumes: the local app host, the test-support client, Playwright `IPage`
- Produces: step definitions that satisfy the existing feature files without Docker startup

- [ ] **Step 1: Recreate the shared scenario context**

Replace the container-oriented context with a simple local host/context object that tracks base URL, seeded estate ID, and any scenario-scoped data the page helpers need.

- [ ] **Step 2: Reimplement the background steps**

Translate the current background setup into calls to the test-support API and default seed state instead of creating roles, clients, and users through Docker services.

- [ ] **Step 3: Reuse the existing page paths and assertions**

Keep the same feature text, but make the step definitions navigate against the local test-mode app and assert the same visible UI states.

- [ ] **Step 4: Run the fast feature suite**

Run the new project against the local app host and fix any selectors or data assumptions exposed by test mode.

### Task 4: Add solution and docs updates, then compare suite behavior

**Files:**
- Modify: `EstateManagementUI.sln`
- Modify: `README.md`
- Modify: `EstateManagementUI.IntegrationTests/README.md` if cross-referencing the new project helps
- Create or modify: a short note under `docs/`

**Interfaces:**
- Consumes: the new project name and test-mode behavior
- Produces: discoverable documentation and a solution entry for the separate fast suite

- [ ] **Step 1: Add the new project to the solution**

Make the new project visible in the solution so it runs alongside the existing integration suite.

- [ ] **Step 2: Document when to use each suite**

State clearly that the fast suite is the default day-to-day UI regression path and the Docker suite remains the full environment smoke test.

- [ ] **Step 3: Run both suites or the closest available subsets**

Compare the fast suite and the Docker suite on at least the dashboard and one CRUD flow so the divergence is understood.

