namespace tinyurlwebapi.Model
{
    public class Urls
    {

        public int Id { get; set; }
        public string OriginalUrl { get; set; }
        public string ShortCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ClickCount { get; set; } 
        public bool isPrivate { get; set; }





    }
}
