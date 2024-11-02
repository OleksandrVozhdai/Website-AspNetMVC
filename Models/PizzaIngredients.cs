using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class PizzaIngredients
    {
        [Key]
        public int PizzaID { get; set; }
        public PizzaList Pizza { get; set; }
        public string PizzaName { get; set; }
        [Key]
        public int IngredientID { get; set; }
        public Ingredients Ingredient { get; set; }

        public decimal Quantity { get; set; }
    }
}
