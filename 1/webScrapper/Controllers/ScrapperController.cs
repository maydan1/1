using Microsoft.AspNetCore.Mvc;
using WebScrapper.Models;
using System.Linq;
using System.Net;
using MongoDB.Driver;
using HtmlAgilityPack;

namespace WebScrapper.Controllers
{
    [Route("api/[controller]")]
    public class ScrapperController : Controller
    {

        [HttpGet]
        public IActionResult GetAll()
        {
            return Content("HELLO! :)");
        }

        [HttpGet("howMany")]
        public IActionResult GetCounter()
        {
            var connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase("ScannedURL");
            MongoCollection collection = database.GetCollection<UrlItem>("URLScanningResults");

            return Content(collection.Count()+" URLS were scanned and saved in the MongoDB ");
        }
        

        [HttpPost]
        public IActionResult Create([FromBody] UrlItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            //count HTML tags
            using (WebClient client1 = new WebClient())
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc = web.Load(item.Url);
                item.DivCounter = doc.DocumentNode.Descendants("div").Count();
                item.ACounter = doc.DocumentNode.Descendants("a").Count();
                item.SpanCounter = doc.DocumentNode.Descendants("span").Count();
            }
            var connectionString = "mongodb://localhost:27017";
            MongoClient client = new MongoClient(connectionString);
            MongoServer server = client.GetServer();
            MongoDatabase database = server.GetDatabase("ScannedURL");
            MongoCollection collection = database.GetCollection<UrlItem>("URLScanningResults");

            item.Id = collection.Count() + 1;
            collection.Insert(item);
            string res ="Scanning number "+ item.Id +"\nURL: "+ item.Url
                +"\nNumber of div element: " + item.DivCounter + 
                "\nNumber of A element: "+item.ACounter+ "\nNumber of span element: " + item.SpanCounter;
            return Content(res);
        }


    }
}