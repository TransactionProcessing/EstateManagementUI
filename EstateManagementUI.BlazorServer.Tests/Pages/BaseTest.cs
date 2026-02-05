using System.Security.Claims;
using Bunit;
using Bunit.TestDoubles;
using EstateManagementUI.BlazorServer.Components.Permissions;
using EstateManagementUI.BlazorServer.Permissions;
using EstateManagementUI.BlazorServer.UIServices;
using MediatR;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EstateManagementUI.BlazorServer.Tests.Pages;

public abstract class BaseTest :TestContext {
    protected BaseTest() {
        this._mockMediator = new Mock<IMediator>();
        this._mockNavigationManager = new Mock<NavigationManager>();
        this._mockPermissionKeyProvider = new Mock<IPermissionKeyProvider>();
        this._mockAuthStateProvider = new Mock<AuthenticationStateProvider>();
        this._mockPermissionService = new Mock<IPermissionService>();
        this._mockPermissionStore = new Mock<IPermissionStore>();
        this._fakeNavigationManager = new FakeNavigationManager();
        this.EstateUIService = new Mock<IEstateUIService>();

        this._mockPermissionKeyProvider.Setup(x => x.GetKey()).Returns("test-key");
        this._mockPermissionService.Setup(x => x.HasPermissionAsync(It.IsAny<PermissionSection>(), It.IsAny<PermissionFunction>())).ReturnsAsync(true);

        this.Services.AddSingleton(this._mockMediator.Object);
        //this.Services.AddSingleton(this._mockNavigationManager.Object);
        Services.AddSingleton<NavigationManager>(_fakeNavigationManager); // register FakeNavigationManager
        this.Services.AddSingleton(this._mockPermissionKeyProvider.Object);
        this.Services.AddSingleton(this._mockPermissionService.Object);
        this.Services.AddSingleton(this._mockAuthStateProvider.Object);
        this.Services.AddSingleton(this._mockPermissionStore.Object);
        this.Services.AddSingleton(this.EstateUIService.Object);


        // Add required permission components that render their children
        this.ComponentFactories.AddStub<RequirePermission>(
            parameters => parameters.Get(p => p.ChildContent));
        this.ComponentFactories.AddStub<RequireSectionAccess>(
            parameters => parameters.Get(p => p.ChildContent));

        var claims = new[] { new Claim(ClaimTypes.Role, "Estate"), new Claim("estateId", Guid.NewGuid().ToString()), new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", "EstateUser") };
        this.AddTestAuthorization().SetClaims(claims);
    }

    protected readonly Mock<IMediator> _mockMediator;
    protected readonly Mock<NavigationManager> _mockNavigationManager;
    protected readonly Mock<IPermissionKeyProvider> _mockPermissionKeyProvider;
    protected readonly Mock<IPermissionService> _mockPermissionService;
    protected readonly Mock<AuthenticationStateProvider> _mockAuthStateProvider;
    protected readonly Mock<IPermissionStore> _mockPermissionStore;
    protected readonly FakeNavigationManager _fakeNavigationManager;
    protected readonly Mock<IEstateUIService> EstateUIService;

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