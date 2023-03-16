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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Image>().ToTable("Image");
            modelBuilder.Entity<Video>().ToTable("Video");
        }

    }
}
