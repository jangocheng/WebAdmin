using System;

namespace MSDev.Task.Models
{
    public class BingNews : Model
    {
        /// <summary>
        /// 标题
        /// </summary>
        public String Title { get; set; }
        /// <summary>
        /// 内容概要
        /// </summary>
        public String Description { get; set; }
        /// <summary>
        /// 来源地址
        /// </summary>
        public String Url { get; set; }
        /// <summary>
        /// 缩略图链接
        /// </summary>
        public String ThumbnailUrl { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public String Tags { get; set; }
        /// <summary>
        /// 来源
        /// </summary>
        public String Provider { get; set; }

    }

}
