using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EstateManagementUI.ViewModels
{
    public class MerchantListModel
    {
        public List<SelectListItem> Merchants { get; set; }

        public String MerchantId { get; set; }
    }

    public class DateModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime? SelectedDate { get; set; }
    }

    public class OperatorListModel
    {
        public List<SelectListItem> Operators { get; set; }

        public String OperatorId { get; set; }
    }

    public class ComparisonDateListModel : DateModel
    {
        public List<SelectListItem> Dates { get; set; }
    }

    public class SettlementScheduleListModel
    {
        public List<SelectListItem> SettlementSchedule { get; set; }

        public Int32 SettlementScheduleId { get; set; }
    }

    public class ContractListModel
    {
        public List<SelectListItem> Contracts { get; set; }

        public String ContractId { get; set; }
    }

    public class ProductTypeListModel
    {
        public List<SelectListItem> ProductTypes { get; set; }

        public String ProductTypeId { get; set; }
    }


}
