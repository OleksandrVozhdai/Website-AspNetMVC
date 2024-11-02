using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class Menu
    {
        [Key]
        public int MenuID { get; set; }

        public string DishName { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public List<Ingredients> Ingredients { get; set; }

        public void SendToCart() { /* ... */ }
        public void AddCustomIngredients() { /* ... */ }
        public void RefreshPrice() { /* ... */ }
    }
}
