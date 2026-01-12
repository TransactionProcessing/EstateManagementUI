using EstateManagementUI.BlazorServer.Common;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Common;

public class AuthenticationHelpersTests
{
    [Fact]
    public void GetSecurityServiceAddresses_WithDefaultPorts_ReturnsCorrectAddresses()
    {
        // Arrange
        var authority = "https://localhost:5001";
        
        // Act
        var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
            authority, null, null);
        
        // Assert
        authorityAddress.ShouldBe("https://localhost:5001");
        issuerAddress.ShouldBe("https://localhost:5001");
    }
    
    [Fact]
    public void GetSecurityServiceAddresses_WithCustomLocalPort_ReturnsCorrectAuthorityAddress()
    {
        // Arrange
        var authority = "https://localhost:5001";
        var securityServiceLocalPort = "6001";
        
        // Act
        var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
            authority, securityServiceLocalPort, null);
        
        // Assert
        authorityAddress.ShouldBe("https://localhost:6001");
        issuerAddress.ShouldBe("https://localhost:5001");
    }
    
    [Fact]
    public void GetSecurityServiceAddresses_WithCustomPort_ReturnsCorrectIssuerAddress()
    {
        // Arrange
        var authority = "https://localhost:5001";
        var securityServicePort = "7001";
        
        // Act
        var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
            authority, null, securityServicePort);
        
        // Assert
        authorityAddress.ShouldBe("https://localhost:5001");
        issuerAddress.ShouldBe("https://localhost:7001");
    }
    
    [Fact]
    public void GetSecurityServiceAddresses_WithBothCustomPorts_ReturnsCorrectAddresses()
    {
        // Arrange
        var authority = "https://localhost:5001";
        var securityServiceLocalPort = "6001";
        var securityServicePort = "7001";
        
        // Act
        var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
            authority, securityServiceLocalPort, securityServicePort);
        
        // Assert
        authorityAddress.ShouldBe("https://localhost:6001");
        issuerAddress.ShouldBe("https://localhost:7001");
    }
    
    [Fact]
    public void GetSecurityServiceAddresses_WithTrailingSlash_RemovesTrailingSlash()
    {
        // Arrange
        var authority = "https://localhost:5001/";
        
        // Act
        var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
            authority, null, null);
        
        // Assert
        authorityAddress.ShouldBe("https://localhost:5001");
        issuerAddress.ShouldBe("https://localhost:5001");
    }
    
    [Fact]
    public void GetSecurityServiceAddresses_WithPath_PreservesPath()
    {
        // Arrange
        var authority = "https://localhost:5001/auth";
        var securityServiceLocalPort = "6001";
        
        // Act
        var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
            authority, securityServiceLocalPort, null);
        
        // Assert
        authorityAddress.ShouldBe("https://localhost:6001/auth");
        issuerAddress.ShouldBe("https://localhost:5001/auth");
    }
    
    [Fact]
    public void GetSecurityServiceAddresses_WithEmptyPortStrings_UsesDefaultPort()
    {
        // Arrange
        var authority = "https://localhost:5001";
        
        // Act
        var (authorityAddress, issuerAddress) = AuthenticationHelpers.GetSecurityServiceAddresses(
            authority, string.Empty, string.Empty);
        
        // Assert
        authorityAddress.ShouldBe("https://localhost:5001");
        issuerAddress.ShouldBe("https://localhost:5001");
    }
}
