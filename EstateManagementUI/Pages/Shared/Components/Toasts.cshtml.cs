using EstateManagementUI.Common;
using Hydro;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.Pages.Shared.Components
{
    [ExcludeFromCodeCoverage]
    public class Toasts : StandardHydroComponent
    {
        public List<Toast> ToastsList { get; set; } = new();

        public Toasts()
        {
            Subscribe<ShowMessage>(Handle);
            Subscribe<UnhandledHydroError>(Handle);
        }

        private void Handle(ShowMessage data) =>
            ToastsList.Add(new Toast(
                Id: Guid.NewGuid().ToString("N"),
                Message: data.Message,
                Type: data.Type
            ));

        private void Handle(UnhandledHydroError data) =>
            ToastsList.Add(new Toast(
                Id: Guid.NewGuid().ToString("N"),
                Message: data.Message ?? "Unhand",
                Type: ToastType.Error
            ));

        public void Close(string id) =>
            ToastsList.RemoveAll(t => t.Id == id);

        public record Toast(string Id, string Message, ToastType Type);
    }

    public enum ToastType
    {
        Success,
        Error
    }

    [ExcludeFromCodeCoverage]
    public record ShowMessage(string Message, ToastType Type = ToastType.Success);
}
