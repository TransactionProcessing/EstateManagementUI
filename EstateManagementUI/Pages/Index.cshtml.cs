using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shared.General;

namespace EstateManagementUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration) {
            _logger = logger;
            this._configuration = configuration;


            String authority = ConfigurationReader.GetValue("Authority");
        }

        public void OnGet()
        {

        }

        public async Task LogIn() {

        }
    }
}
