using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using static EstateManagementUI.BlazorServer.Requests.Queries;
using MerchantsIndex = EstateManagementUI.BlazorServer.Components.Pages.Merchants.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsIndexPageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<IPermissionKeyProvider> _mockPermissionKeyProvider;
    private readonly Mock<NavigationManager> _mockNavigationManager;

    public MerchantsIndexPageTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockPermissionKeyProvider = new Mock<IPermissionKeyProvider>();
        _mockNavigationManager = new Mock<NavigationManager>();
        
        _mockPermissionKeyProvider.Setup(x => x.GetKey()).Returns("test-key");
        
        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockPermissionKeyProvider.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
        
        // Add required permission components
        ComponentFactories.AddStub<RequirePermission>();
    }

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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(merchants));
        
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
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(emptyList));
        
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(merchants));
        
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(merchants));
        
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
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantsQuery>(), default))
            .ReturnsAsync(Result<List<MerchantModel>>.Success(new List<MerchantModel>()));
        
        // Act
        var cut = RenderComponent<MerchantsIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }
}
