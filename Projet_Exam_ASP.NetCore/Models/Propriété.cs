using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Exam_ASP.NetCore.Models
{
    public class Propriété
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Valeur { get; set; }
        public int OffreId { get; set; }
        [ForeignKey("OffreId")]
        public Offre Offre { get; set; }
    }
}
