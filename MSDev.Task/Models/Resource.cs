using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace MSDev.DataAgent.Models
{
    /// <summary>
    /// 资源对象
    /// </summary>
    public class Resource : Model
    {
        /// <summary>
        /// 资源名称
        /// </summary>
        [Required]
        [MaxLength(128)]
        public String Name { get; set; }
        /// <summary>
        /// 说明及描述
        /// </summary>
        /// 
        [MaxLength(1024)]
        public String Description { get; set; }
        /// <summary>
        /// 资源类型
        /// 0 文档;1 视频;2 软件;3 代码
        /// </summary>
        public ResourceType Type { get; set; }

        /// <summary>
        /// 语言
        /// 0 中文;1 英文
        /// </summary>
        public LanuageType Language { get; set; }

        /// <summary>
        /// 图片地址
        /// </summary>
        /// 
        [MaxLength(256)]
        public String IMGUrl { get; set; }
        /// <summary>
        /// 相对地址
        /// </summary>
        [MaxLength(128)]
        public String Path { get; set; }

        /// <summary>
        /// 绝对地址
        /// </summary>
        [MaxLength(256)]
        public String AbsolutePath { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public Catalog Catalog { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public List<Sources> SourcesUrls { get; set; }

        public enum ResourceType
        {
            ALL,
            DOCUMENT,
            VIDEO,
            SOFTWARE,
            CODE
        }
        public enum LanuageType
        {
            CHINESE,
            ENGLISH
        }
    }
}
