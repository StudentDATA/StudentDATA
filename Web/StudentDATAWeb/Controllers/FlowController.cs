using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSSFluxSD;

namespace StudentDATAWeb.Controllers
{
    public class FlowController : Controller
    {
        RSS rss;
        public ActionResult Index()
        {
            rss = new RSS("https://fr.news.yahoo.com/?format=rss",Helper.FormatRSSEnum.RSS20);
            List<List<string>> ll = new List<List<string>>();
            ll.Add(new List<string>() { "hello", "hello", "url" });
            ll.Add(new List<string>() { "hello1", "hello1", "url1" });
            ll.Add(new List<string>() { "hello2", "hello2", "url2" });
            ll.Add(new List<string>() { "hello2", "hello2", "url2" });
            ViewBag.FlowList = ll;
            ViewBag.IsWriter = true;
            return View("/Views/SchoolFlow/Flow.cshtml");
        }
        public ActionResult AddPost()
        {
            return View("/Views/SchoolFlow/AddNewFlowPost.cshtml");
        }
        [HttpPost]
        public ActionResult AddNewPost()
        {
            object title;
            ViewData.TryGetValue("PostTitle", out title);
            string titre = (string)title;
            Console.Write(titre);
            return RedirectToAction("Index");
        }


    }
}
