using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LAB2_OOKP.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }
    

        public IActionResult Index(string filterId, string filterName, string filterPrice, string filterSurname, string filterPizzaName)
        {
            var purchases = _context.Purchases.AsQueryable();

            if (!string.IsNullOrEmpty(filterId) && int.TryParse(filterId, out int id))
            {
                purchases = purchases.Where(p => p.Id == id);
            }

            if (!string.IsNullOrEmpty(filterName))
            {
                purchases = purchases.Where(p => p.Name.Contains(filterName));
            }

            if (!string.IsNullOrEmpty(filterPrice) && decimal.TryParse(filterPrice, out decimal price))
            {
                purchases = purchases.Where(p => p.PizzaPrice == price);
            }

            if (!string.IsNullOrEmpty(filterSurname))
            {
                purchases = purchases.Where(p => p.Surname.Contains(filterSurname));
            }

            if (!string.IsNullOrEmpty(filterPizzaName))
            {
                purchases = purchases.Where(p => p.PizzaName.Contains(filterPizzaName));
            }

            ViewData["FilterId"] = filterId;
            ViewData["FilterName"] = filterName;
            ViewData["FilterSurname"] = filterSurname;
            ViewData["FilterPrice"] = filterPrice;
            ViewData["FilterPizzaName"] = filterPizzaName;

            ViewBag.TotalSum = purchases.Sum(p => p.PizzaPrice);

            return View(purchases.ToList());
        }


        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var purchase = _context.Purchases.Find(id);
            if (purchase != null)
            {
                _context.Purchases.Remove(purchase);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }

        public IActionResult DownloadReport()
        { 
            string reportDate = DateTime.Today.ToString("yyyy-MM-dd");
            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports", $"report_{reportDate}.txt");

            if (!System.IO.File.Exists(reportPath))
            {
                return NotFound("Report not found.");
            }

            var fileBytes = System.IO.File.ReadAllBytes(reportPath);
            return File(fileBytes, "application/octet-stream", $"report_{reportDate}.txt");
        }

    }
}
