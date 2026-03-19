using System.Text.Json.Serialization;
using Adliance.Storyblok;
using Adliance.Storyblok.Attributes;

namespace BlokForge.Models;

[StoryblokComponent("page", "Page")]
public class Page : StoryblokComponent 
{
    [JsonPropertyName("body")] 
    public StoryblokComponent[] Body { get; set; }
}