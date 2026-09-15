using EstateManagementUI.BlazorServer.Common;
using EstateManagementUI.BlazorServer.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace EstateManagementUI.BlazorServer.Tests.Common;

public sealed class BootstrapperExtensionsTests
{
    [Fact]
    public async Task ConfigureLiveLogin_registers_only_the_live_authentication_route()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Services.AddRazorComponents();

        WebApplication app = builder.Build();
        app.MapRazorComponents<App>();

        app.ConfigureLiveLogin();

        await app.StartAsync(TestContext.Current.CancellationToken);

        var endpoints = app.Services.GetRequiredService<EndpointDataSource>().Endpoints;
        var loginEndpoints = endpoints
            .OfType<RouteEndpoint>()
            .Where(endpoint => endpoint.RoutePattern.RawText == "/login")
            .Where(endpoint => endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods.Contains("GET") == true)
            .ToList();
        var authenticationLoginEndpoints = endpoints
            .OfType<RouteEndpoint>()
            .Where(endpoint => endpoint.RoutePattern.RawText == "/authentication/login")
            .Where(endpoint => endpoint.Metadata.GetMetadata<HttpMethodMetadata>()?.HttpMethods.Contains("GET") == true)
            .ToList();

        loginEndpoints.Count.ShouldBe(1);
        authenticationLoginEndpoints.Count.ShouldBe(1);

        await app.StopAsync(TestContext.Current.CancellationToken);
    }
}
