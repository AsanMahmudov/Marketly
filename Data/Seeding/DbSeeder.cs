using Marketly.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Marketly.Web.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            await SeedRolesAsync(roleManager);
            await SeedAdminAsync(userManager);
            await SeedCategoriesAndAdsAsync(context, userManager);
        }

        private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Administrator"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrator"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }
        }
        private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@marketly.com") == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = "admin@marketly.com",
                    Email = "admin@marketly.com",
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Administrator");
            }
        }
        private static async Task SeedCategoriesAndAdsAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (context.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Vehicles" },
                new Category { Name = "Fashion" },
                new Category { Name = "Home & Garden" },
                new Category { Name = "Sports & Hobby" },
                new Category { Name = "Real Estate" },
                new Category { Name = "Services" },
                new Category { Name = "Toys & Games" },
                new Category { Name = "Books & Music" },
                new Category { Name = "Pets" }
            };

            await context.Categories.AddRangeAsync(categories);
            await context.SaveChangesAsync();

            var admin = await userManager.FindByEmailAsync("admin@marketly.com");
            if (admin == null) return;

            var ads = new List<Ad>
    {
        new Ad
        {
            Title = "iPhone 15 Pro Max",
            Description = "Brand new condition, 256GB, Titanium Blue. Includes original box and accessories.",
            Price = 1199.00m,
            CategoryId = categories.First(c => c.Name == "Electronics").Id,
            SellerId = admin.Id,
            CreatedOn = DateTime.UtcNow.AddDays(-5),
            Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1696446701796-da61225697cc" } }
        },
        new Ad
        {
            Title = "Mountain Bike - Specialized",
            Description = "Excellent for trails. 29-inch wheels, hydraulic brakes, recently serviced.",
            Price = 850.00m,
            CategoryId = categories.First(c => c.Name == "Sports & Hobby").Id,
            SellerId = admin.Id,
            CreatedOn = DateTime.UtcNow.AddDays(-3),
            Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1532298229144-0ec0c57515c7" } }
        },
        new Ad
        {
            Title = "Vintage Leather Jacket",
            Description = "Genuine brown leather, size L. Great condition with a nice patina.",
            Price = 120.00m,
            CategoryId = categories.First(c => c.Name == "Fashion").Id,
            SellerId = admin.Id,
            CreatedOn = DateTime.UtcNow.AddDays(-2),
            Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1551028719-00167b16eac5" } }
        },
        new Ad
        {
            Title = "Coffee Table - Solid Oak",
            Description = "Handmade solid oak coffee table. Minimalist design, fits any living room.",
            Price = 240.00m,
            CategoryId = categories.First(c => c.Name == "Home & Garden").Id,
            SellerId = admin.Id,
            CreatedOn = DateTime.UtcNow.AddDays(-1),
            Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1533090161767-e6ffed986c88" } }
        },
        new Ad
        {
            Title = "BMW 3 Series (2018)",
            Description = "Diesel, Automatic, 85k miles. Full service history, leather interior.",
            Price = 18500.00m,
            CategoryId = categories.First(c => c.Name == "Vehicles").Id,
            SellerId = admin.Id,
            CreatedOn = DateTime.UtcNow.AddHours(-12),
            Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1555215695-3004980ad54e" } }
        },
        new Ad
        {
            Title = "Golden Retriever Puppies",
            Description = "8 weeks old, vaccinated and microchipped. Looking for a loving home.",
            Price = 1500.00m,
            CategoryId = categories.First(c => c.Name == "Pets").Id,
            SellerId = admin.Id,
            CreatedOn = DateTime.UtcNow.AddHours(-2),
            Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1552053831-71594a27632d" } }
        }
    };

            await context.Ads.AddRangeAsync(ads);
            await context.SaveChangesAsync();
        }
    }
}