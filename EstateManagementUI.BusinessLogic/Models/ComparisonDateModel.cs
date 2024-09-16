using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstateManagementUI.BusinessLogic.Models
{
    [ExcludeFromCodeCoverage]
    public class ComparisonDateModel
    {
        public int OrderValue { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }
    }
}
