namespace LegionWebApp.Models
{
    public class GalleryItem
    {
        public GalleryItem()
        {
            Video = new List<Video>();
        }
        public string Title { get; set; }
        public int Column { get; set; }
        public string[] Image { get; set; }
        public List<Video> Video { get; set; }
        
        public string Poster { get; set; }
        public string Text { get; set; }
    }
    public class Video
    {
        public Video(string link, string poster = null)
        {
            this.Link = link;
            this.Poster = poster;
        }
        public string Link { get; set; }
        public string Poster { get; set; }
    }
}
