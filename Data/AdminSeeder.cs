using Microsoft.AspNetCore.Identity;

namespace Maqalaty.Data
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            var roleManager =
                services.GetRequiredService<RoleManager<IdentityRole>>();

            const string adminRole = "Admin";

            // Ensure the Admin role exists.
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(adminRole));

                if (!result.Succeeded)
                {
                    var errors = string.Join(
                        ", ",
                        result.Errors.Select(e => e.Description));

                    throw new InvalidOperationException(
                        $"Failed to create Admin role: {errors}");
                }
            }

            // Admin account provisioning is disabled.
            // Existing Admin accounts remain unchanged.
        }
    }
}