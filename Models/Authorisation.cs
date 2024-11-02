using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class Authorization
    {
        [Key]
        public int AuthorizationID { get; set; }

        public string Login { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public void Authorize() { /* ... */ }
        public void DenyAccess() { /* ... */ }
        public void ChangePassword() { /* ... */ }
    }
}
