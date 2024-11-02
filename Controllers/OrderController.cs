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

            return RedirectToAction("Index", "Home");
        }


    }
}
