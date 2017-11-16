using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MSDev.DB.Entities
{
    public class EventVideo
    {
        public Guid Id { get; set; }
        public C9Event C9Event { get; set; }

        public string Author { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Language { get; set; }
        public string Mp3Url { get; set; }
        public string Mp4HigUrl { get; set; }
        public string Mp4LowUrl { get; set; }
        public string Mp4MidUrl { get; set; }
        [MaxLength(64)]
        public string SeriesType { get; set; }
        [MaxLength(1000)]
        public string VideoEmbed { get; set; }
        public string SeriesTitle { get; set; }
        public string SeriesTitleUrl { get; set; }
        public string SourceUrl { get; set; }
        public int? Status { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 字幕链接内容
        /// </summary>
        [MaxLength(4000)]
        public string Caption { get; set; }
        public string ThumbnailUrl { get; set; }
        public string Title { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int? Views { get; set; }
    }
}
