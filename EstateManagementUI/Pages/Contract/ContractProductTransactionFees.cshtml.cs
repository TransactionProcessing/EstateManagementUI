using EstateManagementUI.BusinessLogic.Clients;
using EstateManagementUI.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EstateManagementUI.Pages.Contract
{
    public class ContractProductTransactionFees : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid ContractId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid ContractProductId { get; set; }
    }
}
