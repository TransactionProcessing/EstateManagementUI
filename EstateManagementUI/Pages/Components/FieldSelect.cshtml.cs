using Hydro;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EstateManagementUI.Pages.Components;

public class FieldSelect : HydroView
{
    public string Id { get; set; }
    public string Label { get; set; }
    public ModelExpression Field { get; set; }
    public IEnumerable<OptionItem> Options { get; set; } = [];
    public bool UseBlank { get; set; }
}

public record OptionItem(object Value, string Text);