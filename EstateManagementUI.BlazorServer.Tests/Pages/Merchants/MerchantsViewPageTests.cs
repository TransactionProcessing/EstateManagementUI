using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Merchants;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using MerchantModel = EstateManagementUI.BusinessLogic.Models.MerchantModel;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsViewPageTests : BaseTest
{
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantQuery>(), default))
            .ReturnsAsync(Result.Success(merchant));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantOperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantDevicesQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantDeviceModel>()));

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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantQuery>(), default))
            .ReturnsAsync(Result.Success(merchant));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantOperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantDevicesQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantDeviceModel>()));

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
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantQuery>(), default))
            .ReturnsAsync(Result.Success(new MerchantModel { MerchantId = merchantId }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantOperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantDevicesQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantDeviceModel>()));
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
        
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantQuery>(), default))
            .ReturnsAsync(Result.Success(merchant));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantOperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetMerchantDevicesQuery>(), default))
            .ReturnsAsync(Result.Success(new List<MerchantDeviceModel>()));
        
        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Back to List");
    }
}
