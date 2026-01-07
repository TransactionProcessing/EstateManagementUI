# Test Infrastructure Enhancement

## Overview

This document describes the enhanced test infrastructure that enables Blazor integration tests to run without Docker containers for OIDC authentication and provides editable test data management.

## Key Features

### 1. Test Mode Configuration

The application can run in "Test Mode" by setting the `AppSettings:TestMode` configuration to `true`. In test mode:

- **OIDC Authentication is bypassed** using a `TestAuthenticationHandler`
- **Test data is managed in-memory** using the `TestDataStore`
- **No external dependencies** are required for authentication or data storage

#### Configuration

In `appsettings.Test.json` or via environment variables:

```json
{
  "AppSettings": {
    "TestMode": true
  }
}
```

### 2. OIDC Authentication Bypass

The `TestAuthenticationHandler` provides automatic authentication for tests without requiring an OIDC server:

- Automatically authenticates all requests
- Provides test user claims (user ID, name, email, estate ID, role)
- Eliminates the need for SecurityService Docker containers during testing

**Implementation**: See `EstateManagementUI.BlazorServer/Common/TestAuthenticationHandler.cs`

### 3. Editable In-Memory Test Data

The test infrastructure provides a complete in-memory data store with CRUD operations:

#### ITestDataStore Interface

Provides methods for managing test data:

- **Estate Management**: `GetEstate()`, `SetEstate()`
- **Merchant Management**: `GetMerchants()`, `GetMerchant()`, `AddMerchant()`, `UpdateMerchant()`, `RemoveMerchant()`
- **Operator Management**: `GetOperators()`, `GetOperator()`, `AddOperator()`, `UpdateOperator()`, `RemoveOperator()`
- **Contract Management**: `GetContracts()`, `GetContract()`, `AddContract()`, `UpdateContract()`, `RemoveContract()`
- **Reset**: `Reset()` - Clears all data and reinitializes with defaults

**Implementation**: See `EstateManagementUI.BlazorServer/Services/ITestDataStore.cs`

#### TestDataStore Implementation

Thread-safe in-memory storage using `ConcurrentDictionary`:

- Stores data organized by Estate ID
- Provides data isolation between estates
- Includes default test data on initialization
- Can be reset between test scenarios

**Implementation**: See `EstateManagementUI.BlazorServer/Services/TestDataStore.cs`

### 4. TestMediatorService

The `TestMediatorService` maintains the mediator pattern while using the in-memory test data store:

- All queries read from the `TestDataStore`
- All commands execute against the `TestDataStore`
- CRUD operations are properly reflected in test data
- Dashboard and file processing queries return mock data

**Implementation**: See `EstateManagementUI.BlazorServer/Services/TestMediatorService.cs`

## Default Test Data

When the `TestDataStore` is initialized (or reset), it contains:

### Estate
- **Estate ID**: `11111111-1111-1111-1111-111111111111`
- **Name**: "Test Estate"

### Merchants
1. **Test Merchant 1** (ID: `22222222-2222-2222-2222-222222222222`)
   - Reference: MERCH001
   - Balance: £10,000.00
   - Settlement: Immediate

2. **Test Merchant 2** (ID: `22222222-2222-2222-2222-222222222223`)
   - Reference: MERCH002
   - Balance: £5,000.00
   - Settlement: Weekly

3. **Test Merchant 3** (ID: `22222222-2222-2222-2222-222222222224`)
   - Reference: MERCH003
   - Balance: £15,000.00
   - Settlement: Monthly

### Operators
1. **Safaricom** (ID: `33333333-3333-3333-3333-333333333333`)
   - Requires Custom Merchant Number: Yes
   - Requires Custom Terminal Number: No

2. **Voucher** (ID: `33333333-3333-3333-3333-333333333334`)
   - Requires Custom Merchant Number: No
   - Requires Custom Terminal Number: No

### Contracts
1. **Standard Transaction Contract** (ID: `44444444-4444-4444-4444-444444444444`)
   - Operator: Safaricom
   - Products: Mobile Topup with transaction fees

2. **Voucher Sales Contract** (ID: `44444444-4444-4444-4444-444444444445`)
   - Operator: Voucher
   - Products: Voucher Purchase

## Using Test Data in Integration Tests

### TestDataHelper

The `TestDataHelper` class provides easy access to test data operations:

```csharp
// Access default estate
var estateId = testDataHelper.DefaultEstateId;

// Get merchants
var merchants = testDataHelper.GetMerchants(estateId);

// Add a new merchant
var newMerchant = testDataHelper.CreateTestMerchant(
    estateId, 
    "New Merchant", 
    "NEWMERCH001"
);

// Update a merchant
var merchant = testDataHelper.GetMerchant(estateId, merchantId);
merchant.MerchantName = "Updated Name";
testDataHelper.UpdateMerchant(estateId, merchant);

// Remove a merchant
testDataHelper.RemoveMerchant(estateId, merchantId);

// Reset all data between tests
testDataHelper.Reset();
```

### Test Isolation

To ensure tests don't interfere with each other:

1. **Use `Reset()` before each scenario**: Resets data to default state
2. **Create unique test data**: Use unique IDs for test-specific entities
3. **Clean up after tests**: Remove test-specific data if not using `Reset()`

Example in Reqnroll hooks:

```csharp
[BeforeScenario]
public void BeforeScenario()
{
    // Reset test data to default state
    testDataHelper.Reset();
}
```

## Running Tests in Test Mode

### Option 1: Environment Variable

Set the environment variable before running tests:

```bash
export AppSettings__TestMode=true
dotnet test
```

### Option 2: Test Configuration File

The application will automatically load `appsettings.Test.json` when running in test environment:

```bash
ASPNETCORE_ENVIRONMENT=Test dotnet test
```

### Option 3: Docker Environment Variables

When running the Blazor app in Docker for integration tests:

```yaml
environment:
  - AppSettings__TestMode=true
```

## Benefits

1. **Faster Test Execution**: No need to start Docker containers for OIDC
2. **Simpler Setup**: Reduced infrastructure requirements
3. **Flexible Data Management**: Easy to create, modify, and verify test data
4. **Data Isolation**: Each test can have its own data state
5. **Maintains Architecture**: Continues using the mediator pattern

## Architecture Notes

The test infrastructure maintains the application's architectural patterns:

- **Mediator Pattern**: `TestMediatorService` implements `IMediator`
- **Dependency Injection**: Test services are registered in the DI container
- **Configuration-Based**: Test mode is controlled via configuration
- **Clean Separation**: Test code is isolated in specific classes

## Migration from Docker-Based Tests

Existing tests can be migrated to use test mode:

1. Remove Docker setup for OIDC containers
2. Set `AppSettings:TestMode=true` in test configuration
3. Use `TestDataHelper` to set up test data
4. Remove OIDC-specific test steps
5. Update test steps to use in-memory data

## Troubleshooting

### Tests can't authenticate
- Ensure `AppSettings:TestMode` is set to `true`
- Check that `TestAuthenticationHandler` is registered

### Data not persisting between steps
- Verify you're not calling `Reset()` between steps in the same scenario
- Check that you're using the same `TestDataStore` instance (it's registered as singleton)

### Data from previous tests appearing
- Call `Reset()` in `BeforeScenario` hooks to ensure clean state
- Verify test isolation is properly configured

## Future Enhancements

Potential improvements to consider:

1. **Custom test user claims**: Allow tests to specify different user contexts
2. **Test data fixtures**: Pre-defined test data sets for common scenarios
3. **Data snapshots**: Save and restore test data states
4. **Query verification**: Track which queries were executed during tests
5. **Command history**: Record all commands executed for verification
