using System;

namespace MSDev.Task.Entities
{
	public class C9ArticleEntity
	{
		/// <summary>
		/// 标题
		/// </summary>
		public string Title { get; set; }
		/// <summary>
		/// 系列名称
		/// </summary>
		public string SeriesTitle { get; set; }

		/// <summary>
		/// 系列地址
		/// </summary>
		public string SeriesTitleUrl { get; set; }

		/// <summary>
		/// 后三位尺寸 100/220/512/960
		/// </summary>
		public string ThumbnailUrl { get; set; }
		/// <summary>
		/// 链接
		/// </summary>
		public string SourceUrl { get; set; }
		/// <summary>
		/// 视频时长
		/// </summary>
		public string Duration { get; set; }

		/// <summary>
		/// 状态 新
		/// </summary>
		public int? Status { get; set; }

		public DateTime UpdatedTime { get; set; }
		public DateTime CreatedTime { get; set; }

	}
}