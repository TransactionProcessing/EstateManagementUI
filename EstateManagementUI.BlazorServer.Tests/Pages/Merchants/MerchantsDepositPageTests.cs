using AngleSharp.Dom;
using Bunit;
using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Models;
using EstateManagementUI.BusinessLogic.Requests;
using Moq;
using Shouldly;
using SimpleResults;
using MerchantsDeposit = EstateManagementUI.BlazorServer.Components.Pages.Merchants.Deposit;

namespace EstateManagementUI.BlazorServer.Tests.Pages.Merchants;

public class MerchantsDepositPageTests : BaseTest
{
    private readonly Guid _testMerchantId = Guid.NewGuid();

    [Fact]
    public void Deposit_RendersCorrectly()
    {
        // Arrange
        SetupSuccessfulMerchantLoad();

        // Act
        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        
        // Assert
        cut.Markup.ShouldContain("Make Merchant Deposit");
    }

    [Fact]
    public void Deposit_HasCorrectPageTitle()
    {
        // Arrange
        SetupSuccessfulMerchantLoad();

        // Act
        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        
        // Assert
        IRenderedComponent<Microsoft.AspNetCore.Components.Web.PageTitle> pageTitle = 
            cut.FindComponent<Microsoft.AspNetCore.Components.Web.PageTitle>();
        pageTitle.Instance.ChildContent.ShouldNotBeNull();
    }

    [Fact]
    public void Deposit_DisplaysMerchantName()
    {
        // Arrange
        SetupSuccessfulMerchantLoad("Test Merchant Name");

        // Act
        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("For merchant: Test Merchant Name");
    }

    [Fact]
    public void Deposit_DisplaysFormFields()
    {
        // Arrange
        SetupSuccessfulMerchantLoad();

        // Act
        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));
        
        // Assert
        cut.Markup.ShouldContain("Deposit Amount");
        cut.Markup.ShouldContain("Date of Deposit");
        cut.Markup.ShouldContain("Reference");
        
        IElement depositAmountInput = cut.Find("#depositAmount");
        depositAmountInput.ShouldNotBeNull();
        
        IElement depositDateInput = cut.Find("#depositDate");
        depositDateInput.ShouldNotBeNull();
        
        IElement depositReferenceInput = cut.Find("#depositReference");
        depositReferenceInput.ShouldNotBeNull();
    }

    [Fact]
    public void Deposit_MakeDepositButton_SubmitsForm()
    {
        // Arrange
        SetupSuccessfulMerchantLoad();
        this.MerchantUIService.Setup(m => m.MakeMerchantDeposit(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.DepositModel>()))
            .ReturnsAsync(Result.Success);

        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        cut.Instance.SetDelayOverride(0);
        cut.Render(); // required to trigger re-render
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Fill form fields
        IElement depositAmountInput = cut.Find("#depositAmount");
        depositAmountInput.Change(100);
        
        IElement depositDateInput = cut.Find("#depositDate");
        depositDateInput.Change(DateTime.Today.ToString("yyyy-MM-dd"));
        
        IElement depositReferenceInput = cut.Find("#depositReference");
        depositReferenceInput.Change("TEST-REF-001");

        // Find and click the Make Deposit button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? makeDepositButton = buttons.FirstOrDefault(b => 
            b.TextContent.Contains("Make Deposit") && 
            (b.GetAttribute("id") ?? "") == "makeDepositButton");
        makeDepositButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => 
            cut.Markup.ShouldContain("Deposit recorded successfully"), 
            timeout: TimeSpan.FromSeconds(5));
        
        // Verify navigation to merchants index
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    [Fact]
    public void Deposit_MakeDepositButton_ShowsErrorOnFailure()
    {
        // Arrange
        SetupSuccessfulMerchantLoad();
        this.MerchantUIService.Setup(m => m.MakeMerchantDeposit(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>(), 
            It.IsAny<MerchantModels.DepositModel>()))
            .ReturnsAsync(Result.Failure());

        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Fill form fields
        IElement depositAmountInput = cut.Find("#depositAmount");
        depositAmountInput.Change(100);
        
        IElement depositDateInput = cut.Find("#depositDate");
        depositDateInput.Change(DateTime.Today.ToString("yyyy-MM-dd"));
        
        IElement depositReferenceInput = cut.Find("#depositReference");
        depositReferenceInput.Change("TEST-REF-001");

        // Find and click the Make Deposit button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? makeDepositButton = buttons.FirstOrDefault(b => 
            b.TextContent.Contains("Make Deposit") && 
            (b.GetAttribute("id") ?? "") == "makeDepositButton");
        makeDepositButton?.Click();
        
        // Assert
        cut.WaitForAssertion(() => 
            cut.Markup.ShouldContain("Failed to make deposit"), 
            timeout: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Deposit_CancelButton_NavigatesToMerchantsIndex()
    {
        // Arrange
        SetupSuccessfulMerchantLoad();

        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        cut.WaitForState(() => !cut.Markup.Contains("animate-spin"), TimeSpan.FromSeconds(5));

        // Act - Find and click the Cancel button
        IRefreshableElementCollection<IElement> buttons = cut.FindAll("button");
        IElement? cancelButton = buttons.FirstOrDefault(b => b.TextContent.Contains("Cancel"));
        cancelButton?.Click();

        // Assert
        _fakeNavigationManager.Uri.ShouldContain("/merchants");
    }

    [Fact]
    public void Deposit_LoadMerchant_LoadFails_NavigatesToError()
    {
        // Arrange
        this.MerchantUIService.Setup(m => m.GetMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>()))
            .ReturnsAsync(Result.Failure());

        // Act
        IRenderedComponent<MerchantsDeposit> cut = RenderComponent<MerchantsDeposit>(parameters => 
            parameters.Add(p => p.MerchantId, _testMerchantId));
        
        // Assert
        _fakeNavigationManager.Uri.ShouldContain("error");
    }

    // Helper methods
    private void SetupSuccessfulMerchantLoad(string merchantName = "Test Merchant")
    {
        MerchantModels.MerchantModel merchant = new()
        {
            MerchantId = _testMerchantId,
            MerchantName = merchantName,
            MerchantReference = "TEST-REF"
        };

        this.MerchantUIService.Setup(m => m.GetMerchant(
            It.IsAny<CorrelationId>(), 
            It.IsAny<Guid>(), 
            It.IsAny<Guid>()))
            .ReturnsAsync(Result.Success(merchant));
    }
}
