using MSDev.DB.Entities;

namespace WebAdmin.Models.ViewModels
{
    public class DownloadViewModel
    {
        public static Resource EditDownload { get; set; }

        public Resource DownloadItem { get; set; }

        public Catalog Catalog { get; set; }
    }
}
