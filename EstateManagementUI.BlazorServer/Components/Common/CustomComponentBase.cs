using Microsoft.AspNetCore.Components;

namespace EstateManagementUI.BlazorServer.Components.Common;

public abstract class CustomComponentBase : ComponentBase {

    private Int32? DelayOverride;

    public void SetDelayOverride(Int32 delay) => this.DelayOverride = delay;

    public async Task WaitOnUIRefresh(Int32 delay=2500) {
        Int32 localDelay = delay;
        if (this.DelayOverride.HasValue) {
            localDelay = this.DelayOverride.GetValueOrDefault();
        }
        await Task.Delay(localDelay);
    }

    protected string? successMessage;
    protected string? errorMessage;
    protected string activeTab;
    protected void ClearMessages()
    {
        this.successMessage = null;
        this.errorMessage = null;
    }

    protected void SetActiveTab(string tab)
    {
        this.activeTab = tab;
        this.ClearMessages();
    }
}