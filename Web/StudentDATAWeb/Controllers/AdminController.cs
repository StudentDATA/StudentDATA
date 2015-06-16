using StudentDATAWeb.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace StudentDATAWeb.Controllers
{
    public class AdminController : Controller
    {
        private UserProfile currentuser;
        private UserProfile user;

        private System.Data.Entity.DbSet<UserProfile> userlist;
        int i = 0;
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManageAccount(UsersContext db)
        {
            userlist = db.UserProfiles;
            List<UserProfile> tmpProfiles = new List<UserProfile>();
            int i = 0;
            while (tmpProfiles.Count < 10 && userlist.ToList().LastOrDefault() != userlist.Find(i))
            {
                if (userlist.Find(i) != null)
                {
                    tmpProfiles.Add(userlist.Find(i));
                }
                i++;
            }

            List<SelectListItem> permissionManage = new List<SelectListItem>();
            permissionManage.Add(new SelectListItem { Text = "Student", Value = "0" });
            permissionManage.Add(new SelectListItem { Text = "WriterStudent", Value = "1" });
            permissionManage.Add(new SelectListItem { Text = "Admin", Value = "2" });


            ViewBag.PermissionList = permissionManage;
            ViewBag.UserList = tmpProfiles;
            if (userlist.Count() % 10 > 0)
                ViewBag.PageQuant = Math.Truncate((double)userlist.Count() / 10) + 1;
            else
                ViewBag.PageQuant = Math.Truncate((double)userlist.Count() / 10);
            ViewBag.ActualPage = 1;
            return View("/Views/ManageAccount/ManageAllUser.cshtml");
        }



        /// <summary>
        /// The code cutter gets the code registered in the database and pick the semester and the studyfield up
        /// [0] = semester, [1] = StudyField
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public List<string> CodeCutter(string code)
        {
            if (code != null)
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
            else
                return null;


        }
        [HttpPost]
        public ActionResult ManageAccountRed(UsersContext db, FormCollection collection)
        {
            int index;
            if (collection.Count == 0)
            {
                index = 0;
            }
            else
            {
                index = Convert.ToInt32(collection["Index"]);
            }
            userlist = db.UserProfiles;
            List<UserProfile> tmpProfiles = new List<UserProfile>();
            int i = 10 * (index - 1);
            while (tmpProfiles.Count < 10
                && tmpProfiles.Count < userlist.Count() - (10 * (index - 1))
                && userlist.ToList().LastOrDefault() != userlist.Find(i))
            {
                if (i > 0)
                {

                    tmpProfiles.Add(userlist.ToList().Find(u => u != null));

                }

                i++;
            }

            List<SelectListItem> permissionManage = new List<SelectListItem>();
            permissionManage.Add(new SelectListItem { Text = "Student", Value = "0" });
            permissionManage.Add(new SelectListItem { Text = "WriterStudent", Value = "1" });
            permissionManage.Add(new SelectListItem { Text = "Admin", Value = "2" });


            ViewBag.PermissionList = permissionManage;
            ViewBag.UserList = tmpProfiles;
            if (userlist.Count() % 10 > 0)
                ViewBag.PageQuant = Math.Truncate((double)userlist.Count() / 10) + 1;
            else
                ViewBag.PageQuant = Math.Truncate((double)userlist.Count() / 10);
            ViewBag.ActualPage = 1;
            ViewBag.ActualPage = index;
            return View("/Views/ManageAccount/ManageAllUser.cshtml");
        }

        [HttpPost]
        public ActionResult ManageProfile(ProfileModel pm, UsersContext db, string PermissionList, FormCollection collection)
        {
            user = db.UserProfiles.Find(WebSecurity.GetUserId(User.Identity.Name));
            string userName = collection["UserName"];
            try
            {
                currentuser = db.UserProfiles.Find(WebSecurity.GetUserId(userName));

                //if (pm.FirstName != null)
                //    currentuser.FirstName = pm.FirstName;
                //if (pm.LastName != null)
                //    currentuser.LastName = pm.LastName;
                //if (pm.MailAdress != null)
                //    currentuser.MailAdress = pm.MailAdress;
                //if (pm.ActualActivity != null)
                //    currentuser.ActualActivity = pm.ActualActivity;
                if (PermissionList != "")
                {
                    if (PermissionList == "0")
                    {
                        currentuser.Permission = PermissionEnum.Student;
                    }
                    if (PermissionList == "1")
                    {
                        currentuser.Permission = PermissionEnum.WriterStudent;
                    }
                    if (PermissionList == "2")
                    {
                        currentuser.Permission = PermissionEnum.Admin;
                    }
                }

                db.Entry(currentuser).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Modifying = false;
                if (user.Permission == PermissionEnum.Admin)
                {
                    return RedirectToAction("ManageAccount", pm);
                }
                else return RedirectToAction("ViewProfile", "Profile");
            }
            catch (Exception e)
            {
                ViewBag.Error = e.ToString();
                return View();
            }

        }


    }
}
