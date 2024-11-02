namespace LAB2_OOKP.Models 
{
    public class PizzaList
    {
        public int Id { get; set; } 
        public string pizza_name { get; set; }
        public string Description { get; set; }
        public decimal price { get; set; }
        public DateTime? LastPurchase { get; set; }


        public ICollection<PizzaIngredients> PizzaIngredient { get; set; } = new List<PizzaIngredients>();
    }
}
