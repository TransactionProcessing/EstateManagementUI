using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Merchants;
using EstateManagementUI.BlazorServer.Models;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using static EstateManagementUI.BlazorServer.Requests.Queries;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsViewPageTests : TestContext
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<NavigationManager> _mockNavigationManager;

    public MerchantsViewPageTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockNavigationManager = new Mock<NavigationManager>();
        
        Services.AddSingleton(_mockMediator.Object);
        Services.AddSingleton(_mockNavigationManager.Object);
    }

    [Fact]
    public void MerchantsView_InitialState_ShowsLoadingIndicator()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchant = new MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "REF001"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantQuery>(), default))
            .ReturnsAsync(Result<MerchantModel>.Success(merchant));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        
        // Assert
        cut.Markup.ShouldContain("View Merchant");
    }

    [Fact]
    public void MerchantsView_WithMerchant_DisplaysMerchantName()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchant = new MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "REF001"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantQuery>(), default))
            .ReturnsAsync(Result<MerchantModel>.Success(merchant));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Merchant");
        cut.Markup.ShouldContain("REF001");
    }

    [Fact]
    public void MerchantsView_HasCorrectPageTitle()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantQuery>(), default))
            .ReturnsAsync(Result<MerchantModel>.Success(new MerchantModel { MerchantId = merchantId }));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void MerchantsView_HasBackButton()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchant = new MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<GetMerchantQuery>(), default))
            .ReturnsAsync(Result<MerchantModel>.Success(merchant));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Back to List");
    }
}
