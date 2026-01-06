# Skipping Remote Calls in Tests

## Overview

The test infrastructure now supports a "skip remote calls" mode that allows tests to run without making API calls to backend services (SecurityService, TransactionProcessor, etc.). This enables:

- **UI-Only Testing**: Test the UI without requiring backend Docker containers
- **Faster Test Execution**: No network latency or backend service setup time
- **Simplified Setup**: Only the UI container needs to be running

## How It Works

When skip remote calls mode is enabled, the test steps will:

1. **Skip all remote API calls** to SecurityService, TransactionProcessor, etc.
2. **Use the in-memory TestDataStore** in the Blazor application (when TestMode is enabled)
3. **Only interact with the UI** via Playwright browser automation

## Configuration

### Environment Variables

Set these environment variables to enable the feature:

```bash
# Skip remote API calls in test steps
export SKIP_REMOTE_CALLS=true

# Enable test mode in the Blazor application
export APP_TEST_MODE=true
```

### Running Tests

```bash
# Set environment variables
export SKIP_REMOTE_CALLS=true
export APP_TEST_MODE=true

# Run tests
dotnet test EstateManagementUI.BlazorIntegrationTests/EstateManagementUI.BlazorIntegrationTests.csproj
```

### Docker Compose

When running tests with Docker, set the environment variables in your docker-compose.yml:

```yaml
services:
  blazor-ui:
    image: estatemanagementuiblazor:latest
    environment:
      - AppSettings__TestMode=true
    ports:
      - "5004:5004"

  tests:
    image: test-runner:latest
    environment:
      - SKIP_REMOTE_CALLS=true
      - APP_TEST_MODE=true
    depends_on:
      - blazor-ui
```

## What Gets Skipped

When `SKIP_REMOTE_CALLS=true`, the following steps skip their remote API calls:

### Security Service Steps
- `Given I create the following api resources`
- `Given I create the following api scopes`
- `Given I create the following clients`
- `Given I create the following identity resources`
- `Given I create the following roles`
- `Given I create the following users`
- `Given I have a token to access the estate management resource`

### Transaction Processor Steps
- `Given I have created the following estates`
- `Given I have created the following operators`
- `Given I have assigned the following operators to the estates`
- `Given I have created the following security users`

### What Still Runs
- All UI interaction steps (clicking, typing, verifying)
- Browser navigation and page interactions
- Screenshot capture on failures

## Test Data

When running in skip remote calls mode:

1. **Default Test Data**: The TestDataStore initializes with default data:
   - 1 Estate (ID: `11111111-1111-1111-1111-111111111111`)
   - 3 Merchants (MERCH001, MERCH002, MERCH003)
   - 2 Operators (Safaricom, Voucher)
   - 2 Contracts with products

2. **Test Context**: The TestingContext is populated with dummy IDs and references so tests can continue

3. **Authentication**: A dummy token is set instead of requesting a real OAuth token

## Example Scenarios

### Full Integration Test (Default)
```bash
# Requires all Docker containers: SecurityService, TransactionProcessor, UI
export SKIP_REMOTE_CALLS=false
dotnet test
```

### UI-Only Test (Fast)
```bash
# Requires only UI Docker container
export SKIP_REMOTE_CALLS=true
export APP_TEST_MODE=true
dotnet test
```

## Configuration Checks

The test infrastructure uses `TestConfiguration.SkipRemoteCalls` to check if remote calls should be skipped. You can verify the configuration:

```csharp
if (TestConfiguration.SkipRemoteCalls)
{
    // Skip remote API call
    return;
}

// Normal remote API call
await this.SecurityServiceSteps.SomeRemoteCall(...);
```

## Benefits

### Faster Tests
- No Docker container startup for backend services (~30-60s saved)
- No network latency for API calls
- Immediate test execution

### Simpler Setup
- Only UI container required
- No backend service configuration
- Reduced infrastructure complexity

### Local Development
- Easier to run tests locally
- No need for complex Docker setup
- Quick feedback loop

## Limitations

When running with skip remote calls:

1. **Backend Logic Not Tested**: Only UI behavior is tested, not backend integration
2. **Default Data Only**: Tests work with pre-configured test data
3. **Limited Scenarios**: Some complex scenarios may require backend services

## Combining With Test Mode

For full UI-only testing, enable both features:

```bash
# In the Blazor application
export AppSettings__TestMode=true

# In the test project
export SKIP_REMOTE_CALLS=true
export APP_TEST_MODE=true
```

This combination:
- Skips OIDC authentication (TestAuthenticationHandler)
- Uses in-memory data (TestDataStore)
- Skips remote API calls in tests (TestConfiguration.SkipRemoteCalls)

## Troubleshooting

### Tests still making remote calls
- Verify `SKIP_REMOTE_CALLS` environment variable is set to `true`
- Check that the step definition uses `TestConfiguration.SkipRemoteCalls`

### Test data not found
- Ensure `APP_TEST_MODE=true` is set in the Blazor application
- Verify the TestDataStore is initialized with default data
- Check that the test estate ID matches the default (`11111111-1111-1111-1111-111111111111`)

### Authentication failures
- Confirm `AppSettings__TestMode=true` in the Blazor application
- Verify TestAuthenticationHandler is registered
- Check that dummy token is set in test context

## Migration Guide

To convert existing tests to support skip remote calls mode:

1. **No changes needed** - Tests automatically respect the environment variable
2. **Optional**: Add custom test data setup if default data is insufficient
3. **Verify**: Run tests with `SKIP_REMOTE_CALLS=true` to ensure they pass

## See Also

- [TEST_INFRASTRUCTURE.md](./TEST_INFRASTRUCTURE.md) - Complete test infrastructure guide
- [IMPLEMENTATION_GUIDE.md](../IMPLEMENTATION_GUIDE.md) - Implementation details
- [TestConfiguration.cs](./Common/TestConfiguration.cs) - Configuration source code
