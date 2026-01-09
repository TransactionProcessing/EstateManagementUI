using EstateManagementUI.BlazorServer.Permissions;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace EstateManagementUI.BlazorServer.Tests.Permissions;

public class PermissionServiceTests
{
    private readonly Mock<AuthenticationStateProvider> _authStateProvider;
    private readonly Mock<IPermissionStore> _permissionStore;
    private readonly Mock<ILogger<PermissionService>> _logger;
    private readonly PermissionService _permissionService;

    public PermissionServiceTests()
    {
        _authStateProvider = new Mock<AuthenticationStateProvider>();
        _permissionStore = new Mock<IPermissionStore>();
        _logger = new Mock<ILogger<PermissionService>>();
        _permissionService = new PermissionService(_authStateProvider.Object, _permissionStore.Object, _logger.Object);
    }

    [Fact]
    public async Task HasPermissionAsync_WithValidPermission_ReturnsTrue()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View),
            new Permission(PermissionSection.Merchant, PermissionFunction.Create)
        };
        var role = new Role("Estate", permissions);
        
        var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Estate") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        _permissionStore.Setup(x => x.GetRoleAsync("Estate")).ReturnsAsync(role);
        
        // Act
        var result = await _permissionService.HasPermissionAsync(PermissionSection.Merchant, PermissionFunction.View);
        
        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task HasPermissionAsync_WithInvalidPermission_ReturnsFalse()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View)
        };
        var role = new Role("Viewer", permissions);
        
        var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Viewer") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        _permissionStore.Setup(x => x.GetRoleAsync("Viewer")).ReturnsAsync(role);
        
        // Act
        var result = await _permissionService.HasPermissionAsync(PermissionSection.Merchant, PermissionFunction.Create);
        
        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasPermissionAsync_WithUnauthenticatedUser_ReturnsFalse()
    {
        // Arrange
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        
        // Act
        var result = await _permissionService.HasPermissionAsync(PermissionSection.Merchant, PermissionFunction.View);
        
        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HasSectionAccessAsync_WithAccessToSection_ReturnsTrue()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View),
            new Permission(PermissionSection.Merchant, PermissionFunction.Create)
        };
        var role = new Role("Estate", permissions);
        
        var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Estate") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        _permissionStore.Setup(x => x.GetRoleAsync("Estate")).ReturnsAsync(role);
        
        // Act
        var result = await _permissionService.HasSectionAccessAsync(PermissionSection.Merchant);
        
        // Assert
        result.ShouldBeTrue();
    }

    [Fact]
    public async Task HasSectionAccessAsync_WithoutAccessToSection_ReturnsFalse()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View)
        };
        var role = new Role("Viewer", permissions);
        
        var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Viewer") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        _permissionStore.Setup(x => x.GetRoleAsync("Viewer")).ReturnsAsync(role);
        
        // Act
        var result = await _permissionService.HasSectionAccessAsync(PermissionSection.Operator);
        
        // Assert
        result.ShouldBeFalse();
    }

    [Fact]
    public async Task GetUserPermissionsAsync_WithValidRole_ReturnsPermissions()
    {
        // Arrange
        var permissions = new List<Permission>
        {
            new Permission(PermissionSection.Merchant, PermissionFunction.View),
            new Permission(PermissionSection.Operator, PermissionFunction.View)
        };
        var role = new Role("Viewer", permissions);
        
        var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Viewer") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        _permissionStore.Setup(x => x.GetRoleAsync("Viewer")).ReturnsAsync(role);
        
        // Act
        var result = await _permissionService.GetUserPermissionsAsync();
        
        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Fact]
    public async Task GetUserPermissionsAsync_WithNoRole_ReturnsEmptyList()
    {
        // Arrange
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        
        // Act
        var result = await _permissionService.GetUserPermissionsAsync();
        
        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Fact]
    public async Task GetUserRoleAsync_WithValidRole_ReturnsRoleName()
    {
        // Arrange
        var claims = new List<Claim> { new Claim(ClaimTypes.Role, "Estate") };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        
        // Act
        var result = await _permissionService.GetUserRoleAsync();
        
        // Assert
        result.ShouldBe("Estate");
    }

    [Fact]
    public async Task GetUserRoleAsync_WithUnauthenticatedUser_ReturnsNull()
    {
        // Arrange
        var identity = new ClaimsIdentity();
        var user = new ClaimsPrincipal(identity);
        var authState = new AuthenticationState(user);
        
        _authStateProvider.Setup(x => x.GetAuthenticationStateAsync()).ReturnsAsync(authState);
        
        // Act
        var result = await _permissionService.GetUserRoleAsync();
        
        // Assert
        result.ShouldBeNull();
    }
}
