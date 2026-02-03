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
using EstateManagementUI.BlazorServer.Models;

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
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

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
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

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
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));

        // Act
        var cut = RenderComponent<EstateIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void EstateIndex_TabSwitch_ToOperators_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Assigned Operators"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_TabSwitch_BackToOverview_UpdatesActiveTab()
    {
        // Arrange
        SetupSuccessfulDataLoad();
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators first
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
        // Act - switch back to overview
        var overviewTab = cut.Find("button:contains('Overview')");
        overviewTab.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Estate Name"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_AddOperator_Success_UpdatesAssignedOperators()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorToAdd = new OperatorDropDownModel
        {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(new List<OperatorDropDownModel> { operatorToAdd });
        
        var operatorDetails = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorQuery>(), default))
            .ReturnsAsync(Result.Success(operatorDetails));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.AddOperatorToEstateCommand>(), default))
            .ReturnsAsync(Result.Success());
        
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
        // Click Add Operator button
        var addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        var selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        var addButton = cut.Find("button:contains('Add')");
        addButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator added successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_AddOperator_Failure_ShowsErrorMessage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var operatorToAdd = new OperatorDropDownModel
        {
            OperatorId = operatorId,
            OperatorName = "Test Operator"
        };
        
        SetupSuccessfulDataLoadWithOperators(new List<OperatorDropDownModel> { operatorToAdd });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.AddOperatorToEstateCommand>(), default))
            .ReturnsAsync(Result.Failure("Failed to add operator"));
        
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
        // Click Add Operator button
        var addOperatorButton = cut.Find("#addOperatorButton");
        addOperatorButton.Click();
        
        // Act - Select operator and add
        var selectElement = cut.Find("select");
        selectElement.Change(operatorId.ToString());
        var addButton = cut.Find("button:contains('Add')");
        addButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to add operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_RemoveOperator_Success_RemovesFromList()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var assignedOperator = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.RemoveOperatorFromEstateCommand>(), default))
            .ReturnsAsync(Result.Success());
        
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
        // Act - Remove operator
        var removeButton = cut.Find("button:contains('Remove')");
        removeButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Operator removed successfully"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_RemoveOperator_Failure_ShowsErrorMessage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var assignedOperator = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = false
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateCommands.RemoveOperatorFromEstateCommand>(), default))
            .ReturnsAsync(Result.Failure("Failed to remove operator"));
        
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Switch to operators tab
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
        // Act - Remove operator
        var removeButton = cut.Find("button:contains('Remove')");
        removeButton.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Failed to remove operator"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void EstateIndex_DisplaysMerchants_WhenPresent()
    {
        // Arrange
        var merchants = new List<RecentMerchantsModel>
        {
            new RecentMerchantsModel
            {
                MerchantId = Guid.NewGuid(),
                Name = "Test Merchant",
                Reference = "MERCH001",
                CreatedDateTime = DateTime.Now
            }
        };
        
        SetupSuccessfulDataLoadWithMerchants(merchants);
        
        // Act
        var cut = RenderComponent<EstateIndex>();
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
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("No merchants found");
    }

    [Fact]
    public void EstateIndex_DisplaysContracts_WhenPresent()
    {
        // Arrange
        var contracts = new List<RecentContractModel>
        {
            new RecentContractModel
            {
                ContractId = Guid.NewGuid(),
                Description = "Test Contract",
                OperatorName = "Test Operator"
            }
        };
        
        SetupSuccessfulDataLoadWithContracts(contracts);
        
        // Act
        var cut = RenderComponent<EstateIndex>();
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
        var cut = RenderComponent<EstateIndex>();
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
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _mockNavigationManager.Verify(x => x.NavigateTo(It.Is<string>(s => s.Contains("error")), It.IsAny<bool>()), Times.AtLeastOnce());
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
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _mockNavigationManager.Verify(x => x.NavigateTo(It.Is<string>(s => s.Contains("error")), It.IsAny<bool>()), Times.AtLeastOnce());
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
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _mockNavigationManager.Verify(x => x.NavigateTo(It.Is<string>(s => s.Contains("error")), It.IsAny<bool>()), Times.AtLeastOnce());
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
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _mockNavigationManager.Verify(x => x.NavigateTo(It.Is<string>(s => s.Contains("error")), It.IsAny<bool>()), Times.AtLeastOnce());
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
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        _mockNavigationManager.Verify(x => x.NavigateTo(It.Is<string>(s => s.Contains("error")), It.IsAny<bool>()), Times.AtLeastOnce());
    }

    [Fact]
    public void EstateIndex_DisplaysOperatorRequirements_WhenPresent()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        var assignedOperator = new OperatorModel
        {
            OperatorId = operatorId,
            Name = "Test Operator",
            RequireCustomMerchantNumber = true,
            RequireCustomTerminalNumber = true
        };
        
        SetupSuccessfulDataLoadWithAssignedOperators(new List<OperatorModel> { assignedOperator });
        
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
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
        
        var cut = RenderComponent<EstateIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Act - Switch to operators tab
        var operatorsTab = cut.Find("button:contains('Operators')");
        operatorsTab.Click();
        
        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("No operators assigned"), timeout: TimeSpan.FromSeconds(5));
    }

    // Helper methods
    private void SetupSuccessfulDataLoad()
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid(), EstateName = "Test Estate" }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
    }

    private void SetupSuccessfulDataLoadWithOperators(List<OperatorDropDownModel> operators)
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid(), EstateName = "Test Estate" }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(operators));
    }

    private void SetupSuccessfulDataLoadWithAssignedOperators(List<OperatorModel> assignedOperators)
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid(), EstateName = "Test Estate" }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(assignedOperators));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
    }

    private void SetupSuccessfulDataLoadWithMerchants(List<RecentMerchantsModel> merchants)
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid(), EstateName = "Test Estate" }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(merchants));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentContractModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
    }

    private void SetupSuccessfulDataLoadWithContracts(List<RecentContractModel> contracts)
    {
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetEstateQuery>(), default))
            .ReturnsAsync(Result.Success(new EstateModel { EstateId = Guid.NewGuid(), EstateName = "Test Estate" }));
        _mockMediator.Setup(x => x.Send(It.IsAny<MerchantQueries.GetRecentMerchantsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<RecentMerchantsModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<ContractQueries.GetRecentContractsQuery>(), default))
            .ReturnsAsync(Result.Success(contracts));
        _mockMediator.Setup(x => x.Send(It.IsAny<EstateQueries.GetAssignedOperatorsQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorModel>()));
        _mockMediator.Setup(x => x.Send(It.IsAny<OperatorQueries.GetOperatorsForDropDownQuery>(), default))
            .ReturnsAsync(Result.Success(new List<OperatorDropDownModel>()));
    }
}
