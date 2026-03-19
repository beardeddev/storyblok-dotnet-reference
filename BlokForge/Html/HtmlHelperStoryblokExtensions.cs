using BlokForge.ProtoModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BlokForge.Html;

public static class HtmlHelperStoryblokExtensions
{
    public static IHtmlContent StoryblokRichText(this IHtmlHelper htmlHelper, RichTextDocument? document)
    {
        var renderer = htmlHelper.ViewContext.HttpContext.RequestServices
            .GetRequiredService<IStoryblokRichTextRenderer>();

        return renderer.Render(document);
    }
}