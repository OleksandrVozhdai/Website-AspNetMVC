using Microsoft.AspNetCore.Mvc;

namespace LAB2_OOKP.Models
{
    public class Create
    {

        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
