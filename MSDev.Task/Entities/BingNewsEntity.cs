using System;

namespace MSDev.Task.Entities
{

    public class BingNewsEntity
    {

        public String Title { get; set; }

        public String Description { get; set; }

        public String Url { get; set; }

        public String ThumbnailUrl { get; set; }

        public String Provider { get; set; }

        public DateTime DatePublished { get; set; }

        public String CateGory { get; set; }
    }
}
