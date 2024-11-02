namespace LAB2_OOKP.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public string PizzaName { get; set; }
        public decimal PizzaPrice { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
