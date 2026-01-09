using System;
using Reqnroll;
using Shouldly;

namespace EstateManagementUI.OfflineIntegrationTests.Common
{
    [Binding]
    public class Setup
    {
        public static async Task GlobalSetup()
        {
            ShouldlyConfiguration.DefaultTaskTimeout = TimeSpan.FromMinutes(1);
            await Task.CompletedTask;
        }
    }
}