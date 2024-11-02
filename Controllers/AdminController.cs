using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;

namespace LAB2_OOKP.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Index(string filterId, string filterName, string filterPhoneNumber)
        {
            var users = _context.UsersTable.AsQueryable();

            if (!string.IsNullOrEmpty(filterId) && int.TryParse(filterId, out int id))
            {
                users = users.Where(u => u.Id == id);
            }

            if (!string.IsNullOrEmpty(filterName))
            {
                users = users.Where(u => u.Name.Contains(filterName));
            }

            if (!string.IsNullOrEmpty(filterPhoneNumber))
            {
                users = users.Where(u => u.PhoneNumber.Contains(filterPhoneNumber));
            }

            return View(users.ToList()); 
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var user = _context.UsersTable.Find(id); 
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

    
        [HttpPost]
        public IActionResult Edit(User model)
        {

            var currentUser = _context.UsersTable.Find(model.Id);
            if (currentUser != null)
            {
 
                currentUser.Name = model.Name;
                currentUser.Surname = model.Surname;
                currentUser.Email = model.Email;
                currentUser.PhoneNumber = model.PhoneNumber;

                _context.SaveChanges(); 
                return RedirectToAction(nameof(Index));
            }


          
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage); 
            }

            return View(model); 
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var user = _context.UsersTable.Find(id);
            if (user != null)
            {
                _context.UsersTable.Remove(user);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

    }
}
