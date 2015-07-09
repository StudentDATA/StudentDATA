using StudentDATAWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace StudentDATAWeb.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            if (WebSecurity.IsAuthenticated)
                return RedirectToAction("Index", "Flow");
            
            else
			{
				RemoveCookies();
				return View();
			}
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your app description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
 
		public void RemoveCookies()
		{
			string[] myCookies = Request.Cookies.AllKeys;
			foreach (string cookie in myCookies)
			{
				Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
			}
		}
	}
}
