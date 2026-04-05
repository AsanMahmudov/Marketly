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

            builder.Entity<UserAd>()
                .HasKey(ua => new { ua.UserId, ua.AdId });

            builder.Entity<Message>()
                .HasOne(m => m.Ad)
                .WithMany()
                .HasForeignKey(m => m.AdId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Ad>()
                .HasOne(a => a.Category)
                .WithMany(c => c.Ads)
                .HasForeignKey(a => a.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}