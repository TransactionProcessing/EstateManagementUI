using EstateManagementUI.BlazorServer.Permissions;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Permissions;

public class PermissionKeyProviderTests
{
    [Fact]
    public void GetKey_ReturnsNonEmptyKey()
    {
        // Arrange
        var provider = new PermissionKeyProvider();
        
        // Act
        var key = provider.GetKey();
        
        // Assert
        key.ShouldNotBeNullOrEmpty();
    }
    
    [Fact]
    public void GetKey_ReturnsSameKeyWhenCalledMultipleTimes()
    {
        // Arrange
        var provider = new PermissionKeyProvider();
        
        // Act
        var key1 = provider.GetKey();
        var key2 = provider.GetKey();
        
        // Assert
        key1.ShouldBe(key2);
    }
    
    [Fact]
    public void RefreshKey_ChangesKey()
    {
        // Arrange
        var provider = new PermissionKeyProvider();
        var originalKey = provider.GetKey();
        
        // Act
        provider.RefreshKey();
        var newKey = provider.GetKey();
        
        // Assert
        newKey.ShouldNotBe(originalKey);
    }
    
    [Fact]
    public void RefreshKey_GeneratesUniqueKeys()
    {
        // Arrange
        var provider = new PermissionKeyProvider();
        var keys = new HashSet<string>();
        
        // Act
        for (int i = 0; i < 100; i++)
        {
            provider.RefreshKey();
            keys.Add(provider.GetKey());
        }
        
        // Assert
        keys.Count.ShouldBe(100);
    }
}
