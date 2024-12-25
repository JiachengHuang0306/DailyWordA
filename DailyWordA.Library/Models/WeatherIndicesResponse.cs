using System.Text.Json.Serialization;

namespace DailyWordA.Library.Models;

public class WeatherIndicesResponse
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("daily")]
    public List<WeatherIndex> Daily { get; set; }
}

public class WeatherIndex
{ 
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("level")]
    public string Level { get; set; }

    [JsonPropertyName("category")]
    public string Category { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; }
}
