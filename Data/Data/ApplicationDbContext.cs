using Marketly.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Marketly.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Ad> Ads { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;
        public DbSet<Message> Messages { get; set; } = null!;
        public DbSet<UserAd> UsersAds { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure UserAd (Many-to-Many / Favorites)
            builder.Entity<UserAd>(entity =>
            {
                entity.HasKey(ua => new { ua.UserId, ua.AdId });

                // Break the multiple cascade path by setting Restrict here
                entity.HasOne(ua => ua.User)
                    .WithMany()
                    .HasForeignKey(ua => ua.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(ua => ua.Ad)
                    .WithMany()
                    .HasForeignKey(ua => ua.AdId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Fix 900-byte warning by limiting string key length
                entity.Property(ua => ua.UserId)
                    .HasMaxLength(450);
            });

            // Precision for Price
            builder.Entity<Ad>()
                .Property(a => a.Price)
                .HasPrecision(18, 2);

            // Messages - Restrict delete to avoid cycles
            builder.Entity<Message>(entity =>
            {
                entity.HasOne(m => m.Ad)
                    .WithMany()
                    .HasForeignKey(m => m.AdId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(m => m.SenderId).HasMaxLength(450);
                entity.Property(m => m.ReceiverId).HasMaxLength(450);
            });

            // Ads -> Category relationship
            builder.Entity<Ad>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Ads)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}