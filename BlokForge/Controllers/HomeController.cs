using System.Diagnostics;
using System.Net;
using Adliance.Storyblok;
using Microsoft.AspNetCore.Mvc;
using BlokForge.Models;
using Adliance.Storyblok.Clients;
using Page = BlokForge.Models.Page;

namespace BlokForge.Controllers;

public class HomeController : Controller
{
    private readonly StoryblokStoryClient _storyblokClient;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public HomeController(
        StoryblokStoryClient storyblokClient,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _storyblokClient = storyblokClient;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet("api/story-raw/{**slug}")]
    public async Task<IActionResult> RawStory(string slug = "home")
    {
        slug = string.IsNullOrWhiteSpace(slug) ? "home" : slug;

        var isDraft = Request.Query.ContainsKey("_storyblok");
        var version = isDraft ? "draft" : "published";
        var token = isDraft
            ? _configuration["Storyblok:ApiKeyPreview"]
            : _configuration["Storyblok:ApiKeyPublic"];

        if (string.IsNullOrWhiteSpace(token))
        {
            return Problem("Storyblok API token is not configured.");
        }

        var query = new List<string>
        {
            $"token={WebUtility.UrlEncode(token)}",
            $"version={WebUtility.UrlEncode(version)}",
            "resolve_links=url"
        };

        if (isDraft && Request.Query.TryGetValue("_storyblok", out var cacheVersion) &&
            !string.IsNullOrWhiteSpace(cacheVersion))
        {
            query.Add($"cv={WebUtility.UrlEncode(cacheVersion!)}");
        }

        var requestUri = $"https://api.storyblok.com/v2/cdn/stories/{Uri.EscapeDataString(slug)}?{string.Join("&", query)}";
        var client = _httpClientFactory.CreateClient();
        using var response = await client.GetAsync(requestUri, HttpContext.RequestAborted);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound();
        }

        var rawJson = await response.Content.ReadAsStringAsync(HttpContext.RequestAborted);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, rawJson);
        }

        return Content(rawJson, "application/json");
    }

    [HttpGet("{**slug}")]
    public async Task<IActionResult> Story(string slug = "home")
    {
        var story = await _storyblokClient.Story().WithSlug(slug)
            .ResolveLinks(ResolveLinksType.Url)
            .Load<Page>();
        
        if (story == null)
        {
            return NotFound();
        }
        
        return View(story);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
