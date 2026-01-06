using System;

namespace EstateManagementUI.BlazorIntegrationTests.Common;

/// <summary>
/// Configuration for test execution mode
/// Controls whether tests make remote calls to backend services or use in-memory test data
/// </summary>
public static class TestConfiguration
{
    /// <summary>
    /// Gets whether the test is running in UI-only mode (skipping remote backend calls)
    /// When true, tests will:
    /// - Skip remote API calls to SecurityService, TransactionProcessor, etc.
    /// - Use in-memory test data via TestDataStore
    /// - Only interact with the UI container
    /// 
    /// Set via environment variable: SKIP_REMOTE_CALLS=true
    /// </summary>
    public static bool SkipRemoteCalls
    {
        get
        {
            var envValue = Environment.GetEnvironmentVariable("SKIP_REMOTE_CALLS");
            return !string.IsNullOrEmpty(envValue) && 
                   (envValue.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                    envValue.Equals("1", StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Gets whether the application is running in test mode (with TestDataStore)
    /// This should match the AppSettings:TestMode in the Blazor application
    /// </summary>
    public static bool IsTestMode
    {
        get
        {
            var envValue = Environment.GetEnvironmentVariable("APP_TEST_MODE");
            return !string.IsNullOrEmpty(envValue) && 
                   (envValue.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                    envValue.Equals("1", StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Gets whether to use UI-only test mode (combination of skip remote calls and test mode)
    /// </summary>
    public static bool IsUIOnlyTestMode => SkipRemoteCalls && IsTestMode;
}
