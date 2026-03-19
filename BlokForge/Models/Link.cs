using System.Text.Json.Serialization;
using Adliance.Storyblok;
using Adliance.Storyblok.Attributes;

namespace BlokForge.Models;

[StoryblokComponent("link")]
public class Link : StoryblokComponent, IStoryblokBlock
{
    [JsonPropertyName("text")] 
    public string Text { get; set; }
    
    [JsonPropertyName("ref")] 
    public StoryblokLink Ref { get; set; }
}