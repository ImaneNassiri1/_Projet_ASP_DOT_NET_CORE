using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data;
using Projet_Exam_ASP.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projet_Exam_ASP.NetCore.Controllers
{
    public static class Extensions
    {
        public static K FindFirstKeyByValue<K, V>(this Dictionary<K, V> dict, V val)
        {
            return dict.FirstOrDefault(entry =>
                EqualityComparer<V>.Default.Equals(entry.Value, val)).Key;
        }
    }
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
         
        public HomeController(ILogger<HomeController> logger,AppDbContext appDbContext): base(appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }
        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

            return Redirect(Request.Headers["Referer"].ToString());
        }
        public IActionResult Index()
        {
            ClaimsPrincipal currentUser = this.User;
            AppUser CurrentUser=null;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                CurrentUser= _context.Users.Find(currentUserID);
                if (User.IsInRole("Admin"))
                    return RedirectToAction("Dashboard", "Admin");
            }
            
            List<Offre> offres = _appDbContext.Offres.Where(o => o.reduction > 0).OrderByDescending(o => o.reduction).Take(12).ToList();
            List<Image> imagesOffresSpéciales=new List<Image>();
            List<Boolean> isFavoriList = new List<bool>();
            
            foreach (Offre o in offres)
            {
                if(_appDbContext.Images.Where(i => i.OffreId == o.Id).ToList().Count()!=0)
                imagesOffresSpéciales.Add(_appDbContext.Images.Where(i => i.OffreId == o.Id).ToList().First());
                else
                imagesOffresSpéciales.Add((new Image {Nom= "Offre_Icone_Par_Defaut.png" }));
                if (User.Identity.IsAuthenticated)
                {
                    if (_appDbContext.Favoris.Where(f => f.AppUserId == CurrentUser.Id && o.Id == f.OffreId).Count() == 0)
                        isFavoriList.Add(false);
                    else
                        isFavoriList.Add(true);
                }
            }
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.isFavoriList_OffresSpéciales = isFavoriList;
            }
            ViewBag.imagesOffresSpéciales = imagesOffresSpéciales;
            ViewBag.OffresSpéciales = offres;
            ViewBag.OffresSpéciales_Count = offres.Count();




            Dictionary<Offre, int> NbFavorisParOffre = new Dictionary<Offre, int>();
            List<Image> imagesMeilleuresOffres = new List<Image>();
            List<Boolean> isFavoriList_MeilleuresOffres = new List<bool>();
            foreach (var offre in _appDbContext.Offres.ToList())
                NbFavorisParOffre.Add(offre, _appDbContext.Favoris.Where(f => f.Offre == offre).Count());
            List<int> nombreFavories = new List<int>();
            List<Offre> MeilleuresOffres = new List<Offre>();
            for(int i=0;i<16;i++)
            {
                if (NbFavorisParOffre.Count() != 0)
                {
                    Offre offre = NbFavorisParOffre.FindFirstKeyByValue(NbFavorisParOffre.Values.Max());
                    MeilleuresOffres.Add(offre);
                    nombreFavories.Add(NbFavorisParOffre[offre]);
                    if (_appDbContext.Images.Where(i => i.OffreId == offre.Id).ToList().Count() != 0)
                        imagesMeilleuresOffres.Add(_appDbContext.Images.Where(i => i.OffreId == offre.Id).ToList().First());
                    else
                        imagesMeilleuresOffres.Add((new Image { Nom = "Offre_Icone_Par_Defaut.png" }));
                    NbFavorisParOffre.Remove(offre);
                    if (User.Identity.IsAuthenticated)
                    {
                        if (_appDbContext.Favoris.Where(f => f.AppUserId == CurrentUser.Id && offre.Id == f.OffreId).Count() == 0)
                            isFavoriList_MeilleuresOffres.Add(false);
                        else
                            isFavoriList_MeilleuresOffres.Add(true);
                    }
                }
                else break;
            }
            ViewBag.nombreFavories = nombreFavories;
            ViewBag.MeilleuresOffres = MeilleuresOffres;
            ViewBag.imagesMeilleuresOffres = imagesMeilleuresOffres;
            ViewBag.MeilleuresOffres_Count = MeilleuresOffres.Count();
            ViewBag.isFavoriList_MeilleuresOffres = isFavoriList_MeilleuresOffres;



            offres= _appDbContext.Offres.OrderByDescending(f => f.Date_Dépot).Take(16).ToList();
            List<bool> isFavoriList_Nouveauté = new List<bool>();
            List<Image> imagesNouveauté = new List<Image>();
            foreach (Offre offre in offres)
            {
                if (User.Identity.IsAuthenticated)
                {
                    if (_appDbContext.Favoris.Where(f => f.AppUserId == CurrentUser.Id && offre.Id == f.OffreId).Count() == 0)
                        isFavoriList_Nouveauté.Add(false);
                    else
                        isFavoriList_Nouveauté.Add(true);
                }
                if (_appDbContext.Images.Where(i => i.OffreId == offre.Id).ToList().Count() != 0)
                    imagesNouveauté.Add(_appDbContext.Images.Where(i => i.OffreId == offre.Id).ToList().First());
                else
                    imagesNouveauté.Add((new Image { Nom = "Offre_Icone_Par_Defaut.png" }));
            }
            ViewBag.Nouveauté = offres;
            ViewBag.imagesNouveauté = imagesNouveauté;
            ViewBag.Nouveauté_Count = offres.Count();
            ViewBag.isFavoriList_Nouveauté = isFavoriList_Nouveauté;
            return View();
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
