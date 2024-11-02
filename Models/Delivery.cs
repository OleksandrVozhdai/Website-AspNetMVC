using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class Delivery
    {
        [Key]
        public string DeliveryID { get; set; }

        public string Address { get; set; }
        public DateTime Time { get; set; }
        public string Vehicle { get; set; }
        public bool Status { get; set; }

        public void SendOrder() { /* ... */ }
        public void RefreshStatus() { /* ... */ }
    }
}
