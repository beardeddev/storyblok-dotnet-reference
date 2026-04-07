using BlokForge.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BlokForge.TagHelpers;

[HtmlTargetElement("a", Attributes = StoryblokLinkAttributeName)]
public class StoryblokLinkTagHelper : TagHelper
{
    private const string StoryblokLinkAttributeName = "model";

    [HtmlAttributeName(StoryblokLinkAttributeName)]
    public Link? Model { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (Model is null)
        {
            output.SuppressOutput();
            return;
        }

        output.Attributes.RemoveAll(StoryblokLinkAttributeName);
        output.Attributes.SetAttribute("href", ResolveHref(Model.Ref));

        if (!string.IsNullOrWhiteSpace(Model.Ref?.Target))
        {
            output.Attributes.SetAttribute("target", Model.Ref.Target);
        }

        if (output.IsContentModified)
        {
            return;
        }

        output.Content.SetContent(Model.Text ?? string.Empty);
    }

    private static string ResolveHref(StoryblokLinkModel? link)
    {
        if (!string.IsNullOrWhiteSpace(link?.Url))
        {
            return link.Url;
        }

        if (!string.IsNullOrWhiteSpace(link?.CachedUrl))
        {
            return "/" + link.CachedUrl.TrimStart('/');
        }

        return "#";
    }
}
