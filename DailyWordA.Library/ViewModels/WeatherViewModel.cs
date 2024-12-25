namespace DailyWordA.Library.ViewModels;

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DailyWordA.Library.Models;
using DailyWordA.Library.Services;


    public partial class WeatherViewModel : ViewModelBase {
        private readonly IWeatherService _weatherService;

        public ObservableCollection<WeatherDaily> DailyWeather { get; } = new();
        
        // 逐小时天气数据集合
        public ObservableCollection<HourlyWeather> HourlyWeather { get; } = new();
        
        public ObservableCollection<string> WeatherIndices { get; } = new();

        
        // 构造函数
        public WeatherViewModel(IWeatherService weatherService)
        {
            _weatherService = weatherService;
            CityName = "北京"; // 默认城市名称
            Temperature = "N/A";
            WeatherDescription = "加载中...";
            SearchCityName = string.Empty;

            _ = LoadWeatherDataAsync();
            _ = Load7DaysWeatherAsync();
            _ = LoadHourlyWeatherAsync();
            _ = LoadWeatherIndicesAsync("101010100");
        }
        

        // 城市名称
        [ObservableProperty]
        private string cityName;
        
        // 温度
        [ObservableProperty]
        private string temperature;

        // 天气描述
        [ObservableProperty]
        private string weatherDescription;

        // 风向和风速
        [ObservableProperty]
        private string windInfo;

        // 湿度
        [ObservableProperty]
        private string humidity;

        // 降水量
        [ObservableProperty]
        private string precipitation;

        // 气压
        [ObservableProperty]
        private string pressure;

        // 能见度
        [ObservableProperty]
        private string visibility;

        // 云量
        [ObservableProperty]
        private string cloudCover;

        // 露点温度
        [ObservableProperty]
        private string dewPoint;
        
        // 当前天气图标代码（对应和风天气的 icon 字段）
        [ObservableProperty]
        private Bitmap currentWeatherIcon;
        
        // 用户输入的城市名称
        [ObservableProperty]
        private string searchCityName;
        
        // 当前定位状态
        [ObservableProperty]
        private string locationStatus = "未定位";
        
        // 是否有生活建议
        // [ObservableProperty]
        // private bool isWeatherIndicesEmpty = true;
        
        private void UpdateWeatherIcon(string iconCode)
        {
            try
            {
                var uri = new Uri($"avares://DailyWordA/Assets/QWeatherIcons/icons/{iconCode}.png");
                CurrentWeatherIcon = new Bitmap(AssetLoader.Open(uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Failed to load current weather icon: {ex.Message}");
                CurrentWeatherIcon = null;
            }
        }
        
        
        private Bitmap GetBitmapFromIconCode(string iconCode)
        {
            try
            {
                var uri = new Uri($"avares://DailyWordA/Assets/QWeatherIcons/icons/{iconCode}.png");
                return new Bitmap(AssetLoader.Open(uri));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DEBUG] Failed to load bitmap for icon {iconCode}: {ex.Message}");
                return null; // 返回默认或空图标
            }
        }

        
        // 加载天气数据的命令
        [RelayCommand]
        public async Task LoadWeatherDataAsync() {
            string location = "101010100"; // 默认 Location ID 北京
            string apiKey = "2ae6633866264232a41d4f9f216dc61b";

            var weatherResponse = await _weatherService.GetCurrentWeatherAsync(location, apiKey);

            if (weatherResponse?.Now != null) {
                Console.WriteLine($"Icon Path: {CurrentWeatherIcon}");
                Temperature = $"{weatherResponse.Now.Temp}°C";
                WeatherDescription = weatherResponse.Now.Text;
                WindInfo = $"{weatherResponse.Now.WindDir} | {weatherResponse.Now.WindSpeed} km/h";
                Humidity = $"{weatherResponse.Now.Humidity}%";
                Precipitation = $"{weatherResponse.Now.Precip} mm";
                Pressure = $"{weatherResponse.Now.Pressure} hPa";
                Visibility = $"{weatherResponse.Now.Vis} km";
                CloudCover = $"{weatherResponse.Now.Cloud}%";
                DewPoint = $"{weatherResponse.Now.Dew}°C";
                UpdateWeatherIcon(weatherResponse.Now.Icon);
            }else {
                Temperature = "N/A";
                WeatherDescription = "无法获取天气数据";
                WindInfo = "N/A";
                Humidity = "N/A";
                Precipitation = "N/A";
                Pressure = "N/A";
                Visibility = "N/A";
                CloudCover = "N/A";
                DewPoint = "N/A";
                UpdateWeatherIcon("999");
            }
        }
        
        // 多日天气查询
        public async Task Load7DaysWeatherAsync() {
            try {
                string location = "101010100"; // 默认 Location ID
                string apiKey = "2ae6633866264232a41d4f9f216dc61b";

                var weatherResponse = await _weatherService.Get7DaysWeatherAsync(location, apiKey);

                if (weatherResponse?.Daily != null) {
                    // 清空旧数据
                    DailyWeather.Clear();

                    // 添加新数据
                    foreach (var daily in weatherResponse.Daily) {
                        daily.IconBitmap = GetBitmapFromIconCode(daily.IconDay);
                        DailyWeather.Add(daily);
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine($"Error loading 7-day weather data: {ex.Message}");
            }
        }
        
        // 加载逐小时天气数据的命令
        [RelayCommand]
        public async Task LoadHourlyWeatherAsync() {
            try {
                string location = "101010100"; // 默认 Location ID
                string apiKey = "2ae6633866264232a41d4f9f216dc61b";

                var hourlyWeatherResponse = await _weatherService.GetHourlyWeatherAsync(location, apiKey);

                if (hourlyWeatherResponse?.Hourly != null) {
                    // 清空旧数据
                    HourlyWeather.Clear();

                    // 添加新数据
                    foreach (var hourly in hourlyWeatherResponse.Hourly) {
                        hourly.IconBitmap = GetBitmapFromIconCode(hourly.Icon);
                        // 确保解析 FxTime 为 DateTime
                        if (DateTime.TryParse(hourly.FxTime, out var parsedTime)) {
                            hourly.ParsedFxTime = parsedTime; // 将解析的时间存入一个新的 DateTime 属性
                        }
                        HourlyWeather.Add(hourly);
                    }
                }
            }catch (Exception ex) {
                Console.WriteLine($"Error loading hourly weather data: {ex.Message}");
            }
        }
        
        // 加载天气指数
        public async Task LoadWeatherIndicesAsync(string locationId) {
            try {
                string apiKey = "2ae6633866264232a41d4f9f216dc61b";

                var weatherIndicesResponse = await _weatherService.GetWeatherIndicesAsync(locationId, apiKey);

                if (weatherIndicesResponse?.Daily != null&& weatherIndicesResponse.Daily.Count > 0) {
                    WeatherIndices.Clear();

                    foreach (var index in weatherIndicesResponse.Daily) {
                        WeatherIndices.Add(index.Text);
                    }
                    // IsWeatherIndicesEmpty = false;
                }else {
                    // IsWeatherIndicesEmpty = true; // 无生活建议
                    WeatherIndices.Clear();
                    WeatherIndices.Add("未获取到生活建议数据。");
                }
            }catch (Exception ex) {
                WeatherIndices.Clear();
                // IsWeatherIndicesEmpty = true; // 无生活建议
                WeatherIndices.Add($"加载生活建议时出错：{ex.Message}");
            }
        }
        
        [RelayCommand]
        public async Task SearchAndLoadWeatherAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchCityName))
            {
                Console.WriteLine("请输入有效的城市名称！");
                return;
            } try {
                string apiKey = "2ae6633866264232a41d4f9f216dc61b"; 

                // 调用 GeoAPI 搜索城市
                var searchResult = await _weatherService.SearchCityAsync(SearchCityName, apiKey);

                if (searchResult?.Locations?.Count > 0) {
                    var firstResult = searchResult.Locations[0];

                    // 更新城市名称
                    CityName = firstResult.Name;

                    // 使用 Location ID 获取天气数据
                    await LoadWeatherDataByLocationAsync(firstResult.Id);
                    await Load7DaysWeatherByLocationAsync(firstResult.Id);
                    await LoadHourlyWeatherByLocationAsync(firstResult.Id);
                    await LoadWeatherIndicesAsync(firstResult.Id);
                }else {
                    Console.WriteLine("未找到匹配的城市！");
                }
            }catch (Exception ex) {
                Console.WriteLine($"搜索城市时发生错误：{ex.Message}");
            }
        }
        
        // 自动定位命令
        [RelayCommand]
        public async Task AutoLocateAndLoadWeatherAsync() {
            try {
                LocationStatus = "定位中...";
                var ipLocation = await _weatherService.GetCoordinatesFromIpAsync();

                if (ipLocation != null && ipLocation.Status == "success") {
                    double latitude = ipLocation.Lat;
                    double longitude = ipLocation.Lon;

                    var citySearchResult = await _weatherService.GetCityInfoByCoordinatesAsync(latitude, longitude);

                    if (citySearchResult?.Locations?.Count > 0) {
                        var firstCity = citySearchResult.Locations[0];
                        CityName = firstCity.Name;

                        // 更新天气数据
                        await LoadWeatherDataByLocationAsync(firstCity.Id);
                        await Load7DaysWeatherByLocationAsync(firstCity.Id);
                        await LoadHourlyWeatherByLocationAsync(firstCity.Id);
                        await LoadWeatherIndicesAsync(firstCity.Id);

                        LocationStatus = "定位成功";
                    }else {
                        LocationStatus = "定位失败，未找到匹配的城市";
                    }
                }else {
                    LocationStatus = "定位失败，无法获取经纬度";
                }
            }catch (Exception ex) {
                LocationStatus = $"定位失败：{ex.Message}";
            }
        }
        
        //通过搜索按钮更新天气信息
        private async Task LoadWeatherDataByLocationAsync(string locationId)
        {
            try
            {
                string apiKey = "2ae6633866264232a41d4f9f216dc61b"; 

                var weatherResponse = await _weatherService.GetCurrentWeatherAsync(locationId, apiKey);

                if (weatherResponse?.Now != null)
                {
                    Temperature = $"{weatherResponse.Now.Temp}°C";
                    WeatherDescription = weatherResponse.Now.Text;
                    WindInfo = $"{weatherResponse.Now.WindDir} | {weatherResponse.Now.WindSpeed} km/h";
                    Humidity = $"{weatherResponse.Now.Humidity}%";
                    Precipitation = $"{weatherResponse.Now.Precip} mm";
                    Pressure = $"{weatherResponse.Now.Pressure} hPa";
                    Visibility = $"{weatherResponse.Now.Vis} km";
                    CloudCover = $"{weatherResponse.Now.Cloud}%";
                    DewPoint = $"{weatherResponse.Now.Dew}°C";
                    
                    UpdateWeatherIcon(weatherResponse.Now.Icon);
                }
                else
                {
                    Temperature = "N/A";
                    WeatherDescription = "无法获取天气数据";
                    WindInfo = "N/A";
                    Humidity = "N/A";
                    Precipitation = "N/A";
                    Pressure = "N/A";
                    Visibility = "N/A";
                    CloudCover = "N/A";
                    DewPoint = "N/A";
                    
                    UpdateWeatherIcon("999");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取天气数据时发生错误：{ex.Message}");
            }
        }
        private async Task Load7DaysWeatherByLocationAsync(string locationId)
        {
            try
            {
                string apiKey = "2ae6633866264232a41d4f9f216dc61b"; // 替换为你的 API 密钥

                var weatherResponse = await _weatherService.Get7DaysWeatherAsync(locationId, apiKey);

                if (weatherResponse?.Daily != null)
                {
                    // 清空旧数据
                    DailyWeather.Clear();

                    // 添加新数据
                    foreach (var daily in weatherResponse.Daily)
                    {
                        daily.IconBitmap = GetBitmapFromIconCode(daily.IconDay);
                        DailyWeather.Add(daily);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取七日天气数据时发生错误：{ex.Message}");
            }
        }
        
        private async Task LoadHourlyWeatherByLocationAsync(string locationId)
        {
            try
            {
                string apiKey = "2ae6633866264232a41d4f9f216dc61b";

                var hourlyWeatherResponse = await _weatherService.GetHourlyWeatherAsync(locationId, apiKey);
                
                if (hourlyWeatherResponse?.Hourly != null)
                {
                    // 清空旧数据
                    HourlyWeather.Clear();

                    // 添加新数据
                    foreach (var hourly in hourlyWeatherResponse.Hourly)
                    {
                        hourly.IconBitmap = GetBitmapFromIconCode(hourly.Icon);
                        // 确保解析 FxTime
                        if (DateTime.TryParse(hourly.FxTime, out var parsedTime))
                        {
                            hourly.ParsedFxTime = parsedTime;
                        }
                        else
                        {
                            Console.WriteLine($"解析 FxTime 失败：{hourly.FxTime}");
                        }

                        HourlyWeather.Add(hourly);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading hourly weather data: {ex.Message}");
            }
        }
    }

