using System.Security.Claims;
using EstateManagementUI.Common;
using Shouldly;

namespace EstateManagementUI.UITests;

public class HelpersTests
{
    [Fact]
    public void GetClaimValue_ValidClaim_ReturnsClaimValue()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(Helpers.EstateIdClaimType, "12345")
        };
        var claimsIdentity = new ClaimsIdentity(claims);

        // Act
        var result = Helpers.GetClaimValue<string>(claimsIdentity, Helpers.EstateIdClaimType);

        // Assert
        result.ShouldBe("12345");
    }

    [Fact]
    public void GetClaimValue_InvalidClaim_ThrowsInvalidOperationException()
    {
        // Arrange
        var claims = new List<Claim>();
        var claimsIdentity = new ClaimsIdentity(claims);

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => Helpers.GetClaimValue<string>(claimsIdentity, Helpers.EstateIdClaimType));
    }

    [Fact]
    public void GetSecurityServiceAddresses_ValidInputs_ReturnsCorrectAddresses()
    {
        // Arrange
        string authority = "https://localhost:5000";
        string securityServiceLocalPort = "5001";
        string securityServicePort = "5002";

        // Act
        var (authorityAddress, issuerAddress) = Helpers.GetSecurityServiceAddresses(authority, securityServiceLocalPort, securityServicePort);

        // Assert
        authorityAddress.ShouldBe("https://localhost:5001");
        issuerAddress.ShouldBe("https://localhost:5002");
    }

    [Fact]
    public void SafeDivision_ValidInputs_ReturnsCorrectResult()
    {
        // Arrange
        decimal numerator = 10;
        decimal denominator = 2;

        // Act
        var result = numerator.SafeDivision(denominator);

        // Assert
        result.ShouldBe(5);
    }

    [Fact]
    public void SafeDivision_DivideByZero_ReturnsZero()
    {
        // Arrange
        decimal numerator = 10;
        decimal denominator = 0;

        // Act
        var result = numerator.SafeDivision(denominator);

        // Assert
        result.ShouldBe(0);
    }

    [Theory]
    [InlineData(-0.1, true, "info-box bg-success")]
    [InlineData(0, true, "info-box bg-info")]
    [InlineData(0.1, true, "info-box bg-warning")]
    [InlineData(0.3, true, "info-box bg-danger")]
    [InlineData(0.1, false, "info-box bg-success")]
    [InlineData(0, false, "info-box bg-info")]
    [InlineData(-0.1, false, "info-box bg-warning")]
    [InlineData(-0.3, false, "info-box bg-danger")]
    public async Task RenderKpiCardClass_ValidInputs_ReturnsCorrectClass(decimal variance, bool lessIsGood, string expectedClass)
    {
        // Act
        var result = await Helpers.RenderKpiCardClass(variance, lessIsGood);

        // Assert
        result.ShouldBe(expectedClass);
    }
}