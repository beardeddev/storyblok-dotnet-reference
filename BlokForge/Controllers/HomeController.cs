using System.Diagnostics;
using Adliance.Storyblok;
using Microsoft.AspNetCore.Mvc;
using BlokForge.Models;
using Adliance.Storyblok.Clients;
using Page = BlokForge.Models.Page;

namespace BlokForge.Controllers;

public class HomeController : Controller
{
    private readonly StoryblokStoryClient _storyblokClient;

    public HomeController(StoryblokStoryClient storyblokClient)
    {
        _storyblokClient = storyblokClient;
    }


    [Route("{*slug}")]
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