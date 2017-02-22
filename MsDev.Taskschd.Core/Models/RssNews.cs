using MsDev.Taskschd.Core.EnumTypes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MsDev.Taskschd.Core.Models
{
    public class RssNews
    {
        public int Id { get; set; }

        public NewsTypes Type { get; set; }

        public List<string> Categories { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string MobileContent { get; set; }

        public string Link { get; set; }

        public string Author { get; set; }

        public int PublishId { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public DateTime CreateTime { get; set; }

        public ItemStatus Status { get; set; }
    }
}