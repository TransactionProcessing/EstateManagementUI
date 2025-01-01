using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.Pages.Operator;
using Hydro;
using Microsoft.AspNetCore.Authentication;
using SimpleResults;
using System.Security.Claims;
using EstateManagementUI.BusinessLogic.PermissionService;
using Hydro.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Common;

[ExcludeFromCodeCoverage]
public class SecureHydroComponent : HydroComponent {

    private readonly String SectionName;
    private readonly String PageName;
    private readonly IPermissionsService PermissionsService;

    public SecureHydroComponent(String sectionName,
                                String pageName,
                                IPermissionsService permissionsService) : base() {
        this.SectionName = sectionName;
        this.PermissionsService = permissionsService;
        this.PageName = pageName;
    }

    private static string GetFullTypeName(Type type) =>
        type.DeclaringType != null
            ? type.DeclaringType.Name + "+" + type.Name
            : type.Name;

    protected String AccessToken;
    protected Guid EstateId;
    public new void Dispatch<TEvent>(TEvent data, Scope scope = Scope.Parent, bool asynchronous = false)
    {
        Dispatch(GetFullTypeName(typeof(TEvent)), data, scope, asynchronous);
    }

    public List<Object> Events = new List<Object>();
    public String LocationUrl;
    public object Payload;


    public new void Dispatch<TEvent>(string name,
                                     TEvent data,
                                     Scope scope = Scope.Parent,
                                     bool asynchronous = false,
                                     string subject = null) {
        if (this.HttpContext != null) {
            base.Dispatch(name, data, scope, asynchronous, subject);
        }
        else {
            this.Events.Add(data);
        }
    }

    public new void Location(string url,
                             object payload = null) {
        if (this.HttpContext != null) {
            base.Location(this.Url.Page(url, payload));
        }
        else {
            this.LocationUrl = url;
            this.Payload = payload;
        }
    }

    protected async Task PopulateTokenAndEstateId() {
        if (this.HttpContext != null)
            this.AccessToken = await this.HttpContext.GetTokenAsync("access_token");

        if (this.User != null)
            this.EstateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
    }

    public override async Task RenderAsync()
    {
        String userName = this.User.Identity.Name;
        Result permissionsResult = await this.PermissionsService.DoIHavePermissions(userName, this.SectionName, this.PageName);
        if (permissionsResult.IsFailed)
        {
            Shared.Logger.Logger.LogWarning(permissionsResult.Message);
            // TODO: might make this configurable
            this.HttpContext.Response.Redirect("/Error");
            return;
        }
        else {
            Shared.Logger.Logger.LogWarning($"Permission granted for userName: {userName} SectionName: {this.SectionName} PageName: {this.PageName}");
        }
        
        await base.RenderAsync();
    }

    public async Task<Boolean> CanRenderButton(String sectionName, String pageName) {
        String userName = this.User.Identity.Name;
        Result permissionsResult = await this.PermissionsService.DoIHavePermissions(userName, sectionName, pageName);
        return permissionsResult.IsSuccess;
    }
}