using System.Linq.Expressions;

namespace EstateManagementUI.Pages.Shared;

public record TableDropdownItem(string Text, Expression<Action> Action);
