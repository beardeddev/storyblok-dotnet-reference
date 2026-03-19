using System.Text.Json.Serialization;
using Adliance.Storyblok;
using Adliance.Storyblok.Attributes;

namespace BlokForge.Models;

[StoryblokComponent("teaser")]
public class Teaser : StoryblokComponent, IStoryblokBlock
{
    [JsonPropertyName("headline")] 
    public string Headline { get; set; }
}