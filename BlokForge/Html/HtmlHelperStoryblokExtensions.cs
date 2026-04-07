using System.Text.Json;
using Adliance.Storyblok;
using BlokForge.ProtoModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace BlokForge.Html;

public static class HtmlHelperStoryblokExtensions
{
    public static IHtmlContent StoryblokEditable(this IHtmlHelper htmlHelper, StoryblokComponent? component)
    {
        if (component is null || string.IsNullOrWhiteSpace(component.Editable))
        {
            return HtmlString.Empty;
        }

        var editable = component.Editable.Trim();
        var json = ExtractEditableJson(editable);

        if (json is null)
        {
            return new HtmlString(editable);
        }

        using var document = JsonDocument.Parse(json);
        if (!document.RootElement.TryGetProperty("uid", out var uidProperty))
        {
            return HtmlString.Empty;
        }

        var encodedJson = WebUtility.HtmlEncode(document.RootElement.GetRawText());
        var encodedUid = WebUtility.HtmlEncode(uidProperty.GetString());

        return new HtmlString($"data-blok-c=\"{encodedJson}\" data-blok-uid=\"{encodedUid}\"");
    }

    public static IHtmlContent StoryblokRichText(this IHtmlHelper htmlHelper, RichTextDocument? document)
    {
        var renderer = htmlHelper.ViewContext.HttpContext.RequestServices
            .GetRequiredService<IStoryblokRichTextRenderer>();

        return renderer.Render(document);
    }

    private static string? ExtractEditableJson(string editable)
    {
        const string prefix = "<!--#storyblok#";
        const string suffix = "-->";

        if (!editable.StartsWith(prefix, StringComparison.Ordinal) ||
            !editable.EndsWith(suffix, StringComparison.Ordinal))
        {
            return null;
        }

        return editable[prefix.Length..^suffix.Length].Trim();
    }
}
