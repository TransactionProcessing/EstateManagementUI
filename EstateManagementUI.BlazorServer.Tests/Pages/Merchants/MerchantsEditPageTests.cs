using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Shouldly;
using SimpleResults;
using MerchantsEdit = EstateManagementUI.BlazorServer.Components.Pages.Merchants.Edit;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsEditPageTests : BaseTest
{
    [Fact]
    public void MerchantsEdit_RendersCorrectly()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);

        // Act
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        
        // Assert
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        cut.Markup.ShouldContain("Edit Merchant");
    }

    [Fact]
    public void MerchantsEdit_HasCorrectPageTitle()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);

        // Act
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        
        // Assert
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        IRenderedComponent<PageTitle> pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void MerchantsEdit_DisplaysMerchantDetails()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId, "Test Merchant", "MERCH001");

        // Act
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        
        // Assert
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        cut.Markup.ShouldContain("Test Merchant");
        cut.Markup.ShouldContain("MERCH001");
    }

    [Fact]
    public void MerchantsEdit_BackToListButton_NavigatesToMerchantsList()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find Back to List button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? backButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Back to List"));
        backButton?.Click();
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    [Fact]
    public void MerchantsEdit_TabSwitch_ToAddress_UpdatesActiveTab()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find address button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addressButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Address Details"));
        addressButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Address Line 1"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_TabSwitch_ToContact_UpdatesActiveTab()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find contact button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contactButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Contact Details"));
        contactButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Contact Name"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_TabSwitch_ToOperators_UpdatesActiveTab()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find operators button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Assigned Operators"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_TabSwitch_ToContracts_UpdatesActiveTab()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find contracts button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Assigned Contracts"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_TabSwitch_ToDevices_UpdatesActiveTab()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find devices button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Assigned Devices"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_TabSwitch_BackToDetails_UpdatesActiveTab()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to address first
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? addressButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Address Details"));
        addressButton?.Click();
        
        // Act - switch back to details
        IElement? detailsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Merchant Details"));
        detailsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Merchant Name"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_LoadMerchantData_LoadFails_NavigatesToError()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Failure());

        // Act
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        
        // Assert
        cut.WaitForState(() => _fakeNavigationManager.Uri.Contains("error"), TimeSpan.FromSeconds(5));
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void MerchantsEdit_DisplaysNoOperators_WhenNoneAssigned()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No operators assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_DisplaysNoContracts_WhenNoneAssigned()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No contracts assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_DisplaysNoDevices_WhenNoneAssigned()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No devices assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_AddOperatorButton_TogglesAddOperatorForm()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Act - Click Add Operator button
        IElement addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Select Operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_AddContractButton_TogglesAddContractForm()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Act - Click Add Contract button
        IElement addContractButton = cut.Find("#addContractButton");
        addContractButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Select Contract"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_AddDeviceButton_TogglesAddDeviceForm()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Act - Click Add Device button
        IElement addDeviceButton = cut.Find("#addDeviceButton");
        addDeviceButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Device Identifier"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_AddOperator_Success_ShowsSuccessMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid operatorId = Guid.NewGuid();
        OperatorModels.OperatorDropDownModel operatorToAdd = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(merchantId, new List<OperatorModels.OperatorDropDownModel> { operatorToAdd });
        
        OperatorModels.OperatorModel operatorDetails = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), operatorId))
            .ReturnsAsync(Result.Success(operatorDetails));
        this.MerchantUIService.Setup(m => m.AddOperatorToMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, operatorId, It.IsAny<String?>(), It.IsAny<String?>()))
            .ReturnsAsync(Result.Success);

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Click Add Operator button
        IElement addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        IElement selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        cut.WaitForState(() => cut.Markup.Contains("Add"), TimeSpan.FromSeconds(5));
        
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addOperatorButton");
        addButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator added successfully"), timeout: TimeSpan.FromSeconds(10));
    }

    [Fact]
    public void MerchantsEdit_AddOperator_Failure_ShowsErrorMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid operatorId = Guid.NewGuid();
        OperatorModels.OperatorDropDownModel operatorToAdd = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(merchantId, new List<OperatorModels.OperatorDropDownModel> { operatorToAdd });
        
        OperatorModels.OperatorModel operatorDetails = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = false,
            RequireCustomTerminalNumber = false
        };
        
        this.OperatorUIService.Setup(o => o.GetOperator(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), operatorId))
            .ReturnsAsync(Result.Success(operatorDetails));
        this.MerchantUIService.Setup(m => m.AddOperatorToMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, operatorId, It.IsAny<String?>(), It.IsAny<String?>()))
            .ReturnsAsync(Result.Failure());

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Click Add Operator button
        IElement addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        IElement selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        cut.WaitForState(() => cut.Markup.Contains("Add"), TimeSpan.FromSeconds(5));
        
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addOperatorButton");
        addButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to add operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_RemoveOperator_Success_ShowsSuccessMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid operatorId = Guid.NewGuid();
        MerchantModels.MerchantOperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(merchantId, new List<MerchantModels.MerchantOperatorModel> { assignedOperator });

        this.MerchantUIService.Setup(m => m.RemoveOperatorFromMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, operatorId))
            .ReturnsAsync(Result.Success);

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Act - Remove operator
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator removed successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_RemoveOperator_Failure_ShowsErrorMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid operatorId = Guid.NewGuid();
        MerchantModels.MerchantOperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(merchantId, new List<MerchantModels.MerchantOperatorModel> { assignedOperator });

        this.MerchantUIService.Setup(m => m.RemoveOperatorFromMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, operatorId))
            .ReturnsAsync(Result.Failure(String.Empty));

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Act - Remove operator
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to remove operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_AssignContract_Success_ShowsSuccessMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid contractId = Guid.NewGuid();
        ContractModels.ContractDropDownModel contractToAdd = new() {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithContracts(merchantId, new List<ContractModels.ContractDropDownModel> { contractToAdd });

        this.MerchantUIService.Setup(m => m.AssignContractToMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, contractId))
            .ReturnsAsync(Result.Success);

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Click Add Contract button
        IElement addContractButton = cut.Find("#addContractButton");
        addContractButton.Click();
        
        // Act - Select contract and assign
        IElement selectElement = cut.Find("select");
        selectElement.Change(contractId.ToString());
        IElement assignButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Assign");
        assignButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Contract assigned successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_AssignContract_Failure_ShowsErrorMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid contractId = Guid.NewGuid();
        ContractModels.ContractDropDownModel contractToAdd = new() {
            ContractId = contractId,
            Description = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithContracts(merchantId, new List<ContractModels.ContractDropDownModel> { contractToAdd });

        this.MerchantUIService.Setup(m => m.AssignContractToMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, contractId))
            .ReturnsAsync(Result.Failure());

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Click Add Contract button
        IElement addContractButton = cut.Find("#addContractButton");
        addContractButton.Click();
        
        // Act - Select contract and assign
        IElement selectElement = cut.Find("select");
        selectElement.Change(contractId.ToString());
        IElement assignButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Assign");
        assignButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to assign contract"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_RemoveContract_Success_ShowsSuccessMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid contractId = Guid.NewGuid();
        MerchantModels.MerchantContractModel assignedContract = new() {
            ContractId = contractId,
            ContractName = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithAssignedContracts(merchantId, new List<MerchantModels.MerchantContractModel> { assignedContract });

        this.MerchantUIService.Setup(m => m.RemoveContractFromMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, contractId))
            .ReturnsAsync(Result.Success);

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Act - Remove contract
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Contract removed successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_RemoveContract_Failure_ShowsErrorMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid contractId = Guid.NewGuid();
        MerchantModels.MerchantContractModel assignedContract = new() {
            ContractId = contractId,
            ContractName = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithAssignedContracts(merchantId, new List<MerchantModels.MerchantContractModel> { assignedContract });

        this.MerchantUIService.Setup(m => m.RemoveContractFromMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, contractId))
            .ReturnsAsync(Result.Failure(String.Empty));

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Act - Remove contract
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to remove contract"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_DisplaysAssignedOperators_WhenPresent()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid operatorId = Guid.NewGuid();
        MerchantModels.MerchantOperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(merchantId, new List<MerchantModels.MerchantOperatorModel> { assignedOperator });

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_DisplaysAssignedContracts_WhenPresent()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        Guid contractId = Guid.NewGuid();
        MerchantModels.MerchantContractModel assignedContract = new() {
            ContractId = contractId,
            ContractName = "Test Contract",
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithAssignedContracts(merchantId, new List<MerchantModels.MerchantContractModel> { assignedContract });

        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to contracts tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? contractsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Contracts"));
        contractsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Contract"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_LoadMerchant_Success_LoadsAllData()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId, "Test Merchant", "MERCH001");
        
        // Act
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Merchant"), timeout: TimeSpan.FromSeconds(5));
        cut.Markup.ShouldContain("MERCH001");
        this.MerchantUIService.Verify(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId), Times.Once);
        this.MerchantUIService.Verify(m => m.GetMerchantOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId), Times.Once);
        this.MerchantUIService.Verify(m => m.GetMerchantContracts(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId), Times.Once);
        this.MerchantUIService.Verify(m => m.GetMerchantDevices(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId), Times.Once);
    }

    [Fact]
    public void MerchantsEdit_AddDeviceToMerchant_Success_ShowsSuccessMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        this.MerchantUIService.Setup(m => m.AddMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, It.IsAny<String>()))
            .ReturnsAsync(Result.Success);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Click Add Device button
        IElement addDeviceButton = cut.Find("#addDeviceButton");
        addDeviceButton.Click();
        
        // Act - Enter device identifier and add
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Device Identifier"), timeout: TimeSpan.FromSeconds(5));
        IElement deviceInput = cut.Find("input[placeholder*='device']");
        deviceInput.Change("DEV123");
        
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addDeviceButton");
        addButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Device added successfully"), timeout: TimeSpan.FromSeconds(10));
        this.MerchantUIService.Verify(m => m.AddMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, "DEV123"), Times.Once);
    }

    [Fact]
    public void MerchantsEdit_AddDeviceToMerchant_Failure_ShowsErrorMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        this.MerchantUIService.Setup(m => m.AddMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, It.IsAny<String>()))
            .ReturnsAsync(Result.Failure());
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Click Add Device button
        IElement addDeviceButton = cut.Find("#addDeviceButton");
        addDeviceButton.Click();
        
        // Act - Enter device identifier and add
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Device Identifier"), timeout: TimeSpan.FromSeconds(5));
        IElement deviceInput = cut.Find("input[placeholder*='device']");
        deviceInput.Change("DEV123");
        
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addDeviceButton");
        addButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to add device"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsEdit_SaveAllChanges_Success_ShowsSuccessMessageAndNavigates()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        this.MerchantUIService.Setup(m => m.UpdateMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, It.IsAny<MerchantModels.MerchantEditModel>()))
            .ReturnsAsync(Result.Success);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find and click save button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? saveButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Save Changes"));
        saveButton?.Click();
        
        // Assert - Component has WaitOnUIRefresh() with 2.5s delay before navigation
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Merchant details updated successfully"), timeout: TimeSpan.FromSeconds(5));
        cut.WaitForState(() => _fakeNavigationManager.Uri.Contains("/merchants"), TimeSpan.FromSeconds(10));
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
        this.MerchantUIService.Verify(m => m.UpdateMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, It.IsAny<MerchantModels.MerchantEditModel>()), Times.Once);
    }

    [Fact]
    public void MerchantsEdit_SaveAllChanges_Failure_ShowsErrorMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        SetupSuccessfulDataLoad(merchantId);
        
        this.MerchantUIService.Setup(m => m.UpdateMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, It.IsAny<MerchantModels.MerchantEditModel>()))
            .ReturnsAsync(Result.Failure());
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find and click save button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? saveButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Save Changes"));
        saveButton?.Click();
        
        // Assert - Use longer timeout to account for async operations
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to update merchant"), timeout: TimeSpan.FromSeconds(10));
        this.MerchantUIService.Verify(m => m.UpdateMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, It.IsAny<MerchantModels.MerchantEditModel>()), Times.Once);
    }

    [Fact]
    public void MerchantsEdit_SwapDeviceConfirm_Success_ShowsSuccessMessage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var existingDevice = new MerchantModels.MerchantDeviceModel { DeviceIdentifier = "DEV123" };
        SetupSuccessfulDataLoad(merchantId, assignedDevices: new List<MerchantModels.MerchantDeviceModel> { existingDevice });
        
        this.MerchantUIService.Setup(m => m.SwapMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, "DEV123", "DEV456"))
            .ReturnsAsync(Result.Success);
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Click Swap button for the device
        cut.WaitForAssertion(() => cut.FindAll("button").Any(b => b.TextContent.Contains("Swap")).ShouldBeTrue(), timeout: TimeSpan.FromSeconds(5));
        IElement swapButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Swap"));
        swapButton.Click();
        
        // Act - Enter new device identifier and confirm swap
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("New Device Identifier"), timeout: TimeSpan.FromSeconds(5));
        IElement swapInput = cut.FindAll("input").Last();
        swapInput.Change("DEV456");
        
        IElement confirmButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Confirm"));
        confirmButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Device DEV123 swapped for DEV456"), timeout: TimeSpan.FromSeconds(5));
        this.MerchantUIService.Verify(m => m.SwapMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, "DEV123", "DEV456"), Times.Once);
    }

    [Fact]
    public void MerchantsEdit_SwapDeviceConfirm_EmptyIdentifier_ShowsError()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var existingDevice = new MerchantModels.MerchantDeviceModel { DeviceIdentifier = "DEV123" };
        SetupSuccessfulDataLoad(merchantId, assignedDevices: new List<MerchantModels.MerchantDeviceModel> { existingDevice });
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Click Swap button for the device
        cut.WaitForAssertion(() => cut.FindAll("button").Any(b => b.TextContent.Contains("Swap")).ShouldBeTrue(), timeout: TimeSpan.FromSeconds(5));
        IElement swapButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Swap"));
        swapButton.Click();
        
        // Act - Try to confirm swap without entering identifier
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("New Device Identifier"), timeout: TimeSpan.FromSeconds(5));
        IElement confirmButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Confirm"));
        confirmButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("New device identifier is required"), timeout: TimeSpan.FromSeconds(5));
        this.MerchantUIService.Verify(m => m.SwapMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<String>()), Times.Never);
    }

    [Fact]
    public void MerchantsEdit_SwapDeviceConfirm_SameIdentifier_ShowsError()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var existingDevice = new MerchantModels.MerchantDeviceModel { DeviceIdentifier = "DEV123" };
        SetupSuccessfulDataLoad(merchantId, assignedDevices: new List<MerchantModels.MerchantDeviceModel> { existingDevice });
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Click Swap button for the device
        cut.WaitForAssertion(() => cut.FindAll("button").Any(b => b.TextContent.Contains("Swap")).ShouldBeTrue(), timeout: TimeSpan.FromSeconds(5));
        IElement swapButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Swap"));
        swapButton.Click();
        
        // Act - Enter same device identifier and try to confirm swap
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("New Device Identifier"), timeout: TimeSpan.FromSeconds(5));
        IElement swapInput = cut.FindAll("input").Last();
        swapInput.Change("DEV123");
        
        IElement confirmButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Confirm"));
        confirmButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("New device identifier cannot be the same as the current device"), timeout: TimeSpan.FromSeconds(5));
        this.MerchantUIService.Verify(m => m.SwapMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<String>()), Times.Never);
    }

    [Fact]
    public void MerchantsEdit_SwapDeviceConfirm_DuplicateIdentifier_ShowsError()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var device1 = new MerchantModels.MerchantDeviceModel { DeviceIdentifier = "DEV123" };
        var device2 = new MerchantModels.MerchantDeviceModel { DeviceIdentifier = "DEV456" };
        SetupSuccessfulDataLoad(merchantId, assignedDevices: new List<MerchantModels.MerchantDeviceModel> { device1, device2 });
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Click Swap button for the first device
        cut.WaitForAssertion(() => cut.FindAll("button").Any(b => b.TextContent.Contains("Swap")).ShouldBeTrue(), timeout: TimeSpan.FromSeconds(5));
        IElement swapButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Swap"));
        swapButton.Click();
        
        // Act - Try to swap to an already assigned device identifier
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("New Device Identifier"), timeout: TimeSpan.FromSeconds(5));
        IElement swapInput = cut.FindAll("input").Last();
        swapInput.Change("DEV456");
        
        IElement confirmButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Confirm"));
        confirmButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("The specified device identifier is already assigned"), timeout: TimeSpan.FromSeconds(5));
        this.MerchantUIService.Verify(m => m.SwapMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<String>(), It.IsAny<String>()), Times.Never);
    }

    [Fact]
    public void MerchantsEdit_SwapDeviceConfirm_Failure_ShowsError()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var existingDevice = new MerchantModels.MerchantDeviceModel { DeviceIdentifier = "DEV123" };
        SetupSuccessfulDataLoad(merchantId, assignedDevices: new List<MerchantModels.MerchantDeviceModel> { existingDevice });
        
        this.MerchantUIService.Setup(m => m.SwapMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, "DEV123", "DEV456"))
            .ReturnsAsync(Result.Failure());
        
        IRenderedComponent<MerchantsEdit> cut = RenderComponent<MerchantsEdit>(parameters => parameters
            .Add(p => p.MerchantId, merchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to devices tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? devicesButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Assigned Devices"));
        devicesButton?.Click();
        
        // Click Swap button for the device
        cut.WaitForAssertion(() => cut.FindAll("button").Any(b => b.TextContent.Contains("Swap")).ShouldBeTrue(), timeout: TimeSpan.FromSeconds(5));
        IElement swapButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Swap"));
        swapButton.Click();
        
        // Act - Enter new device identifier and confirm swap
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("New Device Identifier"), timeout: TimeSpan.FromSeconds(5));
        IElement swapInput = cut.FindAll("input").Last();
        swapInput.Change("DEV456");
        
        IElement confirmButton = cut.FindAll("button")
            .First(b => b.TextContent.Contains("Confirm"));
        confirmButton.Click();
        
        // Assert - Use longer timeout to account for async operations
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Original device not found"), timeout: TimeSpan.FromSeconds(10));
        this.MerchantUIService.Verify(m => m.SwapMerchantDevice(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId, "DEV123", "DEV456"), Times.Once);
    }

    // Helper methods
    private void SetupSuccessfulDataLoad(Guid merchantId, 
                                         string merchantName = "Test Merchant", 
                                         string merchantReference = "MERCH001",
                                         List<MerchantModels.MerchantOperatorModel>? assignedOperators = null,
                                         List<OperatorModels.OperatorDropDownModel>? availableOperators = null,
                                         List<MerchantModels.MerchantContractModel>? assignedContracts = null,
                                         List<ContractModels.ContractDropDownModel>? availableContracts = null,
                                         List<MerchantModels.MerchantDeviceModel>? assignedDevices = null)
    {
        MerchantModels.MerchantModel merchant = new() {
            MerchantId = merchantId,
            MerchantName = merchantName,
            MerchantReference = merchantReference,
            AddressId = Guid.NewGuid(),
            AddressLine1 = "123 Test St",
            AddressLine2 = "Suite 100",
            Town = "Test Town",
            Region = "Test Region",
            PostalCode = "12345",
            Country = "GB",
            ContactId = Guid.NewGuid(),
            ContactName = "Test Contact",
            ContactEmailAddress = "test@example.com",
            ContactPhoneNumber = "1234567890",
            SettlementSchedule = "Immediate",
            Balance = 1000.00m,
            AvailableBalance = 500.00m
        };
        
        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(merchant));
        
        this.MerchantUIService.Setup(m => m.GetMerchantOperators(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(assignedOperators ?? new List<MerchantModels.MerchantOperatorModel>()));
        
        this.MerchantUIService.Setup(m => m.GetMerchantContracts(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(assignedContracts ?? new List<MerchantModels.MerchantContractModel>()));
        
        this.MerchantUIService.Setup(m => m.GetMerchantDevices(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(assignedDevices ?? new List<MerchantModels.MerchantDeviceModel>()));
        
        this.OperatorUIService.Setup(o => o.GetOperatorsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(availableOperators ?? new List<OperatorModels.OperatorDropDownModel>()));
        
        this.ContractUIService.Setup(c => c.GetContractsForDropDown(It.IsAny<CorrelationId>(), It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(availableContracts ?? new List<ContractModels.ContractDropDownModel>()));
    }

    private void SetupSuccessfulDataLoadWithOperators(Guid merchantId, List<OperatorModels.OperatorDropDownModel> operators)
        => SetupSuccessfulDataLoad(merchantId, availableOperators: operators);

    private void SetupSuccessfulDataLoadWithAssignedOperators(Guid merchantId, List<MerchantModels.MerchantOperatorModel> assignedOperators)
        => SetupSuccessfulDataLoad(merchantId, assignedOperators: assignedOperators);

    private void SetupSuccessfulDataLoadWithContracts(Guid merchantId, List<ContractModels.ContractDropDownModel> contracts)
        => SetupSuccessfulDataLoad(merchantId, availableContracts: contracts);

    private void SetupSuccessfulDataLoadWithAssignedContracts(Guid merchantId, List<MerchantModels.MerchantContractModel> assignedContracts)
        => SetupSuccessfulDataLoad(merchantId, assignedContracts: assignedContracts);
}
