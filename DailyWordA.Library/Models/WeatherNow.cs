namespace DailyWordA.Library.Models;

public class WeatherNow
{
    public string Temp { get; set; }           // 温度
    public string FeelsLike { get; set; }     // 体感温度
    public string Icon { get; set; }          // 天气图标编号
    public string Text { get; set; }          // 天气描述
    public string Wind360 { get; set; }       // 风向（角度）
    public string WindDir { get; set; }       // 风向
    public string WindScale { get; set; }     // 风力等级
    public string WindSpeed { get; set; }     // 风速
    public string Humidity { get; set; }      // 湿度
    public string Precip { get; set; }        // 降水量
    public string Pressure { get; set; }      // 气压
    public string Vis { get; set; }           // 能见度
    public string Cloud { get; set; }         // 云量
    public string Dew { get; set; }           // 露点温度
}