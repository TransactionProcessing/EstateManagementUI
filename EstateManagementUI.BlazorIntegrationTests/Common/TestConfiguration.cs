using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace EstateManagementUI.BlazorIntegrationTests.Common;

/// <summary>
/// Configuration for test execution mode
/// Controls whether tests make remote calls to backend services or use in-memory test data
/// </summary>
public static class TestConfiguration
{
    private static IConfiguration? _configuration;
    private static readonly object _lock = new object();

    /// <summary>
    /// Gets the configuration instance, loading from appsettings.json if not already loaded
    /// </summary>
    private static IConfiguration Configuration
    {
        get
        {
            if (_configuration == null)
            {
                lock (_lock)
                {
                    if (_configuration == null)
                    {
                        var builder = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
                        
                        _configuration = builder.Build();
                    }
                }
            }
            return _configuration;
        }
    }

    /// <summary>
    /// Gets whether the test is running in UI-only mode (skipping remote backend calls)
    /// When true, tests will:
    /// - Skip remote API calls to SecurityService, TransactionProcessor, etc.
    /// - Use in-memory test data via TestDataStore
    /// - Only interact with the UI container
    /// 
    /// Configured via appsettings.json: TestSettings:SkipRemoteCalls
    /// Falls back to environment variable: SKIP_REMOTE_CALLS
    /// </summary>
    public static bool SkipRemoteCalls
    {
        get
        {
            // First check appsettings.json
            var configValue = Configuration["TestSettings:SkipRemoteCalls"];
            if (!string.IsNullOrEmpty(configValue))
            {
                return configValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                       configValue.Equals("1", StringComparison.OrdinalIgnoreCase);
            }

            // Fall back to environment variable for backward compatibility
            var envValue = Environment.GetEnvironmentVariable("SKIP_REMOTE_CALLS");
            return !string.IsNullOrEmpty(envValue) && 
                   (envValue.Equals("true", StringComparison.OrdinalIgnoreCase) || 
                    envValue.Equals("1", StringComparison.OrdinalIgnoreCase));
        }
    }

    /// <summary>
    /// Gets whether the application is running in test mode (with TestDataStore)
    /// This should match the AppSettings:TestMode in the Blazor application
    /// 
    /// Configured via appsettings.json: TestSettings:EnableTestMode
    /// Falls back to environment variable: APP_TEST_MODE
    /// </summary>
    public static bool IsTestMode
    {
        get
        {
            // First check appsettings.json
            var configValue = Configuration["TestSettings:EnableTestMode"];
            if (!string.IsNullOrEmpty(configValue))
            {
                return configValue.Equals("true", StringComparison.OrdinalIgnoreCase) ||
                       configValue.Equals("1", StringComparison.OrdinalIgnoreCase);
            }

            // Fall back to environment variable for backward compatibility
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
