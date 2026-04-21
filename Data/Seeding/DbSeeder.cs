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
            await SeedUsersAsync(userManager);
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

        private static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            // 1. Seed Admin
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

            // 2. Seed Regular User
            if (await userManager.FindByEmailAsync("user@marketly.com") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "user@marketly.com",
                    Email = "user@marketly.com",
                    FirstName = "John",
                    LastName = "Doe",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "User123!");
                await userManager.AddToRoleAsync(user, "User");
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
            var user = await userManager.FindByEmailAsync("user@marketly.com");

            var ads = new List<Ad>
            {
                // Electronics
                new Ad {
                    Title = "iPhone 15 Pro Max",
                    Description = "Brand new condition, 256GB, Titanium Blue. Includes original box.",
                    Price = 1199.00m,
                    CategoryId = categories.First(c => c.Name == "Electronics").Id,
                    SellerId = admin.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-5),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1696446701796-da61225697cc" } }
                },
                new Ad {
                    Title = "MacBook Air M2",
                    Description = "8GB RAM, 256GB SSD. Perfect for students and light work. Like new.",
                    Price = 950.00m,
                    CategoryId = categories.First(c => c.Name == "Electronics").Id,
                    SellerId = user.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-4),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1611186871348-b1ec696e52c9" } }
                },
                // Vehicles
                new Ad {
                    Title = "BMW 3 Series (2018)",
                    Description = "Diesel, Automatic, 85k miles. Full service history, leather interior.",
                    Price = 18500.00m,
                    CategoryId = categories.First(c => c.Name == "Vehicles").Id,
                    SellerId = admin.Id,
                    CreatedOn = DateTime.UtcNow.AddHours(-12),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1555215695-3004980ad54e" } }
                },
                new Ad {
                    Title = "Electric Scooter - Xiaomi",
                    Description = "M365 Pro model. Top speed 25km/h, range 45km. Great city commuter.",
                    Price = 300.00m,
                    CategoryId = categories.First(c => c.Name == "Vehicles").Id,
                    SellerId = user.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1597075095302-60cc767b4587" } }
                },
                // Fashion
                new Ad {
                    Title = "Vintage Leather Jacket",
                    Description = "Genuine brown leather, size L. Great condition with a nice patina.",
                    Price = 120.00m,
                    CategoryId = categories.First(c => c.Name == "Fashion").Id,
                    SellerId = admin.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-2),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1551028719-00167b16eac5" } }
                },
                new Ad {
                    Title = "Nike Air Jordan 1",
                    Description = "Chicago colorway, size 42. Limited edition, never worn.",
                    Price = 450.00m,
                    CategoryId = categories.First(c => c.Name == "Fashion").Id,
                    SellerId = user.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-6),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1552346154-21d32810aba3" } }
                },
                // Home & Garden
                new Ad {
                    Title = "Coffee Table - Solid Oak",
                    Description = "Handmade solid oak coffee table. Minimalist design.",
                    Price = 240.00m,
                    CategoryId = categories.First(c => c.Name == "Home & Garden").Id,
                    SellerId = admin.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-1),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1533090161767-e6ffed986c88" } }
                },
                // Sports & Hobby
                new Ad {
                    Title = "Mountain Bike - Specialized",
                    Description = "Excellent for trails. 29-inch wheels, hydraulic brakes.",
                    Price = 850.00m,
                    CategoryId = categories.First(c => c.Name == "Sports & Hobby").Id,
                    SellerId = admin.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-3),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1532298229144-0ec0c57515c7" } }
                },
                new Ad {
                    Title = "Acoustic Guitar - Yamaha",
                    Description = "Perfect for beginners. Includes gig bag and tuner.",
                    Price = 150.00m,
                    CategoryId = categories.First(c => c.Name == "Books & Music").Id,
                    SellerId = user.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-10),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1510915361894-db8b60106cb1" } }
                },
                // Pets
                new Ad {
                    Title = "Golden Retriever Puppies",
                    Description = "8 weeks old, vaccinated and microchipped. Very friendly.",
                    Price = 1500.00m,
                    CategoryId = categories.First(c => c.Name == "Pets").Id,
                    SellerId = admin.Id,
                    CreatedOn = DateTime.UtcNow.AddHours(-2),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1552053831-71594a27632d" } }
                },
                // Real Estate
                new Ad {
                    Title = "Modern 1-Bedroom Studio",
                    Description = "Located in city center. Fully furnished, modern kitchen.",
                    Price = 120000.00m,
                    CategoryId = categories.First(c => c.Name == "Real Estate").Id,
                    SellerId = admin.Id,
                    CreatedOn = DateTime.UtcNow.AddDays(-15),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267" } }
                },
                // Toys & Games
                new Ad {
                    Title = "LEGO Star Wars Millennium Falcon",
                    Description = "Collector Series, 7500+ pieces. Sealed box.",
                    Price = 700.00m,
                    CategoryId = categories.First(c => c.Name == "Toys & Games").Id,
                    SellerId = user.Id,
                    CreatedOn = DateTime.UtcNow.AddHours(-5),
                    Images = new List<Image> { new Image { Url = "https://images.unsplash.com/photo-1585366119957-e9730b6d0f60" } }
                }
            };

            await context.Ads.AddRangeAsync(ads);
            await context.SaveChangesAsync();
        }
    }
}