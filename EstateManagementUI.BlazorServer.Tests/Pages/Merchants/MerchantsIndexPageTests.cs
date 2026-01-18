using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using MerchantsIndex = EstateManagementUI.BlazorServer.Components.Pages.Merchants.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsIndexPageTests : BaseTest
{
    [Fact]
    public void MerchantsIndex_InitialState_ShowsLoadingIndicator()
    {
        // Arrange
        var merchants = new List<MerchantModel>
        {
            new MerchantModel
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant",
                MerchantReference = "REF001",
                Balance = 1000m,
                AvailableBalance = 500m,
                SettlementSchedule = "Daily"
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(merchants));
        
        // Act
        var cut = RenderComponent<MerchantsIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Merchant Management");
    }

    [Fact]
    public void MerchantsIndex_WithNoMerchants_ShowsEmptyState()
    {
        // Arrange
        var emptyList = new List<MerchantModel>();
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(emptyList));
        
        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"));
        
        // Assert
        cut.Markup.ShouldContain("No merchants found");
    }

    [Fact]
    public void MerchantsIndex_WithMerchants_DisplaysMerchantList()
    {
        // Arrange
        var merchants = new List<MerchantModel>
        {
            new MerchantModel
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant 1",
                MerchantReference = "REF001",
                Balance = 1000m,
                AvailableBalance = 500m,
                SettlementSchedule = "Daily"
            },
            new MerchantModel
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant 2",
                MerchantReference = "REF002",
                Balance = 2000m,
                AvailableBalance = 1500m,
                SettlementSchedule = "Weekly"
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(merchants));
        
        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Merchant 1");
        cut.Markup.ShouldContain("Test Merchant 2");
        cut.Markup.ShouldContain("REF001");
        cut.Markup.ShouldContain("REF002");
    }

    [Fact]
    public void MerchantsIndex_WithMerchants_DisplaysSummaryCards()
    {
        // Arrange
        var merchants = new List<MerchantModel>
        {
            new MerchantModel
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Merchant 1",
                MerchantReference = "REF001",
                Balance = 1000m,
                AvailableBalance = 500m
            },
            new MerchantModel
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Merchant 2",
                MerchantReference = "REF002",
                Balance = 2000m,
                AvailableBalance = 1500m
            }
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(merchants));
        
        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Total Merchants");
        cut.Markup.ShouldContain("Total Balance");
        cut.Markup.ShouldContain("Available Balance");
    }

    [Fact]
    public void MerchantsIndex_HasCorrectPageTitle()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<Queries.GetMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantModel>()));
        
        // Act
        var cut = RenderComponent<MerchantsIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
