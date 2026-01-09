# EstateManagementUI.OfflineIntegrationTests

This is an **offline integration test project** that targets the **Blazor Server UI** (`EstateManagementUI.BlazorServer`) using **Playwright** for browser automation.

## Overview

This project provides comprehensive integration tests for the Blazor UI **without requiring backend API dependencies**. It uses an in-memory TestDataStore for all test data.

### Key Differences from BlazorIntegrationTests

This project is specifically designed for **offline testing**:

- **No Docker dependencies** - Tests run directly against the Blazor app
- **No backend API calls** - Uses TestDataStore for all data operations
- **No OIDC authentication** - Uses TestAuthenticationHandler to bypass auth
- **Faster execution** - No network latency or service startup time
- **Simpler setup** - Just build and run tests

The existing `EstateManagementUI.BlazorIntegrationTests` project is for full integration testing with real backend services via Docker.

## Key Features

✅ **Multi-Browser Support**: Chrome, Firefox, WebKit (Edge/Safari)  
✅ **Parallel Execution**: Configurable NUnit parallel execution (default: 3 workers)  
✅ **Reqnroll/SpecFlow**: BDD-style feature files organized by application section  
✅ **Offline Testing**: No backend API required - uses in-memory test data  
✅ **NUnit Test Backend**: Industry-standard test framework  
✅ **Comprehensive Coverage**: Feature files for all application sections  
✅ **Extensible Architecture**: Ready for future integration with real auth/backend APIs

## Project Structure

```
EstateManagementUI.OfflineIntegrationTests/
├── Common/
│   ├── BlazorUiHelpers.cs          # High-level UI interactions
│   ├── PlaywrightExtensions.cs     # Playwright helper methods
│   ├── Hooks.cs                     # Test lifecycle management
│   ├── TestConfiguration.cs        # Configuration management
│   ├── TestDataHelper.cs            # Test data utilities
│   ├── DockerHelper.cs              # Docker infrastructure (optional)
│   ├── Setup.cs                     # Test setup
│   ├── SharedSteps.cs               # Shared step definitions
│   └── GenericSteps.cs              # Generic step definitions
├── Steps/
│   ├── BlazorUiSteps.cs            # Main UI step definitions
│   ├── TestDataManagementSteps.cs  # Test data steps
│   ├── FileProcessingSteps.cs      # File processing steps
│   ├── PermissionsSteps.cs         # Permissions steps
│   └── ReportingSteps.cs           # Reporting steps
├── Tests/
│   ├── EstateTests.feature
│   ├── MerchantTests.feature
│   ├── OperatorTests.feature
│   ├── ContractTests.feature
│   ├── FileProcessingTests.feature
│   ├── PermissionsTests.feature
│   └── ReportingTests.feature
├── AssemblyInfo.cs                  # Parallel execution config
├── test.runsettings                 # Test runner settings
├── EstateManagementUI.OfflineIntegrationTests.csproj
├── README.md                        # This file
├── TESTING_GUIDE.md                 # Complete testing guide
├── EXTENSIBILITY_GUIDE.md           # Extension guide
├── FRAMEWORK_SUMMARY.md             # Implementation summary
└── PR_SUMMARY.md                    # PR summary
```

## Prerequisites

- .NET 10.0 SDK
- Node.js (for Playwright browser installation)

## Getting Started

### Installation

```bash
# Navigate to the test project
cd EstateManagementUI.OfflineIntegrationTests

# Restore NuGet packages
dotnet restore

# Install Playwright browsers (first time only)
pwsh bin/Debug/net10.0/playwright.ps1 install
# OR on Linux/Mac:
# playwright install
```

### Running Tests

**Run all tests:**
```bash
dotnet test
```

**Run with Firefox:**
```bash
Browser=Firefox dotnet test
```

**Run with WebKit (Edge/Safari):**
```bash
Browser=WebKit dotnet test
```

**Run with custom parallel workers:**
```bash
dotnet test -- NUnit.NumberOfTestWorkers=5
```

**Run with settings file:**
```bash
dotnet test --settings test.runsettings
```

## Test Coverage

### Application Sections (7 feature files)

1. **EstateTests.feature** - Estate management functionality
2. **MerchantTests.feature** - Merchant creation, editing, and management
3. **OperatorTests.feature** - Operator management
4. **ContractTests.feature** - Contract and product management
5. **FileProcessingTests.feature** - File upload and processing
6. **PermissionsTests.feature** - User permissions and role management
7. **ReportingTests.feature** - All reporting features with query filters:
   - Transaction Detail Report (with filters: date, merchant, operator, status)
   - Transaction Summary by Merchant (with grouping options)
   - Transaction Summary by Operator
   - Settlement Summary
   - Settlement Reconciliation
   - Merchant Settlement History
   - Product Performance
   - Analytical Charts
   - Export functionality (CSV, Excel)

**Total: 61+ test scenarios**

## Multi-Browser Testing

The framework supports testing across multiple browsers:

**Chrome (Default):**
```bash
dotnet test
# OR
Browser=Chrome dotnet test
```

**Firefox:**
```bash
Browser=Firefox dotnet test
```

**Microsoft Edge/Safari (WebKit):**
```bash
Browser=WebKit dotnet test
```

## Parallel Test Execution

Tests are configured to run in parallel for faster execution:

**Using Default Settings (3 parallel workers):**
```bash
dotnet test
```

**Using Custom Run Settings:**
```bash
dotnet test --settings test.runsettings
```

**Custom Number of Workers:**
```bash
dotnet test -- NUnit.NumberOfTestWorkers=5
```

**Disable Parallel Execution:**
```bash
dotnet test -- NUnit.NumberOfTestWorkers=0
```

The parallel execution is configured at the fixture level:
- Each feature file runs as a separate fixture
- Multiple feature files can run in parallel
- Scenarios within a feature file run sequentially
- Each test gets its own browser context for isolation

## Configuration

The framework uses `appsettings.json` for configuration:

```json
{
  "TestSettings": {
    "SkipRemoteCalls": true,
    "EnableTestMode": true
  }
}
```

## Test Data Management

The framework uses an in-memory TestDataStore when running in offline mode:

- **Default Test Data**: Pre-populated with estates, merchants, operators, contracts
- **CRUD Operations**: Full support for Create, Read, Update, Delete
- **Test Isolation**: Reset between scenarios for clean state
- **Thread-Safe**: Concurrent dictionary implementation

## Documentation

- **TESTING_GUIDE.md** - Complete guide for running and writing tests
- **EXTENSIBILITY_GUIDE.md** - Guide for extending to use real auth/backend
- **FRAMEWORK_SUMMARY.md** - Complete implementation summary
- **PR_SUMMARY.md** - PR documentation

## Architecture

### Test Execution Flow

1. **BeforeTestRun**: Install Playwright browsers, initialize services
2. **BeforeScenario**: Create browser page, reset test data
3. **Test Execution**: Run Given/When/Then steps
4. **AfterScenario**: Take screenshot on failure, close page
5. **AfterTestRun**: Clean up browser and Playwright

### Offline Mode

When running in offline mode (default):
- OIDC authentication is bypassed using `TestAuthenticationHandler`
- Test data is managed in-memory using `TestDataStore`
- No external dependencies are required

## Extensibility

The framework is designed to be extended for future online testing:

- Service abstraction layer for authentication and backend
- Configuration-based mode switching
- Clear migration path documented in `EXTENSIBILITY_GUIDE.md`

See **EXTENSIBILITY_GUIDE.md** for detailed information on extending the framework to use real authentication endpoints and backend APIs.

## Troubleshooting

### Playwright browsers not installed
```
Error: Executable doesn't exist at [path]
```
**Solution:**
```bash
pwsh bin/Debug/net10.0/playwright.ps1 install
```

### Tests timing out
```
Error: Timeout 30000ms exceeded
```
**Solution:**
- Check if the Blazor app is running
- Increase timeout in PlaywrightExtensions.cs
- Check network connectivity

## CI/CD Integration

### GitHub Actions Example

```yaml
name: Offline Integration Tests

on: [push, pull_request]

jobs:
  test:
    runs-on: ubuntu-latest
    
    strategy:
      matrix:
        browser: [Chrome, Firefox, WebKit]
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '10.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
    
    - name: Install Playwright
      run: pwsh EstateManagementUI.OfflineIntegrationTests/bin/Debug/net10.0/playwright.ps1 install
    
    - name: Run tests
      env:
        Browser: ${{ matrix.browser }}
        IsCI: true
      run: dotnet test EstateManagementUI.OfflineIntegrationTests/EstateManagementUI.OfflineIntegrationTests.csproj --logger "trx;LogFileName=test-results.trx"
    
    - name: Upload test results
      if: always()
      uses: actions/upload-artifact@v3
      with:
        name: test-results-${{ matrix.browser }}
        path: '**/test-results.trx'
```

## Additional Resources

- [Playwright .NET Documentation](https://playwright.dev/dotnet/)
- [Reqnroll Documentation](https://docs.reqnroll.net/)
- [NUnit Parallel Execution](https://docs.nunit.org/articles/nunit/writing-tests/attributes/parallelizable.html)

## Support

For issues or questions:
1. Check this documentation
2. Review existing feature files for examples
3. Check Playwright documentation for browser automation issues
4. Open an issue in the repository
