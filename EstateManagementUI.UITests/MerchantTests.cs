using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Merchant.MerchantDetails;
using EstateManagementUI.Testing;
using EstateManagementUI.ViewModels;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using Merchant = EstateManagementUI.Pages.Merchant.MerchantDetails.Merchant;

namespace EstateManagementUI.UITests;

public static class TestHelper {
    public static IUrlHelper GetTestUrlHelper() {
        Mock<IUrlHelper> urlHelperMock = new Mock<IUrlHelper>();

        // Example: Mock Url.Action() to return some URL
        urlHelperMock
            .Setup(u => u.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns("/");
        urlHelperMock.Setup(u => u.ActionContext).Returns(new ActionContext
        {
            RouteData = new RouteData()
        });

        return urlHelperMock.Object;
    }
    public static ViewContext GetTestViewContext() {
        var httpContext = GetTestHttpContext();
        var viewData = new ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), new ModelStateDictionary());
        var writer = new System.IO.StringWriter();

        return new ViewContext
        {
            HttpContext = httpContext,
            ViewData = viewData,
            Writer = writer
        };
    }
    public static HttpContext GetTestHttpContext() {
        // Arrange
        var httpContext = new DefaultHttpContext();

        // Mock the authentication service
        var authenticationServiceMock = new Mock<IAuthenticationService>();

        // Return an authentication ticket with the access token
        var authTicket = new AuthenticationTicket(
            new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim("access_token", "mock-access-token")
            }, "Bearer")),
            "Bearer"
        );

        authenticationServiceMock
            .Setup(a => a.AuthenticateAsync(httpContext, It.IsAny<string>()))
            .ReturnsAsync(AuthenticateResult.Success(authTicket));

        httpContext.RequestServices = new ServiceCollection()
            .AddSingleton(authenticationServiceMock.Object)
            .BuildServiceProvider();

        var claims = new[] { new Claim("estateId", "45DD2B9C-C9B1-4CCA-89AC-CE0480FBC804") };
        var identity = new ClaimsIdentity(claims, "Bearer");
        var principal = new ClaimsPrincipal(identity);
        httpContext.User = principal;
        return httpContext;
    }
}

public class MerchantTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly Merchant _merchant;

    public MerchantTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._merchant = new Merchant(this._mediatorMock.Object, this._permissionsServiceMock.Object, "MerchantFunction");
        this._merchant.ViewContext = TestHelper.GetTestViewContext();
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
        this._merchant.SettlementScheduleId = 1;
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Commands.AddMerchantCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        // Act
        await this._merchant.Save();

        // Assert
        this._mediatorMock.Verify(m => m.Send(It.IsAny<Commands.AddMerchantCommand>(), It.IsAny<CancellationToken>()), Times.Once);
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
        this._merchant.SettlementScheduleId = 1;
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