namespace DailyWordA.Library.Models;

public class WeatherResponse
{
    public string Code { get; set; }          // API 响应代码
    public string UpdateTime { get; set; }    // 数据更新时间
    public WeatherNow Now { get; set; }       // 当前天气信息
}