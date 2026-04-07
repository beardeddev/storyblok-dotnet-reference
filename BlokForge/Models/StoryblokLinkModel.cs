using System.Text.Json.Serialization;

namespace BlokForge.Models;

public class StoryblokLinkModel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("url")]
    public string? Url { get; set; }

    [JsonPropertyName("target")]
    public string? Target { get; set; }

    [JsonPropertyName("linktype")]
    public string? LinkType { get; set; }

    [JsonPropertyName("fieldtype")]
    public string? FieldType { get; set; }

    [JsonPropertyName("cached_url")]
    public string? CachedUrl { get; set; }
}
