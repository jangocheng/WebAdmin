using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MSDev.Task.Entities
{
    public class RssEntity
    {
        public List<String> Categories { get; set; }

        public String Title { get; set; }

        public String Description { get; set; }

        public DateTime CreateTime { get; set; }

        public String Link { get; set; }

        public String Author { get; set; }

        public Int32 PublishId { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public String MobileContent { get; set; }
    }
}