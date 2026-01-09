# Quick Start Guide - Offline Integration Tests

This guide will get you running tests in 3 minutes.

## Prerequisites

- .NET 10.0 SDK installed
- Node.js installed (for Playwright)

## Step-by-Step Instructions

### 1. Run Tests

The tests will **automatically start and stop** the Blazor application for you!

```bash
cd EstateManagementUI.OfflineIntegrationTests

# First time only: Install Playwright browsers
pwsh bin/Debug/net10.0/playwright.ps1 install
# OR on Linux/Mac: playwright install

# Run all tests - application starts automatically!
dotnet test
```

That's it! The framework will:
1. ✅ Build the Blazor application
2. ✅ Start it with TestMode enabled on port 5004
3. ✅ Set default test user to **Estate** role (can view Estate information)
4. ✅ Wait for it to be ready
5. ✅ Run your tests
6. ✅ Stop the application when done

### 2. Manual Application Start (Optional)

If you prefer to start the application manually (e.g., for debugging), you can still do that:

```bash
# Terminal 1 (Optional - only if you want manual control)
cd EstateManagementUI.BlazorServer
export TestMode=true     # Linux/Mac
# OR
$env:TestMode="true"     # Windows PowerShell
dotnet run

# Terminal 2
cd EstateManagementUI.OfflineIntegrationTests
dotnet test
```

The framework detects if the application is already running and uses it instead of starting a new instance.

## Common Issues

### ❌ Error: `Could not find solution directory`

**Problem**: Running from wrong directory.

**Solution**: Make sure you run `dotnet test` from the `EstateManagementUI.OfflineIntegrationTests` directory.

### ❌ Tests failing? Check the screenshots!

**Solution**: When tests fail, screenshots are automatically saved to the `Screenshots/` subfolder in the test project. Check these screenshots to see what state the application was in when the test failed.

Example: `EstateManagementUI.OfflineIntegrationTests/Screenshots/screenshot-ViewEstateDashboard-20260109153000.png`

### ❌ Error: Tests fail with authentication errors

**Problem**: TestMode is not enabled.

**Solution**: The framework automatically sets TestMode=true. If you're running the app manually, make sure to set the environment variable.

### ❌ Error: Playwright browsers not found

**Problem**: Playwright browsers not installed.

**Solution**: Run `playwright install` or `pwsh bin/Debug/net10.0/playwright.ps1 install`

### ❌ Error: Port 5004 already in use

**Problem**: Another instance of the application is already running.

**Solution**: Stop the other instance or the tests will detect and use the running instance.

## Test Options

### Run with Different Browsers

```bash
# Firefox
Browser=Firefox dotnet test

# WebKit (Safari/Edge)
Browser=WebKit dotnet test
```

### Run Specific Tests

```bash
# Run only Estate tests
dotnet test --filter "FullyQualifiedName~EstateTests"

# Run only a specific scenario
dotnet test --filter "FullyQualifiedName~ViewEstateDashboard"
```

### Parallel Execution

```bash
# Run with 5 parallel workers
dotnet test -- NUnit.NumberOfTestWorkers=5
```

## What's Happening?

1. **BeforeTestRun**: Framework builds and starts the Blazor app with TestMode=true
2. **Test Execution**: Playwright automates browser interactions against the running app
3. **AfterTestRun**: Framework stops the application and cleans up

## Advantages of Automatic Startup

✅ **No manual steps** - Just run `dotnet test`  
✅ **Consistent environment** - TestMode always enabled  
✅ **Clean state** - Fresh application for each test run  
✅ **CI/CD friendly** - Works in automated pipelines  
✅ **Faster development** - No context switching between terminals

## Next Steps

- Read [TESTING_GUIDE.md](TESTING_GUIDE.md) for detailed information
- Read [README.md](README.md) for framework overview
- Read [EXTENSIBILITY_GUIDE.md](EXTENSIBILITY_GUIDE.md) to learn about extending the framework
