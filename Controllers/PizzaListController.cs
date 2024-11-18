using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace LAB2_OOKP.Controllers
{
    public class PizzaListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PizzaListController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string filterId, string filterName, string filterPrice)
        {
            var pizzaList = _context.PizzaTable.AsQueryable();

            if (!string.IsNullOrEmpty(filterId) && int.TryParse(filterId, out int id))
            {
                pizzaList = pizzaList.Where(p => p.Id == id);
            }

            if (!string.IsNullOrEmpty(filterName))
            {
                pizzaList = pizzaList.Where(p => p.pizza_name.Contains(filterName));
            }

            if (!string.IsNullOrEmpty(filterPrice) && decimal.TryParse(filterPrice, out decimal price))
            {
                pizzaList = pizzaList.Where(p => p.price == price);
            }

            ViewData["FilterId"] = filterId;
            ViewData["FilterName"] = filterName;
            ViewData["FilterPrice"] = filterPrice;

            return View(pizzaList.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(PizzaList model)
        {
            if (_context.PizzaTable.Any(p => p.pizza_name == model.pizza_name))
            {
                ModelState.AddModelError("pizza_name", "A pizza with this name already exists.");
            }

            if (ModelState.IsValid)
            {
                _context.PizzaTable.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        



        [HttpGet]
        public IActionResult Edit(int id)
        {
            var pizza = _context.PizzaTable.Find(id);
            if (pizza == null)
            {
                return NotFound();
            }
            return View(pizza);
        }

        [HttpPost]
        public IActionResult Edit(PizzaList model)
        {
            var currentPizza = _context.PizzaTable.Find(model.Id);
            if (currentPizza != null)
            {
                currentPizza.pizza_name = model.pizza_name;
                currentPizza.price = model.price;
                currentPizza.Description = model.Description; 

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var pizza = _context.PizzaTable.Find(id);
            if (pizza != null)
            {
                _context.PizzaTable.Remove(pizza);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return NotFound();
        }
    }
}
