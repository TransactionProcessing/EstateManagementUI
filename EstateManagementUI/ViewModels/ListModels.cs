﻿using Microsoft.AspNetCore.Mvc.Rendering;
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
}