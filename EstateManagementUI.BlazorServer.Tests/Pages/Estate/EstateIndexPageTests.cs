using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Microsoft.AspNetCore.Components.Web;
using Moq;
using Shouldly;
using SimpleResults;
using EstateIndex = EstateManagementUI.BlazorServer.Components.Pages.Estate.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Estate;

public class EstateIndexPageTests : BaseTest
{
    [Fact]
    public void EstateIndex_RendersCorrectly()
    {
        // Arrange
        EstateModel estate = new() {
            EstateId = Guid.NewGuid(),
            EstateName = "Test Estate",
            Reference = "EST001"
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Estate Management");
    }

    [Fact]
    public void EstateIndex_DisplaysEstateDetails()
    {
        // Arrange
        EstateModel estate = new() {
            EstateId = Guid.NewGuid(),
            EstateName = "Test Estate",
            Reference = "EST001",
            Operators = new List<EstateOperatorModel>()
        };

        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(estate));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        
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
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        
        // Assert
        IRenderedComponent<PageTitle> pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void EstateIndex_TabSwitch_ToOperators_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Find operators button and click it
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Assigned Operators"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_TabSwitch_BackToOverview_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators first
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Act - switch back to overview
        IElement? overviewButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Overview"));
        overviewButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Estate Name"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_AddOperator_Success_UpdatesAssignedOperators()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorDropDownModel operatorToAdd = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(new List<OperatorDropDownModel> { operatorToAdd });
        
        OperatorModel operatorDetails = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorQuery>(), default))
            .ReturnsAsync(Result.Success(operatorDetails));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.AddOperatorToEstateCommand>(), default))
            .ReturnsAsync(Result.Success());
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Click Add Operator button
        IElement addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        IElement selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addOperatorButton");
        addButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator added successfully"), timeout: TimeSpan.FromSeconds(5));
        
    }

    [Fact]
    public void EstateIndex_AddOperator_Failure_ShowsErrorMessage()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorDropDownModel operatorToAdd = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(new List<OperatorDropDownModel> { operatorToAdd });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.AddOperatorToEstateCommand>(), default))
            .ReturnsAsync(Result.Failure("Failed to add operator"));
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Click Add Operator button
        IElement addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        IElement selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addOperatorButton");
        addButton.Click();

        // Assert
        _mockMediator.Verify(x => x.Send(It.IsAny<EstateCommands.AddOperatorToEstateCommand>(), default), Times.AtLeastOnce());
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to add operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_RemoveOperator_Success_RemovesFromList()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.RemoveOperatorFromEstateCommand>(), default))
            .ReturnsAsync(Result.Success());
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Act - Remove operator
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator removed successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_RemoveOperator_Failure_ShowsErrorMessage()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.RemoveOperatorFromEstateCommand>(), default))
            .ReturnsAsync(Result.Failure("Failed to remove operator"));
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Act - Remove operator
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to remove operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_DisplaysMerchants_WhenPresent()
    {
        // Arrange
        List<RecentMerchantsModel> merchants = new() {
            new RecentMerchantsModel
            {
                MerchantId = Guid.NewGuid(),
                Name = "Test Merchant",
                Reference = "MERCH001",
                CreatedDateTime = new DateTime(2024, 1, 1, 12, 0, 0)
            }
        };
        
        SetupSuccessfulDataLoadWithMerchants(merchants);
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Merchant");
        cut.Markup.ShouldContain("MERCH001");
    }

    [Fact]
    public void EstateIndex_DisplaysNoMerchants_WhenEmpty()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("No merchants found");
    }

    [Fact]
    public void EstateIndex_DisplaysContracts_WhenPresent()
    {
        // Arrange
        List<RecentContractModel> contracts = new() {
            new RecentContractModel
            {
                ContractId = Guid.NewGuid(),
                Description = "Test Contract",
                OperatorName = "Test Operator"
            }
        };
        
        SetupSuccessfulDataLoadWithContracts(contracts);
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Contract");
    }

    [Fact]
    public void EstateIndex_DisplaysNoContracts_WhenEmpty()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("No contracts found");
    }

    [Fact]
    public void EstateIndex_LoadEstateData_EstateQueryFails_NavigatesToError()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Failure("Failed to load estate"));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void EstateIndex_LoadEstateData_MerchantQueryFails_NavigatesToError()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid() }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Failure("Failed to load merchants"));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void EstateIndex_LoadEstateData_ContractQueryFails_NavigatesToError()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid() }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Failure("Failed to load contracts"));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void EstateIndex_LoadEstateData_AssignedOperatorsQueryFails_NavigatesToError()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid() }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Failure("Failed to load assigned operators"));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void EstateIndex_LoadEstateData_AllOperatorsQueryFails_NavigatesToError()
    {
        // Arrange
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid() }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Failure("Failed to load operators"));
        
        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void EstateIndex_DisplaysOperatorRequirements_WhenPresent()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = true
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => {
            cut.Markup.ShouldContain("Requires Merchant Number");
            cut.Markup.ShouldContain("Requires Terminal Number");
        }, timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_DisplaysNoOperators_WhenNoneAssigned()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No operators assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_AddOperator_WhenGetOperatorQueryFails_NavigatesToError()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorDropDownModel operatorToAdd = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(new List<OperatorDropDownModel> { operatorToAdd });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.AddOperatorToEstateCommand>(), default))
            .ReturnsAsync(Result.Success());
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorQuery>(), default))
            .ReturnsAsync(Result.Failure("Failed to get operator details"));
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Click Add Operator button
        IElement addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        IElement selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addOperatorButton");
        addButton.Click();

        // Assert - Should navigate to error page
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void EstateIndex_AddOperator_WhenException_ShowsErrorMessage()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorDropDownModel operatorToAdd = new() {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(new List<OperatorDropDownModel> { operatorToAdd });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.AddOperatorToEstateCommand>(), default))
            .ThrowsAsync(new Exception("Test exception"));
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Click Add Operator button
        IElement addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        IElement selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        IElement addButton = cut.FindAll("button")
            .First(b => b.TextContent.Trim() == "Add" && (b.GetAttribute("id") ?? "") != "addOperatorButton");
        addButton.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("An error occurred: Test exception"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_RemoveOperator_WhenException_ShowsErrorMessage()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.RemoveOperatorFromEstateCommand>(), default))
            .ThrowsAsync(new Exception("Test exception"));
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Act - Remove operator
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("An error occurred: Test exception"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_SuccessMessage_ClearsWhenSwitchingTabs()
    {
        // Arrange
        Guid operatorId = Guid.NewGuid();
        OperatorModel assignedOperator = new() {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.RemoveOperatorFromEstateCommand>(), default))
            .ReturnsAsync(Result.Success());
        
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? operatorsButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Operators"));
        operatorsButton?.Click();
        
        // Remove operator to trigger success message
        IRefreshableElementCollection<IElement> removeButtons = cut.FindAll("button");
        IElement? removeButton = removeButtons.FirstOrDefault(b => b.TextContent.Contains("Remove"));
        removeButton?.Click();
        
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator removed successfully"), timeout: TimeSpan.FromSeconds(5));
        
        // Act - Switch to overview tab
        IRefreshableElementCollection<IElement> overviewButtons = cut.FindAll("button");
        IElement? overviewButton = overviewButtons.FirstOrDefault(b => b.TextContent.Contains("Overview"));
        overviewButton?.Click();
        
        // Assert - Success message should be cleared
        cut.WaitForAssertion(() => cut.Markup.ShouldNotContain("Operator removed successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    // Helper methods
    private void SetupSuccessfulDataLoad(List<RecentMerchantsModel>? merchants = null,
                                         List<RecentContractModel>? contracts = null,
                                         List<OperatorModel>? assignedOperators = null,
                                         List<OperatorDropDownModel>? operators = null)
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid(), EstateName = "Test Estate" }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(merchants ?? new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts ?? new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(assignedOperators ?? new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(operators ?? new List<OperatorDropDownModel>()));
    }

    private void SetupSuccessfulDataLoadWithOperators(List<OperatorDropDownModel> operators)
        => SetupSuccessfulDataLoad(operators: operators);

    private void SetupSuccessfulDataLoadWithAssignedOperators(List<OperatorModel> assignedOperators)
        => SetupSuccessfulDataLoad(assignedOperators: assignedOperators);

    private void SetupSuccessfulDataLoadWithMerchants(List<RecentMerchantsModel> merchants)
        => SetupSuccessfulDataLoad(merchants: merchants);

    private void SetupSuccessfulDataLoadWithContracts(List<RecentContractModel> contracts)
        => SetupSuccessfulDataLoad(contracts: contracts);
}
