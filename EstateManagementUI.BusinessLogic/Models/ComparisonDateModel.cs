﻿using System;
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

    [ExcludeFromCodeCoverage]
    public class TodaysSalesModel
    {
        public Decimal TodaysSalesValue { get; set; }

        public int TodaysSalesCount { get; set; }

        public Decimal ComparisonSalesValue { get; set; }

        public int ComparisonSalesCount { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class TodaysSettlementModel
    {
        public Decimal TodaysSettlementValue { get; set; }

        public int TodaysSettlementCount { get; set; }

        public Decimal ComparisonSettlementValue { get; set; }

        public int ComparisonSettlementCount { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class TodaysSalesCountByHourModel
    {
        public int Hour { get; set; }

        public int TodaysSalesCount { get; set; }

        public int ComparisonSalesCount { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class TodaysSalesValueByHourModel
    {
        public int Hour { get; set; }

        public Decimal TodaysSalesValue { get; set; }

        public Decimal ComparisonSalesValue { get; set; }
    }
}