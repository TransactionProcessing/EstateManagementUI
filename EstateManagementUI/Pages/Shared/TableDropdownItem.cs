using System.Linq.Expressions;

namespace EstateManagementUI.Pages.Shared;

public record TableDropdownItem(string Text, Expression<Func<Task>> Action, (String applicationSection, String function) permission,String Id = null);
