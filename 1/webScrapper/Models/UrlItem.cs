
namespace WebScrapper.Models
{
    public class UrlItem
    {
        public long Id { get; set; }
        public string Url { get; set; }
        public int DivCounter { get; set; }
        public int ACounter { get; set; }
        public int SpanCounter { get; set; }
    }
}
