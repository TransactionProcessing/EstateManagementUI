using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Estate.ViewEstate;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace EstateManagementUI.UITests;

public class ViewEstateTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly ViewEstate _viewEstate;

    public ViewEstateTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        
        this._viewEstate = new ViewEstate(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._viewEstate.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_LoadsEstate_WhenEstateIdIsNotEmpty()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetEstateQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ViewEstateResult);

        // Act
        await this._viewEstate.MountAsync();

        // Assert
        this._viewEstate.Estate.ShouldNotBeNull();
        this._viewEstate.Estate.Name.ShouldBe("Test Estate");
        this._viewEstate.Estate.Id.ShouldBe(TestData.ViewEstate.EstateId);
        this._viewEstate.Estate.Reference.ShouldBe("Ref123");
    }

}