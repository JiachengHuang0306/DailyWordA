namespace DailyWordA.Library.Services;

using System.Text.Json.Serialization;
using DailyWordA.Library.Models;


using System;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

public class WeatherService : IWeatherService
{
    private static readonly HttpClient _httpClient = new HttpClient();
    
    public async Task<WeatherResponse> GetCurrentWeatherAsync(string location, string apiKey) {
        try {
            string url = $"https://api.qweather.com/v7/weather/now?location={location}&key={apiKey}";

            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            using (var response = await _httpClient.GetAsync(url)) {
                response.EnsureSuccessStatusCode();

                string json;

                if (response.Content.Headers.ContentEncoding.Contains("gzip")) {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new GZipStream(stream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(decompressedStream)) {
                        json = await reader.ReadToEndAsync();
                    }
                }else {
                    json = await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine("API 响应 JSON:");
                Console.WriteLine(json);

                // 使用宽松的选项进行反序列化
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    IgnoreNullValues = true
                };

                return JsonSerializer.Deserialize<WeatherResponse>(json, options);
            }
        }catch (Exception ex) {
            Console.WriteLine($"Error fetching weather data: {ex.Message}");
            return null;
        }
    }
    
    public async Task<Weather7DaysResponse> Get7DaysWeatherAsync(string location, string apiKey) {
        try {
            string url = $"https://api.qweather.com/v7/weather/7d?location={location}&key={apiKey}";

            // 设置请求头
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            // 发送请求
            using (var response = await _httpClient.GetAsync(url)) {
                response.EnsureSuccessStatusCode();

                string json;

                // 处理 gzip 压缩
                if (response.Content.Headers.ContentEncoding.Contains("gzip")) {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new GZipStream(stream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(decompressedStream)) {
                        json = await reader.ReadToEndAsync();
                    }
                }else {
                    json = await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine("未来七天天气 API 响应 JSON:");
                Console.WriteLine(json);

                // 使用宽松的选项进行反序列化
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    IgnoreNullValues = true
                };

                return JsonSerializer.Deserialize<Weather7DaysResponse>(json, options);
            }
        }catch (Exception ex) {
            Console.WriteLine($"Error fetching 7-day weather data: {ex.Message}");
            return null;
        }
    }
    
    public async Task<CitySearchResult> SearchCityAsync(string query, string apiKey) {
        try {
            string url = $"https://geoapi.qweather.com/v2/city/lookup?location={query}&key={apiKey}";

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));

            using var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string json;
            if (response.Content.Headers.ContentEncoding.Contains("gzip")) {
                using var stream = await response.Content.ReadAsStreamAsync();
                using var decompressedStream = new GZipStream(stream, CompressionMode.Decompress);
                using var reader = new StreamReader(decompressedStream);
                json = await reader.ReadToEndAsync();
            }else {
                json = await response.Content.ReadAsStringAsync();
            }

            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive = true,
                IgnoreNullValues = true
            };

            return JsonSerializer.Deserialize<CitySearchResult>(json, options);
        }catch (Exception ex) {
            Console.WriteLine($"Error searching city: {ex.Message}");
            return null;
        }
    }
    
    // 获取经纬度
    public async Task<IpLocationResponse> GetCoordinatesFromIpAsync() {
        try {
            string url = "http://ip-api.com/json/";

            using (var response = await _httpClient.GetAsync(url)) {
                response.EnsureSuccessStatusCode();

                string content;
                if (response.Content.Headers.ContentEncoding.Contains("gzip")) {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new GZipStream(stream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(decompressedStream))
                    {
                        content = await reader.ReadToEndAsync();
                    }
                }else {
                    content = await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine("IP API 响应内容:");
                Console.WriteLine(content);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<IpLocationResponse>(content, options);
            }
        }catch (Exception ex) {
            Console.WriteLine($"获取经纬度失败: {ex.Message}");
            return null;
        }
    }

    // 获取城市信息
    public async Task<CitySearchResult> GetCityInfoByCoordinatesAsync(double latitude, double longitude) {
        try {
            string location = $"{longitude},{latitude}"; // 经纬度格式
            string apiKey = "2ae6633866264232a41d4f9f216dc61b";
            string url = $"https://geoapi.qweather.com/v2/city/lookup?location={location}&key={apiKey}";

            using (var response = await _httpClient.GetAsync(url)) {
                response.EnsureSuccessStatusCode();

                string content;
                if (response.Content.Headers.ContentEncoding.Contains("gzip")) {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new GZipStream(stream, CompressionMode.Decompress))
                    using (var reader = new StreamReader(decompressedStream)) {
                        content = await reader.ReadToEndAsync();
                    }
                }else {
                    content = await response.Content.ReadAsStringAsync();
                }

                Console.WriteLine("GeoAPI 响应内容:");
                Console.WriteLine(content);

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<CitySearchResult>(content, options);
            }
        }catch (Exception ex) {
            Console.WriteLine($"获取城市信息失败: {ex.Message}");
            return null;
        }
    }

    // 自动定位：从 IP 获取经纬度并查询城市信息
    public async Task<City> AutoLocateCityAsync()
    {
        try
        {
            var ipLocation = await GetCoordinatesFromIpAsync();

            if (ipLocation != null && ipLocation.Status == "success")
            {
                var citySearchResult = await GetCityInfoByCoordinatesAsync(ipLocation.Lat, ipLocation.Lon);

                if (citySearchResult?.Locations?.Count > 0)
                {
                    return citySearchResult.Locations[0]; // 返回第一个城市信息
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"自动定位失败: {ex.Message}");
        }

        return null; // 定位失败
    }
    
    public async Task<HourlyWeatherResponse> GetHourlyWeatherAsync(string location, string apiKey) {
            try {
                string url = $"https://api.qweather.com/v7/weather/24h?location={location}&key={apiKey}";

                _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));

                using (var response = await _httpClient.GetAsync(url)) {
                    response.EnsureSuccessStatusCode();

                    string json;

                    // 检查是否需要解压 gzip
                    if (response.Content.Headers.ContentEncoding.Contains("gzip")) {
                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var decompressedStream = new GZipStream(stream, CompressionMode.Decompress))
                        using (var reader = new StreamReader(decompressedStream)) {
                            json = await reader.ReadToEndAsync();
                        }
                    }else {
                        json = await response.Content.ReadAsStringAsync();
                    }

                    Console.WriteLine("API 响应 JSON:");
                    Console.WriteLine(json);

                    var options = new JsonSerializerOptions {
                        PropertyNameCaseInsensitive = true,
                        IgnoreNullValues = true
                    };

                    return JsonSerializer.Deserialize<HourlyWeatherResponse>(json, options);
                }
            }catch (Exception ex) {
                Console.WriteLine($"Error fetching hourly weather data: {ex.Message}");
                return null;
            }
        }
    
    public async Task<WeatherIndicesResponse> GetWeatherIndicesAsync(string location, string apiKey)
    {
        try {
            string url = $"https://api.qweather.com/v7/indices/1d?type=1,2&location={location}&key={apiKey}";

            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));

            using (var response = await _httpClient.GetAsync(url)) {
                response.EnsureSuccessStatusCode();

                string json;

                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var decompressedStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                    using (var reader = new System.IO.StreamReader(decompressedStream))
                    {
                        json = await reader.ReadToEndAsync();
                    }
                }else {
                    json = await response.Content.ReadAsStringAsync();
                }

                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive = true,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };

                return JsonSerializer.Deserialize<WeatherIndicesResponse>(json, options);
            }
        }catch (Exception ex) {
            Console.WriteLine($"Error fetching weather indices: {ex.Message}");
            return null;
        }
    }
}