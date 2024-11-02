using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LAB2_OOKP.Controllers
{
    public class HomeController : Controller
    {

        
        public ActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserEmail = HttpContext.Session.GetString("UserEmail");
            return View();
        }

        public ActionResult About()

        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}