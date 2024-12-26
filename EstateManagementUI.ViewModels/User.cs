using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.ViewModels;

[ExcludeFromCodeCoverage]
public record User
{
    public string EmailAddress { get; set; }

    public Guid Id { get; set; }
}