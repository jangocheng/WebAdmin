using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSDev.DB.Models
{
	public class DevBlog
	{
		public Guid Id { get; set; }

		/// <summary>
		/// 博客类别
		/// </summary>
		[MaxLength(32)]
		public string Category { get; set; }
		/// <summary>
		/// 标题
		/// </summary>
		[MaxLength(128)]
		public string Title { get; set; }

		/// <summary>
		/// 原标题
		/// </summary>
		[MaxLength(128)]
		public string SourceTitle { get; set; }
		/// <summary>
		/// 内容
		/// </summary>
		public string Content { get; set; }
		/// <summary>
		/// 原内容
		/// </summary>
		public string SourcContent { get; set; }
		/// <summary>
		/// 原链接
		/// </summary>
		[MaxLength(128)]
		public string Link { get; set; }
		/// <summary>
		/// 作者
		/// </summary>
		[MaxLength(64)]
		public string Author { get; set; }

		public DateTime UpdatedTime { get; set; }

		public DateTime CreatedTime { get; set; }

		/// <summary>
		/// 状态
		/// </summary>
		public int Status { get; set; }
	}
}
