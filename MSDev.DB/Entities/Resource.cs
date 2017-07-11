using System;
using System.Collections.Generic;

namespace MSDev.DB.Entities

{
    public partial class Resource
    {
        public Resource()
        {
            Sources = new HashSet<Sources>();
        }

        public Guid Id { get; set; }
        public string AbsolutePath { get; set; }
        public Guid? CatalogId { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int? Status { get; set; }
        public int Type { get; set; }
        public DateTime UpdatedTime { get; set; }
        public string Imgurl { get; set; }
        public int Language { get; set; }

        public Catalog Catalog { get; set; }
        public ICollection<Sources> Sources { get; set; }
    }
}
