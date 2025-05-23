using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using EstateManagementUI.Pages.Shared.Components;
using EstateManagementUI.Testing;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using Hydro;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Shouldly;
using SimpleResults;
using System.Reflection;
using Merchant = EstateManagementUI.Pages.Merchant.MerchantDetails.Merchant;

namespace EstateManagementUI.UITests;

public class MerchantDetailsTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Merchant _merchant;

    public MerchantDetailsTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();
        
        this._merchant = new Merchant(this._mediatorMock.Object, this._permissionsServiceMock.Object, "MerchantFunction");
        this._merchant.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task Clone_NavigatesToMerchantIndexPage()
    {
        // Arrange
        //var merchantId = Guid.NewGuid();
        this._merchant.Url = TestHelper.GetTestUrlHelper();

        // Act
        this._merchant.Close();

        // Assert
        this._merchant.LocationUrl.ShouldNotBeNull();
        this._merchant.LocationUrl.ShouldBe("/Merchant/Index");
    }

    [Fact]
    public async Task MountAsync_LoadsMerchant_WhenMerchantIdIsNotEmpty()
    {
        // Arrange
        this._merchant.MerchantId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantResult);
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetContractsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.ContractsResult);

        // Act
        await this._merchant.MountAsync();

        // Assert
        this._merchant.Name.ShouldBe("Test Merchant");
        this._merchant.Reference.ShouldBe("Ref123");
        this._merchant.Address.AddressLine1.ShouldBe("123 Main St");
        this._merchant.Contact.ContactName.ShouldBe("John Doe");
        this._merchant.Operators.ShouldNotBeNull();
        this._merchant.Operators.Count.ShouldBe(1);
        this._merchant.Contracts.ShouldNotBeNull();
        this._merchant.Contracts.Count.ShouldBe(1);
        this._merchant.Devices.ShouldNotBeNull();
        this._merchant.Devices.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Save_AddsNewMerchant_WhenMerchantIdIsEmpty()
    {
        // Arrange
        this._merchant.MerchantId = Guid.Empty;
        this._merchant.Name = "New Merchant";
        this._merchant.Reference = "Ref123";
        this._merchant.Address = new AddressViewModel
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 100",
            Town = "Anytown",
            Region = "Anystate",
            Country = "USA",
            PostCode = "12345"
        };
        this._merchant.Contact = new ContactViewModel
        {
            ContactName = "John Doe",
            EmailAddress = "john.doe@example.com",
            PhoneNumber = "555-1234"
        };
        this._merchant.SettlementScheduleId = 0;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AddMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._merchant.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AddMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Save_CreateFails()
    {
        // Arrange
        this._merchant.MerchantId = Guid.Empty;
        this._merchant.Name = "New Merchant";
        this._merchant.Reference = "Ref123";
        this._merchant.Address = new AddressViewModel
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 100",
            Town = "Anytown",
            Region = "Anystate",
            Country = "USA",
            PostCode = "12345"
        };
        this._merchant.Contact = new ContactViewModel
        {
            ContactName = "John Doe",
            EmailAddress = "john.doe@example.com",
            PhoneNumber = "555-1234"
        };
        this._merchant.SettlementScheduleId = 0;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AddMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<String>() {"Error"}));

        // Act
        await this._merchant.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AddMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._merchant.Events.Count.ShouldBe(1);
        this._merchant.Events[0].ShouldBeOfType(typeof(ShowMessage));
    }

    [Fact]
    public async Task Save_UpdatesMerchant_WhenMerchantIdIsNotEmpty()
    {
        // Arrange
        this._merchant.MerchantId = Guid.NewGuid();
        this._merchant.Name = "Updated Merchant";
        this._merchant.Reference = "Ref123";
        this._merchant.Address = new AddressViewModel
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 100",
            Town = "Anytown",
            Region = "Anystate",
            Country = "USA",
            PostCode = "12345"
        };
        this._merchant.Contact = new ContactViewModel
        {
            ContactName = "John Doe",
            EmailAddress = "john.doe@example.com",
            PhoneNumber = "555-1234"
        };
        this._merchant.SettlementScheduleId = 0;

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._merchant.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Save_UpdateMerchantFails()
    {
        // Arrange
        this._merchant.MerchantId = Guid.NewGuid();
        this._merchant.Name = "Updated Merchant";
        this._merchant.Reference = "Ref123";
        this._merchant.Address = new AddressViewModel
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 100",
            Town = "Anytown",
            Region = "Anystate",
            Country = "USA",
            PostCode = "12345"
        };
        this._merchant.Contact = new ContactViewModel
        {
            ContactName = "John Doe",
            EmailAddress = "john.doe@example.com",
            PhoneNumber = "555-1234"
        };
        this._merchant.SettlementScheduleId = 0;

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<String>() {"Error"}));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._merchant.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        this._merchant.Events.Count.ShouldBe(1);
        this._merchant.Events[0].ShouldBeOfType<ShowMessage>();
    }

    [Fact]
    public async Task Save_UpdateMerchantAddressFails()
    {
        // Arrange
        this._merchant.MerchantId = Guid.NewGuid();
        this._merchant.Name = "Updated Merchant";
        this._merchant.Reference = "Ref123";
        this._merchant.Address = new AddressViewModel
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 100",
            Town = "Anytown",
            Region = "Anystate",
            Country = "USA",
            PostCode = "12345"
        };
        this._merchant.Contact = new ContactViewModel
        {
            ContactName = "John Doe",
            EmailAddress = "john.doe@example.com",
            PhoneNumber = "555-1234"
        };
        this._merchant.SettlementScheduleId = 0;

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<String>() { "Error" }));
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._merchant.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        this._merchant.Events.Count.ShouldBe(1);
        this._merchant.Events[0].ShouldBeOfType<ShowMessage>();
    }

    [Fact]
    public async Task Save_UpdateMerchantContactFails()
    {
        // Arrange
        this._merchant.MerchantId = Guid.NewGuid();
        this._merchant.Name = "Updated Merchant";
        this._merchant.Reference = "Ref123";
        this._merchant.Address = new AddressViewModel
        {
            AddressLine1 = "123 Main St",
            AddressLine2 = "Suite 100",
            Town = "Anytown",
            Region = "Anystate",
            Country = "USA",
            PostCode = "12345"
        };
        this._merchant.Contact = new ContactViewModel
        {
            ContactName = "John Doe",
            EmailAddress = "john.doe@example.com",
            PhoneNumber = "555-1234"
        };
        this._merchant.SettlementScheduleId = 0;

        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure(new List<String>() { "Error" }));

        // Act
        await this._merchant.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantAddressCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.UpdateMerchantContactCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        this._merchant.Events.Count.ShouldBe(1);
        this._merchant.Events[0].ShouldBeOfType<ShowMessage>();
    }

    [Fact]
    public async Task RemoveOperator_RemovesOperatorFromMerchant()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var operatorId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.RemoveOperatorFromMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._merchant.RemoveOperator(merchantId, operatorId);

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.RemoveOperatorFromMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveContract_RemovesContractFromMerchant()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var contractId = Guid.NewGuid();
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.RemoveContractFromMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._merchant.RemoveContract(merchantId, contractId);

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.RemoveContractFromMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}