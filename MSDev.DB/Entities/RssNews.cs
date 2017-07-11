using System;
using System.Collections.Generic;

namespace MSDev.DB.Entities

{
    public partial class RssNews
    {
        public int Id { get; set; }
        public string Author { get; set; }
        public string Categories { get; set; }
        public DateTime CreateTime { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public string Link { get; set; }
        public string MobileContent { get; set; }
        public int PublishId { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
    }
}
