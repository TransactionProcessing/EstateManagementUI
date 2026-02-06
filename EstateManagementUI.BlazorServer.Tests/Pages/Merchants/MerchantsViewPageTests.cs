using Bunit;
using EstateManagementUI.BlazorServer.Components.Pages.Merchants;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using MerchantContractModel = EstateManagementUI.BusinessLogic.Models.MerchantContractModel;
using MerchantDeviceModel = EstateManagementUI.BusinessLogic.Models.MerchantDeviceModel;
using MerchantModel = EstateManagementUI.BusinessLogic.Models.MerchantModel;
using MerchantOperatorModel = EstateManagementUI.BusinessLogic.Models.MerchantOperatorModel;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsViewPageTests : BaseTest
{
    [Fact]
    public void MerchantsView_InitialState_ShowsLoadingIndicator()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "REF001"
        };

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(merchant));
        this.MerchantUIService.Setup(m => m.GetMerchantOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantOperatorModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantContracts(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantContractModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantDevices(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantDeviceModel>()));

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
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "REF001"
        };

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(merchant));
        this.MerchantUIService.Setup(m => m.GetMerchantOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantOperatorModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantContracts(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantContractModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantDevices(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantDeviceModel>()));


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
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "REF001"
        };

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(merchant));
        this.MerchantUIService.Setup(m => m.GetMerchantOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantOperatorModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantContracts(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantContractModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantDevices(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantDeviceModel>()));

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
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = merchantId,
            MerchantName = "Test Merchant",
            MerchantReference = "REF001"
        };

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(merchant));
        this.MerchantUIService.Setup(m => m.GetMerchantOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantOperatorModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantContracts(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantContractModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantDevices(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(new List<MerchantModels.MerchantDeviceModel>()));


        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Back to List");
    }
}
