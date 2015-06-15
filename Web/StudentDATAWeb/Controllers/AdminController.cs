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
        //int i = 0;
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ManageAccount(UsersContext db)
        {
            userlist = db.UserProfiles;
            //foreach (UserProfile user in userlist)
            //{

            //    List<string> tmpCodeList = CodeCutter(user.Code);
            //    ViewBag.UserPseudo = user.UserName;
            //    ViewBag.UserName = user.FirstName + " " + user.LastName;
            //    if (tmpCodeList != null)
            //    {
            //        ViewBag.UserSemester = tmpCodeList[0];
            //        ViewBag.UserField = tmpCodeList[1];
            //    }
            //    ViewBag.UserActivity = user.ActualActivity;
            //    ViewBag.MailAdress = user.MailAdress;
            //    //Add here the Url for photo
            //    ViewBag.Modifying = false;
            //    ViewBag.UserPermission = user.Permission;
            //    i++;
            //}
            //    ViewBag.UserlistLength = i;

                List<SelectListItem> studyFieldManage = new List<SelectListItem>();
                studyFieldManage.Add(new SelectListItem { Text = "IL", Value = "IL" });
                studyFieldManage.Add(new SelectListItem { Text = "SR", Value = "SR" });
                studyFieldManage.Add(new SelectListItem { Text = "Tronc Commun", Value = "Common" });

                List<SelectListItem> semesterManage = new List<SelectListItem>();
                semesterManage.Add(new SelectListItem { Text = "01", Value = "01" });
                semesterManage.Add(new SelectListItem { Text = "02", Value = "02" });
                semesterManage.Add(new SelectListItem { Text = "03", Value = "03" });
                semesterManage.Add(new SelectListItem { Text = "04", Value = "04" });
                semesterManage.Add(new SelectListItem { Text = "05", Value = "05" });
                semesterManage.Add(new SelectListItem { Text = "06", Value = "06" });
                semesterManage.Add(new SelectListItem { Text = "07", Value = "07" });
                semesterManage.Add(new SelectListItem { Text = "08", Value = "08" });
                semesterManage.Add(new SelectListItem { Text = "09", Value = "09" });
                semesterManage.Add(new SelectListItem { Text = "10", Value = "10" });

                List<SelectListItem> permissionManage = new List<SelectListItem>();
                permissionManage.Add(new SelectListItem { Text = "Student", Value = "0" });
                permissionManage.Add(new SelectListItem { Text = "WriterStudent", Value = "1" });
                permissionManage.Add(new SelectListItem { Text = "Admin", Value = "2" });


                ViewBag.FieldList = studyFieldManage;
                ViewBag.SemesterList = semesterManage;
                ViewBag.PermissionList = permissionManage;
                ViewBag.UserList = userlist;
                
            return View("/Views/ManageAccount/ManageAllUser.cshtml");
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
