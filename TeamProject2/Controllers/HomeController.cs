using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace TeamProject2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        
        [Authorize]
        public ActionResult Index()
        {
            //return View();
            //return RedirectToAction("Login", "User");
            return RedirectToAction("Index", "zRequests");
        }

    }
}
