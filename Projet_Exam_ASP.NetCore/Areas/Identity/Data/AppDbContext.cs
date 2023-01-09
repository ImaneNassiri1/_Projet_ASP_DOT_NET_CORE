using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Models;

namespace Projet_Exam_ASP.NetCore.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Favori>().HasKey(f => new { f.AppUserId, f.OffreId });
            builder.Entity<Favori>().HasOne(f => f.Offre).WithMany(f => f.Favoris).HasForeignKey(f => f.OffreId);
            builder.Entity<Favori>().HasOne(f => f.AppUser).WithMany(f => f.Favoris).HasForeignKey(f => f.AppUserId);
            //builder.Entity<Image>().HasOne(f => f.Offre).WithMany(f => f.Images).HasForeignKey(f => f.OffreId);
        }
        public DbSet<Offre> Offres { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Propriété> Propriétés { get; set; }
        public DbSet<Boutique> Boutiques { get; set; }
        public DbSet<Favori> Favoris { get; set; }

    }
}
