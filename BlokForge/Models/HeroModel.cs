using System.Text.Json.Serialization;
using Adliance.Storyblok;
using Adliance.Storyblok.Attributes;
using BlokForge.ProtoModels;

namespace BlokForge.Models;

[StoryblokComponent("hero")]
public class HeroModel : StoryblokComponent, IStoryblokBlock
{
    [JsonPropertyName("tagline")] 
    public string? Tagline { get; set; }
    
    [JsonPropertyName("title")] 
    public string? Title { get; set; }
    
    [JsonPropertyName("description")] 
    public RichTextDocument? Description { get; set; }

    [JsonPropertyName("ctas")]
    public Link[]? Ctas { get; set; }
}
