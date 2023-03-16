using LegionWebApp.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace LegionWebApp.Models
{
    public class GalleryModel
    {
        public List<GalleryItem> ItemList { get; set; }

        public GalleryModel(ApplicationDbContext dbContext)
        {
            ItemList = dbContext.GalleryItems.Include(gi => gi.Media).ToList();
        }
    }

    public class GalleryItem
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public DateOnly Date { get; set; }

        [ForeignKey("GalleryItemId")]
        public ICollection<Media> Media { get; set; }
    }

    public abstract class Media
    {
        [Key]
        public int Id { get; set; }
        public int GalleryItemId { get; set; }
    }

    public class Image : Media
    {
        public string Link { get; set; }
    }

    public class Video : Media
    {
        public string Link { get; set; }
        public string? Poster { get; set; }
    }
}
