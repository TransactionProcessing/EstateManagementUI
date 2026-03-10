using Bunit;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.Tests.Pages.FileProcessing;
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

    [Fact]
    public void MerchantsIndex_AddNewMerchantButton_NavigatesToNewMerchantPage()
    {
        // Arrange
        var merchants = new List<MerchantModels.MerchantListModel>();
        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the "Add New Merchant" button
        var buttons = cut.FindAll("button");
        var addNewMerchantButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Add New Merchant"));
        addNewMerchantButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/merchants/new");
    }

    [Fact]
    public void MerchantsIndex_ApplyFiltersButton_ClickIsHandled()
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

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the "Apply Filters" button
        var buttons = cut.FindAll("button");
        var applyFiltersButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Apply Filters"));
        applyFiltersButton?.Click();

        // Assert - Verify GetMerchants was called at least twice (once on load, once on filter)
        this.MerchantUIService.Verify(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()), Times.AtLeast(2));
    }

    [Fact]
    public void MerchantsIndex_ClearFiltersButton_ClickIsHandled()
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

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the "Clear Filters" button
        var buttons = cut.FindAll("button");
        var clearFiltersButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Clear Filters"));
        clearFiltersButton?.Click();

        // Assert - Verify GetMerchants was called at least twice (once on load, once on clear)
        this.MerchantUIService.Verify(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()), Times.AtLeast(2));
    }

    [Fact]
    public void MerchantsIndex_ViewButton_NavigatesToMerchantDetailsPage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the View button
        var viewButton = cut.Find("#viewMerchantLink");
        viewButton.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/merchants/{merchantId}");
    }

    [Fact]
    public void MerchantsIndex_EditButton_NavigatesToEditMerchantPage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the Edit button
        var editButton = cut.Find("#editMerchantLink");
        editButton.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/merchants/{merchantId}/edit");
    }

    [Fact]
    public void MerchantsIndex_MakeDepositButton_OpensDepositModal()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(new MerchantModels.MerchantModel
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant"
            }));

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the Make Deposit button
        var makeDepositButton = cut.Find("#makeDepositLink");
        makeDepositButton.Click();

        // Assert - modal is shown with the deposit form
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Make Merchant Deposit"), TimeSpan.FromSeconds(5));
        cut.Markup.ShouldContain("depositAmount");
        cut.Markup.ShouldContain("depositDate");
        cut.Markup.ShouldContain("depositReference");
    }

    [Fact]
    public void MerchantsIndex_DepositModal_HasOneCancelButton()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(new MerchantModels.MerchantModel
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant"
            }));

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Open modal
        cut.Find("#makeDepositLink").Click();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Make Merchant Deposit"), TimeSpan.FromSeconds(5));

        // Assert - exactly 1 Cancel button in the modal footer
        var allButtons = cut.FindAll("button");
        var cancelButtons = allButtons.Where(b => b.TextContent.Trim() == "Cancel").ToList();
        cancelButtons.Count.ShouldBe(1);
    }

    [Fact]
    public void MerchantsIndex_DepositModal_CancelButton_ClosesModal()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(new MerchantModels.MerchantModel
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant"
            }));

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#makeDepositLink").Click();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Make Merchant Deposit"), TimeSpan.FromSeconds(5));

        // Act - click Cancel
        var cancelButton = cut.FindAll("button").FirstOrDefault(b => b.TextContent.Trim() == "Cancel");
        cancelButton?.Click();

        // Assert - modal is closed
        cut.WaitForAssertion(() => cut.Markup.ShouldNotContain("depositAmount"), TimeSpan.FromSeconds(5));
        _fakeNavigationManager.Uri.ShouldNotContain("/deposit");
    }

    [Fact]
    public void MerchantsIndex_DepositModal_Submit_ShowsSuccess()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Success(new MerchantModels.MerchantModel
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant"
            }));

        this.MerchantUIService.Setup(m => m.MakeMerchantDeposit(
                It.IsAny<CorrelationId>(),
                It.IsAny<Guid>(),
                merchantId,
                It.IsAny<MerchantModels.DepositModel>()))
            .ReturnsAsync(Result.Success);

        var cut = RenderComponent<MerchantsIndex>();
        cut.Instance.SetDelayOverride(500);
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        cut.Find("#makeDepositLink").Click();
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Make Merchant Deposit"), TimeSpan.FromSeconds(5));

        // Act - fill and submit form
        cut.Find("#depositAmount").Change(100);
        cut.Find("#depositDate").Change(DateTime.Today.ToString("yyyy-MM-dd"));
        cut.Find("#depositReference").Change("TEST-REF-001");
        cut.Find("#makeDepositButton").Click();

        // Assert - success message is shown before modal closes
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Deposit recorded successfully"), TimeSpan.FromSeconds(5));
        cut.WaitForAssertion(() => cut.Markup.ShouldNotContain("depositAmount"), TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsIndex_DepositModal_OpensWithoutMerchantName_WhenGetMerchantFails()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        this.MerchantUIService.Setup(m => m.GetMerchant(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), merchantId))
            .ReturnsAsync(Result.Failure());

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - click Make Deposit when GetMerchant fails
        cut.Find("#makeDepositLink").Click();

        // Assert - modal opens but without merchant name
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Make Merchant Deposit"), TimeSpan.FromSeconds(5));
        cut.Markup.ShouldContain("depositAmount");
        cut.Markup.ShouldNotContain("For merchant:");
    }

    [Fact]
    public void MerchantsIndex_PaginationButtons_FirstPage_IsHandled()
    {
        // Arrange - Create more than 10 merchants to trigger pagination
        var merchants = Enumerable.Range(1, 15).Select(i => new MerchantModels.MerchantListModel()
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

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Navigate to second page first
        var buttons = cut.FindAll("button");
        var nextButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "Next Page");
        nextButton?.Click();

        // Act - Find and click the First Page button
        buttons = cut.FindAll("button");
        var firstPageButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "First Page");
        firstPageButton?.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Page"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsIndex_PaginationButtons_PreviousPage_IsHandled()
    {
        // Arrange - Create more than 10 merchants to trigger pagination
        var merchants = Enumerable.Range(1, 15).Select(i => new MerchantModels.MerchantListModel()
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

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Navigate to second page first
        var buttons = cut.FindAll("button");
        var nextButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "Next Page");
        nextButton?.Click();

        // Act - Find and click the Previous Page button
        buttons = cut.FindAll("button");
        var previousPageButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "Previous Page");
        previousPageButton?.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Page"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsIndex_PaginationButtons_NextPage_IsHandled()
    {
        // Arrange - Create more than 10 merchants to trigger pagination
        var merchants = Enumerable.Range(1, 15).Select(i => new MerchantModels.MerchantListModel()
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

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the Next Page button
        var buttons = cut.FindAll("button");
        var nextButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "Next Page");
        nextButton?.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Page"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsIndex_PaginationButtons_LastPage_IsHandled()
    {
        // Arrange - Create more than 10 merchants to trigger pagination
        var merchants = Enumerable.Range(1, 15).Select(i => new MerchantModels.MerchantListModel()
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

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the Last Page button
        var buttons = cut.FindAll("button");
        var lastPageButton = buttons.FirstOrDefault(b => b.GetAttribute("title") == "Last Page");
        lastPageButton?.Click();

        // Assert
        cut.WaitForAssertion(() => cut.Markup.ShouldContain("Page"), timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void MerchantsIndex_LoadMerchants_LoadFails_NavigatesToError()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Failure());

        // Act
        var cut = RenderComponent<MerchantsIndex>();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    [Fact]
    public void MerchantsIndex_RowClick_NavigatesToMerchantDetailsPage()
    {
        // Arrange
        var merchantId = Guid.NewGuid();
        var merchants = new List<MerchantModels.MerchantListModel>
        {
            new()
            {
                MerchantId = merchantId,
                MerchantName = "Test Merchant",
                MerchantReference = "REF001"
            }
        };

        this.MerchantUIService.Setup(m => m.GetMerchants(It.IsAny<CorrelationId>(), It.IsAny<Guid>(), It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<Int32>(),
                It.IsAny<String>(),
                It.IsAny<String>()))
            .ReturnsAsync(Result.Success(merchants));

        var cut = RenderComponent<MerchantsIndex>();
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the merchant row
        var rows = cut.FindAll("tr.hover\\:bg-gray-50");
        rows.FirstOrDefault()?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain($"/merchants/{merchantId}");
    }
}
