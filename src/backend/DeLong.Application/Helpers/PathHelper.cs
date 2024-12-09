using Newtonsoft.Json;

namespace DeLong.Application.Helpers;

public class PathHelper
{
    public static string WebRootPath { get; set; } = string.Empty;
    [JsonProperty("CountryFilePaths")]
    public static string CountryPath { get; set; } = string.Empty;
    [JsonProperty("RegionFilePaths")]
    public static string RegionPath { get; set; } = string.Empty;
    [JsonProperty("DictrictsFilePaths")]
    public static string DistrictPath { get; set; } = string.Empty;
}
