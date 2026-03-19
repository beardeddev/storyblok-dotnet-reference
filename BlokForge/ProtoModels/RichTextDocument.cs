using System.Text.Json.Serialization;

namespace BlokForge.ProtoModels;

public class RichTextDocument
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("content")]
    public List<RichTextNode>? Content { get; set; }
}

public class RichTextNode
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }

    [JsonPropertyName("content")]
    public List<RichTextNode>? Content { get; set; }

    [JsonPropertyName("marks")]
    public List<RichTextMark>? Marks { get; set; }

    [JsonPropertyName("attrs")]
    public Dictionary<string, object>? Attrs { get; set; }
}

public class RichTextMark
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("attrs")]
    public Dictionary<string, object>? Attrs { get; set; }
}