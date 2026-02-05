using AngleSharp.Dom;
using Bunit;
using Castle.Components.DictionaryAdapter;
using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
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
        BlazorServer.Models.EstateModel estate = new(Guid.NewGuid(), "Test Estate", "EST001");
        estate = estate with {
                ContractCount = 0,
                RecentContracts = new List<Models.RecentContractModel>(),
                OperatorCount = 0,
                AllOperators = new List<Models.OperatorDropDownModel>(),
                AssignedOperators = new List<Models.OperatorModel>(),
                MerchantCount = 0,
                RecentMerchants = new List<Models.RecentMerchantsModel>(),
                UserCount = 0
            };    
            
        this.EstateUIService.Setup(e => e.LoadEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(estate));

        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Estate Management");
    }

    [Fact]
    public void EstateIndex_DisplaysEstateDetails()
    {
        // Arrange
        BlazorServer.Models.EstateModel estate = new(Guid.NewGuid(), "Test Estate", "EST001");
        estate = estate with
        {
            ContractCount = 0,
            RecentContracts = new List<Models.RecentContractModel>(),
            OperatorCount = 0,
            AllOperators = new List<Models.OperatorDropDownModel>(),
            AssignedOperators = new List<Models.OperatorModel>(),
            MerchantCount = 0,
            RecentMerchants = new List<Models.RecentMerchantsModel>(),
            UserCount = 0
        };
        this.EstateUIService.Setup(e => e.LoadEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(estate));

        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Test Estate"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_HasCorrectPageTitle()
    {
        // Arrange
        BlazorServer.Models.EstateModel estate = new(Guid.NewGuid(), "Test Estate", "EST001");
        estate = estate with
        {
            ContractCount = 0,
            RecentContracts = new List<Models.RecentContractModel>(),
            OperatorCount = 0,
            AllOperators = new List<Models.OperatorDropDownModel>(),
            AssignedOperators = new List<Models.OperatorModel>(),
            MerchantCount = 0,
            RecentMerchants = new List<Models.RecentMerchantsModel>(),
            UserCount = 0
        };
        this.EstateUIService.Setup(e => e.LoadEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(estate));

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

        this.EstateUIService.Setup(e => e.AddOperatorToEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>())).ReturnsAsync(Result.Success);

        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.Instance.DelayOverride = 0;
        cut.Render(); // required to trigger re-render

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
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator added successfully"), timeout: TimeSpan.FromSeconds(10));
        
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

        this.EstateUIService.Setup(e => e.AddOperatorToEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>())).ReturnsAsync(Result.Failure);

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

        this.EstateUIService.Setup(e => e.RemoveOperatorFromEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success);

        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        cut.Instance.DelayOverride = 0;
        cut.Render(); // required to trigger re-render
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

        this.EstateUIService.Setup(e => e.RemoveOperatorFromEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Failure(String.Empty));

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
    public void EstateIndex_LoadEstateData_LoadFails_NavigatesToError()
    {
        // Arrange
        this.EstateUIService.Setup(e => e.LoadEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Failure());

        // Act
        IRenderedComponent<EstateIndex> cut = RenderComponent<EstateIndex>();
        
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

        this.EstateUIService.Setup(e => e.RemoveOperatorFromEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success);

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
        BlazorServer.Models.EstateModel estate = new(Guid.NewGuid(), "Test Estate", "EST001");
        estate = estate with
        {
            ContractCount = 5,
            RecentContracts = contracts ?? new List<RecentContractModel>(),
            OperatorCount = 3,
            AllOperators = operators ?? new List<OperatorDropDownModel>(),
            AssignedOperators = assignedOperators ?? new List<OperatorModel>(),
            MerchantCount = 10,
            RecentMerchants = merchants ?? new List<RecentMerchantsModel>(),
            UserCount = 2
        };
        this.EstateUIService.Setup(e => e.LoadEstate(It.IsAny<CorrelationId>(), It.IsAny<Guid>())).ReturnsAsync(Result.Success(estate));
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
