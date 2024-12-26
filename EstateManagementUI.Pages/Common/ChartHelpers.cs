using System.Text;
using Microsoft.AspNetCore.Html;

public static class ChartHelpers {
    public static string ConvertChartOptionsToHtml(String chartOptions)
    {
        IHtmlContent htmlContent = new HtmlContentBuilder().AppendHtml(chartOptions);
        StringWriter sw = new StringWriter();
        htmlContent.WriteTo(sw, System.Text.Encodings.Web.HtmlEncoder.Default);
        return sw.ToString();
    }

    public static string GetScriptForCharts(String chartId, String chartOptions)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"var options = {chartOptions};");
        sb.Append($"var element = document.querySelector(\"#{chartId}\");");
        sb.Append($"var {chartId} = new ApexCharts(element, options);");
        sb.Append($"{chartId}.render();");

        return sb.ToString();
    }
}