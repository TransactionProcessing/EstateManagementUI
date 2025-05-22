using EstateManagementUI.BusinessLogic.PermissionService;
using EstateManagementUI.Pages.Merchant.MerchantsList;
using EstateManagementUI.Testing;
using EstateManagmentUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Moq;
using Shouldly;
using System.Security.Claims;

namespace EstateManagementUI.UITests;

public class MerchantsListTests
{
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPermissionsService> _permissionsServiceMock;
    private readonly MerchantsList _merchantsList;

    public MerchantsListTests()
    {
        this._mediatorMock = new Mock<IMediator>();
        this._permissionsServiceMock = new Mock<IPermissionsService>();

        this._merchantsList = new MerchantsList(this._mediatorMock.Object, this._permissionsServiceMock.Object);
        this._merchantsList.ViewContext = TestHelper.GetTestViewContext();
    }

    [Fact]
    public async Task MountAsync_PopulatesMerchants()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._merchantsList.MountAsync();

        // Assert
        this._merchantsList.Merchants.ShouldNotBeNull();
        this._merchantsList.Merchants.Count.ShouldBe(2);
        this._merchantsList.Merchants[0].Name.ShouldBe("Merchant1");
        this._merchantsList.Merchants[1].Name.ShouldBe("Merchant2");
    }
    
    [Fact]
    public void Add_NavigatesToNewMerchantPage()
    {
        this._merchantsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        this._merchantsList.Add();

        // Assert
        this._merchantsList.LocationUrl.ShouldNotBeNull();
        this._merchantsList.LocationUrl.ShouldBe("/Merchant/NewMerchant");
    }

    [Fact]
    public async Task View_NavigatesToViewMerchantPage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        this._merchantsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await this._merchantsList.View(merchantId);

        // Assert
        this._merchantsList.LocationUrl.ShouldNotBeNull();
        this._merchantsList.LocationUrl.ShouldBe("/Merchant/ViewMerchant");
        Guid payloadMerchantId = TestHelpers.GetPropertyValue<Guid>(this._merchantsList.Payload, "MerchantId");
        payloadMerchantId.ShouldBe(merchantId);
    }

    [Fact]
    public async Task Edit_NavigatesToEditMerchantPage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        this._merchantsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await this._merchantsList.Edit(merchantId);

        // Assert
        this._merchantsList.LocationUrl.ShouldNotBeNull();
        this._merchantsList.LocationUrl.ShouldBe("/Merchant/EditMerchant");
        Guid payloadMerchantId = TestHelpers.GetPropertyValue<Guid>(this._merchantsList.Payload, "MerchantId");
        payloadMerchantId.ShouldBe(merchantId);
    }
    
    [Fact]
    public async Task MakeDeposit_NavigatesToMakeMerchantDepositPage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        this._merchantsList.Url = TestHelper.GetTestUrlHelper();

        // Act
        await this._merchantsList.MakeDeposit(merchantId);

        // Assert
        this._merchantsList.LocationUrl.ShouldNotBeNull();
        this._merchantsList.LocationUrl.ShouldBe("/Merchant/MakeDeposit");
        Guid payloadMerchantId = TestHelpers.GetPropertyValue<Guid>(this._merchantsList.Payload, "MerchantId");
        payloadMerchantId.ShouldBe(merchantId);
    }

    [Fact]
    public async Task Sort_SortsMerchantsByName()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._merchantsList.Sort(MerchantSorting.Name);

        // Assert
        this._merchantsList.Merchants.ShouldNotBeNull();
        this._merchantsList.Merchants.Count.ShouldBe(2);
        this._merchantsList.Merchants[0].Name.ShouldBe("Merchant2");
        this._merchantsList.Merchants[1].Name.ShouldBe("Merchant1");

        // Act
        await this._merchantsList.Sort(MerchantSorting.Name);

        this._merchantsList.Merchants[0].Name.ShouldBe("Merchant1");
        this._merchantsList.Merchants[1].Name.ShouldBe("Merchant2");
    }

    [Fact]
    public async Task Sort_SortsMerchantsByContact()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._merchantsList.Sort(MerchantSorting.Contact);

        // Assert
        this._merchantsList.Merchants.ShouldNotBeNull();
        this._merchantsList.Merchants.Count.ShouldBe(2);
        this._merchantsList.Merchants[0].ContactName.ShouldBe("Contact1");
        this._merchantsList.Merchants[1].ContactName.ShouldBe("Contact2");

        // Act
        await this._merchantsList.Sort(MerchantSorting.Contact);

        this._merchantsList.Merchants[0].ContactName.ShouldBe("Contact2");
        this._merchantsList.Merchants[1].ContactName.ShouldBe("Contact1");
    }

    [Fact]
    public async Task Sort_SortsMerchantsByAddressLine1()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._merchantsList.Sort(MerchantSorting.AddressLine1);

        // Assert
        this._merchantsList.Merchants.ShouldNotBeNull();
        this._merchantsList.Merchants.Count.ShouldBe(2);
        this._merchantsList.Merchants[0].AddressLine1.ShouldBe("AddressLine1");
        this._merchantsList.Merchants[1].AddressLine1.ShouldBe("AddressLine2");

        // Act
        await this._merchantsList.Sort(MerchantSorting.AddressLine1);

        this._merchantsList.Merchants[0].AddressLine1.ShouldBe("AddressLine2");
        this._merchantsList.Merchants[1].AddressLine1.ShouldBe("AddressLine1");
    }

    [Fact]
    public async Task Sort_SortsMerchantsByReference()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._merchantsList.Sort(MerchantSorting.Reference);

        // Assert
        this._merchantsList.Merchants.ShouldNotBeNull();
        this._merchantsList.Merchants.Count.ShouldBe(2);
        this._merchantsList.Merchants[0].Reference.ShouldBe("Reference1");
        this._merchantsList.Merchants[1].Reference.ShouldBe("Reference2");

        // Act
        await this._merchantsList.Sort(MerchantSorting.Reference);

        this._merchantsList.Merchants[0].Reference.ShouldBe("Reference2");
        this._merchantsList.Merchants[1].Reference.ShouldBe("Reference1");
    }

    [Fact]
    public async Task Sort_SortsMerchantsBySettlementSchedule()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._merchantsList.Sort(MerchantSorting.SettlementSchedule);

        // Assert
        this._merchantsList.Merchants.ShouldNotBeNull();
        this._merchantsList.Merchants.Count.ShouldBe(2);
        this._merchantsList.Merchants[0].SettlementSchedule.ShouldBe("Immediate");
        this._merchantsList.Merchants[1].SettlementSchedule.ShouldBe("Monthly");

        // Act
        await this._merchantsList.Sort(MerchantSorting.SettlementSchedule);

        this._merchantsList.Merchants[0].SettlementSchedule.ShouldBe("Monthly");
        this._merchantsList.Merchants[1].SettlementSchedule.ShouldBe("Immediate");
    }

    [Fact]
    public async Task Sort_SortsMerchantsByTown()
    {
        // Arrange
        this._mediatorMock.Setup(m => m.Send(It.IsAny<Queries.GetMerchantsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(TestData.MerchantsResult);

        // Act
        await this._merchantsList.Sort(MerchantSorting.Town);

        // Assert
        this._merchantsList.Merchants.ShouldNotBeNull();
        this._merchantsList.Merchants.Count.ShouldBe(2);
        this._merchantsList.Merchants[0].Town.ShouldBe("Town1");
        this._merchantsList.Merchants[1].Town.ShouldBe("Town2");

        // Act
        await this._merchantsList.Sort(MerchantSorting.Town);

        this._merchantsList.Merchants[0].Town.ShouldBe("Town2");
        this._merchantsList.Merchants[1].Town.ShouldBe("Town1");
    }
}