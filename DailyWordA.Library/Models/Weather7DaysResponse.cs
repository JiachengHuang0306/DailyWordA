namespace DailyWordA.Library.Models;

public class Weather7DaysResponse
{
    public string Code { get; set; }
    public string UpdateTime { get; set; }
    public List<WeatherDaily> Daily { get; set; } // 包含的七天天气数据
}