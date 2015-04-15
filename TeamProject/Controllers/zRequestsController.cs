using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject;
using TeamProject.Models;

namespace TeamProject.Controllers
{   
    public class zRequestsController : Controller
    {
		private readonly IzRequestRepository zrequestRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public zRequestsController() : this(new zRequestRepository())
        {
        }

        public zRequestsController(IzRequestRepository zrequestRepository)
        {
			this.zrequestRepository = zrequestRepository;
        }

        //
        // GET: /zRequests/

        public ViewResult Index()
        {
            populateStatusList();
            var db = new DatabaseContext();
            var userQry = from user in db.zUser where user.DeptCode == User.Identity.Name select user.UserId;
            var userID = userQry.FirstOrDefault();
            if(userID == 1)
            {
                var reqQry = from request in db.zRequests select request;
                return View(reqQry);
            }
            else
            {
                var reqQry = from request in db.zRequests where request.UserId == userID select request;
                return View(reqQry);
            }
            //zrequestRepository.AllIncluding(zrequest => zrequest.zFacility, zrequest => zrequest.zRoom)
        }

        //
        // GET: /zRequests/Details/5

        public ViewResult Details(int id)
        {
            return View(zrequestRepository.Find(id));
        }

        //
        // GET: /zRequests/Create

        public void populateStatusList()
        {
            var db = new DatabaseContext();

            var statusList = new List<string>();
            var statusQry = from Status in db.zStatus select Status.StatusValue;
            statusList.AddRange(statusQry.Distinct());
            ViewBag.statusList = statusList;
        }

        public void populateDeptList()
        {
            var db = new DatabaseContext();
            var deptList = new List<string>();
            var deptQry = from Department in db.zDepartment select Department.DeptCode;
            ViewBag.deptList = deptQry.ToList();
        }

        public void populateModuleList()
        {
            var db = new DatabaseContext();
            var moduleList = new List<string>();
            if (User.Identity.Name == "CA")
            {
                var moduleQry = db.zModule.Select(module => new{module.ModCode, module.ModTitle}).ToList();
                foreach (var module in moduleQry)
                {
                    string output = module.ModCode + " - " + module.ModTitle;
                    moduleList.Add(output);
                }
            }
            else
            {
                var moduleQry = db.zModule
                    .Where(module => module.DeptCode == User.Identity.Name)
                    .Select(module => new { module.ModCode, module.ModTitle }).ToList();
                foreach (var module in moduleQry)
                {
                    string output = module.ModCode + " - " + module.ModTitle;
                    moduleList.Add(output);
                }         
            }
            ViewBag.moduleList = moduleList;
        }

        public void populateDayList()
        {
            var db = new DatabaseContext();
            var dayQry = from Day in db.zDay orderby Day.DayId select Day.DayValue;
            ViewBag.dayList = dayQry.ToList();
        }

        public void populatePeriodList()
        {
            var db = new DatabaseContext();
            var periodList = new List<string>();
            var periodQry = from Period in db.zPeriod select Period.PeriodValue;
            var period = 1;
            foreach (var item in periodQry.ToList())
            {
                periodList.Add(period + " - " + item);
                period++;
            }
            ViewBag.periodList = periodList;
        }

        public void populateSeshLengthList()
        {
            var seshLengthList = new List<string>();
            seshLengthList.Add("1 Hour");
            for (int i = 2; i < 10; i++)
            {
                seshLengthList.Add(i + " Hours");
            }
                ViewBag.seshLengthList = seshLengthList;
        }

        public ActionResult Create()
        {
            populateDeptList();
            populateModuleList();
            populateDayList();
            populatePeriodList();
            populateSeshLengthList();
            return View();
        } 

        //
        // POST: /zRequests/Create

        [HttpPost]
        public ActionResult Create
        (
            string DeptCode,
            string ModCode,
            string Day,
            string Period,
            string SeshLength,
            string Weeks,
            zRequest zrequest
        )
        {
            var db = new DatabaseContext();

            if (User.Identity.Name == "CA")
            {
                var userQry = from user in db.zUser where user.DeptCode == DeptCode select user.UserId;
                zrequest.UserId = userQry.FirstOrDefault();
            }
            else
            {
                var userQry = from user in db.zUser where user.DeptCode == User.Identity.Name select user.UserId;
                zrequest.UserId = userQry.FirstOrDefault();
            }
            zrequest.ModCode = ModCode.Substring(0, 6);
            var dayQry = from day in db.zDay where day.DayValue == Day select day.DayId;
            zrequest.DayId = dayQry.FirstOrDefault();
            zrequest.PeriodId = short.Parse(Period.Substring(0, 1));
            zrequest.SessionLength = short.Parse(SeshLength.Substring(0, 1));

            string[] separator = { "," };
            string[] weeksStringArray = Weeks.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            int[] weeksIntArray = Array.ConvertAll(weeksStringArray, int.Parse);

            zrequest.RoundNo = 1;
            zrequest.Semester = 1;
            zrequest.StatusId = 3;
            zrequest.WeekId = 1;

            zrequestRepository.InsertOrUpdate(zrequest);
            zrequestRepository.Save();
            return RedirectToAction("Index");
            /*if (ModelState.IsValid) {
                zrequestRepository.InsertOrUpdate(zrequest);
                zrequestRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}*/
        }
        
        //
        // GET: /zRequests/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(zrequestRepository.Find(id));
        }

        //
        // POST: /zRequests/Edit/5

        [HttpPost]
        public ActionResult Edit(zRequest zrequest)
        {
            if (ModelState.IsValid) {
                zrequestRepository.InsertOrUpdate(zrequest);
                zrequestRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /zRequests/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(zrequestRepository.Find(id));
        }

        //
        // POST: /zRequests/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            zrequestRepository.Delete(id);
            zrequestRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                zrequestRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

