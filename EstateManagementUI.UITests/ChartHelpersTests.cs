using System.Text;
using Shouldly;

namespace EstateManagementUI.UITests;

public class ChartHelpersTests
{
    [Fact]
    public void ConvertChartOptionsToHtml_ValidChartOptions_ReturnsHtmlString()
    {
        // Arrange
        string chartOptions = "<div>Chart Options</div>";

        // Act
        string result = ChartHelpers.ConvertChartOptionsToHtml(chartOptions);

        // Assert
        result.ShouldBe(chartOptions);
    }

    [Fact]
    public void GetScriptForCharts_ValidInputs_ReturnsScriptString()
    {
        // Arrange
        string chartId = "chart1";
        string chartOptions = "{ type: 'bar' }";

        // Act
        string result = ChartHelpers.GetScriptForCharts(chartId, chartOptions);

        // Assert
        StringBuilder expectedScript = new StringBuilder();
        expectedScript.Append($"var options = {chartOptions};");
        expectedScript.Append($"var element = document.querySelector(\"#{chartId}\");");
        expectedScript.Append($"var {chartId} = new ApexCharts(element, options);");
        expectedScript.Append($"{chartId}.render();");

        result.ShouldBe(expectedScript.ToString());
    }
}

/*
public static class TestData
{
    public static List<ComparisonDateModel> ComparisonDates => new List<ComparisonDateModel>
    {
        new ComparisonDateModel { Date = DateTime.Parse("2023-01-01"), Description = "2023-01-01", OrderValue = 1 }
    };

    public static TodaysSalesModel TodaysSales => new TodaysSalesModel
    {
        TodaysSalesValue = 100,
        ComparisonSalesValue = 80
    };

    public static MerchantKpiModel MerchantKpi => new MerchantKpiModel
    {
        MerchantsWithNoSaleInLast7Days = 5,
        MerchantsWithNoSaleToday = 3,
        MerchantsWithSaleInLastHour = 2
    };

    public static List<TopBottomMerchantDataModel> BottomMerchants => new List<TopBottomMerchantDataModel>
    {
        new TopBottomMerchantDataModel { MerchantName = "Merchant1", SalesValue = 50 },
        new TopBottomMerchantDataModel { MerchantName = "Merchant2", SalesValue = 30 }
    };

    public static List<TopBottomOperatorDataModel> BottomOperators => new List<TopBottomOperatorDataModel>
    {
        new TopBottomOperatorDataModel { OperatorName = "Operator1", SalesValue = 40 },
        new TopBottomOperatorDataModel { OperatorName = "Operator2", SalesValue = 20 }
    };

    public static List<TopBottomProductDataModel> BottomProducts => new List<TopBottomProductDataModel>
    {
        new TopBottomProductDataModel { ProductName = "Product1", SalesValue = 60 },
        new TopBottomProductDataModel { ProductName = "Product2", SalesValue = 25 }
    };

    public static TodaysSalesModel TodaysFailedSales => new TodaysSalesModel
    {
        TodaysSalesValue = 70,
        ComparisonSalesValue = 50
    };
}*/