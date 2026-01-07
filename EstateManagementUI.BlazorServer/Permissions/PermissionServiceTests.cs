using EstateManagementUI.BlazorServer.Permissions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
using Xunit;

namespace EstateManagementUI.BlazorServer.Tests.Permissions;

/// <summary>
/// Unit tests for the Blazor permission service
/// </summary>
public class PermissionServiceTests
{
    private readonly Mock<AuthenticationStateProvider> _authStateProviderMock;
    private readonly Mock<IPermissionStore> _permissionStoreMock;
    private readonly Mock<ILogger<PermissionService>> _loggerMock;
    private readonly PermissionService _permissionService;

    public PermissionServiceTests()
    {
        _authStateProviderMock = new Mock<AuthenticationStateProvider>();
        _permissionStoreMock = new Mock<IPermissionStore>();
        _loggerMock = new Mock<ILogger<PermissionService>>();
        _permissionService = new PermissionService(
            _authStateProviderMock.Object,
            _permissionStoreMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task HasPermissionAsync_UserNotAuthenticated_ReturnsFalse()
    {
        // Arrange
        var user = new ClaimsPrincipal();
        var authState = new AuthenticationState(user);
        _authStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        // Act
        var result = await _permissionService.HasPermissionAsync(
            PermissionSection.Merchant, 
            PermissionFunction.View
        );

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task HasPermissionAsync_UserHasPermission_ReturnsTrue()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Role, "Administrator")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);

        var role = new Role("Administrator", new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View)
        });

        _authStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);
        _permissionStoreMock.Setup(x => x.GetRoleAsync("Administrator"))
            .ReturnsAsync(role);

        // Act
        var result = await _permissionService.HasPermissionAsync(
            PermissionSection.Merchant,
            PermissionFunction.View
        );

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HasPermissionAsync_UserDoesNotHavePermission_ReturnsFalse()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Role, "Viewer")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);

        var role = new Role("Viewer", new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View)
        });

        _authStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);
        _permissionStoreMock.Setup(x => x.GetRoleAsync("Viewer"))
            .ReturnsAsync(role);

        // Act
        var result = await _permissionService.HasPermissionAsync(
            PermissionSection.Merchant,
            PermissionFunction.Edit  // Viewer doesn't have Edit permission
        );

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task HasSectionAccessAsync_UserHasAnyPermissionInSection_ReturnsTrue()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Role, "Viewer")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);

        var role = new Role("Viewer", new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View)
        });

        _authStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);
        _permissionStoreMock.Setup(x => x.GetRoleAsync("Viewer"))
            .ReturnsAsync(role);

        // Act
        var result = await _permissionService.HasSectionAccessAsync(PermissionSection.Merchant);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task HasSectionAccessAsync_UserHasNoPermissionInSection_ReturnsFalse()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Role, "Viewer")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);

        var role = new Role("Viewer", new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View)
        });

        _authStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);
        _permissionStoreMock.Setup(x => x.GetRoleAsync("Viewer"))
            .ReturnsAsync(role);

        // Act
        var result = await _permissionService.HasSectionAccessAsync(PermissionSection.Operator);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetUserRoleAsync_AuthenticatedUser_ReturnsRole()
    {
        // Arrange
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "test-user"),
            new Claim(ClaimTypes.Name, "Test User"),
            new Claim(ClaimTypes.Role, "Administrator")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);

        _authStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        // Act
        var result = await _permissionService.GetUserRoleAsync();

        // Assert
        Assert.Equal("Administrator", result);
    }

    [Fact]
    public async Task GetUserRoleAsync_NotAuthenticated_ReturnsNull()
    {
        // Arrange
        var user = new ClaimsPrincipal();
        var authState = new AuthenticationState(user);
        _authStateProviderMock.Setup(x => x.GetAuthenticationStateAsync())
            .ReturnsAsync(authState);

        // Act
        var result = await _permissionService.GetUserRoleAsync();

        // Assert
        Assert.Null(result);
    }
}

/// <summary>
/// Unit tests for the InMemoryPermissionStore
/// </summary>
public class InMemoryPermissionStoreTests
{
    [Fact]
    public async Task GetRoleAsync_AdministratorRole_ReturnsFullPermissions()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<InMemoryPermissionStore>>();
        var environmentMock = new Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
        environmentMock.Setup(x => x.ContentRootPath).Returns(Path.GetTempPath());
        
        var store = new InMemoryPermissionStore(loggerMock.Object, environmentMock.Object);

        // Act
        var role = await store.GetRoleAsync("Administrator");

        // Assert
        Assert.NotNull(role);
        Assert.Equal("Administrator", role.Name);
        
        // Administrator should have all permissions
        var expectedPermissionCount = Enum.GetValues(typeof(PermissionSection)).Length * 
                                      Enum.GetValues(typeof(PermissionFunction)).Length;
        Assert.Equal(expectedPermissionCount, role.Permissions.Count);
    }

    [Fact]
    public async Task GetRoleAsync_ViewerRole_ReturnsOnlyViewPermissions()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<InMemoryPermissionStore>>();
        var environmentMock = new Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
        environmentMock.Setup(x => x.ContentRootPath).Returns(Path.GetTempPath());
        
        var store = new InMemoryPermissionStore(loggerMock.Object, environmentMock.Object);

        // Act
        var role = await store.GetRoleAsync("Viewer");

        // Assert
        Assert.NotNull(role);
        Assert.Equal("Viewer", role.Name);
        
        // Viewer should only have View permissions for all sections
        Assert.All(role.Permissions, p => Assert.Equal(PermissionFunction.View, p.Function));
        Assert.Equal(Enum.GetValues(typeof(PermissionSection)).Length, role.Permissions.Count);
    }

    [Fact]
    public async Task GetRoleAsync_MerchantManagerRole_HasFullMerchantAccess()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<InMemoryPermissionStore>>();
        var environmentMock = new Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
        environmentMock.Setup(x => x.ContentRootPath).Returns(Path.GetTempPath());
        
        var store = new InMemoryPermissionStore(loggerMock.Object, environmentMock.Object);

        // Act
        var role = await store.GetRoleAsync("MerchantManager");

        // Assert
        Assert.NotNull(role);
        var merchantPermissions = role.Permissions
            .Where(p => p.Section == PermissionSection.Merchant)
            .ToList();
        
        // Should have all merchant functions
        Assert.Contains(merchantPermissions, p => p.Function == PermissionFunction.View);
        Assert.Contains(merchantPermissions, p => p.Function == PermissionFunction.Create);
        Assert.Contains(merchantPermissions, p => p.Function == PermissionFunction.Edit);
        Assert.Contains(merchantPermissions, p => p.Function == PermissionFunction.Delete);
        Assert.Contains(merchantPermissions, p => p.Function == PermissionFunction.MakeDeposit);
    }

    [Fact]
    public async Task GetRoleAsync_NonExistentRole_ReturnsNull()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<InMemoryPermissionStore>>();
        var environmentMock = new Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
        environmentMock.Setup(x => x.ContentRootPath).Returns(Path.GetTempPath());
        
        var store = new InMemoryPermissionStore(loggerMock.Object, environmentMock.Object);

        // Act
        var role = await store.GetRoleAsync("NonExistentRole");

        // Assert
        Assert.Null(role);
    }

    [Fact]
    public async Task SaveRoleAsync_NewRole_RoleCanBeRetrieved()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<InMemoryPermissionStore>>();
        var environmentMock = new Mock<Microsoft.AspNetCore.Hosting.IWebHostEnvironment>();
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempPath);
        environmentMock.Setup(x => x.ContentRootPath).Returns(tempPath);
        
        var store = new InMemoryPermissionStore(loggerMock.Object, environmentMock.Object);
        var newRole = new Role("TestRole", new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View)
        });

        // Act
        await store.SaveRoleAsync(newRole);
        var retrievedRole = await store.GetRoleAsync("TestRole");

        // Assert
        Assert.NotNull(retrievedRole);
        Assert.Equal("TestRole", retrievedRole.Name);
        Assert.Single(retrievedRole.Permissions);

        // Cleanup
        Directory.Delete(tempPath, true);
    }
}
