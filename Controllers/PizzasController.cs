using LAB2_OOKP.Models;
using Microsoft.AspNetCore.Mvc;

namespace LAB2_OOKP.Controllers
{
    public class PizzasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PizzasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            var pizzas = _context.PizzaTable.ToList(); 
            return View(pizzas);
        }

        public ActionResult Order(int id)
        {
            var pizza = _context.PizzaTable.FirstOrDefault(p => p.Id == id);

            if (pizza == null)
            {
                return NotFound(); 
            }

            var orderModel = new Order
            {
                SelectedPizzaImage = $"{pizza.Id}.png",
                SelectedPizzaName = pizza.pizza_name,
                SelectedPizzaDescription = pizza.Description,
                SelectedPizzaPrice = pizza.price    
            };

            return View(orderModel);
        }
    }
}
