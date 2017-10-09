using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSDev.DB.Entities;

namespace WebAdmin.Models.ViewModels
{
    public class EditVideoView
    {
        public Video Video{ get; set; }
        public List<Catalog> Catalogs { get; set; }
    }
}
