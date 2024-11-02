using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public bool Status { get; set; }

        public void AcceptPayment() { /* ... */ }
        public void RefreshStatus() { /* ... */ }
    }
}
