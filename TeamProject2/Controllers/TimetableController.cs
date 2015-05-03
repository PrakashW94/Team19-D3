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

        public ViewResult Index(string sortOrder, string filter)
        {//this code shows the user only their own requests, filtered by UserID

            //Allows sorting in descending order too
            ViewBag.ModSortParm = String.IsNullOrEmpty(sortOrder) ? "ModCode_desc" : "";
            ViewBag.NumRoomSortParm = sortOrder == "NumRoom" ? "NumRoom_desc" : "NumRoom";
            ViewBag.NumFacSortParm = sortOrder == "NumFac" ? "NumFac_desc" : "NumFac";
            ViewBag.DaySortParm = sortOrder == "Day" ? "Day_desc" : "Day";
            ViewBag.StatSortParm = sortOrder == "Status" ? "Status_desc" : "Status";

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
                    }//convert the boolean values for week1-16 to a list
                    var reqId = zrequest.RequestId;
                    ViewData.Add("" + zrequest.RequestId, String.Join(",", weeksList.ToArray()));
                    //ViewBag.reqId = String.Join(",", weeksList.ToArray());
                }
                ViewBag.RequestIdList = String.Join(",", ReqIdList.ToArray());
                reqQry = SortRequests(reqQry, sortOrder);
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
                    }//convert the boolean values for week1-16 to a list
                    var reqId = zrequest.RequestId;
                    ViewData.Add("" + zrequest.RequestId, String.Join(",", weeksList.ToArray()));
                    //ViewBag.reqId = String.Join(",", weeksList.ToArray());
                }
                ViewBag.RequestIdList = String.Join(",", ReqIdList.ToArray());
                reqQry = SortRequests(reqQry, sortOrder);
                return View(reqQry);
            }
            //zrequestRepository.AllIncluding(zrequest => zrequest.zFacility, zrequest => zrequest.zRoom)
        }

        private IQueryable<zRequest> SortRequests(IQueryable<zRequest> requests, string sortOrder)
        {//check what user wants to sort by and orders it
            switch (sortOrder)
            {
                case "Status":
                    requests = requests.OrderBy(r => r.StatusId); break;
                case "Day":
                    requests = requests.OrderBy(r => r.zDay.DayId); break;
                case "NumRoom":
                    requests = requests.OrderBy(r => r.RoomCount); break;
                case "NumFac":
                    requests = requests.OrderBy(r => r.zFacility.Count); break;
                case "ModCode_desc":
                    requests = requests.OrderByDescending(r => r.ModCode); break;
                case "Status_desc":
                    requests = requests.OrderByDescending(r => r.StatusId); break;
                case "Day_desc":
                    requests = requests.OrderByDescending(r => r.zDay.DayId); break;
                case "NumRoom_desc":
                    requests = requests.OrderByDescending(r => r.RoomCount); break;
                case "NumFac_desc":
                    requests = requests.OrderByDescending(r => r.zFacility.Count); break;
                default:
                    requests = requests.OrderBy(r => r.ModCode); break;
            }
            return (requests);
        }
    }
}
