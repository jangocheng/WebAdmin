using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MSDev.Task.Models
{
    /// <summary>
    /// 各种类型的目录
    /// </summary>
    public class Catalog : Model
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        public String Name { get; set; }

        /// <summary>
        /// 分类值
        /// </summary>
        public String Value { get; set; }
        /// <summary>
        /// 分类的类别
        /// </summary>
        public String Type { get; set; }

        /// <summary>
        /// 是否为顶级分类
        /// </summary>
        public Int32 IsTop { get; set; }

        /// <summary>
        /// 上级/顶级(如果存在)分类
        /// </summary>
        public Catalog TopCatalog { get; set; }

        public List<Resource> Resource { get; set; }

    }
}
