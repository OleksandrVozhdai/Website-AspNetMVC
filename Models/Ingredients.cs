using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class Ingredients
    {
        [Key]
        public int IngredientID { get; set; }

        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Unit { get; set; }
        public string Price { get; set; }

        public ICollection<PizzaIngredients> PizzaIngredient { get; set; } = new List<PizzaIngredients>();

        public void RefreshNumber() { /* ... */ }
        public void DeleteIngredient() { /* ... */ }
        public void AddIngredient() { /* ... */ }
    }
}
