using MSDev.DB.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models.ViewModels
{
    public class DownloadViewModel
    {
        public static Resource EditDownload { get; set; }

        public Resource DownloadItem { get; set; }

        public Catalog Catalog { get; set; }
    }
}
