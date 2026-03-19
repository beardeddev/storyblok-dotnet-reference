using System.Net;
using System.Text;
using BlokForge.ProtoModels;
using Microsoft.AspNetCore.Html;

namespace BlokForge.Html;

public class StoryblokRichTextRenderer : IStoryblokRichTextRenderer
{
    public IHtmlContent Render(RichTextDocument? document)
    {
        if (document?.Content == null || document.Content.Count == 0)
            return HtmlString.Empty;

        var sb = new StringBuilder();

        foreach (var node in document.Content)
        {
            RenderNode(node, sb);
        }

        return new HtmlString(sb.ToString());
    }

    private void RenderNode(RichTextNode? node, StringBuilder sb)
    {
        if (node == null || string.IsNullOrWhiteSpace(node.Type))
            return;

        switch (node.Type)
        {
            case "paragraph":
                sb.Append("<p>");
                RenderChildren(node, sb);
                sb.Append("</p>");
                break;

            case "heading":
                var level = GetHeadingLevel(node);
                sb.Append($"<h{level}>");
                RenderChildren(node, sb);
                sb.Append($"</h{level}>");
                break;

            case "bullet_list":
                sb.Append("<ul>");
                RenderChildren(node, sb);
                sb.Append("</ul>");
                break;

            case "ordered_list":
                sb.Append("<ol>");
                RenderChildren(node, sb);
                sb.Append("</ol>");
                break;

            case "list_item":
                sb.Append("<li>");
                RenderChildren(node, sb);
                sb.Append("</li>");
                break;

            case "blockquote":
                sb.Append("<blockquote>");
                RenderChildren(node, sb);
                sb.Append("</blockquote>");
                break;

            case "horizontal_rule":
                sb.Append("<hr />");
                break;

            case "hard_break":
                sb.Append("<br />");
                break;

            case "text":
                RenderTextNode(node, sb);
                break;

            default:
                RenderChildren(node, sb);
                break;
        }
    }

    private void RenderChildren(RichTextNode node, StringBuilder sb)
    {
        if (node.Content == null)
            return;

        foreach (var child in node.Content)
        {
            RenderNode(child, sb);
        }
    }

    private void RenderTextNode(RichTextNode node, StringBuilder sb)
    {
        var text = WebUtility.HtmlEncode(node.Text ?? string.Empty);

        if (node.Marks == null || node.Marks.Count == 0)
        {
            sb.Append(text);
            return;
        }

        var openingTags = new StringBuilder();
        var closingTags = new StringBuilder();

        foreach (var mark in node.Marks)
        {
            switch (mark.Type)
            {
                case "bold":
                    openingTags.Append("<strong>");
                    closingTags.Insert(0, "</strong>");
                    break;
                case "italic":
                    openingTags.Append("<em>");
                    closingTags.Insert(0, "</em>");
                    break;
                case "underline":
                    openingTags.Append("<u>");
                    closingTags.Insert(0, "</u>");
                    break;
                case "strike":
                    openingTags.Append("<s>");
                    closingTags.Insert(0, "</s>");
                    break;
                case "code":
                    openingTags.Append("<code>");
                    closingTags.Insert(0, "</code>");
                    break;
                case "link":
                    var href = GetAttr(mark, "href");
                    var target = GetAttr(mark, "target");

                    openingTags.Append("<a");
                    if (!string.IsNullOrWhiteSpace(href))
                        openingTags.Append($" href=\"{WebUtility.HtmlEncode(href)}\"");
                    if (!string.IsNullOrWhiteSpace(target))
                        openingTags.Append($" target=\"{WebUtility.HtmlEncode(target)}\"");
                    if (target == "_blank")
                        openingTags.Append(" rel=\"noopener noreferrer\"");
                    openingTags.Append(">");

                    closingTags.Insert(0, "</a>");
                    break;
            }
        }

        sb.Append(openingTags);
        sb.Append(text);
        sb.Append(closingTags);
    }

    private static int GetHeadingLevel(RichTextNode node)
    {
        if (node.Attrs != null &&
            node.Attrs.TryGetValue("level", out var raw) &&
            int.TryParse(raw?.ToString(), out var level))
        {
            return Math.Clamp(level, 1, 6);
        }

        return 2;
    }

    private static string? GetAttr(RichTextMark mark, string key)
    {
        if (mark.Attrs == null || !mark.Attrs.TryGetValue(key, out var value))
            return null;

        return value?.ToString();
    }
}