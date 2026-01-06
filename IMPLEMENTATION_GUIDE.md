# Test Infrastructure Enhancement - Implementation Summary

## Overview

This implementation enhances the Blazor integration test infrastructure with two major capabilities:

1. **Editable In-Memory Test Data** - A complete CRUD-enabled data store for managing test data during test execution
2. **OIDC Authentication Bypass** - A test authentication handler that eliminates the need for Docker containers running OIDC services

## Key Changes

### 1. Test Mode Configuration

**Files Modified:**
- `EstateManagementUI.BlazorServer/appsettings.json` - Added `TestMode` setting
- `EstateManagementUI.BlazorServer/appsettings.Test.json` - Created test-specific configuration with `TestMode=true`

The application now supports a "Test Mode" that can be enabled via configuration:

```json
{
  "AppSettings": {
    "TestMode": true
  }
}
```

When enabled, the application uses test authentication and in-memory data storage instead of requiring OIDC and external data sources.

### 2. Test Authentication Handler

**New File:** `EstateManagementUI.BlazorServer/Common/TestAuthenticationHandler.cs`

Implements a custom authentication handler that:
- Bypasses OIDC authentication flow
- Automatically authenticates all requests
- Provides test user claims (user ID, name, email, estate ID, role)
- Eliminates dependency on SecurityService Docker containers

**Key Features:**
- Scheme Name: "TestAuthentication"
- Default Test User: "Test User" with estate ID `11111111-1111-1111-1111-111111111111`
- Role: "Estate"

### 3. Test Data Store

**New Files:**
- `EstateManagementUI.BlazorServer/Services/ITestDataStore.cs` - Interface
- `EstateManagementUI.BlazorServer/Services/TestDataStore.cs` - Implementation

Provides a thread-safe, in-memory data store with full CRUD operations:

#### Interface Methods

**Estate Management:**
- `GetEstate(Guid estateId)` - Retrieve estate by ID
- `SetEstate(EstateModel estate)` - Set/update estate

**Merchant Management:**
- `GetMerchants(Guid estateId)` - List all merchants
- `GetMerchant(Guid estateId, Guid merchantId)` - Get specific merchant
- `AddMerchant(Guid estateId, MerchantModel merchant)` - Add new merchant
- `UpdateMerchant(Guid estateId, MerchantModel merchant)` - Update merchant
- `RemoveMerchant(Guid estateId, Guid merchantId)` - Remove merchant

**Operator Management:**
- `GetOperators(Guid estateId)` - List all operators
- `GetOperator(Guid estateId, Guid operatorId)` - Get specific operator
- `AddOperator(Guid estateId, OperatorModel operator)` - Add new operator
- `UpdateOperator(Guid estateId, OperatorModel operator)` - Update operator
- `RemoveOperator(Guid estateId, Guid operatorId)` - Remove operator

**Contract Management:**
- `GetContracts(Guid estateId)` - List all contracts
- `GetContract(Guid estateId, Guid contractId)` - Get specific contract
- `AddContract(Guid estateId, ContractModel contract)` - Add new contract
- `UpdateContract(Guid estateId, ContractModel contract)` - Update contract
- `RemoveContract(Guid estateId, Guid contractId)` - Remove contract

**Data Isolation:**
- `Reset()` - Clears all data and reinitializes with defaults

#### Implementation Details

- Uses `ConcurrentDictionary` for thread-safe operations
- Data organized by Estate ID for isolation
- Includes default test data on initialization
- Supports multiple estates simultaneously

### 4. Test Mediator Service

**New File:** `EstateManagementUI.BlazorServer/Services/TestMediatorService.cs`

Implements `IMediator` interface while using the in-memory test data store:

**Query Handling:**
- All entity queries (Estate, Merchant, Operator, Contract) read from `TestDataStore`
- Dashboard queries return mock data
- File processing queries return mock data

**Command Handling:**
- All CRUD commands execute against `TestDataStore`
- Data changes are persisted in memory for the test session
- Commands like `CreateMerchantCommand`, `UpdateOperatorCommand`, etc. actually modify test data

**Maintains Mediator Pattern:**
- Fully compatible with existing application code
- No changes required to UI components or business logic
- Transparent swap-in for `StubbedMediatorService`

### 5. Program.cs Integration

**Modified File:** `EstateManagementUI.BlazorServer/Program.cs`

Updated to support conditional configuration based on test mode:

```csharp
var testMode = builder.Configuration.GetValue<bool>("AppSettings:TestMode", false);

if (testMode)
{
    // Test mode: Use test authentication and test mediator
    builder.Services.AddAuthentication(...)
        .AddScheme<..., TestAuthenticationHandler>(...);
    
    builder.Services.AddSingleton<ITestDataStore, TestDataStore>();
    builder.Services.AddSingleton<IMediator, TestMediatorService>();
}
else
{
    // Production mode: Use OIDC and stubbed mediator
    builder.Services.AddAuthentication(...)
        .AddCookie(...)
        .AddOpenIdConnect(...);
    
    builder.Services.AddSingleton<IMediator, StubbedMediatorService>();
}
```

### 6. Test Infrastructure Helpers

**New Files:**
- `EstateManagementUI.BlazorIntegrationTests/Common/TestDataHelper.cs` - Helper class for test data manipulation
- `EstateManagementUI.BlazorIntegrationTests/TEST_INFRASTRUCTURE.md` - Comprehensive documentation

**TestDataHelper Features:**
- Simplified API for common test operations
- Helper methods for creating test entities
- Easy access to default estate ID
- Reset functionality for test isolation

Example usage:
```csharp
var helper = new TestDataHelper(testDataStore);

// Create test data
var merchant = helper.CreateTestMerchant(
    helper.DefaultEstateId, 
    "Test Merchant", 
    "TEST001"
);

// Update test data
merchant.Balance = 10000;
helper.UpdateMerchant(helper.DefaultEstateId, merchant);

// Reset for next test
helper.Reset();
```

## Default Test Data

The `TestDataStore` initializes with:

### Estate
- ID: `11111111-1111-1111-1111-111111111111`
- Name: "Test Estate"

### Merchants (3 pre-configured)
1. Test Merchant 1 (MERCH001) - £10,000 balance, Immediate settlement
2. Test Merchant 2 (MERCH002) - £5,000 balance, Weekly settlement
3. Test Merchant 3 (MERCH003) - £15,000 balance, Monthly settlement

### Operators (2 pre-configured)
1. Safaricom - Requires custom merchant number
2. Voucher - No special requirements

### Contracts (2 pre-configured)
1. Standard Transaction Contract - Mobile Topup with fees
2. Voucher Sales Contract - Simple voucher product

## Benefits

### 1. Faster Test Execution
- No Docker container startup time
- No OIDC service initialization
- Immediate test data availability
- Reduced infrastructure overhead

### 2. Simpler Test Environment
- No external dependencies
- No network calls to auth services
- Self-contained test execution
- Works in isolated environments

### 3. Flexible Data Management
- Create custom test scenarios on-the-fly
- Modify data mid-test
- Verify data changes directly
- Easy test data setup and teardown

### 4. Data Isolation
- Each test can reset data to default state
- Tests don't interfere with each other
- Predictable test data state
- Thread-safe operations

### 5. Maintains Architecture
- **Mediator pattern preserved** - Core requirement met
- No changes to UI components
- No changes to business logic
- Drop-in replacement for existing services

## Usage Examples

### Example 1: Testing Merchant Creation

```csharp
[Given(@"I have added a new merchant")]
public void GivenIHaveAddedANewMerchant()
{
    var merchant = new MerchantModel
    {
        MerchantId = Guid.NewGuid(),
        MerchantName = "New Test Merchant",
        MerchantReference = "NEWTEST001",
        Balance = 0,
        SettlementSchedule = "Immediate"
    };
    
    testDataHelper.AddMerchant(testDataHelper.DefaultEstateId, merchant);
}

[Then(@"the merchant should appear in the merchant list")]
public void ThenMerchantShouldAppear()
{
    var merchants = testDataHelper.GetMerchants(testDataHelper.DefaultEstateId);
    merchants.Should().Contain(m => m.MerchantReference == "NEWTEST001");
}
```

### Example 2: Testing Merchant Update

```csharp
[When(@"I update the merchant balance to (.*)")]
public void WhenIUpdateMerchantBalance(decimal newBalance)
{
    var merchantId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    var merchant = testDataHelper.GetMerchant(testDataHelper.DefaultEstateId, merchantId);
    
    merchant.Balance = newBalance;
    testDataHelper.UpdateMerchant(testDataHelper.DefaultEstateId, merchant);
}

[Then(@"the merchant balance should be (.*)")]
public void ThenMerchantBalanceShouldBe(decimal expectedBalance)
{
    var merchantId = Guid.Parse("22222222-2222-2222-2222-222222222222");
    var merchant = testDataHelper.GetMerchant(testDataHelper.DefaultEstateId, merchantId);
    
    merchant.Balance.Should().Be(expectedBalance);
}
```

### Example 3: Test Isolation

```csharp
[BeforeScenario]
public void BeforeScenario()
{
    // Reset to default state before each test
    testDataHelper.Reset();
}

[AfterScenario]
public void AfterScenario()
{
    // Optionally verify data state
    var merchants = testDataHelper.GetMerchants(testDataHelper.DefaultEstateId);
    Console.WriteLine($"Test ended with {merchants.Count} merchants");
}
```

## Migration Guide

### For Existing Tests

To migrate existing Docker-based tests:

1. **Enable Test Mode**
   ```bash
   export AppSettings__TestMode=true
   ```

2. **Remove Docker Setup**
   - Remove SecurityService container setup
   - Remove OIDC-related Docker configuration
   - Keep only UI container if needed

3. **Update Test Steps**
   - Replace API calls with `TestDataHelper` methods
   - Remove authentication setup steps
   - Use in-memory data instead of database queries

4. **Add Data Reset**
   ```csharp
   [BeforeScenario]
   public void ResetData()
   {
       testDataHelper.Reset();
   }
   ```

### For New Tests

1. **Configure Test Mode in appsettings.Test.json**
2. **Inject TestDataHelper into step definitions**
3. **Use TestDataHelper for all data operations**
4. **Reset data between scenarios**

## Technical Details

### Thread Safety
- All data stores use `ConcurrentDictionary`
- Safe for parallel test execution
- No locking required in test code

### Memory Management
- Data stored in memory for session duration
- Reset clears and reinitializes
- Garbage collected when test session ends

### Performance
- Fast in-memory operations
- No I/O overhead
- No network latency
- Suitable for large test suites

## Compatibility

### Backward Compatibility
- Original `StubbedMediatorService` unchanged
- Default behavior (TestMode=false) unchanged
- Existing tests continue to work
- No breaking changes to API

### Forward Compatibility
- Easy to extend with additional entities
- Can add more CRUD operations as needed
- Supports future data requirements

## Limitations

1. **Dashboard/File Data**: Currently returns mock data, not integrated with test data store
2. **Transaction Data**: Not yet implemented in test data store
3. **Complex Relationships**: Some entity relationships simplified
4. **Persistence**: Data only exists for test session duration

## Future Enhancements

Potential improvements:

1. **Custom User Claims**: Allow tests to specify different user contexts
2. **Data Fixtures**: Pre-defined test data sets
3. **Data Snapshots**: Save and restore specific data states
4. **Query Tracking**: Record all queries for verification
5. **Command History**: Log all commands for debugging
6. **Dashboard Integration**: Integrate dashboard queries with test data
7. **Transaction Processing**: Add transaction data to test store

## Conclusion

This implementation successfully addresses the requirements:

✅ **Editable Static Test Data** - Complete CRUD operations with in-memory storage
✅ **OIDC Bypass** - Test authentication handler eliminates external dependencies
✅ **Maintains Mediator Pattern** - Core architectural pattern preserved
✅ **Faster Tests** - No Docker containers for authentication
✅ **Simpler Setup** - Configuration-based test mode
✅ **Data Isolation** - Reset functionality ensures test independence

The implementation provides a solid foundation for fast, flexible integration testing while maintaining the application's architectural integrity.
