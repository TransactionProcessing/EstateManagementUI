using System.Security.Claims;
using EstateManagementUI.Pages.Shared.Profile;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;

namespace EstateManagementUI.UITests;

public class ProfileDropdownTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private ProfileDropdown _profileDropdown;
    private readonly Mock<IServiceProvider> _serviceProviderMock;
    private readonly Mock<IAuthenticationService> _authenticationServiceMock;
    private readonly Mock<HttpContext> httpContextMock;
    public ProfileDropdownTests()
    {
        this._httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        this._serviceProviderMock = new Mock<IServiceProvider>();
        this._authenticationServiceMock = new Mock<IAuthenticationService>();
        this.httpContextMock = new Mock<HttpContext>();
        this._serviceProviderMock.Setup(sp => sp.GetService(typeof(IAuthenticationService)))
            .Returns(this._authenticationServiceMock.Object);

        this.httpContextMock.Setup(c => c.RequestServices)
            .Returns(this._serviceProviderMock.Object);
        ClaimsPrincipal principal = new(new ClaimsIdentity(new[]
        {
            new Claim("given_name", "Test"),
            new Claim("family_name", "User"),
            new Claim("registration_date", new DateTime(2024,12,27).ToString("yyyy-MM-dd HH:mm:ss.fff"))
        }));
        this.httpContextMock.Setup(c => c.User)
            .Returns(principal);

        this._httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(this.httpContextMock.Object);

        this._profileDropdown = new ProfileDropdown(this._httpContextAccessorMock.Object);
    }

    [Fact]
    public async Task ProfileDropdown_SignOut_CallsSignOutAsync()
    {
        // Act
        await this._profileDropdown.SignOut();

        // Assert
        this._authenticationServiceMock.Verify(a => a.SignOutAsync(It.IsAny<HttpContext>(), "oidc", null), Times.Once);
        this._authenticationServiceMock.Verify(a => a.SignOutAsync(It.IsAny<HttpContext>(), "Cookies", null), Times.Once);
    }

    [Fact] 
    public async Task ProfileDropdown_UserFullName_IsExpectedValue()
    {
        // Act
        var userFullname = this._profileDropdown.UserFullName;
        userFullname.ShouldBe("Test User");
    }

    [Fact(Skip = "Date form at issues")]
    public async Task ProfileDropdown_RegistrationText_IsExpectedValue()
    {
        // Act
        var registrationText = this._profileDropdown.RegistrationText;
        registrationText.ShouldBe("Registered on 27/12/2024");
    }

    [Fact]
    public async Task ProfileDropdown_NoRegistrationDate_IsExpectedValue()
    {
        ClaimsPrincipal principal = new(new ClaimsIdentity(new[]
        {
            new Claim("given_name", "Test"),
            new Claim("family_name", "User"),
            //new Claim("registration_date", null)
        }));
        this.httpContextMock.Setup(c => c.User)
            .Returns(principal);

        this._httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(this.httpContextMock.Object);

        this._profileDropdown = new ProfileDropdown(this._httpContextAccessorMock.Object);

        // Act
        var registrationText = this._profileDropdown.RegistrationText;
        registrationText.ShouldBe("Registered on Unknown");
    }
}