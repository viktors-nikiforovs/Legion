using LegionWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LegionWebApp.Data
{
    public class ApplicationDbContext : DbContext
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
                .WithOne("GalleryItem")
                .HasForeignKey("GalleryItemId")
                .IsRequired();

            // Configure TPH for the Media class
            builder.Entity<Media>()
                .ToTable("Media")
                .HasDiscriminator<string>("MediaType")
                .HasValue<Image>("Image")
                .HasValue<Video>("Video");
        }


    }
}
