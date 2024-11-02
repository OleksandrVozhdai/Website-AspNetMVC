using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;

namespace LAB2_OOKP.Controllers
{
    public class AuthorisationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthorisationController(ApplicationDbContext context)
        {
            _context = context; 
        }

        // GET: Authorisation
        [HttpGet]
        public IActionResult Index()
        {
          
            return View(); 
        }

        // POST: Authorisation/Authorize
        [HttpPost]
        public IActionResult Authorize(string name, string surname)
        {
           
            var user = _context.UsersTable
                .FirstOrDefault(u => u.Email == name && u.Password == surname);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.Name);
                ViewBag.UserName = user.Name;
                ViewBag.UserEmail = user.Email;
                return RedirectToAction("Index", "Home");
            }
            else
            {
              
                ViewBag.ErrorMessage = "Invalid email or password.";
                return View("Index");
            }

        }
    }
}
