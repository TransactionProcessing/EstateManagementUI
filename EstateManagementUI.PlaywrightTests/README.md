# Estate Management UI - Playwright Integration Tests

This project contains Playwright-based integration tests for the Estate Management UI application, specifically testing the login flow and dashboard functionality.

## Overview

The tests use:
- **Playwright** for browser automation and UI testing
- **NUnit** as the test framework
- **Testcontainers** for Docker container management
- **Docker** images for:
  - Security Service (Authorization Server/Identity Server)
  - Estate Management UI Application

## Prerequisites

- .NET 10.0 SDK
- Docker installed and running
- Playwright browsers installed (see setup below)

## Required Docker Images

The tests require the following Docker images to be available:

1. **securityservice:latest** - The authorization/identity server
2. **estatemanagementui:latest** - The Estate Management UI application

You need to build these images before running the tests. See the main project's integration tests or build scripts for how to create these images.

## Setup

### 1. Install Playwright Browsers

After building the project for the first time, install the Playwright browsers:

```bash
cd EstateManagementUI.PlaywrightTests
dotnet build
pwsh bin/Debug/net10.0/playwright.ps1 install chromium
```

Or on Linux/Mac:
```bash
cd EstateManagementUI.PlaywrightTests
dotnet build
playwright install chromium
```

### 2. Ensure Docker Images are Available

Build or pull the required Docker images:

```bash
# Build Estate Management UI image (from the solution root)
docker build -t estatemanagementui:latest -f EstateManagementUI.BlazorServer/Dockerfile .

# The Security Service image should be available from the existing integration tests
# Check if it exists:
docker images | grep securityservice
```

## Running the Tests

### Run All Tests

```bash
cd EstateManagementUI.PlaywrightTests
dotnet test
```

### Run Tests with Verbose Output

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Run in CI Environment (Headless)

The tests automatically detect CI environments and run in headless mode. Set the environment variable:

```bash
export IsCI=true
dotnet test
```

Or:

```bash
export CI=true
dotnet test
```

## Test Structure

### Infrastructure

- **DockerHelper.cs** - Manages Docker containers for Security Service and Estate Management UI
- **PlaywrightTestBase.cs** - Base class for all Playwright tests, handles browser setup and teardown

### Tests

- **LoginAndDashboardTests.cs** - Contains tests for:
  - Home page display and Sign In button
  - Login flow redirect to Identity Server
  - Successful authentication and redirect to Dashboard
  - Dashboard navigation menu elements
  - Navigation to Merchants, Operators, and Contracts pages

## Test Execution Flow

1. **OneTimeSetUp**: Starts Docker containers (Security Service and Estate Management UI)
2. **SetUp**: Initializes Playwright, launches browser, creates new page for each test
3. **Test Execution**: Runs individual test scenarios
4. **TearDown**: Closes browser and cleans up Playwright resources after each test
5. **OneTimeTearDown**: Stops and removes Docker containers

## Test Cases

### Test_01_HomePage_Should_Display_SignIn_Button
Verifies that the home page loads and displays the Sign In button.

### Test_02_Login_Flow_Should_Redirect_To_Identity_Server
Verifies that clicking the Sign In button redirects to the Identity Server login page.

### Test_03_Successful_Login_Should_Navigate_To_Dashboard
Tests the complete login flow:
- Navigates to home page
- Clicks Sign In
- Enters credentials
- Submits login form
- Verifies redirect to Dashboard

### Test_04_Dashboard_Should_Display_Navigation_Menu
After successful login, verifies that the dashboard displays all navigation menu items:
- Merchants
- Operators
- Contracts
- Estate Details

### Test_05_Dashboard_Should_Allow_Navigation_To_Merchants
Tests navigation from dashboard to Merchants page.

### Test_06_Dashboard_Should_Allow_Navigation_To_Operators
Tests navigation from dashboard to Operators page.

### Test_07_Dashboard_Should_Allow_Navigation_To_Contracts
Tests navigation from dashboard to Contracts page.

## Configuration

The tests use the following default configuration:

- **Security Service Port**: Dynamically assigned by Docker (internally 5001)
- **Estate Management UI Port**: Dynamically assigned by Docker (internally 5004)
- **Test User**: estateuser@testestate1.co.uk
- **Test Password**: 123456

## Troubleshooting

### Docker Image Not Found

If you get an error about missing Docker images:
```
Error response from daemon: No such image: securityservice:latest
```

You need to build the required Docker images. Check the existing integration tests for the build process.

### Port Conflicts

The tests use dynamically assigned ports to avoid conflicts. If you still encounter issues, ensure no other containers are using the same container names.

### Certificate Errors

The tests are configured to ignore HTTPS certificate errors since we're using self-signed certificates in the Docker containers. The configuration includes:
- `IgnoreHTTPSErrors = true` in Playwright browser context
- `--ignore-certificate-errors` browser argument

### Headless Mode Issues

If tests fail in headless mode but work in headed mode, you can force headed mode for debugging:

```bash
export IsCI=false
dotnet test
```

## Integration with Existing Tests

This Playwright-based test project complements the existing Selenium-based integration tests:

- **Selenium Tests** (EstateManagementUI.IntegrationTests): Full integration tests with backend services using Reqnroll/BDD
- **Playwright Tests** (EstateManagementUI.PlaywrightTests): Focused UI tests for login and dashboard with faster execution

Both test suites serve different purposes and can be run independently.

## CI/CD Integration

These tests are designed to run in CI/CD pipelines. The tests:
- Automatically run in headless mode when `CI=true` or `IsCI=true` environment variable is set
- Use Testcontainers for reliable Docker container management
- Clean up all resources after test execution
- Provide detailed error messages and assertions

## Future Enhancements

Potential improvements for this test suite:
- Add more dashboard functionality tests
- Test error scenarios (invalid login, network errors)
- Add visual regression testing
- Implement page object model pattern for better maintainability
- Add performance metrics collection
