using System.Linq.Expressions;

namespace EstateManagementUI.Pages.Shared;

public record TableDropdownItem(string Text, Expression<Func<Task>> Action);