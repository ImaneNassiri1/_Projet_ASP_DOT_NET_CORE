using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data;
using Projet_Exam_ASP.NetCore.Data.enums;
using Projet_Exam_ASP.NetCore.Models;

namespace Projet_Exam_ASP.NetCore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _hostEnvironment;
        private AppDbContext _appDbContext;
        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment hostEnvironment,
            AppDbContext appDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _hostEnvironment = hostEnvironment;
            _appDbContext = appDbContext;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [NotMapped]
            public string ImageProfileNom{ get; set; }

            [NotMapped]
            [Display(Name = "Image de profile")]
            public IFormFile ImageProfile { get; set; }
            [Display(Name = "Nom de la boutique")]
            [Required]
            public string NomBoutique { get; set; }
            [Display(Name = "Description de la boutique")]
            [Required]
            public string DescriptionBoutique { get; set; }
            [Display(Name = "Ville de la boutique")]
            [Required]
            public string VilleBoutique { get; set; }
            [Display(Name = "Adresse de la boutique")]
            [Required]
            public string AdresseBoutique { get; set; }
            [Display(Name = "Site de la boutique")]
            [Required]
            public string SiteBoutique { get; set; }
            [Display(Name = "Telephone de la boutique")]
            [Required]
            public string TelephoneBoutique { get; set; }
            [Display(Name = "Image de la boutique")]

            public IFormFile ImageBoutique { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                
                var user = new AppUser
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nom = Input.Nom,
                    Prenom = Input.Prenom,
                    Telephone = Input.Telephone,
                    Ville = Input.Ville,
                    EnListeNoire = false,
                    EmailConfirmed = true,
                };
                Image img;
                if (Input.ImageProfile != null)
                {
                    //pour ajouter l'image dans le dossier Images------------
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(Input.ImageProfile.FileName);
                    string fileExtention = Path.GetExtension(Input.ImageProfile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff");
                    string path = Path.Combine(wwwRootPath + "/images/Profiles" + fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await Input.ImageProfile.CopyToAsync(fileStream);
                    }
                    //----------------------------------------------------------
                    //pour ajouter l'image dans la table image de la base de données--------------
                    img = new Image { Nom = fileName };
                    _appDbContext.Images.Add(img);
                    await _appDbContext.SaveChangesAsync();
                    //----------------------------------------------------------
                }
                else
                {
                    img = _appDbContext.Images.Where(i=>i.Nom== "Profile_Icone_Par_Defaut.png").First();
                }
                user.Image = img;

                if (Input.Status == StatutPropriétaire.Particulier)
                    user.Boutique = null;
                else
                {
                    Image imgB;
                    if (Input.ImageBoutique != null)
                    {
                        //pour ajouter l'image dans le dossier Images------------
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(Input.ImageBoutique.FileName);
                        string fileExtention = Path.GetExtension(Input.ImageBoutique.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff");
                        string path = Path.Combine(wwwRootPath + "/images/Boutiques" + fileName);
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
                        imgB = _appDbContext.Images.Where(i => i.Nom == "Boutique_Icone_Par_Defaut.png").First();
                    }
                    Boutique boutique = new Boutique
                    {
                        Nom = Input.NomBoutique,
                        Description = Input.DescriptionBoutique,
                        Adresse = Input.AdresseBoutique,
                        site = Input.SiteBoutique,
                        Telephone = Input.TelephoneBoutique,
                        Ville = Input.VilleBoutique,
                    };
                    boutique.Image = imgB;
                    _appDbContext.Boutiques.Add(boutique);
                    await _appDbContext.SaveChangesAsync();
                    user.Boutique = boutique;
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, "user");
                    if (Input.Status==StatutPropriétaire.Particulier)
                        await _userManager.AddToRoleAsync(user, "Particulier");
                    else if (Input.Status == StatutPropriétaire.Société)
                        await _userManager.AddToRoleAsync(user, "Boutique");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
