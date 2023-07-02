using LegionWebApp.Localization;
using LegionWebApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Image = LegionWebApp.Models.Image;

namespace LegionWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<GalleryItem> GalleryItems { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<LocalizationString> Localization { get; set; }

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
            builder.ApplyConfiguration(new ApplicationUserEntityConfiguration());
        }


    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(255);
            builder.Property(u => u.LastName).HasMaxLength(255);
        }
    }
}
