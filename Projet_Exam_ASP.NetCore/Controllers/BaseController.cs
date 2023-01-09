using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
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
    public class BaseController : Controller
    {
        public readonly AppDbContext _context;

        public BaseController(AppDbContext context)
        {
            _context = context;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ClaimsPrincipal currentUser = this.User;
            if (User.Identity.IsAuthenticated)
            {
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                ViewBag.currentUser = _context.Users.Find(currentUserID);
                ViewBag.context = _context;

            }
            ViewBag.Catégories= Enum.GetNames(typeof(Catégorie)).ToList();
            base.OnActionExecuting(filterContext);
        }
        
    }
}
