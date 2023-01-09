using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data;
using Projet_Exam_ASP.NetCore.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Projet_Exam_ASP
{
    public class Startup
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<AppUser> _userManager;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Projet_Exam_ASP.NetCore;Trusted_Connection=True;MultipleActiveResultSets=true"));

            services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();


            //--------------------------------multilungue--------------------------------------------------------------
            services.AddSingleton<LanguageService>();
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization()
                .AddDataAnnotationsLocalization(options =>
                {
                    options.DataAnnotationLocalizerProvider = (type, factory) =>
                    {
                        var assemblyName = new AssemblyName(typeof(ShareResource).GetTypeInfo().Assembly.FullName);
                        return factory.Create("ShareResource", assemblyName.Name);
                    };
                });
            services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                        {
                            new CultureInfo("en-US"),
                            new CultureInfo("fr-FR"),
                            new CultureInfo("ar-MA"),
                        };
                    options.DefaultRequestCulture = new RequestCulture(culture: "fr-FR", uiCulture: "fr-FR");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                });
            //--------------------------------------------------------------------------------------------------


            services.AddRazorPages(); // Identity Athentification

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider provider, AppDbContext appdbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //--------------------------------multilungue-------------------------------------------------------
            var locOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);
            //--------------------------------------------------------------------------------------------------


            app.UseAuthentication();    // Identity Athentification
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();      // Identity Athentification
            });
            createIconesParDefautAsync(appdbContext).Wait();
            //createAdminWithRole(provider, appdbContext).Wait();
            createOtherRoles(provider).Wait();

        }

        private async Task createAdminWithRole(IServiceProvider serviceProvider, AppDbContext appDbContext)
        {
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            var roleExist = await _roleManager.RoleExistsAsync("Admin");
            var userExist = await _userManager.FindByEmailAsync("Admin@email.com");
            if (!roleExist||userExist==null)
            {
                if(!roleExist)
                {
                    var role = new IdentityRole();
                    role.Name = "Admin";
                    await _roleManager.CreateAsync(role);
                }
                if (userExist==null)
                {
                    var user = new AppUser();
                    user.UserName = "admin@email.com";
                    user.Email= "admin@email.com";
                    Image img = appDbContext.Images.Where(i => i.Nom == "Boutique_Icone_Par_Defaut").First();
                    user.Image = img;
                    string userPassword = "AdminAdmin1*";
                    IdentityResult checkUser = await _userManager.CreateAsync(user, userPassword);
                    if(checkUser.Succeeded)
                    {
                        var result = await _userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }
        }
        private async Task createOtherRoles(IServiceProvider serviceProvider)
        {
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var roleUserExist = await _roleManager.RoleExistsAsync("User");
            var roleParticulierExist = await _roleManager.RoleExistsAsync("Particulier");
            var roleBoutiqueExist = await _roleManager.RoleExistsAsync("Boutique");
            var roleEnListeNoireExist = await _roleManager.RoleExistsAsync("EnListeNoire");
            if (!roleUserExist)
            {
                  var role = new IdentityRole();
                  role.Name = "User";
                  await _roleManager.CreateAsync(role);
            }
            if (!roleParticulierExist)
            {
                var role = new IdentityRole();
                role.Name = "Particulier";
                await _roleManager.CreateAsync(role);
            }
            if (!roleBoutiqueExist)
            {
                var role = new IdentityRole();
                role.Name = "Boutique";
                await _roleManager.CreateAsync(role);
            }
            if (!roleEnListeNoireExist)
            {
                var role = new IdentityRole();
                role.Name = "EnListeNoire";
                await _roleManager.CreateAsync(role);
            }
        }


        private async Task createIconesParDefautAsync(AppDbContext appDbContext)
        {
            var iconeBoutique = appDbContext.Images.Where(p => p.Nom == "Boutique_Icone_Par_Defaut.png");
            var iconeProfile = appDbContext.Images.Where(p => p.Nom == "Profile_Icone_Par_Defaut.png");
            if(iconeBoutique.Count()==0)
            {
                Image img = new Image { Nom = "Boutique_Icone_Par_Defaut.png" };
                appDbContext.Images.Add(img);
                await appDbContext.SaveChangesAsync();
            }

            if (iconeProfile.Count() == 0)
            {
                Image img = new Image { Nom = "Profile_Icone_Par_Defaut.png" };
                appDbContext.Images.Add(img);
                await appDbContext.SaveChangesAsync();
            }

        }
    }
}
  
