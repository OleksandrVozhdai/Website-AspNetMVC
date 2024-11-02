using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Enter the old password.")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "Enter the new password.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm the new password.")]
        [Compare("NewPassword", ErrorMessage = "The new passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
