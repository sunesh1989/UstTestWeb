using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace UstTestWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Queue()
        {
            return View();
        }
        public async Task<ActionResult> RSSFeed()
        {
            var objRss = new Rss();

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                try
                {
                     var response  = await client.GetAsync("https://platform.boomi.com/account/trainingvishnurajnr-48CRQA/feed/category/alert/rss-2.0");

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        var buffer = Encoding.UTF8.GetBytes(content);
                        using (var stream = new MemoryStream(buffer))
                        {
                            var serializer = new XmlSerializer(typeof(Rss));
                            objRss = (Rss)serializer.Deserialize(stream);
                           
                        }
                      
                    }
                    else
                    {
                        
                    }
                }
                catch(Exception e)
                {
                    throw e;
                    // TODO: Log
                }
            }
           

            return View(objRss);
        }        
    }

    [XmlRoot(ElementName = "guid")]
    public class Guid
    {
        [XmlAttribute(AttributeName = "isPermaLink")]
        public string IsPermaLink { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    [XmlRoot(ElementName = "item")]
    public class Item
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "category")]
        public List<string> Category { get; set; }
        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }
        [XmlElement(ElementName = "guid")]
        public Guid Guid { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Date { get; set; }
    }

    [XmlRoot(ElementName = "channel")]
    public class Channel
    {
        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        [XmlElement(ElementName = "link")]
        public string Link { get; set; }
        [XmlElement(ElementName = "description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "pubDate")]
        public string PubDate { get; set; }
        [XmlElement(ElementName = "creator", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Creator { get; set; }
        [XmlElement(ElementName = "date", Namespace = "http://purl.org/dc/elements/1.1/")]
        public string Date { get; set; }
        [XmlElement(ElementName = "item")]
        public List<Item> Item { get; set; }
    }

    [XmlRoot(ElementName = "rss")]
    public class Rss
    {
        [XmlElement(ElementName = "channel")]
        public Channel Channel { get; set; }
        [XmlAttribute(AttributeName = "content", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Content { get; set; }
        [XmlAttribute(AttributeName = "rdf", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Rdf { get; set; }
        [XmlAttribute(AttributeName = "dc", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Dc { get; set; }
        [XmlAttribute(AttributeName = "taxo", Namespace = "http://www.w3.org/2000/xmlns/")]
        public string Taxo { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
    }


}