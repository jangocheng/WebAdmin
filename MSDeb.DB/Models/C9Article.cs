using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSDev.DB.Models
{
	public class C9Article : Model
	{
		/// <summary>
		/// 标题
		/// </summary>
		[MaxLength(256)]
		public string Title { get; set; }
		/// <summary>
		/// 系列名称
		/// </summary>
		[MaxLength(128)]
		public string SeriesTitle { get; set; }

		/// <summary>
		/// 系列地址
		/// </summary>
		[MaxLength(128)]
		public string SeriesTitleUrl { get; set; }

		/// <summary>
		/// 后三位尺寸 100/220/512/960
		/// </summary>
		[MaxLength(256)]
		public string ThumbnailUrl { get; set; }
		/// <summary>
		/// 链接
		/// </summary>
		[MaxLength(128)]
		public string SourceUrl { get; set; }
		/// <summary>
		/// 视频时长
		/// </summary>
		[MaxLength(32)]
		public string Duration { get; set; }
	}
}
