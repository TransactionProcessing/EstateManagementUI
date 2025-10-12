using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Operator;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;
using Operator = EstateManagementUI.Pages.Operator.OperatorDialogs.Operator;
using ShowMessage = EstateManagementUI.Pages.Shared.Components.ShowMessage;

namespace EstateManagementUI.UITests;

public class OperatorTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Operator _operator;

    public OperatorTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._operator = new Operator(this._mediatorMock.Object, this._permissionsServiceMock.Object, "OperatorFunction");
        this._operator.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_LoadsOperator_WhenOperatorIdIsNotEmpty()
    {
        // Arrange
        this._operator.OperatorId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetOperatorQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.OperatorResult);

        // Act
        await this._operator.MountAsync();

        // Assert
        this._operator.Name.ShouldBe("Test Operator");
        this._operator.RequireCustomMerchantNumber.ShouldBeTrue();
        this._operator.RequireCustomTerminalNumber.ShouldBeFalse();
    }

    [Fact]
    public async Task Close_SetsLocationUrl()
    {
        this._operator.Url = TestHelper.GetTestUrlHelper();

        // Act
        this._operator.Close();
        
        // Assert
        this._operator.LocationUrl.ShouldBe("/Operator/Index");
        this._operator.Payload.ShouldBeNull();
    }

    [Fact]
    public async Task Save_AddsNewOperator_WhenOperatorIdIsEmpty()
    {
        // Arrange
        this._operator.OperatorId = Guid.Empty;
        this._operator.Name = "New Operator";
        this._operator.RequireCustomMerchantNumber = true;
        this._operator.RequireCustomTerminalNumber = false;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AddNewOperatorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._operator.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AddNewOperatorCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var events = this._operator.GetDispatchedEvents();
        events.Count.ShouldBe(1);
        events[0].ShouldBeOfType<OperatorPageEvents.OperatorCreatedEvent>();
    }

    [Fact]
    public async Task Save_AddOperatorFailed_MessageShow()
    {
        // Arrange
        this._operator.OperatorId = Guid.Empty;
        this._operator.Name = "New Operator";
        this._operator.RequireCustomMerchantNumber = true;
        this._operator.RequireCustomTerminalNumber = false;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AddNewOperatorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<String>(){"Failed"}));

        // Act
        await this._operator.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AddNewOperatorCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var events = this._operator.GetDispatchedEvents();
        events.Count.ShouldBe(1);
        events[0].ShouldBeOfType<ShowMessage>();
    }

    [Fact]
    public async Task Save_UpdatesOperator_WhenOperatorIdIsNotEmpty()
    {
        // Arrange
        this._operator.OperatorId = Guid.NewGuid();
        this._operator.Name = "Updated Operator";
        this._operator.RequireCustomMerchantNumber = true;
        this._operator.RequireCustomTerminalNumber = false;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateOperatorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._operator.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateOperatorCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var events = this._operator.GetDispatchedEvents();
        events.Count.ShouldBe(1);
        events[0].ShouldBeOfType<OperatorPageEvents.OperatorUpdatedEvent>();
    }

    [Fact]
    public async Task Save_UpdateOperatorFailed_MessageShow()
    {
        // Arrange
        this._operator.OperatorId = Guid.NewGuid();
        this._operator.Name = "Updated Operator";
        this._operator.RequireCustomMerchantNumber = true;
        this._operator.RequireCustomTerminalNumber = false;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateOperatorCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<String>() { "Failed" }));

        // Act
        await this._operator.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateOperatorCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        var events = this._operator.GetDispatchedEvents();
        events.Count.ShouldBe(1);
        events[0].ShouldBeOfType<ShowMessage>();
    }
}