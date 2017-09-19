using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models.FormModels.Article
{
    public class AddArticleForm
    {
        [Required]
        public Guid CatalogId  { get; set; }
        [Required]
        [MaxLength(128)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Tags { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }
        [Required]
        [MaxLength(4000)]
        public string Content { get; set; }
    }
}
