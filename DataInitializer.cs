using EventsApp.Models;
using Microsoft.AspNetCore.Identity;

namespace EventsApp
{
    public static class DataInitializer
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Initialisation des rôles
            await EnsureRoleAsync(roleManager, "Organisateur");
            await EnsureRoleAsync(roleManager, "Participant");

            // Créer un utilisateur administrateur par défaut
            var defaultUser = new ApplicationUser { UserName = "admin@example.com", Email = "admin@example.com" };
            string defaultUserPassword = "Admin123!";
            await EnsureUserAsync(userManager, defaultUser, defaultUserPassword, "Organisateur");
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
        {
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private static async Task EnsureUserAsync(UserManager<ApplicationUser> userManager, ApplicationUser user, string password, string roleName)
        {
            var existingUser = await userManager.FindByNameAsync(user.UserName);
            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
                // Considérez loguer les erreurs si la création de l'utilisateur échoue
            }
            // Sinon, considérez la mise à jour de l'utilisateur existant si nécessaire
        }
    }
}
