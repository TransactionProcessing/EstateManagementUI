using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Contract
{
    public class ContractProducts : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid ContractId { get; set; }
    }
}
