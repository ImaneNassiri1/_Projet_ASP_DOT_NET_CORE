using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data;
using Projet_Exam_ASP.NetCore.Models;

namespace Projet_Exam_ASP.NetCore.Controllers
{
    public class OffresController : BaseController
    {
        private readonly IWebHostEnvironment _hostEnvironment;
        private AppDbContext _appDbContext;
        AppUser CurrentUser;
        public OffresController(AppDbContext context, IWebHostEnvironment hostEnvironment, AppDbContext appDbContext): base(context)
        {
            _hostEnvironment = hostEnvironment;
            _appDbContext = appDbContext;
        }
        
        // GET: Offres
        public async Task<IActionResult> Index(int? id)
        {

            
            if (User.Identity.IsAuthenticated)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                CurrentUser = _context.Users.Find(currentUserID);
                var appDbContext = _context.Offres.Include(o => o.AppUser);
                List<Boolean> isFavoriList = new List<bool>();
                
                if (id == 0)
                {
                    foreach (Offre offre in appDbContext)
                    {
                        if (_appDbContext.Favoris.Where(f => f.AppUserId == CurrentUser.Id && offre.Id == f.OffreId).Count() != 0)
                            isFavoriList.Add(false);
                        else
                            isFavoriList.Add(true);
                    }
                    ViewBag.isFavoriList = isFavoriList;
                    ViewBag.All_Mine_Favoris = 0;
                    return View(await appDbContext.ToListAsync());
                }
                else if (id == 1)
                {
                    List<Offre> offres = _context.Offres.Where(o => o.AppUser == CurrentUser).ToList();
                    foreach (Offre f in offres)
                    {
                        isFavoriList.Add(false);
                    }
                    ViewBag.isFavoriList = isFavoriList;
                    ViewBag.All_Mine_Favoris = 1;
                    return View(offres);
                }
                else
                {
                    var favories = _context.Favoris.Where(f => f.AppUserId == CurrentUser.Id).ToList();
                    List<Offre> offresFavories = new List<Offre>();

                    foreach (Favori f in favories)
                    {
                        offresFavories.Add(_context.Offres.Find(f.OffreId));
                        isFavoriList.Add(false);
                    }
                    ViewBag.isFavoriList = isFavoriList;
                    ViewBag.All_Mine_Favoris = 2;
                    return View(offresFavories);
                }
            }
            else
            {
                ViewBag.All_Mine_Favoris = 0;
                var appDbContext = _context.Offres.Include(o => o.AppUser);
                return View(await appDbContext.ToListAsync());
            }
            
        }

        public async Task<IActionResult> TouteslesOffres(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<Image> images = new List<Image>();
            List<int> NbFavorisParOffre = new List<int>();
            

            List<bool> isFavoriList = new List<bool>();
            var Propriétaire = _context.Users.Find(id);
            if (Propriétaire == null)
            {
                return NotFound();
            }
            var offres=await _context.Offres.Where(o => o.AppUserId == id).OrderBy(o=>o.Date_Dépot).ToListAsync();
            foreach (Offre o in offres)
            {
                if (_appDbContext.Images.Where(i => i.OffreId == o.Id).ToList().Count() != 0)
                    images.Add(_appDbContext.Images.Where(i => i.OffreId == o.Id).ToList().First());
                else
                    images.Add((new Image { Nom = "Offre_Icone_Par_Defaut.png" }));
                NbFavorisParOffre.Add( _appDbContext.Favoris.Where(f => f.Offre == o).Count());
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
            ViewBag.Proprétaire = Propriétaire;
            ViewBag.isBoutique = Propriétaire.BoutiqueId != null;
            ViewBag.Boutique = _context.Boutiques.Find(Propriétaire.BoutiqueId);
            ViewBag.NbFavorisParOffre = NbFavorisParOffre;
            ViewBag.images = images;
            return View(offres);
        }
        [Authorize]
        public async Task<IActionResult> MesFavories()
        {
            List<Image> images = new List<Image>();
            List<int> NbFavorisParOffre = new List<int>();
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            CurrentUser = _context.Users.Find(currentUserID);
            var favoris = await _context.Favoris.Where(f => f.AppUserId == currentUserID).ToListAsync();
            List<Offre> offres=new List<Offre>();
            foreach (Favori f in favoris)
                offres.Add(_context.Offres.Find(f.OffreId));
            foreach (Offre o in offres)
            {
                if (_appDbContext.Images.Where(i => i.OffreId == o.Id).ToList().Count() != 0)
                    images.Add(_appDbContext.Images.Where(i => i.OffreId == o.Id).ToList().First());
                else
                    images.Add((new Image { Nom = "Offre_Icone_Par_Defaut.png" }));
                NbFavorisParOffre.Add(_appDbContext.Favoris.Where(f => f.Offre == o).Count());
            }
            ViewBag.NbFavorisParOffre = NbFavorisParOffre;
            ViewBag.images = images;
            return View(offres);
        }
        [Authorize]
        public async Task/*<IActionResult>*/ AddRemoveFavoris(int id,Boolean fromFavoris)
        {
            ClaimsPrincipal currentUser = this.User;
            var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
            AppUser CurrentUser = _context.Users.Find(currentUserID);
            Offre offre= _context.Offres.Find(id);
            Favori favori = new Favori {AppUserId= CurrentUser.Id, OffreId=id ,AppUser=CurrentUser,Offre=offre};
            if(_context.Favoris.Where(f=>f.AppUserId==favori.AppUserId&&f.OffreId==favori.OffreId).Count()==0)
            {
                _context.Favoris.Add(favori);
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.Favoris.Remove(favori);
                await _context.SaveChangesAsync();
            }
            //if (fromFavoris)
            //    return RedirectToAction("Index", new { id = 2 });
            //else if (!fromFavoris)
            //    return RedirectToAction("Index", new { id = 0 });
            //else
            //    return RedirectToAction("Index", "Home");
            // return NoContent();
        }
        // GET: Offres/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }
            ViewBag.propriétaire = offre.AppUser;
            return View(offre);
        }

        // GET: Offres/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Offres/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken][Authorize]
        public async Task<IActionResult> Create([Bind("Id,Titre,Ville,prix,Date_Dépot,Description,Catégorie,IndiceSousCatégorie,AppUserId,Valide,reduction,ImagesOffre")] Offre offre,List<IFormFile> images,int sousCategorie)
        {
            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                offre.AppUserId = currentUserID;
                offre.Valide = false;
                offre.reduction = 0;
                offre.Date_Dépot = DateTime.Now;
                offre.IndiceSousCatégorie = sousCategorie - 1;
                offre.Images = new List<Image>();
                foreach (IFormFile img in images)
                {
                    //pour ajouter l'image dans le dossier Images------------
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(img.FileName);
                    string fileExtention = Path.GetExtension(img.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff");
                    string path = Path.Combine(wwwRootPath + "/images/Offres/" + fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await img.CopyToAsync(fileStream);
                    }
                    //----------------------------------------------------------
                    //pour ajouter l'image dans la table image de la base de données--------------
                    Image image = new Image { Nom = fileName };
                    _appDbContext.Images.Add(image);
                    await _appDbContext.SaveChangesAsync();
                    //----------------------------------------------------------
                    offre.Images.Add(image);
                }
                _context.Add(offre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new { id="1" });
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", offre.AppUserId);
            return View(offre);
        }
        
        // GET: Offres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres.FindAsync(id);
            if (offre == null)
            {
                return NotFound();
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", offre.AppUserId);
            return View(offre);
        }

        // POST: Offres/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titre,prix,Ville,Date_Dépot,Description,Catégorie,IndiceSousCatégorie,AppUserId,Valide,reduction")] Offre offre,List<IFormFile> images )
        {
            if (id != offre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                ClaimsPrincipal currentUser = this.User;
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                offre.AppUserId = currentUserID;
                offre.Valide = false;
                offre.Date_Dépot = DateTime.Now;
                offre.Images = new List<Image>();
                foreach (IFormFile img in images)
                {
                    //pour ajouter l'image dans le dossier Images------------
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(img.FileName);
                    string fileExtention = Path.GetExtension(img.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff");
                    string path = Path.Combine(wwwRootPath + "/images/Offres/" + fileName + fileExtention);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await img.CopyToAsync(fileStream);
                    }
                    //----------------------------------------------------------
                    //pour ajouter l'image dans la table image de la base de données--------------
                    Image image = new Image { Nom = fileName+fileExtention };
                    _appDbContext.Images.Add(image);
                    await _appDbContext.SaveChangesAsync();
                    //----------------------------------------------------------
                    offre.Images.Add(image);
                }
                try
                {
                    _context.Update(offre);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OffreExists(offre.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = "1" });
            }
            ViewData["AppUserId"] = new SelectList(_context.Users, "Id", "Id", offre.AppUserId);
            return View(offre);
        }

        // GET: Offres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var offre = await _context.Offres
                .Include(o => o.AppUser)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (offre == null)
            {
                return NotFound();
            }

            return View(offre);
        }

        // POST: Offres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var offre = await _context.Offres.FindAsync(id);
            _context.Offres.Remove(offre);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OffreExists(int id)
        {
            return _context.Offres.Any(e => e.Id == id);
        }
    }
}
