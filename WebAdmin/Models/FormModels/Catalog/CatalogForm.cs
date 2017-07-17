using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.FormModels.Catalog
{
    public class CatalogForm
    {
	    /// <summary>
	    /// 显示名称
	    /// </summary>
	    [Required]
	    public string Name { get; set; }

	    /// <summary>
	    /// 分类值
	    /// </summary>
	    public string Value { get; set; }

	    /// <summary>
	    /// 分类的类别
	    /// </summary>
	    public string Type { get; set; }

	    /// <summary>
	    /// 是否为顶级分类
	    /// </summary>
	    public int IsTop { get; set; }

	    /// <summary>
	    /// 上级/顶级(如果存在)分类
	    /// </summary>
	    public string TopCatalog { get; set; }

	}
}
