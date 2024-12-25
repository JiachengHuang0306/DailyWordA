namespace DailyWordA.Library.Models;

public class IpLocationResponse
{
    public string Status { get; set; } // "success" or "fail"
    public string Country { get; set; }
    public string Region { get; set; }
    public string City { get; set; }
    public double Lat { get; set; } // 纬度
    public double Lon { get; set; } // 经度
    public string Query { get; set; } // 请求的 IP 地址
}