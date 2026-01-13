using EstateManagementUI.BusinessLogic.PermissionService;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Logger;
using SimpleResults;

namespace EstateManagementUI.Pages.Services
{
    public interface INavigationService {
        string GetCurrentPageClass(ViewContext viewContext,
                                   string prefix);

        Task<string> RenderItem(string userName,
                                string title,
                                string name,
                                string id,
                                string pageName = "Index");
    }
    public class NavigationService : INavigationService
    {
        private readonly IPermissionsService PermissionsService;
        public NavigationService(IPermissionsService permissionsService) {
            this.PermissionsService = permissionsService;
        }

        public string GetCurrentPageClass(ViewContext viewContext, string prefix)
        {
            string page = viewContext.RouteData.Values["page"]?.ToString() ?? string.Empty;
            return page.StartsWith($"/{prefix}") ? "btn btn-sm btn-neutral btn-active" : "btn btn-sm btn-ghost";
        }

        public async Task<string> RenderItem(string userName, string title, string name, string id, string pageName = "Index")
        {
        Result permissionResult = await this.PermissionsService.DoIHavePermissions(userName, name);
        if (permissionResult.IsFailed)
        {
            Logger.LogWarning(permissionResult.Message);
            return string.Empty;
        }
        else
        {
            Logger.LogWarning($"Permission granted for userName: {userName} SectionName: {name}");
        }



        var link = $"/{name}/{pageName}";
        var icon = name switch
        {
            "Estate" => "fa-solid fa-network-wired",
            "Merchant" => "fa-solid fa-store",
            "Contract" => "fa-solid fa-file-contract",
            "Operator" => "fa-solid fa-building-columns",
            "Reporting" => "fa-solid fa-chart-simple",
            "File Processing" => "fa-solid fa-file-csv",
            _ => "fa-solid fa-gauge-high"
        };

        return $@"
            <a class='nav-link' href='{link}' id='{id}'>
                <i class='{icon}'></i>
                <partial name='Loading'/>
                <p>{title}</p>
            </a>";
        }
    }
}
