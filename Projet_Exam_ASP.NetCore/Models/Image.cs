using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Exam_ASP.NetCore.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Offre? Offre { get; set; }
        public int? OffreId { get; set; }
    }
}
