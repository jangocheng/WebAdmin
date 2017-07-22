using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.FormModels.Resource
{
    public class ResourceForm
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        [Required]
        [MaxLength(128)]
        public string Name { get; set; }
        /// <summary>
        /// 说明及描述
        /// </summary>
        ///
        [MaxLength(1024)]
        public string Description { get; set; }
        /// <summary>
        /// 资源类型
        /// 0 文档;1 视频;2 软件;3 代码
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 提供方
        /// </summary>
        [MaxLength(128)]
        public string Provider { get; set; }

        /// <summary>
        /// 标签
        /// </summary>
        [MaxLength(128)]
        public string Tag { get; set; }
        /// <summary>
        /// 是否推荐
        /// </summary>
        public int IsRecommend { get; set; }
        /// <summary>
        /// 语言
        /// 0 中文;1 英文
        /// </summary>
        public int Language { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        ///
        [MaxLength(256)]
        public string IMGUrl { get; set; }
        /// <summary>
        /// 相对地址
        /// </summary>
        [MaxLength(128)]
        public string Path { get; set; }

        /// <summary>
        /// 绝对地址
        /// </summary>
        [MaxLength(256)]
        public string AbsolutePath { get; set; }

        /// <summary>
        /// 分类:下载
        /// </summary>
        [Required]
        public string Catalog { get; set; }

    }
}
