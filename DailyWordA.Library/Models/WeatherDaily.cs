using Avalonia.Media.Imaging;

namespace DailyWordA.Library.Models;

public class WeatherDaily
{
    public string FxDate { get; set; } // 日期
    public string Sunrise { get; set; }
    public string Sunset { get; set; }
    public string Moonrise { get; set; }
    public string Moonset { get; set; }
    public string MoonPhase { get; set; }
    public string MoonPhaseIcon { get; set; }
    public string TempMax { get; set; } // 最高温度
    public string TempMin { get; set; } // 最低温度
    public string IconDay { get; set; }
    public Bitmap IconBitmap { get; set; }
    public string TextDay { get; set; } // 白天天气描述
    public string IconNight { get; set; }
    public string TextNight { get; set; } // 夜间天气描述
    public string Wind360Day { get; set; }
    public string WindDirDay { get; set; }
    public string WindScaleDay { get; set; }
    public string WindSpeedDay { get; set; }
    public string Wind360Night { get; set; }
    public string WindDirNight { get; set; }
    public string WindScaleNight { get; set; }
    public string WindSpeedNight { get; set; }
    public string Humidity { get; set; }
    public string Precip { get; set; }
    public string Pressure { get; set; }
    public string Vis { get; set; }
    public string Cloud { get; set; }
    public string UvIndex { get; set; }
}