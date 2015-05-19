using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StudentDATAWeb.Models;
using WebMatrix.WebData;


namespace StudentDATAWeb.Controllers
{
    public class ProfileController : Controller
    {
        private UserProfile user;
        //private UsersContext db;

        public ProfileController()
        {
            ViewBag.UserName = null;
            ViewBag.UserSemester = null;
            ViewBag.UserField = null;
            ViewBag.UserActivity = null;
            ViewBag.MailAdress = null;
        }

        public ActionResult ViewProfile(UsersContext db)
        {
            if (WebSecurity.IsAuthenticated)
            {
                user = db.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));
                List<string> tmpCodeList = CodeCutter(user.Code);
                ViewBag.UserPseudo = user.UserName;
                ViewBag.UserName = user.FirstName + " " + user.LastName;
                ViewBag.UserSemester = tmpCodeList[0];
                ViewBag.UserField = tmpCodeList[1];
                ViewBag.UserActivity = user.ActualActivity;
                ViewBag.MailAdress = user.MailAdress;
                //Add here the Url for photo
                ViewBag.Modifying = false;
            }
            return View("/Views/Account/Profile.cshtml");
        }
        //TODO : Integrte parser for the semester code 
        /// <summary>
        /// The code cutter gets the code registered in the database and pick the semester and the studyfield up
        /// [0] = semester, [1] = StudyField
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<string> CodeCutter(string code)
        {
            List<string> tmpList = new List<string>();
            bool isCommon = true;

            string tmpString = code.Substring(1, 2);
            
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
            if (code.Contains("IL") && !isCommon)
                tmpList.Add("IL");
            else if (code.Contains("SR") && !isCommon)
                tmpList.Add("SR");
            else
                tmpList.Add("Tronc Commun");

            return tmpList;
        }

        public string CodeCreator(List<string> ls)
        {
            if (ls[1] == "Common")
                ls[1] = "";
            return "S" + ls[0].ToString() + ls[1].ToString();
        }
        public ActionResult ChangeFormProfile(UsersContext db)
        {
            user = db.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.Modifying = true;
            ViewBag.UserPseudo = user.UserName;
            ViewBag.FirstName = user.FirstName;
            ViewBag.LastName = user.LastName;
            ViewBag.UserSemester = CodeCutter(user.Code)[0];
            ViewBag.UserField = CodeCutter(user.Code)[1];
            ViewBag.UserActivity = user.ActualActivity;
            ViewBag.MailAdress = user.MailAdress;

            List<SelectListItem> studyField = new List<SelectListItem>();
            studyField.Add(new SelectListItem { Text = "IL", Value = "IL" });
            studyField.Add(new SelectListItem { Text = "SR", Value = "SR" });
            studyField.Add(new SelectListItem { Text = "Tronc Commun", Value = "Common" });
            studyField.Add(new SelectListItem { Text = "Equipe pédagogique", Value = "pedago" });

            List<SelectListItem> semester = new List<SelectListItem>();
            semester.Add(new SelectListItem { Text = "01", Value = "01" });
            semester.Add(new SelectListItem { Text = "02", Value = "02" });
            semester.Add(new SelectListItem { Text = "03", Value = "03" });
            semester.Add(new SelectListItem { Text = "04", Value = "04" });
            semester.Add(new SelectListItem { Text = "05", Value = "05" });
            semester.Add(new SelectListItem { Text = "06", Value = "06" });
            semester.Add(new SelectListItem { Text = "07", Value = "07" });
            semester.Add(new SelectListItem { Text = "08", Value = "08" });
            semester.Add(new SelectListItem { Text = "09", Value = "09" });
            semester.Add(new SelectListItem { Text = "10", Value = "10" });

            ViewBag.FieldList = studyField;
            ViewBag.SemesterList = semester;

            return View("/Views/Account/Profile.cshtml");
        }
        [HttpPost]
        public ActionResult ChangeProfile(ProfileModel pm, UsersContext db, string FieldList, string SemesterList)
        {
            try
            {
                user = db.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));

                if (pm.FirstName != null)
                    user.FirstName = pm.FirstName;
                if (pm.LastName != null)
                    user.LastName = pm.LastName;
                if (pm.MailAdress != null)
                    user.MailAdress = pm.MailAdress;
                if (pm.ActualActivity != null)
                    user.ActualActivity = pm.ActualActivity;
                if (SemesterList != "" && FieldList != "")
                    user.Code = CodeCreator(new List<string>() { SemesterList.ToString(), FieldList.ToString() });
                else if (SemesterList == "" && FieldList != "")
                    user.Code = CodeCreator(new List<string>() { CodeCutter(user.Code)[0], FieldList.ToString() });
                else if (SemesterList != "" && FieldList == "")
                    user.Code = CodeCreator(new List<string>() { SemesterList, CodeCutter(user.Code)[1] });
                else
                    user.Code = CodeCreator(CodeCutter(user.Code));
                    

                db.Entry(user).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                ViewBag.Modifying = false;
                return RedirectToAction("ViewProfile", pm);
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return View();
            }
        }

    }
}
