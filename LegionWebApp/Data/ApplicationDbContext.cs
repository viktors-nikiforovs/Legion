using LegionWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LegionWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<GalleryItem>()
                .HasMany(gi => gi.Media)
                .WithOne()
                .HasForeignKey(m => m.GalleryItemId)
                .IsRequired();

            builder.Entity<Image>()
                .HasBaseType<Media>();

            builder.Entity<Video>()
                .HasBaseType<Media>();
        }
    }
}
