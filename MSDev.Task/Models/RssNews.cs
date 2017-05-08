using MSDev.DataAgent.EnumTypes;
using System;

namespace MSDev.DataAgent.Models
{
    public class RssNews
    {
        public Int32 Id { get; set; }

        public NewsTypes Type { get; set; }
        public String Categories { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public String MobileContent { get; set; }

        public String Link { get; set; }

        public String Author { get; set; }

        public Int32 PublishId { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public DateTime CreateTime { get; set; }

        public ItemStatus Status { get; set; }
    }
}