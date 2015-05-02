using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TeamProject2.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public void populateDeptList()
        {
            var db = new DatabaseContext();
            var deptList = new List<string>();
            var deptQry = from Department in db.zDepartment select Department.DeptName;
            deptList.AddRange(deptQry.Distinct());
            ViewBag.deptList = deptList;
        }

        [HttpGet]
        public ActionResult Login()
        {
            populateDeptList();
            return View();
        }

        [HttpPost]
        public ActionResult Login (TeamProject2.Models.UserModel user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new DatabaseContext())
                {
                    foreach (var dept in db.zDepartment)
                    {
                        if (dept.DeptName == user.DeptName)
                        {
                            user.DeptCode = dept.DeptCode;
                        }
                    }
                }

                if (isValid(user.DeptCode, user.Password))
                {
                    FormsAuthentication.SetAuthCookie(user.DeptCode, false);
                    return RedirectToAction("Index", "Home");
                }
                else 
                {
                    ModelState.AddModelError("", "Login data is incorrect!");
                }
            }
            populateDeptList();
            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool isValid(string deptCode, string password)
        {
            bool isValid = false;
            using (var db = new DatabaseContext())
            {
                foreach(var user in db.zUser)
                {
                    if(user.DeptCode == deptCode && user.Password == password)
                    {
                        isValid = true;
                    }
                }
                /*var user = db.zUser.FirstOrDefault(u => u.DeptCode == deptCode);
                if (user != null)
                {
                    if(user.Password == password)
                    {
                        isValid = true;
                    }
                }*/
            }
            return isValid;
        }
    }
}
