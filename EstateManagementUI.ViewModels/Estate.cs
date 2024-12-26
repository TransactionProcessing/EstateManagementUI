using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public record Estate
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Reference { get; set; }
}