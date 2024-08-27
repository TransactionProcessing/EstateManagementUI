using Hydro;
using Hydro.Utils;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using EstateManagementUI.Common;

namespace EstateManagementUI.Pages.Shared.EditorTemplates;

public abstract class DynamicSelect : HydroComponent
{
    public DynamicSelect() {
        
    }

    public String AccessToken;
    public Guid EstateId;

    protected async Task PopulateTokenAndEstateId()
    {
        if (this.HttpContext != null)
            this.AccessToken = await this.HttpContext.GetTokenAsync("access_token");

        if (this.User != null)
            this.EstateId = Helpers.GetClaimValue<Guid>(this.User.Identity as ClaimsIdentity, Helpers.EstateIdClaimType);
    }

    public string Name { get; set; }
    public string Value { get; set; }
    public string Text { get; set; }
    public string Placeholder { get; set; }
    public bool Opened { get; set; }

    public Cache<Task<List<SelectItem>>> Items => Cache(GetItemsAsync);
    public override string ViewPath => GetViewPath(typeof(DynamicSelect));
    public virtual string ItemPartial => null;

    public void Open() =>
        Opened = true;

    public void Close() =>
        Opened = false;

    public async Task Blur()
    {
        Opened = false;
        await SetValue(await FindValueByTextAsync(Text));
    }

    public override async Task MountAsync() {
        await this.PopulateTokenAndEstateId();
        Text = await GetTextFromValueAsync(Value);
    }

    public override void Bind(PropertyPath property, object value)
    {
        if (property.Name == nameof(Text))
        {
            Open();
        }
    }

    public async Task Select(string key)
    {
        await SetValue(key);
        Close();
    }

    public async Task Clear()
    {
        await SetValue(null);
    }

    private async Task SetValue(string value)
    {
        Text = await GetTextFromValueAsync(value);

        if (value == Value)
        {
            return;
        }

        Value = value;
        Dispatch(new HydroBind(Name, Value));
    }

    protected virtual Task<List<SelectItem>> GetItemsAsync() =>
        Task.FromResult(GetItems());

    protected virtual List<SelectItem> GetItems() => Array.Empty<SelectItem>().ToList();

    protected virtual Task<string> FindValueByTextAsync(string text) =>
        Task.FromResult(FindValueByText(text));

    protected virtual string FindValueByText(string text) =>
        null;

    protected virtual Task<string> GetTextFromValueAsync(string value) =>
        Task.FromResult(GetTextFromValue(value));

    protected virtual string GetTextFromValue(string value) =>
        null;
}

public record SelectItem(string Key, string Text, string Description = null, object Metadata = null);