﻿using System.Diagnostics.CodeAnalysis;

namespace EstateManagementUI.BusinessLogic.Models;

[ExcludeFromCodeCoverage]
public class TodaysSalesCountByHourModel
{
    public int Hour { get; set; }

    public int TodaysSalesCount { get; set; }

    public int ComparisonSalesCount { get; set; }
}