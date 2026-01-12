using Bunit;
using EstateManagementUI.BlazorServer.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using static EstateManagementUI.BlazorServer.Requests.Queries;
using EstateIndex = EstateManagementUI.BlazorServer.Components.Pages.Estate.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Estate;

public class EstateIndexPageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;

    public EstateIndexPageTests()
    {
        _mockMediator = new Mock<IMediator>();
        Services.AddSingleton(_mockMediator.Object);
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetEstateQuery>(), default))
            .ReturnsAsync(Result<EstateModel>.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(new List<MerchantModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOperatorsQuery>(), default))
            .ReturnsAsync(Result<List<OperatorModel>>.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetContractsQuery>(), default))
            .ReturnsAsync(Result<List<ContractModel>>.Success(new List<ContractModel>()));
        
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetEstateQuery>(), default))
            .ReturnsAsync(Result<EstateModel>.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(new List<MerchantModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOperatorsQuery>(), default))
            .ReturnsAsync(Result<List<OperatorModel>>.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetContractsQuery>(), default))
            .ReturnsAsync(Result<List<ContractModel>>.Success(new List<ContractModel>()));
        
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
        _mockMediator.Setup(x => x.Send(It.IsAny<GetEstateQuery>(), default))
            .ReturnsAsync(Result<EstateModel>.Success(new EstateModel { EstateId = Guid.NewGuid() }));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(new List<MerchantModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetOperatorsQuery>(), default))
            .ReturnsAsync(Result<List<OperatorModel>>.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<GetContractsQuery>(), default))
            .ReturnsAsync(Result<List<ContractModel>>.Success(new List<ContractModel>()));
        
        // Act
        var cut = RenderComponent<EstateIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
