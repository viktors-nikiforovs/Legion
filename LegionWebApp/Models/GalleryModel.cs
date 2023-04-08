using LegionWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegionWebApp.Models
{
    public class GalleryModel
    {
        public List<GalleryItem> ItemList { get; set; }

        public GalleryModel(ApplicationDbContext dbContext)
        {
            ItemList = dbContext.GalleryItems
                .Include(gi => gi.Media)
				.Where(gi => gi.Visible)
				.OrderByDescending(gi => gi.Id) // Sort GalleryItems by Id
                .ToList();

            // Sort the Media items in each GalleryItem by Id
            foreach (var galleryItem in ItemList)
            {
                galleryItem.Media = galleryItem.Media
                    .OrderBy(media => media.Id) // Sort Media by Id
                    .ToList();
            }
        }
    }

    public class GalleryItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
		//public DateOnly Date { get; set; }
		public string Date { get; set; }
        public bool Visible { get; set; }

		public ICollection<Media> Media { get; set; }
    }

    public abstract class Media
    {
        [Key]
        public int Id { get; set; }
        public int GalleryItemId { get; set; }

        [ForeignKey("GalleryItemId")]
        public GalleryItem GalleryItem { get; set; }
        public string Link { get; set; }
    }

    public class Image : Media
    {
        // No additional properties needed for the Image class
    }

    public class Video : Media
    {
        public string? Poster { get; set; }
    }
}
