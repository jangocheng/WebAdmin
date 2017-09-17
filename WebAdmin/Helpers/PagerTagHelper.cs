using System;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static System.String;

namespace WebAdmin.Helpers
{
    public class PagerTagHelper : TagHelper
    {
        public MyPagerOption PagerOption { get; set; }
        /// <summary>
        /// 最多显示页数
        /// </summary>
        public int maxPage = 5;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            output.TagName = "div";

            if (PagerOption.PageSize <= 0) { PagerOption.PageSize = 15; }
            if (PagerOption.CurrentPage <= 0) { PagerOption.CurrentPage = 1; }
            if (PagerOption.Total <= 0) { return; }

            //总页数
            int totalPage = PagerOption.Total / PagerOption.PageSize + (PagerOption.Total % PagerOption.PageSize > 0 ? 1 : 0);
            if (totalPage <= 0) { return; }
            //当前路由地址
            if (!IsNullOrEmpty(PagerOption.RouteUrl))
            {
                int lastIndex = PagerOption.RouteUrl.LastIndexOf("/", StringComparison.Ordinal);
                PagerOption.RouteUrl = PagerOption.RouteUrl.Substring(0, lastIndex);
            }
            PagerOption.RouteUrl = PagerOption.RouteUrl.TrimEnd('/');
            PagerOption.RouteUrl = PagerOption.RouteUrl ?? "/";
            //构造分页样式
            if (PagerOption.Total > PagerOption.PageSize)
            {
                var sbPage = new StringBuilder(Empty);
                switch (PagerOption.StyleNum)
                {
                    case 2:
                        {
                            break;
                        }
                    default:
                        {
                            #region 默认样式

                            int startPage = (PagerOption.CurrentPage / maxPage) * maxPage + 1;
                            int endPage = startPage + maxPage > totalPage + 1 ? totalPage + 1 : startPage + maxPage;

                            sbPage.Append("<nav>");
                            sbPage.Append("  <ul class=\"pagination\">");
                            // 前一页
                            sbPage.AppendFormat("       <li><a href=\"{0}?p={1}\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>",
                                PagerOption.RouteUrl,
                                startPage - maxPage <= 0 ? 1 : startPage - maxPage);

                            // 构造页数

                            for (int i = startPage; i < endPage; i++)
                            {

                                sbPage.AppendFormat("       <li {1}><a href=\"{2}?p={0}\">{0}</a></li>",
                                    i,
                                    i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                                    PagerOption.RouteUrl);
                            }

                            sbPage.Append("       <li>");

                            // 后一页
                            sbPage.AppendFormat("         <a href=\"{0}?p={1}\" aria-label=\"Next\">",
                                PagerOption.RouteUrl,
                                endPage + maxPage > totalPage ? PagerOption.CurrentPage : endPage + maxPage);
                            sbPage.Append("               <span aria-hidden=\"true\">&raquo;</span>");
                            sbPage.Append("         </a>");
                            sbPage.Append("       </li>");

                            sbPage.Append("<li><span>");

                            sbPage.Append("<input type='number' id='totalPage' style='width:50px;padding:0;margin:-2px 4px;'/>");
                            sbPage.Append("<a href='javascript:gotoPage()' id='gotoPage' onclick='gotoPage'>跳转</a>");
                            sbPage.Append($" 共:{totalPage}页</span>");
                            sbPage.Append("</li>");
                            sbPage.Append("</ul>");
                            sbPage.Append("</nav>");
                            sbPage.Append(@"
							<script>
								function gotoPage() {
									var page = document.getElementById('totalPage').value;
									window.location.href = '" + PagerOption.RouteUrl + @"?p='+page;
								}
							</script>");
                            #endregion
                        }
                        break;
                }
                output.Content.SetHtmlContent(sbPage.ToString());
            }
            else
            {
                output.Content.SetHtmlContent("");

            }
            //output.TagMode = TagMode.SelfClosing;
            //return base.ProcessAsync(context, output);
        }

    }
    /// <summary>
    /// 分页option属性
    /// </summary>
    public class MyPagerOption
    {
        /// <summary>
        /// 当前页  必传
        /// </summary>
        public int CurrentPage { get; set; }
        /// <summary>
        /// 总条数  必传
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 分页记录数（每页条数 默认每页15条）
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 路由地址(格式如：/Controller/Action) 默认自动获取
        /// </summary>
        public string RouteUrl { get; set; }

        /// <summary>
        /// 样式 默认 bootstrap样式 1
        /// </summary>
        public int StyleNum { get; set; }
    }
}
