using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Operator.OperatorsList;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace EstateManagementUI.UITests;

public class OperatorsListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly OperatorsList _operatorsList;

    public OperatorsListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._operatorsList = new OperatorsList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._operatorsList.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_PopulatesOperators()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.OperatorsResult);

        // Act
        await this._operatorsList.MountAsync();

        // Assert
        this._operatorsList.Operators.ShouldNotBeNull();
        this._operatorsList.Operators.Count.ShouldBe(2);
        this._operatorsList.Operators[0].Name.ShouldBe("Operator1");
        this._operatorsList.Operators[1].Name.ShouldBe("Operator2");
    }
    
    [Fact]
    public void Add_NavigatesToNewOperatorPage()
    {
        this._operatorsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        this._operatorsList.Add();

        // Assert
        this._operatorsList.LocationUrl.ShouldNotBeNull();
        this._operatorsList.LocationUrl.ShouldBe("/Operator/NewOperator");
    }

    [Fact]
    public async Task View_NavigatesToViewOperatorPage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        this._operatorsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await this._operatorsList.View(operatorId);

        // Assert
        this._operatorsList.LocationUrl.ShouldNotBeNull();
        this._operatorsList.LocationUrl.ShouldBe("/Operator/ViewOperator");
        Guid payloadOperatorId = TestHelpers.GetPropertyValue<Guid>(this._operatorsList.Payload, "OperatorId");
        payloadOperatorId.ShouldBe(operatorId);
    }

    [Fact]
    public async Task Edit_NavigatesToEditOperatorPage()
    {
        // Arrange
        var operatorId = Guid.NewGuid();
        this._operatorsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await this._operatorsList.Edit(operatorId);

        this._operatorsList.LocationUrl.ShouldNotBeNull();
        this._operatorsList.LocationUrl.ShouldBe("/Operator/EditOperator");
        Guid payloadOperatorId = TestHelpers.GetPropertyValue<Guid>(this._operatorsList.Payload, "OperatorId");
        payloadOperatorId.ShouldBe(operatorId);
    }

    [Fact]
    public async Task Sort_SortsOperatorsByName()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.OperatorsResult);

        // Act
        await this._operatorsList.Sort(OperatorSorting.Name);

        // Assert
        this._operatorsList.Operators.ShouldNotBeNull();
        this._operatorsList.Operators.Count.ShouldBe(2);
        this._operatorsList.Operators[0].Name.ShouldBe("Operator2");
        this._operatorsList.Operators[1].Name.ShouldBe("Operator1");

        // Act
        await this._operatorsList.Sort(OperatorSorting.Name);

        this._operatorsList.Operators[0].Name.ShouldBe("Operator1");
        this._operatorsList.Operators[1].Name.ShouldBe("Operator2");
    }

    [Fact]
    public async Task Sort_SortsOperatorsByRequireCustomMerchantNumber()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.OperatorsResult);

        // Act
        await this._operatorsList.Sort(OperatorSorting.RequireCustomMerchantNumber);

        // Assert
        this._operatorsList.Operators.ShouldNotBeNull();
        this._operatorsList.Operators.Count.ShouldBe(2);
        this._operatorsList.Operators[0].Name.ShouldBe("Operator2");
        this._operatorsList.Operators[1].Name.ShouldBe("Operator1");

        // Act
        await this._operatorsList.Sort(OperatorSorting.RequireCustomMerchantNumber);

        this._operatorsList.Operators[0].Name.ShouldBe("Operator1");
        this._operatorsList.Operators[1].Name.ShouldBe("Operator2");
    }

    [Fact]
    public async Task Sort_SortsOperatorsByRequireCustomTerminalNumber()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.OperatorsResult);

        // Act
        await this._operatorsList.Sort(OperatorSorting.RequireCustomTerminalNumber);

        // Assert
        this._operatorsList.Operators.ShouldNotBeNull();
        this._operatorsList.Operators.Count.ShouldBe(2);
        this._operatorsList.Operators[0].Name.ShouldBe("Operator1");
        this._operatorsList.Operators[1].Name.ShouldBe("Operator2");

        // Act
        await this._operatorsList.Sort(OperatorSorting.RequireCustomTerminalNumber);

        this._operatorsList.Operators[0].Name.ShouldBe("Operator2");
        this._operatorsList.Operators[1].Name.ShouldBe("Operator1");
    }
}