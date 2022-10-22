namespace LegionWebApp.Models
{
    public class GalleryItem
    {
        public GalleryItem(string name)
        {
            Video = new List<Video>();
            Name = name;
        }
        public string Name { get; set; }
        public string Title { get; set; }
        public int Column { get; set; }
        public string[] Image { get; set; }
        public List<Video> Video { get; set; }
        public string Poster { get; set; }
        public bool HasText { get; set; }
        public bool SeperateVideo { get; set; }

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
        public int SeperateId { get; set; }
    }
}
