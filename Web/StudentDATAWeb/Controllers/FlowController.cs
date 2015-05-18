using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentDATAWeb.Controllers
{
    public class FlowController : Controller
    {
        public ActionResult Index()
        {
            return View("/Views/SchoolFlow/Flow.cshtml");
        }

    }
}
