using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models.FormModels.Videos
{
    public class VideoForm
    {
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        [MaxLength(256)]
        public string Url { get; set; }
        [MaxLength(32)]
        public string Author { get; set; }
        [MaxLength(128)]
        public string Tags { get; set; }
        public string ThumbnailUrl { get; set; }
      
        public bool IsRecommend { get; set; }

    }
}
