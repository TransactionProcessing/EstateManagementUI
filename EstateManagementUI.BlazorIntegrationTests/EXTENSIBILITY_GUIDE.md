# Integration Testing Framework - Extensibility Guide

## Overview

This document describes how the offline integration testing framework can be extended to support real authentication endpoints and backend APIs in the future. The framework is designed with extensibility in mind, allowing you to switch between offline testing mode and full integration testing mode.

## Architecture

The framework uses a layered architecture with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────────┐
│                    Test Layer (Feature Files)               │
│                     Reqnroll/SpecFlow BDD                   │
└─────────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│                  Step Definitions Layer                      │
│              (BlazorUiSteps, SharedSteps, etc.)             │
└─────────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│                   Helper/Service Layer                       │
│         (BlazorUiHelpers, TestDataHelper, etc.)             │
└─────────────────────────────────────────────────────────────┘
                            │
┌─────────────────────────────────────────────────────────────┐
│                  Configuration Layer                         │
│              (TestConfiguration, appsettings)               │
└─────────────────────────────────────────────────────────────┘
                            │
            ┌───────────────┴───────────────┐
            │                               │
┌───────────▼─────────────┐    ┌───────────▼──────────────┐
│   Offline Mode          │    │   Online Mode            │
│   - TestDataStore       │    │   - Real Auth Endpoint   │
│   - TestAuthHandler     │    │   - Real Backend API     │
│   - In-Memory Data      │    │   - Real Database        │
└─────────────────────────┘    └──────────────────────────┘
```

## Configuration-Based Mode Switching

The framework uses configuration settings to determine which mode to operate in:

### Current Configuration (Offline Mode)

**appsettings.json:**
```json
{
  "TestSettings": {
    "SkipRemoteCalls": true,
    "EnableTestMode": true,
    "UseRealAuthentication": false,
    "UseRealBackend": false
  }
}
```

**Blazor App appsettings.Test.json:**
```json
{
  "AppSettings": {
    "TestMode": true
  }
}
```

### Future Configuration (Online Mode)

**appsettings.json:**
```json
{
  "TestSettings": {
    "SkipRemoteCalls": false,
    "EnableTestMode": false,
    "UseRealAuthentication": true,
    "UseRealBackend": true,
    "AuthEndpoint": "https://auth.example.com",
    "BackendApiEndpoint": "https://api.example.com"
  }
}
```

**Blazor App appsettings.Test.json:**
```json
{
  "AppSettings": {
    "TestMode": false
  },
  "Authentication": {
    "Authority": "https://auth.example.com",
    "ClientId": "estateUIClient",
    "ClientSecret": "Secret1"
  }
}
```

## Extending to Real Authentication

### 1. Authentication Service Interface

Create an abstraction for authentication:

**File: `Common/IAuthenticationService.cs`**
```csharp
public interface IAuthenticationService
{
    Task<string> AuthenticateAsync(string username, string password);
    Task<bool> ValidateTokenAsync(string token);
    Task LogoutAsync(string token);
}
```

### 2. Offline Authentication Implementation (Current)

**File: `Common/OfflineAuthenticationService.cs`**
```csharp
public class OfflineAuthenticationService : IAuthenticationService
{
    public Task<string> AuthenticateAsync(string username, string password)
    {
        // Return dummy token for offline testing
        return Task.FromResult("offline-test-token-12345");
    }

    public Task<bool> ValidateTokenAsync(string token)
    {
        return Task.FromResult(true);
    }

    public Task LogoutAsync(string token)
    {
        return Task.CompletedTask;
    }
}
```

### 3. Online Authentication Implementation (Future)

**File: `Common/OnlineAuthenticationService.cs`**
```csharp
public class OnlineAuthenticationService : IAuthenticationService
{
    private readonly HttpClient _httpClient;
    private readonly TestConfiguration _config;

    public OnlineAuthenticationService(HttpClient httpClient, TestConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var authEndpoint = _config.AuthEndpoint;
        var request = new AuthenticationRequest
        {
            Username = username,
            Password = password,
            ClientId = _config.ClientId,
            ClientSecret = _config.ClientSecret
        };

        var response = await _httpClient.PostAsJsonAsync($"{authEndpoint}/connect/token", request);
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
        return result.AccessToken;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var authEndpoint = _config.AuthEndpoint;
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
            
        var response = await _httpClient.GetAsync($"{authEndpoint}/connect/userinfo");
        return response.IsSuccessStatusCode;
    }

    public async Task LogoutAsync(string token)
    {
        var authEndpoint = _config.AuthEndpoint;
        await _httpClient.PostAsync($"{authEndpoint}/connect/logout", null);
    }
}
```

### 4. Service Registration with Mode Selection

**File: `Common/Setup.cs` (Enhanced)**
```csharp
[BeforeTestRun]
public static void RegisterServices()
{
    var config = TestConfiguration.Load();
    
    // Register authentication service based on configuration
    if (config.UseRealAuthentication)
    {
        // Register online authentication
        var httpClient = new HttpClient();
        var authService = new OnlineAuthenticationService(httpClient, config);
        ServiceLocator.Register<IAuthenticationService>(authService);
    }
    else
    {
        // Register offline authentication (current default)
        var authService = new OfflineAuthenticationService();
        ServiceLocator.Register<IAuthenticationService>(authService);
    }
}
```

## Extending to Real Backend API

### 1. Backend Service Interface

Create an abstraction for backend operations:

**File: `Common/IBackendService.cs`**
```csharp
public interface IBackendService
{
    Task<Estate> GetEstateAsync(Guid estateId);
    Task<List<Merchant>> GetMerchantsAsync(Guid estateId);
    Task<Merchant> CreateMerchantAsync(Guid estateId, CreateMerchantRequest request);
    Task UpdateMerchantAsync(Guid estateId, Guid merchantId, UpdateMerchantRequest request);
    Task<List<Operator>> GetOperatorsAsync(Guid estateId);
    Task<List<Contract>> GetContractsAsync(Guid estateId);
    // ... more methods
}
```

### 2. Offline Backend Implementation (Current)

**File: `Common/OfflineBackendService.cs`**
```csharp
public class OfflineBackendService : IBackendService
{
    private readonly ITestDataStore _testDataStore;

    public OfflineBackendService(ITestDataStore testDataStore)
    {
        _testDataStore = testDataStore;
    }

    public Task<Estate> GetEstateAsync(Guid estateId)
    {
        return Task.FromResult(_testDataStore.GetEstate(estateId));
    }

    public Task<List<Merchant>> GetMerchantsAsync(Guid estateId)
    {
        return Task.FromResult(_testDataStore.GetMerchants(estateId));
    }

    // ... implementations using TestDataStore
}
```

### 3. Online Backend Implementation (Future)

**File: `Common/OnlineBackendService.cs`**
```csharp
public class OnlineBackendService : IBackendService
{
    private readonly HttpClient _httpClient;
    private readonly TestConfiguration _config;
    private readonly IAuthenticationService _authService;

    public OnlineBackendService(
        HttpClient httpClient, 
        TestConfiguration config,
        IAuthenticationService authService)
    {
        _httpClient = httpClient;
        _config = config;
        _authService = authService;
    }

    public async Task<Estate> GetEstateAsync(Guid estateId)
    {
        var token = await GetAuthTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
            
        var response = await _httpClient.GetAsync(
            $"{_config.BackendApiEndpoint}/api/estates/{estateId}");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Estate>();
    }

    public async Task<List<Merchant>> GetMerchantsAsync(Guid estateId)
    {
        var token = await GetAuthTokenAsync();
        _httpClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", token);
            
        var response = await _httpClient.GetAsync(
            $"{_config.BackendApiEndpoint}/api/estates/{estateId}/merchants");
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<List<Merchant>>();
    }

    private async Task<string> GetAuthTokenAsync()
    {
        // Get token from auth service or cache
        return await _authService.GetCachedTokenAsync();
    }

    // ... more implementations calling real API
}
```

### 4. Service Registration for Backend

**File: `Common/Setup.cs` (Enhanced)**
```csharp
[BeforeTestRun]
public static void RegisterBackendService()
{
    var config = TestConfiguration.Load();
    
    // Register backend service based on configuration
    if (config.UseRealBackend)
    {
        // Register online backend
        var httpClient = new HttpClient();
        var authService = ServiceLocator.Resolve<IAuthenticationService>();
        var backendService = new OnlineBackendService(httpClient, config, authService);
        ServiceLocator.Register<IBackendService>(backendService);
    }
    else
    {
        // Register offline backend (current default)
        var testDataStore = TestDataHelper.GetTestDataStore();
        var backendService = new OfflineBackendService(testDataStore);
        ServiceLocator.Register<IBackendService>(backendService);
    }
}
```

## Background Data Setup

### 1. Background Data Manager Interface

**File: `Common/IBackgroundDataManager.cs`**
```csharp
public interface IBackgroundDataManager
{
    Task SetupAuthenticationDataAsync();
    Task SetupEstateDataAsync();
    Task SetupMerchantDataAsync();
    Task SetupOperatorDataAsync();
    Task SetupContractDataAsync();
    Task TeardownDataAsync();
}
```

### 2. Offline Background Data Manager (Current)

**File: `Common/OfflineBackgroundDataManager.cs`**
```csharp
public class OfflineBackgroundDataManager : IBackgroundDataManager
{
    private readonly ITestDataStore _testDataStore;

    public async Task SetupAuthenticationDataAsync()
    {
        // No-op for offline mode - TestAuthenticationHandler handles this
        await Task.CompletedTask;
    }

    public async Task SetupEstateDataAsync()
    {
        // TestDataStore already has default data
        await Task.CompletedTask;
    }

    public async Task TeardownDataAsync()
    {
        _testDataStore.Reset();
        await Task.CompletedTask;
    }
}
```

### 3. Online Background Data Manager (Future)

**File: `Common/OnlineBackgroundDataManager.cs`**
```csharp
public class OnlineBackgroundDataManager : IBackgroundDataManager
{
    private readonly IAuthenticationService _authService;
    private readonly IBackendService _backendService;
    private readonly List<Guid> _createdResources = new();

    public async Task SetupAuthenticationDataAsync()
    {
        // Create test users, roles, clients via real auth endpoint
        var adminToken = await _authService.AuthenticateAsync(
            "admin@test.com", "admin-password");
            
        // Create roles
        await CreateRoleAsync(adminToken, "Estate");
        await CreateRoleAsync(adminToken, "Merchant");
        
        // Create API scopes
        await CreateApiScopeAsync(adminToken, "estateManagement");
        
        // Create test user
        var userId = await CreateUserAsync(adminToken, "testuser@test.com");
        _createdResources.Add(userId);
    }

    public async Task SetupEstateDataAsync()
    {
        var token = await _authService.GetTokenAsync();
        
        // Create test estate via real API
        var estateId = await _backendService.CreateEstateAsync(
            new CreateEstateRequest { Name = "Test Estate" });
        _createdResources.Add(estateId);
    }

    public async Task TeardownDataAsync()
    {
        // Clean up all created resources
        var token = await _authService.GetTokenAsync();
        
        foreach (var resourceId in _createdResources)
        {
            await DeleteResourceAsync(token, resourceId);
        }
        
        _createdResources.Clear();
    }
}
```

## Step Definition Updates

Update step definitions to use the service abstractions:

**File: `Steps/BlazorUiSteps.cs` (Enhanced)**
```csharp
[Binding]
public class BlazorUiSteps
{
    private readonly IBackendService _backendService;
    private readonly IAuthenticationService _authService;
    
    public BlazorUiSteps(ScenarioContext scenarioContext)
    {
        // Services are resolved based on current configuration
        _backendService = ServiceLocator.Resolve<IBackendService>();
        _authService = ServiceLocator.Resolve<IAuthenticationService>();
    }

    [Given(@"I have created the following merchants")]
    public async Task GivenIHaveCreatedTheFollowingMerchants(Table table)
    {
        foreach (var row in table.Rows)
        {
            var estateId = GetEstateId(row["EstateName"]);
            var request = new CreateMerchantRequest
            {
                Name = row["MerchantName"],
                // ... map other fields
            };
            
            // Works for both offline and online modes
            var merchant = await _backendService.CreateMerchantAsync(estateId, request);
            StoreCreatedMerchant(merchant);
        }
    }
}
```

## Configuration File Structure

### Enhanced TestConfiguration.cs

**File: `Common/TestConfiguration.cs` (Enhanced)**
```csharp
public class TestConfiguration
{
    // Current properties
    public bool SkipRemoteCalls { get; set; }
    public bool EnableTestMode { get; set; }
    
    // New properties for extensibility
    public bool UseRealAuthentication { get; set; }
    public bool UseRealBackend { get; set; }
    public string AuthEndpoint { get; set; }
    public string BackendApiEndpoint { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string AdminUsername { get; set; }
    public string AdminPassword { get; set; }
    
    // Database settings for background data
    public DatabaseSettings Database { get; set; }
    
    public static TestConfiguration Load()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var config = new TestConfiguration();
        configuration.GetSection("TestSettings").Bind(config);
        
        return config;
    }
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
    public bool AutoMigrate { get; set; }
    public bool CleanupAfterTest { get; set; }
}
```

## Migration Path

### Phase 1: Current State (Offline Testing) ✅
- TestMode enabled
- TestDataStore for all data
- TestAuthenticationHandler for auth
- No external dependencies

### Phase 2: Hybrid Mode (Partial Integration)
- Add authentication service abstraction
- Support real auth endpoint with test data
- Keep TestDataStore for business data
- Step definitions use IAuthenticationService

### Phase 3: Full Integration Mode
- Add backend service abstraction
- Support real backend API
- Support real database
- Complete background data setup/teardown
- Step definitions use IBackendService

### Phase 4: Flexible Testing
- Run tests in any mode via configuration
- Support mixed mode (real auth + test data)
- Performance testing with real backend
- Load testing capabilities

## Usage Examples

### Running Tests in Different Modes

**Offline Mode (Current):**
```bash
dotnet test --settings:test.runsettings
```

**With Real Authentication:**
```bash
dotnet test --settings:test.runsettings \
  -- TestSettings.UseRealAuthentication=true \
     TestSettings.AuthEndpoint=https://auth.example.com
```

**Full Integration Mode:**
```bash
dotnet test --settings:test.runsettings \
  -- TestSettings.UseRealAuthentication=true \
     TestSettings.UseRealBackend=true \
     TestSettings.AuthEndpoint=https://auth.example.com \
     TestSettings.BackendApiEndpoint=https://api.example.com
```

**Mixed Mode (Real Auth + Test Data):**
```bash
dotnet test --settings:test.runsettings \
  -- TestSettings.UseRealAuthentication=true \
     TestSettings.UseRealBackend=false \
     TestSettings.EnableTestMode=true
```

## Benefits of This Architecture

1. **No Breaking Changes**: Existing tests continue to work without modification
2. **Gradual Migration**: Can migrate to real services incrementally
3. **Flexible Testing**: Run same tests in different modes
4. **Easy Debugging**: Test against real services when needed
5. **CI/CD Friendly**: Fast offline tests for PR checks, full integration for releases
6. **Clear Separation**: Business logic separate from infrastructure concerns

## Next Steps

To implement real authentication/backend integration:

1. Create the service interfaces (`IAuthenticationService`, `IBackendService`)
2. Implement offline versions using current infrastructure
3. Update step definitions to use service abstractions
4. Add service registration in `Setup.cs` with mode detection
5. Create online implementations when ready
6. Update configuration files to support new settings
7. Test in hybrid mode before full integration
8. Document the new configuration options

## Conclusion

This architecture provides a clear path forward for extending the testing framework while maintaining backward compatibility. The framework is designed to support any combination of offline/online testing based on your needs.
