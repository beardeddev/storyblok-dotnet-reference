using BlokForge.ProtoModels;
using Microsoft.AspNetCore.Html;

namespace BlokForge.Html;

public interface IStoryblokRichTextRenderer
{
    IHtmlContent Render(RichTextDocument? document);
}