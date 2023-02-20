namespace LegionWebApp.Models
{

    public class Gallery
    {
        public List<GalleryItem> ItemList { get; set; }
        public int i = 0;
        public Gallery()
        {
            ItemList = new List<GalleryItem>();
            CreateList();
        }

        private void CreateList()
        {
            CreateItem("On February 15, medicine for prisoners of war was sent to Kramatorsk", true, new string[] { "1.jpeg", "2.jpeg", "4.jpeg", "3.jpeg" }, new int[] { 4, 4, 4, 4 }, null, null, null, new DateOnly(2023, 02, 15));
            CreateItem("On Feb 7, 2023, delivery of medicines to the Vinnytsia hospital", true, new string[] { "1.jpeg", "2.jpeg", "3.jpeg", "4.jpeg", "5.jpeg", "6.jpeg", "7.jpeg", "8.jpeg" }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4 }, null, null, null, new DateOnly(2023, 02, 07));
            CreateItem("On January 5, 2023, in connection with the withdrawal, the \"Front Line\" charitable foundation sent 5 pallets of humanitarian aid: products, shoes, clothes and hygiene products to the Berislav village of the Kherson region, which was under occupation.", true, null, null, new string[] { "1.mp4" }, new int[] { 4 }, null, new DateOnly(2023, 01, 05));
            CreateItem("On October 23, together with our partner CONROLL company, we are preparing to hand over shoe insoles for the servicemen of the Armed Forces to the Armed Forces.", true, new string[] { "1.jpeg" }, new int[] { 4 }, new string[] { "1.mp4", "4.mp4", "3.mp4", "2.mp4" }, new int[] { 6, 6, 6, 2 }, null, new DateOnly(2022, 10, 23));
            CreateItem("On October 21, 2022, charitable assistance was transferred to Kyiv City Clinical Hospital 6.", true, new string[] { "2.jpeg", "1.jpeg" }, new int[] { 5, 5 }, new string[] { "3.mp4", "1.mp4", "2.mp4" }, new int[] { 4, 4, 4 }, null, new DateOnly(2022, 10, 21));
            CreateItem("On October 16, a large family of a military serviceman - Iskra, Izyum district. A military serviceman's family with many children.", true, new string[] { "2.jpg", "1.jpg" }, new int[] { 4, 3 }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4" }, new int[] { 2, 2, 2, 2 }, null, new DateOnly(2022, 10, 16));
            CreateItem("From 14th to 16th October, a trip with humanitarian aid to the liberated territories in the Kharkiv region was organized together with partners - the Front Line Charitable Foundation. Food, clothing, household chemicals and hygiene products, medicines and toys for children were delivered to the needy.", true, new string[] { "2.jpg", "1.jpg", "4.jpg", "5.jpg", "3.jpg", "6.jpg", "8.jpg", "9.jpg", "7.jpg" }, new int[] { 4, 4, 4, 4, 4, 4, 4, 4, 4 }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4" }, new int[] { 3, 3, 3, 3 }, null, new DateOnly(2022, 10, 15), "-3");
            CreateItem("On October 15, Lysohirka, Izum Region.", false, new string[] { "1.jpg" }, new int[] { 6 }, new string[] { "4.mp4", "3.mp4", "2.mp4", "1.mp4" }, new int[] { 12, 6, 6, 6 }, null, new DateOnly(2022, 10, 15), "-2");
            CreateItem("On October 15, Shchaslyve, Izum Region.", false, new string[] { "1.jpg", "2.jpg", "6.jpg", "4.jpg", "5.jpg", "3.jpg" }, new int[] { 6, 6, 6, 6, 6, 6 }, new string[] { "3.mp4", "2.mp4", "1.mp4" }, new int[] { 12, 6, 6 }, null, new DateOnly(2022, 10, 15), "-1");
            CreateItem("On October 5, 2022, charitable assistance was transferred to Kyiv City Clinical Hospital 6.", false, new string[] { "1.jpeg", "2.jpeg" }, new int[] { 6, 6 }, null, null, null, new DateOnly(2022, 10, 05));
            CreateItem("On October 3, 2022, medicines, bandage materials and hygiene products were purchased and delivered to the State Main Military Medical Clinical Center for the Wounded", false, new string[] { "1.jpeg", "2.jpeg" }, new int[] { 6, 6 }, null, null, null, new DateOnly(2022, 10, 03));
            CreateItem("On October 1, IZUM of the Kharkiv region - (together with our partners - the Front Line Charitable Fund) delivered humanitarian aid to the residents of the city of IZUM, where 80% of the houses were destroyed. Food, medicines, clothes, household chemicals and hygiene products were handed over to residents.", true, new string[] { "01.jpeg", "02.jpeg", "03.jpeg", "04.jpeg", "05.jpeg", "06.jpeg", "07.jpeg", "08.jpeg", "09.jpeg", "10.jpeg", "11.jpeg", "12.jpeg", "13.jpeg", "14.jpeg", "15.jpeg", "16.jpeg" }, new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, new string[] { "4.mp4", "2.mp4", "3.mp4", "1.mp4" }, new int[] { 3, 3, 3, 3 }, null, new DateOnly(2022, 10, 01));
            CreateItem("On September 19, Humaritarian aid has been dispached from Kyiv locals to Lypivka, Kyiv region.", true, new string[] { "01.jpg", "02.jpg", "03.jpg", }, new int[] { 4, 4, 4 }, null, null, null, new DateOnly(2022, 09, 19));
            CreateItem("On September 11, Humaritarian aid has been dispached from Kyiv locals to Andriivka, Kyiv region.", true, new string[] { "01.jpg", "02.jpg", }, new int[] { 4, 4, 4 }, null, null, null, new DateOnly(2022, 09, 11));
            CreateItem("10 August, Thank you letter from Main Military Medical Clinical Center.", true, new string[] { "01.jpg" }, new int[] { 6 }, null, null, null, new DateOnly(2022, 08, 10));
            CreateItem("On July 29, diapers and napkins for the care of the wounded were taken to the second hospital.", true, new string[] { "01.PNG", "02.PNG", "03.PNG" }, new int[] { 4, 4, 4 }, null, null, null, new DateOnly(2022, 07, 29));
            CreateItem("On July 28, 2022, they helped the hospital. Diapers and napkins were taken away - for the care of the wounded.", true, new string[] { "03.PNG", "06.jpg", "01.PNG", "04.PNG", "05.PNG", "02.PNG" }, new int[] { 4, 4, 3, 3, 3, 3 }, new string[] { "1.mp4" }, new int[] { 4 }, null, new DateOnly(2022, 07, 28));
            CreateItem("On June 26, together with our partner, the Charitable Foundation «Rear Line» we took humanitarian aid to the Borodyanka community, which was destroyed by the Russian army, and to the village of Andriyivka, Bucha district.", true, new string[] { "01.jpeg", "02.jpeg", "03.jpeg", "22.jpeg", "23.jpeg", "06.jpeg", "07.jpeg", "08.jpeg", "09.jpeg", "10.jpeg", "11.jpeg", "12.jpeg", "13.jpeg", "14.jpeg", "15.jpeg", "16.jpeg", "17.jpeg", "18.jpeg", "19.jpeg", "20.jpeg", "21.jpeg", "04.jpeg", "05.jpeg", "24.jpeg", "25.jpeg", "26.jpeg" }, new int[] { 4, 4, 4, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, new string[] { "1.mp4", "2.mp4", "3.mp4", "4.mp4", "5.mp4", "6.mp4", "7.mp4", "8.mp4", }, new int[] { 3, 3, 3, 3, 3, 3, 3, 3 }, null, new DateOnly(2022, 06, 26));
        }

        private void CreateItem(string Title, bool FullPage, string[]? ImageFiles, int[]? ImgSize, string[]? VideoFiles, int[]? VidSize, string[]? Poster, DateOnly Date, string? multiDay = null)
        {
            GalleryItem item = new()
            {
                Title = Title,
                FullPage = FullPage,
                Id = i

            };
            i++;
            if (ImageFiles != null)
            {
                item.Image = CreateImage(ImageFiles, ImgSize, Date, multiDay);
            }
            if (VideoFiles != null)
            {
                item.Video = CreateVideo(VideoFiles, VidSize, Poster, Date, multiDay);
            }
            item.Date = Date;
            ItemList.Add(item);
        }

        private static Video[] CreateVideo(string[] FileNames, int[]? Size, string[]? Poster, DateOnly Date, string? multiDay)
        {
            if (multiDay != null)
            {
                multiDay = multiDay.Replace("-", "+");
            }
            Video[] result = new Video[FileNames.Length];
            int i = 0;
            foreach (var video in FileNames)
            {
                result[i] = new Video($"https://legion-foundation.s3.eu-central-1.amazonaws.com/{Date:dd.MM.yy}{multiDay}/{video}", Size != null ? Size[i] : 3, Poster?[i]);
                i++;
            }
            return result;
        }
        private static Image[] CreateImage(string[] FileNames, int[]? Size, DateOnly Date, string? multiDay)
        {
            Image[] result = new Image[FileNames.Length];
            int i = 0;
            foreach (var image in FileNames)
            {
                result[i] = new Image($"/images/Gallery/{Date:dd.MM.yy}{multiDay}/{image}", Size != null ? Size[i] : 3);
                i++;
            }
            return result;
        }
    }

    public class GalleryItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool FullPage { get; set; }
        public Image[] Image { get; set; }
        public Video[] Video { get; set; }
        public DateOnly Date { get; set; }
    }

    public class Image
    {
        public Image(string link, int size)
        {
            Link = link;
            Size = size;
        }
        public string Link { get; set; }
        public int Size { get; set; }
    }
    public class Video
    {
        public Video(string link, int size, string? poster)
        {
            this.Link = link;
            this.Poster = poster;
            this.Size = size;
        }
        public string Link { get; set; }
        public string? Poster { get; set; }
        public int Size { get; set; }
    }
}
