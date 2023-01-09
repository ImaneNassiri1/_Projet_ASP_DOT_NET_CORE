using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Exam_ASP.NetCore.Models
{
    public class Favori
    {
        [Key, Column(Order = 0)]
        public int OffreId { get; set; }
        [Key, Column(Order = 1)]
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        [ForeignKey("OffreId")]
        public Offre Offre { get; set; }
    }
}
