using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RSSFluxSD;
using StudentDATAWeb.Models;
using WebMatrix.WebData;
using System.IO;
using System.Data.Entity.Validation;

namespace StudentDATAWeb.Controllers
{
    public class FlowController : Controller
    {
        UserProfile profile;
        UsersContext db;
        RSSManage rssManager;
        public ActionResult Index(UsersContext db)
        {
            if (WebSecurity.IsAuthenticated)
            {
                this.db = db;
                profile = db.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));
                InitializeRSSFlowsDatas();
                List<List<string>> ll = new List<List<string>>();

                rssManager = new RSSManage();
                foreach (string adress in GetRSSByProfile())
                {
                    rssManager.readRSS(adress);
                }
                foreach (RSS rss in rssManager.GetAllRSS())
                {
                    foreach (Flow flow in rss.GetAllFlow())
                    {
                        ll.Add(new List<string>() { flow.Title.ToString(), flow.Content.ToString(), flow.Url.ToString(), flow.Date.ToString() });
                    }
                }
                ll.OrderByDescending(a => a[3]);
                ViewBag.FlowList = ll;
                // TODO : CHange isWriter set
                ViewBag.IsWriter = true;
                return View("/Views/SchoolFlow/Flow.cshtml");
            }
            else
                return View("/Views/Home/Index.cshtml");
        }
        /// <summary>
        /// Create the files on the server if not exists and register them in database
        /// </summary>
        public void InitializeRSSFlowsDatas()
        {
            for (int i = 0; i <= 10; i++)
            {
                string tmpname;
                if (i < 10)
                    tmpname = "S0" + i.ToString();
                else
                    tmpname = "S" + i.ToString();

                var result = db.RSSFlowsDatasList.Where(a => a.FlowName == tmpname);

                string toto = null;
                try
                {
                    if (!result.Any())
                    {
                        RSSFlowsDatas tmp = new RSSFlowsDatas();
                        if (i < 10)
                            tmp.FlowName = "S0" + i.ToString();
                        else
                            tmp.FlowName = "S" + i.ToString();
                        tmp.Adress = AppDomain.CurrentDomain.BaseDirectory + @"Content\RSSXML\" + tmp.FlowName + ".xml";
                        if (!System.IO.File.Exists(tmp.Adress))
                            System.IO.File.Create(tmp.Adress);
                        db.Entry(tmp).State = System.Data.Entity.EntityState.Added;
                        db.SaveChanges();
                    }
                }
                catch (DbEntityValidationException e)
                {

                    foreach (var eve in e.EntityValidationErrors)
                    {
                        toto += "Entity of type \"" + eve.Entry.Entity.GetType().Name + "\" in state \"" + eve.Entry.State + "\" has the following validation errors:";
                        foreach (var ve in eve.ValidationErrors)
                        {
                            toto += "- Property: \"" + ve.PropertyName + "\", Error: \"" + ve.ErrorMessage + "\"";
                        }
                    }
                    Console.WriteLine(toto);
                }
            }
            var result2 = db.RSSFlowsDatasList.Where(a => a.FlowName == "IL");
            if (!result2.Any())
            {
                RSSFlowsDatas tmp = new RSSFlowsDatas();
                tmp.FlowName = "IL";
                tmp.Adress = AppDomain.CurrentDomain.BaseDirectory + @"Content\RSSXML\" + tmp.FlowName + ".xml";
                if (!System.IO.File.Exists(tmp.Adress))
                    System.IO.File.Create(tmp.Adress);
                db.Entry(tmp).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            var result3 = db.RSSFlowsDatasList.Where(a => a.FlowName == "SR");
            if (!result3.Any())
            {
                RSSFlowsDatas tmp = new RSSFlowsDatas();
                tmp.FlowName = "SR";
                tmp.Adress = AppDomain.CurrentDomain.BaseDirectory + @"Content\RSSXML\" + tmp.FlowName + ".xml";
                if (!System.IO.File.Exists(tmp.Adress))
                    System.IO.File.Create(tmp.Adress);
                db.Entry(tmp).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            var result4 = db.RSSFlowsDatasList.Where(a => a.FlowName == "TC");
            if (!result4.Any())
            {
                RSSFlowsDatas tmp = new RSSFlowsDatas();
                tmp.FlowName = "TC";
                tmp.Adress = AppDomain.CurrentDomain.BaseDirectory + @"Content\RSSXML\" + tmp.FlowName + ".xml";
                if (!System.IO.File.Exists(tmp.Adress))
                    System.IO.File.Create(tmp.Adress);
                db.Entry(tmp).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }
            var result5 = db.RSSFlowsDatasList.Where(a => a.FlowName == "pedago");
            if (!result5.Any())
            {
                RSSFlowsDatas tmp = new RSSFlowsDatas();
                tmp.FlowName = "pedago";
                //TODO : Choose more precisely the path
                tmp.Adress = AppDomain.CurrentDomain.BaseDirectory + @"Content\RSSXML\" + tmp.FlowName + ".xml";
                if (!System.IO.File.Exists(tmp.Adress))
                    System.IO.File.Create(tmp.Adress);
                db.Entry(tmp).State = System.Data.Entity.EntityState.Added;
                db.SaveChanges();
            }

        }
        public List<string> GetRSSByProfile()
        {
            List<string> tmpList = CodeCutter(profile.Code);
            List<string> adressStack = new List<string>();
            #region BySemester
            if (tmpList[0] == "01")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S01"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);

            }
            else if (tmpList[0] == "02")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S02"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "03")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S03"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "04")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S04"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "05")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S05"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "06")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S06"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "07")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S07"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "08")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S08"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "09")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S09"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[0] == "10")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "S10"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else
                adressStack.Add("");

            #endregion
            #region ByField
            if (tmpList[1] == "IL")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "IL"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[1] == "SR")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "SR"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[1] == "Tronc Commun")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "TC"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else if (tmpList[1] == "pedago")
            {
                var elements = from element in db.RSSFlowsDatasList
                               where element.FlowName == "TC"
                               select element;

                adressStack.Add(elements.FirstOrDefault().Adress);
            }
            else
                adressStack.Add("");
            #endregion
            return adressStack;
        }
        public List<string> CodeCutter(string code)
        {
            if (code != null)
            {
                List<string> tmpList = new List<string>();
                bool isCommon = true;

                string tmpString = code.Substring(1, 2);
                try
                {
                    int tmpInteger = Convert.ToInt32(tmpString);
                    if (tmpInteger < 10)

                        if (tmpInteger < 10 && tmpInteger >= 3)
                        {
                            tmpList.Add("0" + tmpInteger.ToString());
                            isCommon = false;
                        }
                        else if (tmpInteger < 3)
                        {
                            tmpList.Add("0" + tmpInteger.ToString());
                            isCommon = true;
                        }
                        else if (tmpInteger == 0)
                        {
                            tmpList.Add("00");
                            isCommon = false;
                        }


                    if (code.Contains("IL") && !isCommon)
                        tmpList.Add("IL");
                    else if (code.Contains("SR") && !isCommon)
                        tmpList.Add("SR");
                    else if (code.Contains("TC") && !isCommon)
                        tmpList.Add("Tronc Commun");
                    else
                        tmpList.Add("pedago");
                    return tmpList;
                }
                catch (Exception e)
                {
                    tmpList.Add(e.ToString());
                    tmpList.Add(null);
                    return tmpList;
                }

            }
            else return new List<string>() { "", "" };

        }
        public ActionResult AddPost(FlowPostModel fpm)
        {
            List<string> rsslist = new List<string>();
            foreach (RSSFlowsDatas post in db.RSSFlowsDatasList)
                rsslist.Add(post.FlowName);
            ViewBag.AllRSSList = rsslist;
            return View("/Views/SchoolFlow/AddNewFlowPost.cshtml");
        }

        [HttpPost]
        public ActionResult AddNewPost(FlowPostModel fpm)
        {
            rssManager.addToXml
            return RedirectToAction("Index");
        }

    }
    public enum RSSDataID
    {
        NONE = 0,
        S1 = 1,
        S2 = 2,
        S3 = 3,
        S4 = 4,
        S5 = 5,
        S6 = 6,
        S7 = 7,
        S8 = 8,
        S9 = 9,
        S10 = 10,
        IL = 11,
        SR = 12,
        COMMON = 13,
        ADMIN = 14
    }
}
