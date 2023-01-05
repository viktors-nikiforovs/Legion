namespace LegionWebApp.Models
{

    public class Gallery
    {
        public List<GalleryItem> ItemList { get; set; }
        public Gallery()
        {
            ItemList = new List<GalleryItem>();
            CreateList();
        }


        private void CreateList()
        {
            CreateItem("On October 23, together with our partner CONROLL company, we are preparing to hand over shoe insoles for the servicemen of the Armed Forces to the Armed Forces.", true, new string[] { "1.jpeg" }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4" }, new DateOnly(2022, 10, 23));
            CreateItem("On October 21, charitable assistance was transferred to Kyiv City Clinical Hospital 6.", true, new string[] { "1.jpeg", "2.jpeg" }, new string[] { "3.mp4", "1.mp4", "2.mp4" }, new DateOnly(2022, 10, 21));
            CreateItem("On October 16, a large family of a military serviceman - Iskra, Izyum district. A military serviceman's family with many children", true, new string[] { "1.jpg", "2.jpg" }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4" }, new DateOnly(2022, 10, 16));
            CreateItem("From 14th to 16th October, a trip with humanitarian aid to the liberated territories in the Kharkiv region was organized together with partners - the Front Line Charitable Foundation. Food, clothing, household chemicals and hygiene products, medicines and toys for children were delivered to the needy.", true, new string[] { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg", "7.jpg", "8.jpg", "9.jpg" }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4" }, new DateOnly(2022, 10, 15), "-3");
            CreateItem("On October 15-2, Lysohirka, Izum Region.", false, new string[] { "1.jpg" }, new string[] { "4.mp4", "3.mp4", "2.mp4", "1.mp4" }, new DateOnly(2022, 10, 15), "-2");
            CreateItem("On October 15-1, Щасливе, Izum Region.", false, new string[] { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg" }, new string[] { "1.mp4", "2.mp4", "3.mp4" }, new DateOnly(2022, 10, 15), "-1");
            CreateItem("On October 5, charitable assistance was transferred to Kyiv City Clinical Hospital 6.", false, new string[] { "1.jpeg", "2.jpeg" }, null, new DateOnly(2022, 10, 05));
            CreateItem("On October 3, medicines, bandage materials and hygiene products were purchased and delivered to the State Main Military Medical Clinical Center for the Wounded.", false, new string[] { "1.jpeg", "2.jpeg" }, null, new DateOnly(2022, 10, 03));
            CreateItem("On October 1, IZUM of the Kharkiv region - (together with our partners - the Front Line Charitable Fund) delivered humanitarian aid to the residents of the city of IZUM, where 80% of the houses were destroyed. Food, medicines, clothes, household chemicals and hygiene products were handed over to residents.", true, new string[] { "01.jpeg", "02.jpeg", "03.jpeg", "04.jpeg", "05.jpeg", "06.jpeg", "07.jpeg", "08.jpeg", "09.jpeg", "10.jpeg", "11.jpeg", "12.jpeg", "13.jpeg", "14.jpeg", "15.jpeg", "16.jpeg" }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4" }, new DateOnly(2022, 10, 01));
            CreateItem("On September 19, Humaritarian aid has been dispached from Kyiv locals to Lypivka, Kyiv region.", true, new string[] { "01.jpg", "02.jpg", "03.jpg", }, null, new DateOnly(2022, 09, 19));
            CreateItem("On September 11, Humaritarian aid has been dispached from Kyiv locals to Andriivka, Kyiv region.", true, new string[] { "01.jpg", "02.jpg", }, null, new DateOnly(2022, 09, 11));
            CreateItem("10 August, Thank you letter from Main Military Medical Clinical Center.", true, new string[] { "01.jpg" }, null, new DateOnly(2022, 08, 10));
            CreateItem("On July 29, diapers and napkins for the care of the wounded were taken to the second hospital.", true, new string[] { "01.PNG", "02.PNG", "03.PNG" }, null, new DateOnly(2022, 07, 29));
            CreateItem("On July 28, 2022, they helped the hospital. Diapers and napkins were taken away - for the care of the wounded.", true, new string[] { "01.PNG", "02.PNG", "03.PNG", "04.PNG", "05.PNG" }, new string[] { "1.mp4" }, new DateOnly(2022, 07, 28));
            CreateItem("On June 26, together with our partner, the Charitable Foundation «Rear Line» we took humanitarian aid to the Borodyanka community, which was destroyed by the Russian army, and to the village of Andriyivka, Bucha district.", true, new string[] { "01.jpeg", "02.jpeg", "03.jpeg", "04.jpeg", "05.jpeg", "06.jpeg", "07.jpeg", "08.jpeg", "09.jpeg", "10.jpeg", "11.jpeg", "12.jpeg", "13.jpeg", "14.jpeg", "15.jpeg", "16.jpeg", "17.jpeg", "18.jpeg", "19.jpeg", "20.jpeg", "21.jpeg", "22.jpeg", "23.jpeg", "24.jpeg", "25.jpeg", "26.jpeg" }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4", "5.mp4", "6.mp4", "7.mp4", "8.mp4", }, new DateOnly(2022, 06, 26));
        }

        private void CreateItem(string Title, bool FullPage, string[]? ImageFiles, string[]? VideoFiles, DateOnly Date, string? multiDay = null)
        {
            GalleryItem item = new GalleryItem();
            item.Title = Title;
            item.FullPage = FullPage;
            if (ImageFiles != null)
            {
                item.Image = CreateImage(ImageFiles, Date, multiDay);
            }
            if (VideoFiles != null) { 
                item.Video = CreateVideo(VideoFiles, Date, multiDay); 
            }
            item.Date = Date;
            ItemList.Add(item);
        }

        private Video[] CreateVideo(string[] FileNames, DateOnly Date, string multiDay)
        {
            if (multiDay != null)
            {
                multiDay = multiDay.Replace("-", "+");
            }
            Video[] result = new Video[FileNames.Length];
            int i = 0;
            foreach (var video in FileNames)
            {
                result[i] = new Video($"https://legion-foundation.s3.eu-central-1.amazonaws.com/{Date.ToString("dd.MM.yy")}{multiDay}/{video}");
                i++;
            }
            return result;
        }
        private Image[] CreateImage(string[] FileNames, DateOnly Date, string multiDay = null)
        {
            Image[] result = new Image[FileNames.Length];
            int i = 0;
            foreach (var image in FileNames)
            {
                result[i] = new Image($"/images/Gallery/{Date.ToString("dd.MM.yy")}{multiDay}/{image}");
                i++;
            }
            return result;
        }
    }

    public class GalleryItem
    {
        public string Title { get; set; }
        public bool FullPage { get; set; }
        public Image[] Image { get; set; }
        public Video[] Video { get; set; }
        public DateOnly Date { get; set; }
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
        public Video(string link, string poster = null)
        {
            this.Link = link;
            this.Poster = poster;
        }
        public string Link { get; set; }
        public string Poster { get; set; }
    }
}
