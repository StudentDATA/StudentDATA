using CK.Calendar.Intech;
using CK.Core;
using StudentDATAWeb.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace StudentDATAWeb.Controllers
{
    public class EDTController : Controller
    {
        //
        // GET: /EDT/
        private StudentDATAWeb.Models.UserProfile user;

        IActivityMonitor _monitor;
        ActivityMonitorTextWriterClient _log;
        DateTime currentDay = DateTime.Today;
        string _dbPath;

        public ActionResult Index()
        {
            return View("/Views/EDT/DisplayEDT.cshtml");
        }

		public ActionResult Index2()
		{
			return View("/Views/EDT/DisplayEDT2.cshtml");
		}


        public ActionResult AddEvent()
        {

            return View("/Views/EDT/AddEventToEDT.cshtml");
        }
        public ActionResult AddEventToEDT(EventModel em)
        {
            string titre = em.Title;
            string[] organizer = {em.Teacher};
            string salle = em.Salle;
            CultureInfo frFR = new CultureInfo("fr-FR");

            DateTime beg = DateTime.ParseExact(em.Begin, "yyyy-MM-dd HH:mm", frFR);
            DateTime end = DateTime.ParseExact(em.End, "yyyy-MM-dd HH:mm", frFR);
            
            string heure = beg.TimeOfDay.Hours.ToString() + " : " + beg.TimeOfDay.Minutes.ToString();
                _monitor = new ActivityMonitor();
                _monitor.Output.BridgeTarget.HonorMonitorFilter = false;
                Action<string> _logAction = Log_To_File;
                _log = new ActivityMonitorTextWriterClient(_logAction);
                _monitor.Output.RegisterClients(_log);

            _dbPath = (System.Web.HttpContext.Current == null)
            ? System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CALENDAR/")
            : System.Web.HttpContext.Current.Server.MapPath("~/Content/CALENDAR/");

            CalendarManager m = new CalendarManager(_dbPath);
            m.Load(_monitor, "EventITI");

            m.AddData(titre, organizer, salle, beg.ToLocalTime(), end.ToLocalTime());

            //Sauvergarde du calendrier
            m.SaveData();
            
            return RedirectToAction("ViewPlanning");
        }

        public ActionResult DeleteEvent(FormCollection collection)
        {
            _monitor = new ActivityMonitor();
            _monitor.Output.BridgeTarget.HonorMonitorFilter = false;
            Action<string> _logAction = Log_To_File;
            _log = new ActivityMonitorTextWriterClient(_logAction);
            _monitor.Output.RegisterClients(_log);

            _dbPath = (System.Web.HttpContext.Current == null)
            ? System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CALENDAR/")
            : System.Web.HttpContext.Current.Server.MapPath("~/Content/CALENDAR/");

            CalendarManager m = new CalendarManager(_dbPath);
            m.Load(_monitor, "EventITI");
            
           
            string ev = collection["DeleteEvent"];
            try
            {
                foreach (var e in m.Planning.Events)
                {
                    if (e.Code == ev)
                    {
                        m.RemoveData(e);
                    }
                }
            }
            catch
            {
                return RedirectToAction("ViewPlanning");
            }
            // m.RemoveData(popo);
            m.SaveData();
            return RedirectToAction("ViewPlanning");
        }

        public ActionResult ViewPlanning(UsersContext db)
        {
            var day = TempData["Day"];
            if(day != null)
            {
                ViewBag.currentday = day;
            }
            else
            {
                ViewBag.currentday = currentDay;
            }

            if (WebSecurity.IsAuthenticated)
            {
                user = db.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));
                List<string> tmpCodeList = CodeCutter(user.Code);
                ViewBag.UserPseudo = user.UserName;
                ViewBag.UserName = user.FirstName + " " + user.LastName;
                if (tmpCodeList != null)
                {
                    ViewBag.UserSemester = tmpCodeList[0];
                    ViewBag.UserField = tmpCodeList[1];
                }

                _monitor = new ActivityMonitor();
                _monitor.Output.BridgeTarget.HonorMonitorFilter = false;
                Action<string> _logAction = Log_To_File;
                _log = new ActivityMonitorTextWriterClient(_logAction);
                _monitor.Output.RegisterClients(_log);

                _dbPath = (System.Web.HttpContext.Current == null)
               ? System.Web.Hosting.HostingEnvironment.MapPath("~/Content/CALENDAR/")
               : System.Web.HttpContext.Current.Server.MapPath("~/Content/CALENDAR/");

                string semester = "S"+ViewBag.UserSemester+ViewBag.UserField;
				if ( ViewBag.UserField != null )
				{
					if (ViewBag.UserField == "IL" || ViewBag.UserField == "SR")
					{
						semester += "-" + ViewBag.UserField;
					}
				}
                string eventPlanning = "EventITI";
                CalendarManager m = new CalendarManager(_dbPath);
                CalendarManager m2 = new CalendarManager(_dbPath);
                m.Load(_monitor, semester);
                m2.Load(_monitor, eventPlanning);
               
				if ( m.Planning != null )
				{
					ViewBag.Planning = m.Planning.Events;
					ViewBag.PlanningEventAll = m2.Planning.Events;
				}

				if (user.Code == "Nothing" || user.Code == "S" || user.Code == String.Empty || user.FirstName == String.Empty || user.LastName == String.Empty || user.MailAdress == String.Empty || user.ActualActivity == String.Empty) 
					ViewBag.NoProfil = true;
				else ViewBag.NoProfil = false;

				if (user.Code.Contains("S00")) ViewBag.NoSemester = true;
				else ViewBag.NoSemester = false;


               /* if (ViewBag.UserField == "IL")
                {
                    var planningIL = m.Planning.EventsIL;
                    ViewBag.Planning = planningIL;
                    ViewBag.PlanningEventAll = m2.Planning.Events;
                }
                if (ViewBag.UserField == "SR")
                {
                    var planningSR = m.Planning.EventsSR;
                    ViewBag.Planning = planningSR;
                    ViewBag.PlanningEventAll = m2.Planning.Events;

                }
                if (ViewBag.UserField != "IL" && ViewBag.UserField != "SR")
                {
                    var planningDATE = m.Planning.EventsByDate;
                    ViewBag.Planning = planningDATE;
                    ViewBag.PlanningEventAll = m2.Planning.Events;
                }*/
            }

                return View("/Views/EDT/DisplayEDT.cshtml");
        }

        public ActionResult GetDate(UsersContext db)
        {
            DateTime today = DateTime.Today;
            ViewBag.today = today;
            TempData["Day"] = today;
            return RedirectToAction("ViewPlanning");
        }

        public ActionResult NextDayPlanning(FormCollection collection)
        {
            string currentDayHidden = collection["CurrentDayHidden"];
            DateTime currentDayHiddenConvert = Convert.ToDateTime(currentDayHidden);
            if(currentDayHiddenConvert.Month == 7 && currentDayHiddenConvert.Day == 10)
            {
                return RedirectToAction("ViewPlanning");
            }
            if (currentDayHiddenConvert.DayOfWeek == DayOfWeek.Friday)
            {
                currentDayHiddenConvert = currentDayHiddenConvert.AddDays(3);
            }
            else
            {
                currentDayHiddenConvert = currentDayHiddenConvert.AddDays(1);
            }
            TempData["Day"] = currentDayHiddenConvert;
            return RedirectToAction("ViewPlanning");
        }

        public ActionResult PreviousDayPlanning(FormCollection collection)
        {
            string currentDayHidden = collection["CurrentDayHidden"];
            DateTime currentDayHiddenConvert = Convert.ToDateTime(currentDayHidden);
            if(currentDayHiddenConvert.DayOfWeek == DayOfWeek.Monday)
            {
                currentDayHiddenConvert = currentDayHiddenConvert.AddDays(-3);
            }
            else
            {
                currentDayHiddenConvert = currentDayHiddenConvert.AddDays(-1);
            }
            TempData["Day"] = currentDayHiddenConvert;
            return RedirectToAction("ViewPlanning");
        }

         void Log_To_File(string msg)
        {
            DateTimeOffset _date = DateTimeOffset.Now;
            string _nameFile = _date.Day.ToString() + "_" + _date.Month.ToString() + "_" + _date.Year.ToString() + " Calendar.log";
            msg = Environment.NewLine + _date.ToString() + Environment.NewLine + msg;
            var _pathVar = Path.Combine(_dbPath, "Logs");
            if (!Directory.Exists(_pathVar)) Directory.CreateDirectory(_pathVar);
            System.IO.File.AppendAllText(Path.Combine(_pathVar, _nameFile), msg);
        }

           public List<string> CodeCutter(string code)
        {
			if (code != null && code != "Nothing" && code != String.Empty && code != "S")
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
            else return new List<string>(){"",""};

        }

        public string CodeCreator(List<string> ls)
        {
            if (ls != null)
            {
                if (ls[1] == "Common")
                    ls[1] = "TC";
                else if (ls[1] == "pedago")
                    ls[1] = "PO";

                return "S" + ls[0].ToString() + ls[1].ToString();
            }
            else
                return null;
        }

    }
}
