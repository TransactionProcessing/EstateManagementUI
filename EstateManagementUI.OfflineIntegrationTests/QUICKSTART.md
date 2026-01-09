# Quick Start Guide - Offline Integration Tests

This guide will get you running tests in 5 minutes.

## Prerequisites

- .NET 10.0 SDK installed
- Node.js installed (for Playwright)

## Step-by-Step Instructions

### 1. Start the Blazor Application (Terminal 1)

Open a terminal and run:

```bash
cd EstateManagementUI.BlazorServer

# Enable TestMode (uses in-memory test data instead of real APIs)
export TestMode=true     # Linux/Mac
# OR
$env:TestMode="true"     # Windows PowerShell

# Start the application
dotnet run
```

**Wait** until you see: `Now listening on: https://localhost:5004`

Leave this terminal running. The application must stay running for tests to work.

### 2. Run Tests (Terminal 2)

Open a **new terminal** and run:

```bash
cd EstateManagementUI.OfflineIntegrationTests

# First time only: Install Playwright browsers
pwsh bin/Debug/net10.0/playwright.ps1 install
# OR on Linux/Mac: playwright install

# Run all tests
dotnet test
```

That's it! The tests will now run against the running Blazor application.

## Common Issues

### ❌ Error: `net::ERR_CONNECTION_REFUSED at https://localhost:5004/`

**Problem**: The Blazor application is not running.

**Solution**: Make sure you completed Step 1 above and the application is running on port 5004.

### ❌ Error: Tests fail with authentication errors

**Problem**: TestMode is not enabled.

**Solution**: Restart the Blazor application with `TestMode=true` environment variable.

### ❌ Error: Playwright browsers not found

**Problem**: Playwright browsers not installed.

**Solution**: Run `playwright install` or `pwsh bin/Debug/net10.0/playwright.ps1 install`

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

1. **Terminal 1**: Runs the Blazor application with TestDataStore (no real APIs needed)
2. **Terminal 2**: Playwright launches a browser and automates UI interactions
3. **Tests**: Execute scenarios defined in `.feature` files against the running app
4. **Results**: See test results in the console

## Next Steps

- Read [TESTING_GUIDE.md](TESTING_GUIDE.md) for detailed information
- Read [README.md](README.md) for framework overview
- Read [EXTENSIBILITY_GUIDE.md](EXTENSIBILITY_GUIDE.md) to learn about extending the framework
