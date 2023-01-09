using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Projet_Exam_ASP.NetCore.Areas.Identity.Data;
using Projet_Exam_ASP.NetCore.Data;

[assembly: HostingStartup(typeof(Projet_Exam_ASP.NetCore.Areas.Identity.IdentityHostingStartup))]
namespace Projet_Exam_ASP.NetCore.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                //services.AddDbContext<AppDbContext>(options =>
                //    options.UseSqlServer(
                //        context.Configuration.GetConnectionString("AppDbContextConnection")));

                //services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
                //    .AddRoles<IdentityRole>()
                //    .AddEntityFrameworkStores<AppDbContext>();
            });
        }
    }
}