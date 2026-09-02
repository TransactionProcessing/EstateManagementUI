using System.Security.Claims;
using Bunit;
using Bunit.TestDoubles;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.UIServices;
using EstateManagementUI.BusinessLogic.Models;
using EstateManagementUI.BusinessLogic.Requests;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Imposter.Abstractions;
using SimpleResults;
using TestContext = Bunit.TestContext;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public abstract class BaseTest :TestContext {
    protected BaseTest() {
        this._mockMediator = new IMediatorImposter();
        this._mockNavigationManager = new NavigationManagerImposter();
        this._mockPermissionKeyProvider = new IPermissionKeyProviderImposter();
        this._mockAuthStateProvider = new AuthenticationStateProviderImposter();
        this._mockPermissionService = new IPermissionServiceImposter();
        this._mockPermissionStore = new IPermissionStoreImposter();
        this._fakeNavigationManager = new FakeNavigationManager();

        this._mockPermissionKeyProvider.GetKey().Returns("test-key");
        this._mockPermissionService.HasPermissionAsync(Arg<PermissionSection>.Any(), Arg<PermissionFunction>.Any()).ReturnsAsync(true);
        this.MerchantUIService.GetMerchants(Arg<CorrelationId>.Any(),
                                                         Arg<Guid>.Any(),
                                                         Arg<string>.Any(),
                                                         Arg<string>.Any(),
                                                         Arg<int?>.Any(),
                                                         Arg<string>.Any(),
                                                         Arg<string>.Any())
            .ReturnsAsync(Result.Success(new List<EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantListModel>()));
        this.MerchantUIService.GetMerchantsForDropDown(Arg<CorrelationId>.Any(), Arg<Guid>.Any())
            .ReturnsAsync(Result.Success(new List<EstateManagementUI.BlazorServer.Models.MerchantModels.MerchantDropDownModel>
            {
                new()
                {
                    MerchantId = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                    MerchantReportingId = 3333,
                    MerchantName = "Test Merchant"
                }
            }));
        this.FileProcessingUIService.GetFileProfiles(Arg<CancellationToken>.Any())
            .ReturnsAsync(Result.Success(new List<EstateManagementUI.BlazorServer.Models.FileProfileDropDownModel>
            {
                new()
                {
                    FileProfileId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Name = "SafaricomTopup"
                },
                new()
                {
                    FileProfileId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Name = "AirtelTopup"
                },
                new()
                {
                    FileProfileId = Guid.Parse("33333333-3333-3333-3333-333333333334"),
                    Name = "SettlementFile"
                }
            }));

        this.Services.AddSingleton(this._mockMediator.Instance());
        //this.Services.AddSingleton(this._mockNavigationManager.Instance());
        Services.AddSingleton<NavigationManager>(_fakeNavigationManager); // register FakeNavigationManager
        this.Services.AddSingleton(this._mockPermissionKeyProvider.Instance());
        this.Services.AddSingleton(this._mockPermissionService.Instance());
        this.Services.AddSingleton(this._mockAuthStateProvider.Instance());
        this.Services.AddSingleton(this._mockPermissionStore.Instance());
        this.Services.AddSingleton(this.EstateUIService.Instance());
        this.Services.AddSingleton(this.OperatorUIService.Instance());
        this.Services.AddSingleton(this.ContractUIService.Instance());
        this.Services.AddSingleton(this.MerchantUIService.Instance());
        this.Services.AddSingleton(this.FileProcessingUIService.Instance());


        // Add required permission components that render their children
        this.ComponentFactories.AddStub<RequirePermission>(
            parameters => parameters.Get(p => p.ChildContent));
        this.ComponentFactories.AddStub<RequireSectionAccess>(
            parameters => parameters.Get(p => p.ChildContent));

        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"), new Claim("estateId", Guid.NewGuid().ToString()), new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser") };
        this.AddTestAuthorization().SetClaims(claims);
    }

    protected readonly IMediatorImposter _mockMediator;
    protected readonly NavigationManagerImposter _mockNavigationManager;
    protected readonly IPermissionKeyProviderImposter _mockPermissionKeyProvider;
    protected readonly IPermissionServiceImposter _mockPermissionService;
    protected readonly AuthenticationStateProviderImposter _mockAuthStateProvider;
    protected readonly IPermissionStoreImposter _mockPermissionStore;
    protected readonly FakeNavigationManager _fakeNavigationManager;
    protected readonly IEstateUIServiceImposter EstateUIService = new IEstateUIServiceImposter();
    protected readonly IOperatorUIServiceImposter OperatorUIService = new IOperatorUIServiceImposter();
    protected readonly IContractUIServiceImposter ContractUIService = new IContractUIServiceImposter();
    protected readonly IMerchantUIServiceImposter MerchantUIService = new IMerchantUIServiceImposter();
    protected readonly IFileProcessingUIServiceImposter FileProcessingUIService = new IFileProcessingUIServiceImposter();
    /// <summary>
    /// Minimal test double for NavigationManager.
    /// Register in DI as NavigationManager so components receive it in tests.
    /// Use the <see cref="NavigatedUris"/> or <see cref="LastUri"/> to assert navigation.
    /// </summary>
    public class FakeNavigationManager : NavigationManager
    {
        public List<string> NavigatedUris { get; } = new();

        public FakeNavigationManager()
        {
            // sensible defaults for tests
            Initialize("http://localhost/", "http://localhost/");
        }

        protected override void NavigateToCore(String uri,
                                               NavigationOptions options) {
            var absolute = ToAbsoluteUri(uri).ToString();
            Uri = absolute; // protected setter on base is accessible here
            NavigatedUris.Add(absolute);
        }

        protected override void NavigateToCore(string uri, bool forceLoad)
        {
            // Ensure an absolute URI is recorded
            var absolute = ToAbsoluteUri(uri).ToString();
            Uri = absolute; // protected setter on base is accessible here
            NavigatedUris.Add(absolute);
        }

        public string? LastUri => NavigatedUris.Count > 0 ? NavigatedUris[^1] : null;
    }
}
