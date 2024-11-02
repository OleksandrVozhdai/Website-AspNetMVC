using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace LAB2_OOKP.Models
{
    public class Admin
    {
        [Key]
        public int AdminID { get; set; }

        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public void ManageMenu() { /* ... */ }
        public void ManagePersonnel() { /* ... */ }
        public void SystemManage() { /* ... */ }
    }
}
