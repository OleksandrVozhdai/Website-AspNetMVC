namespace LAB2_OOKP.Models
{
    public class Order
    {
        public string SelectedPizzaName { get; set; }
        public string SelectedPizzaImage { get; set; }
        public string SelectedPizzaDescription { get; set; }

        public decimal SelectedPizzaPrice { get; set; }


        public string ErrorMessage { get; set; }
    }

}
