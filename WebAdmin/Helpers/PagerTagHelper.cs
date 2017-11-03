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
            //if (!IsNullOrEmpty(PagerOption.RouteUrl))
            //{
            //    int lastIndex = PagerOption.RouteUrl.LastIndexOf("/", StringComparison.Ordinal);
            //    PagerOption.RouteUrl = PagerOption.RouteUrl.Substring(0, lastIndex);
            //}
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
                            //判断url是否已包含参数
                            var pageParam = "?p=";
                            if (PagerOption.RouteUrl.Contains("?"))
                            {
                                pageParam = "&p=";
                            }

                            sbPage.Append("<nav class='row ml-0'>");
                            sbPage.Append("  <ul class=\"pagination col col-md-auto pr-0\">");
                            // 前一页
                            sbPage.AppendFormat("       <li class=\"page-item\"><a class=\"page-link\" href=\"{0}" + pageParam + "{1}\" aria-label=\"Previous\"><span aria-hidden=\"true\">&laquo;</span></a></li>",
                                PagerOption.RouteUrl,
                                startPage - maxPage <= 0 ? 1 : startPage - maxPage);

                            // 构造页数
                            for (int i = startPage; i < endPage; i++)
                            {

                                sbPage.AppendFormat("       <li class=\"page-item\" {1}><a class=\"page-link\" href=\"{2}" + pageParam + "{0}\">{0}</a></li>",
                                    i,
                                    i == PagerOption.CurrentPage ? "class=\"active\"" : "",
                                    PagerOption.RouteUrl);
                            }

                            sbPage.Append("       <li class=\"page-item\">");

                            // 后一页
                            sbPage.AppendFormat("         <a class=\"page-link\" href=\"{0}" + pageParam + "{1}\" aria-label=\"Next\">",
                                PagerOption.RouteUrl,
                                endPage + maxPage > totalPage ? PagerOption.CurrentPage : endPage);
                            sbPage.Append("               <span aria-hidden=\"true\">&raquo;</span>");
                            sbPage.Append("         </a>");
                            sbPage.Append("       </li>");
                            sbPage.Append("</ul>");

                            sbPage.Append("<ul class='d-flex col-md-auto pl-0' >");
                            sbPage.Append("<li class=\"page-item page-link\"><span>");
                            sbPage.Append("<input type='number' id='totalPage' class='col-sm-12' style='width:50px;padding:0;margin:-2px 4px;'/>");
                            sbPage.Append("<a href='javascript:gotoPage()' id='gotoPage' onclick='gotoPage'>跳转</a>");
                            sbPage.Append($" 共:{totalPage}页</span>");
                            sbPage.Append("</li>");
                            sbPage.Append("</ul>");
                            sbPage.Append("</nav>");


                            sbPage.Append(@"
							<script>
								function gotoPage() {
									var page = document.getElementById('totalPage').value;
                                    if(page<1) page = 1;
									window.location.href = '" + PagerOption.RouteUrl + pageParam + @"'+page;
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
