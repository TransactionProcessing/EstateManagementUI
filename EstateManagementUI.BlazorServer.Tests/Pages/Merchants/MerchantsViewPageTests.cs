using AngleSharp.Dom;
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

    [Fact]
    public void MerchantsView_TabSwitch_ToAddress_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find address button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addressButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Address Details"));
        addressButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Address Line 1"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_TabSwitch_ToContact_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find contact button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contactButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Contact Details"));
        contactButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Contact Name"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_TabSwitch_ToOperators_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find operators button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No operators assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_TabSwitch_ToContracts_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find contracts button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No contracts assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_TabSwitch_ToDevices_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find devices button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No devices assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_TabSwitch_BackToDetails_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators first
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Act - switch back to details
        IElement? detailsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Merchant Details"));
        detailsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Merchant Name"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_DisplaysOperators_WhenPresent()
    {
        // Arrange
        List<MerchantModels.MerchantOperatorModel> operators = new() {
            new MerchantModels.MerchantOperatorModel
            {
                OperatorId = Guid.NewGuid(),
                OperatorName = "Test Operator",
                MerchantNumber = "MERCH001",
                TerminalNumber = "TERM001"
            }
        };
        
        SetupSuccessfulDataLoadWithOperators(operators);
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Test Operator");
            cut.Markup.ShouldContain("MERCH001");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_DisplaysNoOperators_WhenNoneAssigned()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No operators assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_DisplaysContracts_WhenPresent()
    {
        // Arrange
        List<MerchantModels.MerchantContractModel> contracts = new() {
            new MerchantModels.MerchantContractModel
            {
                ContractId = Guid.NewGuid(),
                ContractName = "Test Contract",
                OperatorName = "Test Operator",
                ContractProducts = new List<MerchantModels.MerchantContractProductModel>
                {
                    new MerchantModels.MerchantContractProductModel
                    {
                        ProductId = Guid.NewGuid(),
                        ProductName = "Test Product",
                        DisplayText = "£10 Topup"
                    }
                }
            }
        };
        
        SetupSuccessfulDataLoadWithContracts(contracts);
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Test Contract");
            cut.Markup.ShouldContain("Test Product");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_DisplaysNoContracts_WhenEmpty()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No contracts assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_DisplaysDevices_WhenPresent()
    {
        // Arrange
        List<MerchantModels.MerchantDeviceModel> devices = new() {
            new MerchantModels.MerchantDeviceModel
            {
                DeviceId = Guid.NewGuid(),
                DeviceIdentifier = "DEVICE12345"
            }
        };
        
        SetupSuccessfulDataLoadWithDevices(devices);
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("DEVICE12345"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_DisplaysNoDevices_WhenEmpty()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No devices assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_LoadMerchantData_LoadFails_NavigatesToError()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Failure());

        // Act
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void MerchantsView_DisplaysMerchantDetails_WhenPresent()
    {
        // Arrange
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = "Test Merchant",
            MerchantReference = "REF001",
            Balance = 1000.50m,
            AvailableBalance = 500.25m,
            SettlementSchedule = "Weekly"
        };

        SetupSuccessfulDataLoadWithMerchant(merchant);
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchant.MerchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Merchant");
        cut.Markup.ShouldContain("REF001");
        cut.Markup.ShouldContain("Weekly");
    }

    [Fact]
    public void MerchantsView_DisplaysAddressDetails_WhenPresent()
    {
        // Arrange
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = "Test Merchant",
            MerchantReference = "REF001",
            AddressLine1 = "123 Main Street",
            AddressLine2 = "Suite 100",
            Town = "Test Town",
            Region = "Test Region",
            PostalCode = "TE1 1ST",
            Country = "United Kingdom"
        };

        SetupSuccessfulDataLoadWithMerchant(merchant);
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchant.MerchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to address tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addressButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Address Details"));
        addressButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("123 Main Street");
            cut.Markup.ShouldContain("Test Town");
            cut.Markup.ShouldContain("TE1 1ST");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_DisplaysContactDetails_WhenPresent()
    {
        // Arrange
        var merchant = new MerchantModels.MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = "Test Merchant",
            MerchantReference = "REF001",
            ContactName = "John Doe",
            ContactEmailAddress = "john.doe@test.com",
            ContactPhoneNumber = "01234567890"
        };

        SetupSuccessfulDataLoadWithMerchant(merchant);
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, merchant.MerchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to contact tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contactButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Contact Details"));
        contactButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("John Doe");
            cut.Markup.ShouldContain("john.doe@test.com");
            cut.Markup.ShouldContain("01234567890");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsView_BackButton_NavigatesToMerchantsList()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        
        var cut = RenderComponent<View>(parameters => parameters
            .Add(p => p.MerchantId, Guid.NewGuid()));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Click back button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton?.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    // Helper methods
    private void SetupSuccessfulDataLoad(
        MerchantModels.MerchantModel? merchant = null,
        List<MerchantModels.MerchantOperatorModel>? operators = null,
        List<MerchantModels.MerchantContractModel>? contracts = null,
        List<MerchantModels.MerchantDeviceModel>? devices = null)
    {
        merchant ??= new MerchantModels.MerchantModel
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = "Test Merchant",
            MerchantReference = "REF001"
        };

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(merchant));
        this.MerchantUIService.Setup(m => m.GetMerchantOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(operators ?? new List<MerchantModels.MerchantOperatorModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantContracts(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(contracts ?? new List<MerchantModels.MerchantContractModel>()));
        this.MerchantUIService.Setup(m => m.GetMerchantDevices(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(devices ?? new List<MerchantModels.MerchantDeviceModel>()));
    }

    private void SetupSuccessfulDataLoadWithMerchant(MerchantModels.MerchantModel merchant)
        => SetupSuccessfulDataLoad(merchant: merchant);

    private void SetupSuccessfulDataLoadWithOperators(List<MerchantModels.MerchantOperatorModel> operators)
        => SetupSuccessfulDataLoad(operators: operators);

    private void SetupSuccessfulDataLoadWithContracts(List<MerchantModels.MerchantContractModel> contracts)
        => SetupSuccessfulDataLoad(contracts: contracts);

    private void SetupSuccessfulDataLoadWithDevices(List<MerchantModels.MerchantDeviceModel> devices)
        => SetupSuccessfulDataLoad(devices: devices);
}
