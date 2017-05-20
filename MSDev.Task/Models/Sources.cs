using System;
using System.ComponentModel.DataAnnotations;

namespace MSDev.Task.Models
{
    //资源来源/下载点
    public class Sources : Model
    {
        [MaxLength(128)]
        public string Name { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [MaxLength(32)]
        public string Type { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        [MaxLength(32)]
        public string Tag { get; set; }
        [MaxLength(256)]
        public string Url { get; set; }
        [MaxLength(256)]
        public string Hash { get; set; }

        public Resource Resource { get; set; }
    }
}
