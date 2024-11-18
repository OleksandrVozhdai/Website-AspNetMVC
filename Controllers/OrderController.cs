using Microsoft.AspNetCore.Mvc;
using System.Linq;
using LAB2_OOKP.Models;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;


namespace LAB2_OOKP.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context; 

        public OrderController(ApplicationDbContext context)
        {
            _context = context; 
        }

        private void GenerateReport(string name, string surname, string selectedPizzaName, decimal selectedPizzaPrice)
        {
            string reportDate = DateTime.Today.ToString("yyyy-MM-dd");
            string reportPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "reports", $"report_{reportDate}.txt");
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath));

            var allIngredients = _context.IngredientsTable
                .Select(i => new { i.Name, i.Quantity })
                .ToList();

            var todayPurchases = _context.Purchases
                .Where(p => p.PurchaseDate.Date == DateTime.Today)
                .Select(p => new { p.Name, p.Surname, p.PizzaName, p.PizzaPrice, p.PurchaseDate })
                .ToList();

            var totalRevenue = _context.Purchases
                .Where(p => p.PurchaseDate.Date == DateTime.Today)
                .Sum(p => p.PizzaPrice);

            using (StreamWriter writer = new StreamWriter(reportPath, false))
            {
                writer.WriteLine($"Звіт");
                writer.WriteLine(new string('-', 40));
                writer.WriteLine($"Дата звіту: {DateTime.Today:yyyy-MM-dd}");

                foreach (var purchase in todayPurchases)
                {
                    writer.WriteLine($"{purchase.PurchaseDate:HH:mm:ss}: {purchase.Name} {purchase.Surname} купив {purchase.PizzaName} за {purchase.PizzaPrice:C}");
                }

                writer.WriteLine("\nЗагальні залишки інгредієнтів:");

                var usedIngredients = _context.PizzaIngredients
                    .Where(pi => todayPurchases.Select(p => p.PizzaName).Contains(pi.PizzaName))
                    .GroupBy(pi => pi.Ingredient.Name)
                    .Select(group => new
                    {
                        IngredientName = group.Key,
                        TotalUsedQuantity = group.Sum(pi => pi.Quantity) 
                    })
                    .ToList();

                foreach (var ingredient in allIngredients)
                {
                    var usedIngredient = usedIngredients.FirstOrDefault(u => u.IngredientName == ingredient.Name);
                    decimal remainingQuantity = usedIngredient == null ? Convert.ToDecimal(ingredient.Quantity) : Convert.ToDecimal(ingredient.Quantity) - usedIngredient.TotalUsedQuantity;
                    writer.WriteLine($"- {ingredient.Name}: {remainingQuantity}");
                }

                writer.WriteLine("\nПіц, зроблених за день:");
                foreach (var pizza in todayPurchases.GroupBy(p => p.PizzaName))
                {
                    writer.WriteLine($"- {pizza.Key} x{pizza.Count()}");
                }

                writer.WriteLine($"\nПрибуток за сьогодні: {totalRevenue:C}");
                writer.WriteLine(new string('-', 40));
            }
        }


        public IActionResult Index(string pizzaName, string pizzaImage, string pizzaDescription, decimal pizzaPrice)
        {
            var model = new Order
            {
                SelectedPizzaName = pizzaName,
                SelectedPizzaImage = pizzaImage,
                SelectedPizzaDescription = pizzaDescription,
                SelectedPizzaPrice = pizzaPrice
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult PlaceOrder(string name, string surname, string street, string house, string selectedPizzaName, decimal selectedPizzaPrice)
        {
          
            var pizzaIngredients = _context.PizzaIngredients
                .Where(pi => pi.PizzaName == selectedPizzaName)
                .ToList();

            foreach (var pizzaIngredient in pizzaIngredients)
            {
                var ingredientStock = _context.IngredientsTable
                    .FirstOrDefault(i => i.IngredientID == pizzaIngredient.IngredientID);

                if (ingredientStock == null ||
                    Convert.ToDecimal(ingredientStock.Quantity) < Convert.ToDecimal(pizzaIngredient.Quantity))
                {
                    var model = new Order
                    {
                        SelectedPizzaName = selectedPizzaName,
                        SelectedPizzaImage = "", 
                        SelectedPizzaDescription = "", 
                        SelectedPizzaPrice = selectedPizzaPrice,
                        ErrorMessage = "Sorry, pizza is not available due to insufficient ingredients."
                    };
                    return View("Index", model);
                }
            }

           

            var purchase = new Purchase
            {
                Name = name,
                Surname = surname,
                Street = street,
                House = house,
                PizzaName = selectedPizzaName,
                PizzaPrice = selectedPizzaPrice,
                PurchaseDate = DateTime.Now
            };

            _context.Purchases.Add(purchase);
            _context.SaveChanges();

        
            foreach (var pizzaIngredient in pizzaIngredients)
            {
                var ingredientStock = _context.IngredientsTable
                    .FirstOrDefault(i => i.IngredientID == pizzaIngredient.IngredientID);

                if (ingredientStock != null)
                {
                    decimal currentQuantity = Convert.ToDecimal(ingredientStock.Quantity);
                    decimal ingredientQuantity = Convert.ToDecimal(pizzaIngredient.Quantity);

                    if (currentQuantity >= ingredientQuantity)
                    {
                        ingredientStock.Quantity = (currentQuantity - ingredientQuantity).ToString();
                    }
                }
            }

            var pizza = _context.PizzaTable.FirstOrDefault(p => p.pizza_name == selectedPizzaName);
            if (pizza != null)
            {
              
                if (pizza.LastPurchase != default(DateTime)) 
                {
                    
                }
                else
                {
                   
                    pizza.LastPurchase = DateTime.Now; 
                }

                pizza.LastPurchase = DateTime.Now; 
                _context.PizzaTable.Update(pizza); 
            }


            _context.SaveChanges();

            GenerateReport(name, surname, selectedPizzaName, selectedPizzaPrice);

            return RedirectToAction("Index", "Home");
        }


    }
}
