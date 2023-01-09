using Microsoft.AspNetCore.Mvc;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data;
using Projet_Exam_ASP.NetCore.Data.enums;
using Projet_Exam_ASP.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projet_Exam_ASP.NetCore.Controllers
{
    public class FiltreController : BaseController
    {

        public FiltreController(AppDbContext context) : base(context) { }
        static int? catégorie=null;
        static int? sousCatégorie = null;
        static String searchText = null;

        public AppUser CurrentUser { get; private set; }

        public IActionResult Index(string SearchText = "")
        {
            ViewBag.categorie = "Aucune catégorie séléctionnée";
            ViewBag.search = "Aucune rechrche effectuée";
            List<Offre> offres = _context.Offres.ToList();
            var offresFiltrées = _context.Offres.Where(o => true);
            if (SearchText != "" && SearchText != null)
            {
                searchText = SearchText;
                ViewBag.search = searchText;
                 //offres = _context.Offres.Where(p => p.Titre.Contains(SearchText)|| p.Description.Contains(SearchText)).ToList();
                 offresFiltrées = offresFiltrées.Where(p => p.Titre.Contains(searchText)|| p.Description.Contains(searchText));
            }
            if (catégorie!=null )
            {
                offresFiltrées =offresFiltrées.Where(p => p.Catégorie==(Catégorie)catégorie );
                //List<String> sousCatList = GetsousCategories((int)catégorie);
                //ViewBag.categorie = sousCatList[(int)sousCatégorie-1];
            }
            if (sousCatégorie != null)
            {
                offresFiltrées = offresFiltrées.Where(p => p.IndiceSousCatégorie == sousCatégorie );
                //List<String> sousCatList = GetsousCategories((int)catégorie);
                //ViewBag.categorie = sousCatList[(int)sousCatégorie - 1];
            }
            List<Image> images = new List<Image>();
            List<bool> isFavoriList = new List<bool>();
            List<int> NbFavorisParOffre = new List<int>();

            offres = _context.Offres.ToList();
            foreach (Offre o in offres)
            {
                if (_context.Images.Where(i => i.OffreId == o.Id).ToList().Count() != 0)
                    images.Add(_context.Images.Where(i => i.OffreId == o.Id).ToList().First());
                else
                    images.Add((new Image { Nom = "Offre_Icone_Par_Defaut.png" }));
                NbFavorisParOffre.Add(_context.Favoris.Where(f => f.Offre == o).Count());
                if (User.Identity.IsAuthenticated)
                {
                    ClaimsPrincipal currentUser = this.User;
                    var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                    CurrentUser = _context.Users.Find(currentUserID);
                    if (_context.Favoris.Where(f => f.AppUserId == CurrentUser.Id && o.Id == f.OffreId).Count() == 0)
                        isFavoriList.Add(false);
                    else
                        isFavoriList.Add(true);
                    ViewBag.isFavorisList = isFavoriList;
                }
            }
            
            ViewBag.NbFavorisParOffre = NbFavorisParOffre;
            ViewBag.images = images;
            return View(offresFiltrées.ToList());
        }
        public List<String> GetCategories()
        {
            return Enum.GetNames(typeof(Catégorie)).ToList();
        }
        public List<String> GetVilles()
        {
            return Enum.GetNames(typeof(Ville)).ToList();
        }
        public List<String> GetsousCategories(int cat)
        {
            List<String> sousCatList;
            switch (cat)
            {
                case 0:
                    sousCatList= Enum.GetNames(typeof(INFORMATIQUE_ET_MULTIMEDIA)).ToList();
                    break;
                case 1:
                    sousCatList = Enum.GetNames(typeof(HABILLEMENT_ET_BIEN_ETRE)).ToList();
                    break;
                case 2:
                    sousCatList = Enum.GetNames(typeof(VEHICULES)).ToList();
                    break;
                case 3:
                    sousCatList = Enum.GetNames(typeof(LOISIRS_ET_DIVERTISSEMENT)).ToList();
                    break;
                case 4:
                    sousCatList = Enum.GetNames(typeof(IMMOBILIER)).ToList();
                    break;
                default:
                    sousCatList = Enum.GetNames(typeof(POUR_LA_MAISON_ET_JARDIN)).ToList();
                    break;
            }
            return sousCatList;
        }
        //public void test()
        //{
        //    ViewBag.random = (new Random()).Next();
        //}
        public IActionResult FiltreParCategories(int cat, int? souscat)
        {
            catégorie = cat;
            sousCatégorie = souscat;
            return RedirectToAction("Index");
        }
        public void removeCat()
        {
            catégorie = null;
            sousCatégorie = null;
        }
        public void removeSearch()
        {
            searchText = null;
        }
    }
}
