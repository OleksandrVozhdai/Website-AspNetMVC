using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;

namespace LAB2_OOKP.Controllers
{
    public class IngredientsController : Controller
    {
            private readonly ApplicationDbContext _context;

            public IngredientsController(ApplicationDbContext context)
            {
                _context = context;
            }

            public IActionResult Index(string filterId, string filterName, string QuantityFiler)
            {
                var ingredientsList = _context.IngredientsTable.AsQueryable();

                if (!string.IsNullOrEmpty(filterId) && int.TryParse(filterId, out int id))
                {
                    ingredientsList = ingredientsList.Where(p => p.IngredientID == id);
                }

                if (!string.IsNullOrEmpty(filterName))
                {
                    ingredientsList = ingredientsList.Where(p => p.Name.Contains(filterName));
                }

                if (!string.IsNullOrEmpty(QuantityFiler))
                {
                    ingredientsList = ingredientsList.Where(p => p.Price.Contains(QuantityFiler));
                }

                ViewData["FilterId"] = filterId;
                ViewData["FilterName"] = filterName;
                ViewData["Quantity"] = QuantityFiler;

                return View(ingredientsList.ToList());
            }

            [HttpGet]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public IActionResult Create(Ingredients model)
            {
                if (ModelState.IsValid)
                {
                    _context.IngredientsTable.Add(model);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return View(model);
            }

            [HttpGet]
            public IActionResult Edit(int id)
            {
                var ingredients = _context.IngredientsTable.Find(id);
                if (ingredients == null)
                {
                    return NotFound();
                }
                return View(ingredients);
            }

        [HttpPost]
        public IActionResult Edit(Ingredients model)
        {
            var currentIngredient = _context.IngredientsTable.Find(model.IngredientID);
            if (currentIngredient != null)
            {
                currentIngredient.Name = model.Name;
                currentIngredient.Price = model.Price;
                currentIngredient.Quantity = model.Quantity;
                currentIngredient.Unit = model.Unit;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }


        [HttpPost]
            public IActionResult DeleteConfirmed(int id)
            {
                var ingredient = _context.IngredientsTable.Find(id);
                if (ingredient != null)
                {
                    _context.IngredientsTable.Remove(ingredient);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
        }
    
}
