//This logic will run once.
//We create/use this AFTER we set up identity AND do a migration for that
using MiniProject.Models;

using Microsoft.AspNetCore.Identity;
using MiniProject.Data;


namespace MiniProject.API;

//Im making this class static, I don't want anything to create a RolesInitializer object
public static class Seeder
{
    //This class will provide one static method to SeedRoles
    public static async Task SeedAdmin(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
        const string email = "admin@admin.com";
        const string password = "Admin1234";

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            if (!roleResult.Succeeded)
                throw new Exception($"Could not create Admin role: {string.Join("; ", roleResult.Errors.Select(e => e.Description))}");
        }



        if (await userManager.FindByEmailAsync(email) is null)
        {
            var admin = new User
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,

            };

            var result = await userManager.CreateAsync(admin, password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Seeding admin user failed: {errors}");
            }



            if (!await userManager.IsInRoleAsync(admin, "Admin"))
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }

        }
    }
    public static async Task SeedUser(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
        const string email = "user@user.com";
        const string password = "User1234";
        if (await userManager.FindByEmailAsync(email) is null)
        {
            var User = new User
            {
                Id = Guid.NewGuid(),
                UserName = email,
                Email = email,

            };

            var result = await userManager.CreateAsync(User, password);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new Exception($"Seeding User user failed: {errors}");
            }
        }
    }
}

//     public static async Task SeedProperty(IServiceProvider serviceProvider)
//     {


//         var db = serviceProvider.GetRequiredService<JazaContext>();
//         var userManager = serviceProvider.GetRequiredService<UserManager<User>>();


//         const string userEmail = "user@user.com";
//         var user = await userManager.FindByEmailAsync(userEmail);
//         if (user is null)
//         {
//             return;
//         }

//         if (!db.Property.Any())
//         {
//             var property = new Property(
//                 "USA",              // country
//                 "Florida",          // state
//                 "Miami",            // city
//                 "33101",            // zipCode
//                 "321 Pine St",      // streetAddress
//                 25.7617,            // latitude
//                 -80.1918,           // longitude
//                 "",                 // imageLink
//                 1000000m,           // startingPrice
//                 3,                  // bedrooms
//                 2.5m,               // bathrooms
//                 1,                  // garages
//                 1,                  // pools
//                 true,               // hasBasement
//                 user.Id             // ownerId
//                 );
//             property.Coordinates = new Point(0, 0) { SRID = 4326 };
//             db.Property.Add(property);
//             await db.SaveChangesAsync();
//         }
//     }
// }