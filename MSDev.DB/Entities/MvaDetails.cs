using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MSDev.DB.Entities
{
    public class MvaDetails : Model
    {

        public MvaVideos MvaVideo { get; set; }

        [MaxLength(128)]
        public string  Title { get; set; }

        [MaxLength(256)]
        public string SourceUrl { get; set; }

        [MaxLength(256)]
        public string LowDownloadUrl { get; set; }
        [MaxLength(256)]

        public string MidDownloadUrl { get; set; }
        [MaxLength(256)]

        public string HighDownloadUrl { get; set; }

        [DataType(DataType.Duration)]
        public DateTime Duration { get; set; }

    }
}
