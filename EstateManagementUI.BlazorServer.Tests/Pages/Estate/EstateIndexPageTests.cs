using Bunit;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateIndex = EstateManagementUI.BlazorServer.Components.Pages.Estate.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Estate;

public class EstateIndexPageTests : BaseTest
{
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));

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
            Reference = "EST001",
            Operators = new List<EstateOperatorModel>()
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));

        // Act
        var cut = RenderComponent<EstateIndex>();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Estate"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_HasCorrectPageTitle()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid() }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));

        // Act
        var cut = RenderComponent<EstateIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
