
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data.enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Resources;
namespace Projet_Exam_ASP.NetCore.Models
{
    public class Offre
    {

        public int Id { get; set; }
        [Required]
        public String Titre { get; set; }
        [Required]
        public String Ville { get; set; }
        [Required]
        public double prix { get; set; }

        public DateTime Date_Dépot { get; set; }
        [Required]
        public String Description { get; set; }
        [Required]
        public Catégorie Catégorie { get; set; }

        public int IndiceSousCatégorie { get; set; }
        [Required]
        public List<Image> Images { get; set; }
        public List<Propriété> Proprirétés { get; set; }
        public string AppUserId { get; set; }
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }
        public List<Favori> Favoris { get; set; }
        public Boolean Valide { get; set; }
        public int reduction { get; set; }

    }
}
