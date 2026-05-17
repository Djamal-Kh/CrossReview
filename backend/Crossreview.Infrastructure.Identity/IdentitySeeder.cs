using Microsoft.AspNetCore.Identity;

namespace Crossreview.Infrastructure.Identity;

public static class IdentitySeeder
{
    public static async Task SeedAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        // 1. Роли
        if (!await roleManager.RoleExistsAsync("User"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("User"));

        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));

        // 2. Админ пользователь
        var adminEmail = "admin@system.local";

        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            admin = new AppUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "System",
                LastName = "Admin"
            };

            var result = await userManager.CreateAsync(admin, "Admin123!");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }
    }
}