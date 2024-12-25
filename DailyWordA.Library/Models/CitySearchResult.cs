using System.Text.Json.Serialization;

namespace DailyWordA.Library.Models;

public class CitySearchResult
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("location")]
    public List<City> Locations { get; set; }

    [JsonPropertyName("refer")]
    public Refer Refer { get; set; }
}

public class City
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("adm2")]
    public string Adm2 { get; set; }

    [JsonPropertyName("adm1")]
    public string Adm1 { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("lat")]
    public string Latitude { get; set; }

    [JsonPropertyName("lon")]
    public string Longitude { get; set; }

    [JsonPropertyName("rank")]
    public string Rank { get; set; }
}

public class Refer
{
    [JsonPropertyName("sources")]
    public List<string> Sources { get; set; }

    [JsonPropertyName("license")]
    public List<string> License { get; set; }
}