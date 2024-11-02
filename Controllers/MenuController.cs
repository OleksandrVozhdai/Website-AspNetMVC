using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;

namespace LAB2_OOKP.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }


        public ActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var pizzas = _context.PizzaTable.ToList(); 
            return View(pizzas); 
        }

    }
}
