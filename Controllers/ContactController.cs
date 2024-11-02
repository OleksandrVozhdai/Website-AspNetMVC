using Microsoft.AspNetCore.Mvc;

namespace LAB2_OOKP.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            return View();
        }
    }
}
