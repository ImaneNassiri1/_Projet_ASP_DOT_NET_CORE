using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data;
using Projet_Exam_ASP.NetCore.Data.enums;
using Projet_Exam_ASP.NetCore.Models;

namespace Projet_Exam_ASP.NetCore.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _hostEnvironment;
        private AppDbContext _appDbContext;


        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IWebHostEnvironment hostEnvironment,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _hostEnvironment = hostEnvironment;
            _appDbContext = appDbContext;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }
        public IFormFile _ImageProfile;

        public class InputModel
        {
            [Required]
            [Display(Name = "Nom")]
            public string Nom { get; set; }

            [Required]
            [Display(Name = "Prenom")]
            public string Prenom { get; set; }

            [Required]
            [Display(Name = "Telehopne")]
            public string Telephone { get; set; }

            [Required]
            [Display(Name = "Ville")]
            public string Ville { get; set; }

            [Required]
            [Display(Name = "Status")]
            public StatutPropriétaire Status { get; set; }

            [NotMapped]
            [Display(Name = "Image de profile")]
            public IFormFile ImageProfile { get; set; }
            public string NomBoutique { get; set; }
            [Display(Name = "Description de la boutique")]
            public string DescriptionBoutique { get; set; }
            [Display(Name = "Ville de la boutique")]
            public string VilleBoutique { get; set; }
            [Display(Name = "Adresse de la boutique")]
            public string AdresseBoutique { get; set; }
            [Display(Name = "Site de la boutique")]
            public string SiteBoutique { get; set; }
            [Display(Name = "Telephone de la boutique")]
            public string TelephoneBoutique { get; set; }
            [Display(Name = "Image de la boutique")]
            public IFormFile ImageBoutique { get; set; }
        }

        private async Task LoadAsync(AppUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;


        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _ImageProfile = Input.ImageProfile;
            Input = new InputModel
            {
                Nom = Input.Nom,
                Prenom = Input.Prenom,
                Telephone = Input.Telephone,
                Ville = Input.Ville,
                NomBoutique = Input.NomBoutique,
                AdresseBoutique = Input.AdresseBoutique,
                DescriptionBoutique = Input.DescriptionBoutique,
                ImageBoutique = Input.ImageBoutique,
                Status = Input.Status,
                SiteBoutique = Input.SiteBoutique,
                TelephoneBoutique = Input.TelephoneBoutique,
                VilleBoutique=Input.VilleBoutique

            };
            


            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Nom != user.Nom)
            {
                user.Nom = Input.Nom;
            }
            if (Input.Prenom != user.Prenom)
            {
                user.Prenom = Input.Prenom;
            }
            if (Input.Ville != user.Ville)
            {
                user.Ville = Input.Ville;
            }
            
            if (Input.Telephone!= user.Telephone)
            {
                user.Telephone = Input.Telephone;
            }
            if (_ImageProfile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(_ImageProfile.FileName);
                string fileExtention = Path.GetExtension(_ImageProfile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff");
                string path = Path.Combine(wwwRootPath + "/images/" + fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await _ImageProfile.CopyToAsync(fileStream);
                }
                Image img;
                img = new Image { Nom = fileName };
                _appDbContext.Images.Add(img);
                await _appDbContext.SaveChangesAsync();
                user.Image = img;
            }
            if (Input.Status == StatutPropriétaire.Particulier)
                user.Boutique = null;
            else
            {
                Boutique boutique = new Boutique
                {
                    Nom = Input.NomBoutique,
                    Description = Input.DescriptionBoutique,
                    Adresse = Input.AdresseBoutique,
                    site = Input.SiteBoutique,
                    Telephone = Input.TelephoneBoutique,
                    Ville = Input.VilleBoutique,
                };
                Image imgB;
                if (Input.ImageBoutique != null)
                {
                    //pour ajouter k=l'image dans le dossier Images------------
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(Input.ImageBoutique.FileName);
                    string fileExtention = Path.GetExtension(Input.ImageBoutique.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff");
                    string path = Path.Combine(wwwRootPath + "/images/" + fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Input.ImageBoutique.CopyToAsync(fileStream);
                    }
                    //----------------------------------------------------------
                    //pour ajouter l'image dans la table image de la base de données--------------
                    imgB = new Image { Nom = fileName };
                    _appDbContext.Images.Add(imgB);
                    await _appDbContext.SaveChangesAsync();
                    //----------------------------------------------------------

                }
                else
                {
                    imgB = _appDbContext.Images.Where(i => i.Nom == "Boutique_Icone_Par_Defaut").First();
                }


                boutique.Image = imgB;
                _appDbContext.Boutiques.Add(boutique);
                await _appDbContext.SaveChangesAsync();
                user.Boutique = boutique;
            }
                await _userManager.UpdateAsync(user);
                await _userManager.AddToRoleAsync(user, "user");
         
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
