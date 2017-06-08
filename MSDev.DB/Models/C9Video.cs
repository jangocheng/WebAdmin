using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MSDev.DB.Models
{
	public class C9Video : Model
	{
		/// <summary>
		/// 标题
		/// </summary>
		[MaxLength(256)]
		public string Title { get; set; }
		/// <summary>
		/// 系列名称
		/// </summary>
		[MaxLength(256)]
		public string SeriesTitle { get; set; }
		/// <summary>
		/// 作者
		/// </summary>
		[MaxLength(128)]
		public string Author { get; set; }
		/// <summary>
		/// 浏览量
		/// </summary>
		public int? Views { get; set; }
		/// <summary>
		/// 语言
		/// </summary>
		[MaxLength(16)]
		public string Language { get; set; }

		public override string ToString()
		{
			return $"Author: {Author}, Description: {Description}, Duration: {Duration}, Language: {Language}, Mp3Url: {Mp3Url}, Mp4HigUrl: {Mp4HigUrl}, Mp4LowUrl: {Mp4LowUrl}, Mp4MidUrl: {Mp4MidUrl}, SeriesTitle: {SeriesTitle}, SeriesTitleUrl: {SeriesTitleUrl}, SourceUrl: {SourceUrl}, Tags: {Tags}, ThumbnailUrl: {ThumbnailUrl}, Title: {Title}, Views: {Views}";
		}

		/// <summary>
		/// mp3 资源地址
		/// </summary>
		[MaxLength(256)]
		public string Mp3Url { get; set; }
		/// <summary>
		/// mp4 低画质 地址
		/// </summary>
		[MaxLength(256)]
		public string Mp4LowUrl { get; set; }
		/// <summary>
		/// mp4 中画质 地址
		/// </summary>
		[MaxLength(256)]
		public string Mp4MidUrl { get; set; }

		/// <summary>
		/// mp4 高画质 地址
		/// </summary>
		[MaxLength(256)]
		public string Mp4HigUrl { get; set; }

		/// <summary>
		/// 系列地址
		/// </summary>
		[MaxLength(256)]
		public string SeriesTitleUrl { get; set; }
		/// <summary>
		/// 介绍说明
		/// </summary>
		public string Description { get; set; }
		/// <summary>
		/// 标签
		/// </summary>
		[MaxLength(256)]
		public string Tags { get; set; }
		/// <summary>
		/// 后三位尺寸 100/220/512/960
		/// </summary>
		[MaxLength(256)]
		public string ThumbnailUrl { get; set; }
		/// <summary>
		/// 链接
		/// </summary>
		[MaxLength(256)]
		public string SourceUrl { get; set; }
		/// <summary>
		/// 视频时长
		/// </summary>
		[MaxLength(32)]
		public string Duration { get; set; }
	}
}
