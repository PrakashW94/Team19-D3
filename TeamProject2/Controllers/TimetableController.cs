using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject2;
using TeamProject2.Models;

namespace TeamProject2.Controllers
{
    public class TimetableController : Controller
    {
        private readonly IzRequestRepository zrequestRepository;

        public TimetableController() : this(new zRequestRepository())
        {
        }

        public TimetableController(IzRequestRepository zrequestRepository)
        {
			this.zrequestRepository = zrequestRepository;
        }



        //
        // GET: /Timetable/

        public ViewResult Index(string sortWeek, string filter)
        {//this code shows the user only their own requests, filtered by UserID

            var weekFilter = Convert.ToInt32(sortWeek);

            var db = new DatabaseContext();
            var userQry = from user in db.zUser where user.DeptCode == User.Identity.Name select user.UserId;
            var userID = userQry.FirstOrDefault();
            if (userID == 1) //if the user is the central admin, show all requests
            {
                var reqQry = from request in db.zRequest where request.StatusId == 4 select request;
                if (!String.IsNullOrWhiteSpace(filter))
                {
                    //Could also search based on module name, etc. if included in view
                    reqQry = reqQry.Where(r => r.ModCode.Contains(filter.ToUpper())
                                                || r.SpecialRequirement.ToUpper().Contains(filter.ToUpper())
                                                || r.zDay.DayValue.ToUpper().Contains(filter.ToUpper()));
                }

                List<string> ReqIdList = new List<string>();
                foreach (var zrequest in reqQry)
                {
                    List<string> weeksList = new List<string>();
                    ReqIdList.Add("" + zrequest.RequestId);
                    for (var number = 1; number < 17; number++)
                    {
                        var value = zrequest.zWeek.GetType().GetProperty("Week" + number).GetValue(zrequest.zWeek, null);
                        if (value.ToString() == "True")
                        {
                            weeksList.Add(number.ToString());
                        }
                    }
                    var reqId = zrequest.RequestId;
                    ViewData.Add("" + zrequest.RequestId, String.Join(",", weeksList.ToArray()));
                    //ViewBag.reqId = String.Join(",", weeksList.ToArray());
                }
                ViewBag.RequestIdList = String.Join(",", ReqIdList.ToArray());
                reqQry = SortRequests(reqQry, weekFilter);
                return View(reqQry);
            }
            else //else show requests associated with the user's account
            {
                var reqQry = from request in db.zRequest where request.UserId == userID select request;
                if (!String.IsNullOrWhiteSpace(filter))
                {
                    //Could also search based on module name, etc. if included in view
                    reqQry = reqQry.Where(r => r.ModCode.Contains(filter.ToUpper())
                                                || r.SpecialRequirement.ToUpper().Contains(filter.ToUpper())
                                                || r.zDay.DayValue.ToUpper().Contains(filter.ToUpper()));
                }
                List<string> ReqIdList = new List<string>();
                foreach (var zrequest in reqQry)
                {
                    List<string> weeksList = new List<string>();
                    ReqIdList.Add("" + zrequest.RequestId);
                    for (var number = 1; number < 17; number++)
                    {
                        var value = zrequest.zWeek.GetType().GetProperty("Week" + number).GetValue(zrequest.zWeek, null);
                        if (value.ToString() == "True")
                        {
                            weeksList.Add(number.ToString());
                        }
                    }
                    var reqId = zrequest.RequestId;
                    ViewData.Add("" + zrequest.RequestId, String.Join(",", weeksList.ToArray()));
                    //ViewBag.reqId = String.Join(",", weeksList.ToArray());
                }
                ViewBag.RequestIdList = String.Join(",", ReqIdList.ToArray());
                reqQry = SortRequests(reqQry, weekFilter);
                return View(reqQry);
            }
            //zrequestRepository.AllIncluding(zrequest => zrequest.zFacility, zrequest => zrequest.zRoom)
        }

        private IQueryable<zRequest> SortRequests(IQueryable<zRequest> requests, int sortWeek)
        {//check what user wants to sort by and orders it
            switch (sortWeek)
            {
                case 1: requests = requests.Where(r => r.zWeek.Week1 == true); break;
                case 2: requests = requests.Where(r => r.zWeek.Week2 == true); break;
                case 3: requests = requests.Where(r => r.zWeek.Week3 == true); break;
                case 4: requests = requests.Where(r => r.zWeek.Week4 == true); break;
                case 5: requests = requests.Where(r => r.zWeek.Week5 == true); break;
                case 6: requests = requests.Where(r => r.zWeek.Week6 == true); break;
                case 7: requests = requests.Where(r => r.zWeek.Week7 == true); break;
                case 8: requests = requests.Where(r => r.zWeek.Week8 == true); break;
                case 9: requests = requests.Where(r => r.zWeek.Week9 == true); break;
                case 10: requests = requests.Where(r => r.zWeek.Week10 == true); break;
                case 11: requests = requests.Where(r => r.zWeek.Week11 == true); break;
                case 12: requests = requests.Where(r => r.zWeek.Week12 == true); break;
                case 13: requests = requests.Where(r => r.zWeek.Week13 == true); break;
                case 14: requests = requests.Where(r => r.zWeek.Week14 == true); break;
                case 15: requests = requests.Where(r => r.zWeek.Week15 == true); break;
                case 16: requests = requests.Where(r => r.zWeek.Week16 == true); break;

                default:
                    requests = requests.OrderBy(r => r.ModCode); break;
            }
            requests = requests.OrderBy(r => r.ModCode);
            return (requests);
        }
    }
}
