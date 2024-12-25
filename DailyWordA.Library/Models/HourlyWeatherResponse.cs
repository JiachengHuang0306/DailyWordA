using System.Text.Json.Serialization;
using Avalonia.Media.Imaging;

namespace DailyWordA.Library.Models;

public class HourlyWeatherResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("updateTime")]
    public string UpdateTime { get; set; }

    [JsonPropertyName("fxLink")]
    public string FxLink { get; set; }

    [JsonPropertyName("hourly")]
    public List<HourlyWeather> Hourly { get; set; }
    
}

public class HourlyWeather
{
    [JsonPropertyName("fxTime")]
    public string FxTime { get; set; } // 时间
    
    public DateTime ParsedFxTime { get; set; } // 解析后的 DateTime 时间

    [JsonPropertyName("temp")]
    public string Temp { get; set; } // 温度

    [JsonPropertyName("icon")]
    public string Icon { get; set; } // 天气图标代码
    
    [JsonPropertyName("iconBitmap")]
    public Bitmap IconBitmap { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } // 天气描述

    [JsonPropertyName("wind360")]
    public string Wind360 { get; set; } // 风向角度

    [JsonPropertyName("windDir")]
    public string WindDir { get; set; } // 风向

    [JsonPropertyName("windScale")]
    public string WindScale { get; set; } // 风力等级

    [JsonPropertyName("windSpeed")]
    public string WindSpeed { get; set; } // 风速

    [JsonPropertyName("humidity")]
    public string Humidity { get; set; } // 湿度

    [JsonPropertyName("pop")]
    public string Pop { get; set; } // 降水概率

    [JsonPropertyName("precip")]
    public string Precip { get; set; } // 降水量

    [JsonPropertyName("pressure")]
    public string Pressure { get; set; } // 气压

    [JsonPropertyName("cloud")]
    public string Cloud { get; set; } // 云量

    [JsonPropertyName("dew")]
    public string Dew { get; set; } // 露点
}
