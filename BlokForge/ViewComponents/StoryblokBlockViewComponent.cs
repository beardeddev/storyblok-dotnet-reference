using BlokForge.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlokForge.ViewComponents;

public class StoryblokBlockViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IStoryblokBlock blok)
    {
        var component = blok.Component as string;

        return View($"Blocks/{component}", blok);
    }
}