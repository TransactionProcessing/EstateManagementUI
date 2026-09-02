using EstateManagementUI.BlazorServer.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Imposter.Abstractions;
using Shouldly;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace EstateManagementUI.BlazorServer.Tests.Common;

public class TestAuthenticationHandlerTests
{
    private readonly IOptionsMonitorImposter<AuthenticationSchemeOptions> _options;
    private readonly ILoggerFactoryImposter _loggerFactory;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessorImposter _httpContextAccessor;
    private readonly UrlEncoder _encoder;

    public TestAuthenticationHandlerTests()
    {
        _options = new IOptionsMonitorImposter<AuthenticationSchemeOptions>();
        _loggerFactory = new ILoggerFactoryImposter();
        _httpContextAccessor = new IHttpContextAccessorImposter();
        _encoder = UrlEncoder.Default;

        _options.Get(Arg<string>.Any()).Returns(new AuthenticationSchemeOptions());
        _loggerFactory.CreateLogger(Arg<string>.Any()).Returns((new ILoggerImposter<AuthenticationSchemeOptions>()).Instance());

        // Create a real configuration
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["AppSettings:TestUserRole"] = "Administrator"
        });
        _configuration = configBuilder.Build();
    }

    [Fact]
    public void SchemeName_ReturnsTestAuthentication()
    {
        // Assert
        TestAuthenticationHandler.SchemeName.ShouldBe("TestAuthentication");
    }

    [Fact]
    public async Task HandleAuthenticateAsync_WithDefaultConfig_UsesAdministratorRole()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new QueryString("");

        // Configure session
        var sessionFeature = new ISessionFeatureImposter();
        var session = new ISessionImposter();
        sessionFeature.Session.Getter().Returns(session.Instance());
        httpContext.Features.Set(sessionFeature.Instance());

        _httpContextAccessor.HttpContext.Getter().Returns(httpContext);

        var handler = new TestAuthenticationHandler(
            _options.Instance(),
            _loggerFactory.Instance(),
            _encoder,
            _configuration,
            _httpContextAccessor.Instance());

        await handler.InitializeAsync(
            new AuthenticationScheme(TestAuthenticationHandler.SchemeName, null, typeof(TestAuthenticationHandler)),
            httpContext);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Principal.ShouldNotBeNull();
        var roleClaim = result.Principal.FindFirst(ClaimTypes.Role);
        roleClaim.ShouldNotBeNull();
        roleClaim.Value.ShouldBe("Administrator");
    }

    [Fact]
    public async Task HandleAuthenticateAsync_CreatesAdministratorUserClaims()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new QueryString("");

        // Configure session
        var sessionFeature = new ISessionFeatureImposter();
        var session = new ISessionImposter();
        sessionFeature.Session.Getter().Returns(session.Instance());
        httpContext.Features.Set(sessionFeature.Instance());

        _httpContextAccessor.HttpContext.Getter().Returns(httpContext);

        var handler = new TestAuthenticationHandler(
            _options.Instance(),
            _loggerFactory.Instance(),
            _encoder,
            _configuration,
            _httpContextAccessor.Instance());

        await handler.InitializeAsync(
            new AuthenticationScheme(TestAuthenticationHandler.SchemeName, null, typeof(TestAuthenticationHandler)),
            httpContext);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Principal.ShouldNotBeNull();

        var roleClaim = result.Principal.FindFirst(ClaimTypes.Role);
        roleClaim?.Value.ShouldBe("Administrator");

        var nameClaim = result.Principal.FindFirst(ClaimTypes.Name);
        nameClaim?.Value.ShouldBe("Admin User");

        var emailClaim = result.Principal.FindFirst(ClaimTypes.Email);
        emailClaim?.Value.ShouldBe("administrator@test.com");

        var estateIdClaim = result.Principal.FindFirst("estateId");
        estateIdClaim?.Value.ShouldBe("11111111-1111-1111-1111-111111111111");
    }

    [Fact]
    public async Task HandleAuthenticateAsync_CreatesAllRequiredClaims()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new QueryString("");

        // Configure session
        var sessionFeature = new ISessionFeatureImposter();
        var session = new ISessionImposter();
        sessionFeature.Session.Getter().Returns(session.Instance());
        httpContext.Features.Set(sessionFeature.Instance());

        _httpContextAccessor.HttpContext.Getter().Returns(httpContext);

        var handler = new TestAuthenticationHandler(
            _options.Instance(),
            _loggerFactory.Instance(),
            _encoder,
            _configuration,
            _httpContextAccessor.Instance());

        await handler.InitializeAsync(
            new AuthenticationScheme(TestAuthenticationHandler.SchemeName, null, typeof(TestAuthenticationHandler)),
            httpContext);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Principal.ShouldNotBeNull();

        result.Principal.FindFirst(ClaimTypes.NameIdentifier).ShouldNotBeNull();
        result.Principal.FindFirst(ClaimTypes.Name).ShouldNotBeNull();
        result.Principal.FindFirst(ClaimTypes.Email).ShouldNotBeNull();
        result.Principal.FindFirst("estateId").ShouldNotBeNull();
        result.Principal.FindFirst(ClaimTypes.Role).ShouldNotBeNull();
        result.Principal.FindFirst("role").ShouldNotBeNull();
    }

    [Fact]
    public async Task HandleAuthenticateAsync_AlwaysSucceeds()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new QueryString("");

        // Configure session
        var sessionFeature = new ISessionFeatureImposter();
        var session = new ISessionImposter();
        sessionFeature.Session.Getter().Returns(session.Instance());
        httpContext.Features.Set(sessionFeature.Instance());

        _httpContextAccessor.HttpContext.Getter().Returns(httpContext);

        var handler = new TestAuthenticationHandler(
            _options.Instance(),
            _loggerFactory.Instance(),
            _encoder,
            _configuration,
            _httpContextAccessor.Instance());

        await handler.InitializeAsync(
            new AuthenticationScheme(TestAuthenticationHandler.SchemeName, null, typeof(TestAuthenticationHandler)),
            httpContext);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Failure.ShouldBeNull();
    }

    [Fact]
    public async Task HandleAuthenticateAsync_CreatesAuthenticationTicket()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Request.QueryString = new QueryString("");

        // Configure session
        var sessionFeature = new ISessionFeatureImposter();
        var session = new ISessionImposter();
        sessionFeature.Session.Getter().Returns(session.Instance());
        httpContext.Features.Set(sessionFeature.Instance());

        _httpContextAccessor.HttpContext.Getter().Returns(httpContext);

        var handler = new TestAuthenticationHandler(
            _options.Instance(),
            _loggerFactory.Instance(),
            _encoder,
            _configuration,
            _httpContextAccessor.Instance());

        await handler.InitializeAsync(
            new AuthenticationScheme(TestAuthenticationHandler.SchemeName, null, typeof(TestAuthenticationHandler)),
            httpContext);

        // Act
        var result = await handler.AuthenticateAsync();

        // Assert
        result.Succeeded.ShouldBeTrue();
        result.Ticket.ShouldNotBeNull();
        result.Ticket.AuthenticationScheme.ShouldBe(TestAuthenticationHandler.SchemeName);
    }
}
