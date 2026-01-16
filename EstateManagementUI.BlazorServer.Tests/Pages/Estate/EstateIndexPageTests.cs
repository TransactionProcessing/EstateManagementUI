using Bunit;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;
using EstateIndex = EstateManagementUI.BlazorServer.Components.Pages.Estate.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Estate;

public class EstateIndexPageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;

    public EstateIndexPageTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockAuthStateProvider = new Mock<AuthenticationStateProvider>();

        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockAuthStateProvider.Object);
    }

    [Fact]
    public void EstateIndex_RendersCorrectly()
    {
        // Arrange
        var estate = new EstateModel
        {
            EstateId = Guid.NewGuid(),
            EstateName = "Test Estate",
            Reference = "EST001"
        };

        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"),
            new Claim("estateId", Guid.NewGuid().ToString()),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));

        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);


        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<ContractModel>()));
        
        // Act
        var cut = RenderComponent<EstateIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Estate Management");
    }

    [Fact]
    public void EstateIndex_DisplaysEstateDetails()
    {
        // Arrange
        var estate = new EstateModel
        {
            EstateId = Guid.NewGuid(),
            EstateName = "Test Estate",
            Reference = "EST001"
        };

        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"),
            new Claim("estateId", Guid.NewGuid().ToString()),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));

        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);

        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<ContractModel>()));
        
        // Act
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Estate");
    }

    [Fact]
    public void EstateIndex_HasCorrectPageTitle()
    {
        // Arrange
        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"),
            new Claim("estateId", Guid.NewGuid().ToString()),
            new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var user = new ClaimsPrincipal(identity);
        var authState = Task.FromResult(new AuthenticationState(user));

        _mockAuthStateProvider.Setup(x => x.GetAuthenticationStateAsync()).Returns(authState);

        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid() }));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<ContractModel>()));
        
        // Act
        var cut = RenderComponent<EstateIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
