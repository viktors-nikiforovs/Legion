using System.Collections;

namespace LegionWebApp.Models
{
    public class Post
    {
        public GalleryItem[] Items { get; set; }
        public Post(GalleryItem[] items)
        {
            Items = items;
        }   
    }



    public class GalleryItem
    {
        public GalleryItem()
		{
            ImgSize = 4;
		}
        public string Title { get; set; }
        public int ImgSize { get; set; }
        public Image[] Image { get; set; }
        public Video[] Video { get; set; }

    }
    public class Image
	{
        public Image(string link)
		{
            this.Link = link;
        }
        public string Link { get; set; }
    }
    public class Video
    {
        public Video(string link, int size = 4, string poster = null)
		{
			this.Link = link;
			this.Poster = poster;
            this.Size = size;
		}
		public string Link { get; set; }
        public string Poster { get; set; }
        public int Size { get; set; }
    }
}
