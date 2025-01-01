using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using static EstateManagementUI.Common.ChartBuilder.ChartObjects;
using JsonConverter = Newtonsoft.Json.JsonConverter;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace EstateManagementUI.Common;


[ExcludeFromCodeCoverage]
public class ChartBuilder {

    public class ChartObjects
    {
        public class Series<T>
        {
            [JsonProperty("name")] 
            public string Name { get; set; }
            [JsonProperty("data")] 
            public List<T> Data { get; set; } = new();
        }

        public class Zoom
        {
            [JsonProperty("enabled")] 
            public bool Enabled { get; set; }
        }

        public class Chart
        {
            [JsonProperty("height")] 
            public int Height { get; set; }
            [JsonProperty("type")] 
            public string Type { get; set; }
            [JsonProperty("zoom")] 
            public Zoom Zoom { get; set; }
        }

        public class DataLabels
        {
            [JsonProperty("enabled")] 
            public bool Enabled { get; set; }
        }

        public class Stroke
        {
            [JsonProperty("curve")] 
            public string Curve { get; set; }
        }

        public class BarStroke : Stroke
        {
            [JsonProperty("colors")]
            public List<String> Colors { get; set; } = new List<String>();
        }

        public class Title
        {
            [JsonProperty("text")] 
            public string Text { get; set; }
            [JsonProperty("align")] 
            public string Align { get; set; }
        }

        public class Row
        {
            [JsonProperty("colors")] 
            public List<string> Colors { get; set; }
            [JsonProperty("opacity")] 
            public double Opacity { get; set; }
        }

        public class Grid
        {
            [JsonProperty("row")] 
            public Row Row { get; set; }
        }

        public class XAxis
        {
            [JsonProperty("categories")] 
            public List<string> Categories { get; set; }
            [JsonProperty("labels")]
            public Label Labels { get; set; }
        }

        public class YAxis
        {
            [JsonProperty("labels")]
            public Label Labels { get; set; }
        }

        public class Label {
            [JsonProperty("formatter")]
            public JavaScriptFunction Formatter { get; set; }
        }

        public class JavaScriptFunction
        {
            public string Body { get; set; }

            public JavaScriptFunction(string body)
            {
                this.Body = body;
            }
        }

        public class PlotOptions{
            [JsonProperty("bar")]
            public Bar Bar { get; set; }
        }

        public class Bar {
            [JsonProperty("horizontal")]
            public Boolean Horizontal { get; set; }
            [JsonProperty("dataLabels")]
            public DataLabels DataLabels { get; set; }
        }

        public class BaseChartOptions<T>
        {
            [JsonProperty("series")] 
            public List<Series<T>> Series { get; set; }
            [JsonProperty("chart")] 
            public Chart Chart { get; set; }
            [JsonProperty("dataLabels")] 
            public DataLabels DataLabels { get; set; }
            [JsonProperty("stroke")] 
            public Stroke Stroke { get; set; }
            [JsonProperty("title")] 
            public Title Title { get; set; }
            [JsonProperty("xaxis")] 
            public XAxis XAxis { get; set; }
            [JsonProperty("yaxis")] 
            public YAxis YAxis { get; set; }
        }

        public class LineChartOptions<T> : BaseChartOptions<T>
        {
            [JsonProperty("grid")] 
            public Grid Grid { get; set; }
        }

        public class BarChartOptions<T> : BaseChartOptions<T>
        {
            [JsonProperty("stroke")]
            public BarStroke Stroke { get; set; }

            [JsonProperty("plotOptions")]
            public PlotOptions PlotOptions { get; set; }
        }
    }

    //public static String BuildChartOptions<T>(List<String> categories,
    //                                          List<ChartObjects.Series<T>> seriesList,
    //                                          String chartType,
    //                                          String title,
    //                                          JavaScriptFunction xAxisFormatFunction = null,
    //                                          JavaScriptFunction yAxisFormatFunction = null) {
    //    ChartObjects.ChartOptions<T> chartOptions = new ChartObjects.ChartOptions<T> {
    //        Series = seriesList,
    //        Chart = new ChartObjects.Chart { Height = 350, Type = chartType, Zoom = new ChartObjects.Zoom { Enabled = true } },
    //        DataLabels = new ChartObjects.DataLabels { Enabled = false },
    //        Title = new ChartObjects.Title { Text = title, Align = "left" },
    //        //Grid = new ChartObjects.Grid { Row = new ChartObjects.Row { Colors = new List<string> { "#f3f3f3", "transparent" }, Opacity = 0.5 } },
    //        XAxis = new ChartObjects.XAxis { Categories = categories, Labels = new Label()},
    //        YAxis = new ChartObjects.YAxis { Labels = new Label()}
    //    };

    //    if (chartType == "line") {
    //        chartOptions.Stroke = new ChartObjects.Stroke { Curve = "straight" };
    //        chartOptions.PlotOptions = new PlotOptions();
    //    }

    //    if (chartType == "bar") {
    //        chartOptions.Stroke = new Stroke { Colors = new List<String> { "transparent" } };
    //        chartOptions.PlotOptions = new PlotOptions { Bar = new Bar { Horizontal = false } };
    //    }

    //    if (xAxisFormatFunction != null) {
    //        chartOptions.XAxis.Labels.Formatter = xAxisFormatFunction;
    //    }
    //    if (yAxisFormatFunction != null)
    //    {
    //        chartOptions.YAxis.Labels.Formatter = yAxisFormatFunction;
    //    }

    //    JsonSerializerSettings settings = new JsonSerializerSettings
    //    {
    //        Converters = new List<JsonConverter> { new JavaScriptFunctionConverter() },
    //    };

    //    return JsonConvert.SerializeObject(chartOptions, settings);
    //}

    public static String BuildLineChartOptions<T>(List<String> categories,
                                              List<ChartObjects.Series<T>> seriesList,
                                              String chartType,
                                              String title,
                                              JavaScriptFunction xAxisFormatFunction = null,
                                              JavaScriptFunction yAxisFormatFunction = null)
    {
        ChartObjects.LineChartOptions<T> chartOptions = new ChartObjects.LineChartOptions<T>()
        {
            Series = seriesList,
            Chart = new ChartObjects.Chart { Height = 350, Type = chartType, Zoom = new ChartObjects.Zoom { Enabled = true } },
            DataLabels = new ChartObjects.DataLabels { Enabled = false },
            Title = new ChartObjects.Title { Text = title, Align = "left" },
            Grid = new ChartObjects.Grid { Row = new ChartObjects.Row { Colors = new List<string> { "#f3f3f3", "transparent" }, Opacity = 0.5 } },
            XAxis = new ChartObjects.XAxis { Categories = categories, Labels = new Label() },
            YAxis = new ChartObjects.YAxis { Labels = new Label() }
        };

        chartOptions.Stroke = new ChartObjects.Stroke { Curve = "straight" };
        
        if (xAxisFormatFunction != null)
        {
            chartOptions.XAxis.Labels.Formatter = xAxisFormatFunction;
        }
        if (yAxisFormatFunction != null)
        {
            chartOptions.YAxis.Labels.Formatter = yAxisFormatFunction;
        }

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new JavaScriptFunctionConverter() },
        };

        var x = JsonConvert.SerializeObject(chartOptions, settings);
        return JsonConvert.SerializeObject(chartOptions, settings);
    }

    public static String BuildBarChartOptions<T>(List<String> categories,
                                                  List<ChartObjects.Series<T>> seriesList,
                                                  String chartType,
                                                  String title,
                                                  JavaScriptFunction xAxisFormatFunction = null,
                                                  JavaScriptFunction yAxisFormatFunction = null)
    {
        ChartObjects.BarChartOptions<T> chartOptions = new ChartObjects.BarChartOptions<T>()
        {
            Series = seriesList,
            Chart = new ChartObjects.Chart { Height = 350, Type = chartType, Zoom = new ChartObjects.Zoom { Enabled = true } },
            DataLabels = new ChartObjects.DataLabels { Enabled = false },
            Title = new ChartObjects.Title { Text = title, Align = "left" },
            XAxis = new ChartObjects.XAxis { Categories = categories, Labels = new Label() },
            YAxis = new ChartObjects.YAxis { Labels = new Label() }
        };
        chartOptions.PlotOptions = new PlotOptions { Bar = new Bar { Horizontal = false, DataLabels = new DataLabels{ Enabled = false}} };
        chartOptions.Stroke = new ChartObjects.BarStroke() { Curve = "straight", Colors = new List<String> {"transparent"}};

        if (xAxisFormatFunction != null)
        {
            chartOptions.XAxis.Labels.Formatter = xAxisFormatFunction;
        }
        else {
            chartOptions.XAxis.Labels.Formatter = StandardJavascriptFunctions.EmptyFunction;
        }
        if (yAxisFormatFunction != null)
        {
            chartOptions.YAxis.Labels.Formatter = yAxisFormatFunction;
        }
        else
        {
            chartOptions.YAxis.Labels.Formatter = StandardJavascriptFunctions.EmptyFunction;
        }
        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            Converters = new List<JsonConverter> { new JavaScriptFunctionConverter() },
        };

        var x = JsonConvert.SerializeObject(chartOptions, settings);
        return JsonConvert.SerializeObject(chartOptions, settings);
    }

    public class JavaScriptFunctionConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var jsFunction = value as JavaScriptFunction;
            if (jsFunction != null && String.IsNullOrEmpty(jsFunction.Body) == false)
            {
                writer.WriteRawValue(jsFunction.Body);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // For deserialization (not implemented as it's generally not needed for JavaScript functions)
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            if (objectType == typeof(JavaScriptFunction)) {
                int i = 0;
            }

            return objectType == typeof(JavaScriptFunction);
        }
    }

    public class StandardJavascriptFunctions {
        public static JavaScriptFunction CurrencyFormatter =>
            new JavaScriptFunction("function (value) {\r\n      return \"KES \" + value;\r\n    }");
        public static JavaScriptFunction EmptyFunction =>
            new JavaScriptFunction("function (value) {\r\n      return value;\r\n    }");
    }
}