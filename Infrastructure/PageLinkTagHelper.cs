using System;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure
{
    [HtmlTargetElement("ul", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        [ViewContext] [HtmlAttributeNotBound] public ViewContext ViewContext { get; set; }
        public PagingInfo PageModel { get; set; }
        public string PageAction { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);

            HtmlContentBuilder result = new HtmlContentBuilder();

            TagBuilder tagLiPrevious = NextPreviousBuilder(PageModel.CurrentPage > 1, "Previous", "&laquo;");

            result.AppendHtml(tagLiPrevious);

            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tagLi = new TagBuilder("li");
                tagLi.Attributes["class"] = "page-item";

                if (PageModel.CurrentPage == i)
                {
                    tagLi.Attributes["class"] = "page-item active";
                    TagBuilder tagSpan = new TagBuilder("span");
                    tagSpan.Attributes["class"] = "page-link";
                    tagSpan.InnerHtml.Append(i.ToString());

                    TagBuilder tagSpanCurrent = new TagBuilder("span");
                    tagSpanCurrent.Attributes["class"] = "sr-only";
                    tagSpanCurrent.InnerHtml.Append("(current)");
                    tagSpan.InnerHtml.AppendHtml(tagSpanCurrent);
                    tagLi.InnerHtml.AppendHtml(tagSpan);
                }
                else
                {
                    TagBuilder tagA = new TagBuilder("a");
                    tagA.Attributes["href"] = urlHelper.Action(PageAction, new {productPage = i});
                    tagA.Attributes["class"] = "page-link";
                    tagA.InnerHtml.Append(i.ToString());
                    tagLi.InnerHtml.AppendHtml(tagA);
                }


                result.AppendHtml(tagLi);
            }

            TagBuilder tagLiNext = NextPreviousBuilder(PageModel.CurrentPage < PageModel.TotalPages, "Next", "&raquo;");

            result.AppendHtml(tagLiNext);

            output.Content.AppendHtml(result);

            TagBuilder NextPreviousBuilder(bool rule, string direction, string symbol)
            {
                TagBuilder tagLiNP = new TagBuilder("li");

                tagLiNP.Attributes["class"] = rule ? "page-item" : "page-item disabled";

                TagBuilder tagAprevious = new TagBuilder("a");

                tagAprevious.Attributes["class"] = "page-link";
                tagAprevious.Attributes["arial-label"] = direction;


                tagAprevious.Attributes["href"] = rule
                    ? urlHelper.Action(PageAction,
                        new
                        {
                            productPage = direction == "Previous"
                                ? PageModel.CurrentPage - 1
                                : PageModel.CurrentPage + 1
                        })
                    : "";

                TagBuilder tagSpanPrevious1 = new TagBuilder("span");
                tagSpanPrevious1.Attributes["arial-hidden"] = "true";
                tagSpanPrevious1.InnerHtml.AppendHtmlLine(symbol);

                TagBuilder tagSpanPrevious2 = new TagBuilder("span");
                tagSpanPrevious2.Attributes["class"] = "sr-only";
                tagSpanPrevious2.InnerHtml.Append(direction);

                tagAprevious.InnerHtml.AppendHtml(tagSpanPrevious1);
                tagAprevious.InnerHtml.AppendHtml(tagSpanPrevious2);
                tagLiNP.InnerHtml.AppendHtml(tagAprevious);

                return tagLiNP;
            }
        }
    }
}