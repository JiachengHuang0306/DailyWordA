namespace DailyWordA.Library.Services;

using DailyWordA.Library.Models;
using System.Threading.Tasks;

/// <summary>
/// 定义天气服务的接口，用于与和风天气 API 交互并获取天气相关数据。
/// </summary>
public interface IWeatherService
{
    /// <summary>
    /// 获取指定位置的当前天气信息。
    /// </summary>
    /// <param name="location">城市或区域的 location ID。</param>
    /// <param name="apiKey">和风天气 API 密钥。</param>
    /// <returns>包含当前天气信息的 <see cref="WeatherResponse"/> 对象。</returns>
    Task<WeatherResponse> GetCurrentWeatherAsync(string location, string apiKey);

    /// <summary>
    /// 获取指定位置的未来七天天气信息。
    /// </summary>
    /// <param name="location">城市或区域的 location ID。</param>
    /// <param name="apiKey">和风天气 API 密钥。</param>
    /// <returns>包含未来七天天气信息的 <see cref="Weather7DaysResponse"/> 对象。</returns>
    Task<Weather7DaysResponse> Get7DaysWeatherAsync(string location, string apiKey);

    /// <summary>
    /// 根据查询字符串搜索城市信息。
    /// </summary>
    /// <param name="query">城市名称或关键词。</param>
    /// <param name="apiKey">和风天气 API 密钥。</param>
    /// <returns>包含匹配城市信息的 <see cref="CitySearchResult"/> 对象。</returns>
    Task<CitySearchResult> SearchCityAsync(string query, string apiKey);

    /// <summary>
    /// 根据当前设备的 IP 地址获取地理位置信息（经纬度）。
    /// </summary>
    /// <returns>包含地理位置信息的 <see cref="IpLocationResponse"/> 对象。</returns>
    Task<IpLocationResponse> GetCoordinatesFromIpAsync();

    /// <summary>
    /// 根据经纬度获取匹配的城市信息。
    /// </summary>
    /// <param name="latitude">纬度。</param>
    /// <param name="longitude">经度。</param>
    /// <returns>包含匹配城市信息的 <see cref="CitySearchResult"/> 对象。</returns>
    Task<CitySearchResult> GetCityInfoByCoordinatesAsync(double latitude, double longitude);

    /// <summary>
    /// 自动定位城市信息。通过 IP 地址获取经纬度并反查对应的城市信息。
    /// </summary>
    /// <returns>包含定位的城市信息的 <see cref="City"/> 对象。</returns>
    Task<City> AutoLocateCityAsync();

    /// <summary>
    /// 获取指定位置的逐小时天气信息。
    /// </summary>
    /// <param name="location">城市或区域的 location ID。</param>
    /// <param name="apiKey">和风天气 API 密钥。</param>
    /// <returns>包含逐小时天气信息的 <see cref="HourlyWeatherResponse"/> 对象。</returns>
    Task<HourlyWeatherResponse> GetHourlyWeatherAsync(string location, string apiKey);

    /// <summary>
    /// 获取指定位置的天气指数信息（生活建议）。
    /// </summary>
    /// <param name="location">城市或区域的 location ID。</param>
    /// <param name="apiKey">和风天气 API 密钥。</param>
    /// <returns>包含天气指数信息的 <see cref="WeatherIndicesResponse"/> 对象。</returns>
    Task<WeatherIndicesResponse> GetWeatherIndicesAsync(string location, string apiKey);
}