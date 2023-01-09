using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Projet_Exam_ASP.NetCore.Data.enums;
using Projet_Exam_ASP.NetCore.Models;

namespace Projet_Exam_ASP.NetCore.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the AppUser class
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Required]
        [Column(TypeName="nvarchar(100)")]
        public String Nom { get; set; }
        [Column(TypeName="nvarchar(100)")]
        [PersonalData]
        [Required]
        public String Prenom { get; set; }
        [Column(TypeName="nvarchar(100)")]
        [PersonalData]
        [Required]
        public String Telephone { get; set; }
        [Column(TypeName="nvarchar(100)")]
        [PersonalData][Required]
        public String Ville { get; set; }
        //[Column(TypeName="nvarchar(100)")]
        //[PersonalData]
        //public StatutPropriétaire Status { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [PersonalData]
        public Boolean EnListeNoire { get; set; }

        [PersonalData]
        public List<Offre> Offres { get; set; }
        [PersonalData]

        [ForeignKey("ImageId")]
        public int? BoutiqueId { get; set; }
        [ForeignKey("BoutiqueId")]
        public Boutique? Boutique { get; set; }
        [PersonalData][Required]
        public int ImageId { get; set; }
        [PersonalData]
        [ForeignKey("ImageId")]
        public Image Image { get; set; }
        [PersonalData]
        public List<Favori> Favoris { get; set; }
    }
}
