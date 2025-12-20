using Microsoft.AspNetCore.Identity;

namespace GestionProductos.Data;

public static class DbInitializer
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await CreateUserIfNotExists(userManager, "admin", "admin@example.com", "Admin123!");
    }

    private static async Task CreateUserIfNotExists(UserManager<ApplicationUser> userManager,
        string username, string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, password);
        }
    }
}
