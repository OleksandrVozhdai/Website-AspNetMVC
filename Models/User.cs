using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Age is required.")]
        public int Age { get; set; }
        public string Password { get; set; }

        public void MakeAnOrder() { /* ... */ }
        public void GetDiscount() { /* ... */ }
        public void LeaveFeedback() { /* ... */ }
    }
}
