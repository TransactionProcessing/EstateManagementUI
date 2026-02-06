using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
using EstateManagementUI.BusinessLogic.BackendAPI.DataTransferObjects;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Shouldly;
using SimpleResults;
using MerchantsIndex = EstateManagementUI.BlazorServer.Components.Pages.Merchants.Index;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsIndexPageTests : BaseTest
{
    [Fact]
    public void MerchantsIndex_InitialState_ShowsLoadingIndicator()
    {
        // Arrange
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new() {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
            It.IsAny<String>(),
            It.IsAny<Int32>(),
            It.IsAny<String>(),
            It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        // Act
        var cut = RenderComponent<MerchantsIndex>();
        
        // Assert
        cut.Markup.ShouldContain("Merchant Management");
    }

    [Fact]
    public void MerchantsIndex_WithNoMerchants_ShowsEmptyState()
    {
        // Arrange
        var merchants = new List<MerchantModels.MerchantListModel>();
        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"));
        
        // Assert
        cut.Markup.ShouldContain("No merchants found");
    }

    [Fact]
    public void MerchantsIndex_WithMerchants_DisplaysMerchantList()
    {
        // Arrange
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new MerchantModels.MerchantListModel()
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant 1",
                MerchantReference = "REF001"
            },
            new MerchantModels.MerchantListModel()
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant 2",
                MerchantReference = "REF002"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));


        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Test Merchant 1");
        cut.Markup.ShouldContain("Test Merchant 2");
        cut.Markup.ShouldContain("REF001");
        cut.Markup.ShouldContain("REF002");
    }

    [Fact]
    public void MerchantsIndex_WithMerchants_DisplaysSummaryCards()
    {
        // Arrange
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Merchant 1"
            },
            new()
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Merchant 2"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));


        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Total Merchants");
        cut.Markup.ShouldContain("Total Balance");
        cut.Markup.ShouldContain("Available Balance");
    }

    [Fact]
    public void MerchantsIndex_HasCorrectPageTitle()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(new List<MerchantModels.MerchantListModel>()));


        // Act
        var cut = RenderComponent<MerchantsIndex>();
        
        // Assert
        var pageTitle = cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void MerchantsIndex_WithMerchants_DisplaysFilters()
    {
        // Arrange
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));


        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Filters");
        cut.Markup.ShouldContain("Name");
        cut.Markup.ShouldContain("Reference");
        cut.Markup.ShouldContain("Settlement Schedule");
        cut.Markup.ShouldContain("Region");
        cut.Markup.ShouldContain("Postcode");
    }

    [Fact]
    public void MerchantsIndex_WithManyMerchants_DisplaysPagination()
    {
        // Arrange - Create more than 10 merchants to trigger pagination
        var merchants = Enumerable.Range(1, 12).Select(i => new  MerchantModels.MerchantListModel()
        {
            MerchantId = Guid.NewGuid(),
            MerchantName = $"Merchant {i}"
        }).ToList();

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));


        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Page");
        cut.Markup.ShouldContain("Showing");
        cut.Markup.ShouldContain("results");
    }

    [Fact]
    public void MerchantsIndex_WithMerchants_DisplaysRegionAndPostcode()
    {
        // Arrange
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = Guid.NewGuid(),
                MerchantName = "Test Merchant",
                Region = "North Region",
                PostalCode = "12345"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));


        // Act
        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("North Region");
        cut.Markup.ShouldContain("12345");
    }
}
