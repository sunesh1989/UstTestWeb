using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace UstTestWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Queue()
        {
            return View();
        }
        public ActionResult RSSFeed()
        {


            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                try
                {
                     response = client.GetAsync("https://platform.boomi.com/account/trainingvishnurajnr-48CRQA/feed/category/alert/rss-2.0").Result;
                }
                catch(Exception e)
                {
                    throw e;
                    // TODO: Log
                }
            }
            ViewData["test"] = response;

            return View();
        }        
    }
}