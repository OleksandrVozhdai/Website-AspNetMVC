using Microsoft.AspNetCore.Identity;

namespace SiteMVC.Models
{
    public class RoleInitializer
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RoleInitializer(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            var adminUser = await _userManager.FindByEmailAsync("test@gmail.com");
            if (adminUser == null)
            {
                var user = new IdentityUser
                {
                    UserName = "Oleksandr",
                    Email = "test@gmail.com"
                };
                var result = await _userManager.CreateAsync(user, "1234");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }
    }
}
