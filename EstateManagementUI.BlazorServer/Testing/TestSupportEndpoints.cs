using EstateManagementUI.BusinessLogic.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using SimpleResults;

namespace EstateManagementUI.BlazorServer.Testing;

public static class TestSupportEndpoints
{
    private const string TestUserRoleKey = "TestUserRole";

    public static IEndpointRouteBuilder MapTestSupportEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/test-support");
        group.MapGet("/ping", () => Results.Ok());
        group.MapPost("/login", (
            HttpContext context,
            [FromForm] string username,
            [FromForm] string password,
            [FromForm] string? returnUrl) =>
        {
            var role = ResolveRole(username);

            context.Session.SetString(TestUserRoleKey, role);
            context.Response.Cookies.Append(
                TestUserRoleKey,
                role,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true
                });

            return Results.Redirect(string.IsNullOrWhiteSpace(returnUrl) ? "/" : returnUrl);
        }).DisableAntiforgery();
        group.MapGet("/login/{role}", (HttpContext context, string role) =>
        {
            context.Session.SetString(TestUserRoleKey, role);
            context.Response.Cookies.Append(
                TestUserRoleKey,
                role,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Path = "/",
                    SameSite = SameSiteMode.Lax,
                    IsEssential = true
                });
            return Results.Redirect("/");
        });
        group.MapPost("/reset", (TestMediator mediator, TestApiClient apiClient, TestSupportState state) =>
        {
            mediator.Reset();
            apiClient.Reset();
            state.Reset();
            return Results.Ok();
        });

        group.MapGet("/merchant-schedules/{estateId:guid}/{merchantId:guid}/{year:int}", (Guid estateId, Guid merchantId, int year, TestSupportState state) =>
        {
            return state.TryGetMerchantSchedule(estateId, merchantId, year, out var schedule) && schedule is not null
                ? Results.Ok(schedule)
                : Results.NotFound();
        });

        group.MapPut("/merchant-schedules/{estateId:guid}/{merchantId:guid}", (Guid estateId, Guid merchantId, MerchantModels.MerchantScheduleModel schedule, TestSupportState state) =>
        {
            state.SetMerchantSchedule(estateId, merchantId, schedule);
            return Results.Ok();
        });

        group.MapGet("/merchant-opening-hours/{estateId:guid}/{merchantId:guid}", (Guid estateId, Guid merchantId, TestSupportState state) =>
        {
            return state.TryGetMerchantOpeningHours(estateId, merchantId, out var openingHours) && openingHours is not null
                ? Results.Ok(openingHours)
                : Results.NotFound();
        });

        group.MapPut("/merchant-opening-hours/{estateId:guid}/{merchantId:guid}", (Guid estateId, Guid merchantId, MerchantModels.MerchantOpeningHoursModel openingHours, TestSupportState state) =>
        {
            state.SetMerchantOpeningHours(estateId, merchantId, openingHours);
            return Results.Ok();
        });

        group.MapGet("/file-import-logs", (TestSupportState state) => Results.Ok(state.GetFileImportLogs()));
        group.MapGet("/file-import-logs/{fileImportLogId:guid}", (Guid fileImportLogId, TestSupportState state) =>
        {
            var log = state.GetFileImportLog(fileImportLogId);
            return log is null ? Results.NotFound() : Results.Ok(log);
        });

        return endpoints;
    }

    private static string ResolveRole(string username)
    {
        return username.StartsWith("administrator@", StringComparison.OrdinalIgnoreCase)
            ? "Administrator"
            : "Estate";
    }
}
