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

        public ViewResult Index(string selectedWeek, string TimetableType, string Selector)
        {//this code shows the user only their own requests, filtered by UserID
            var db = new DatabaseContext();
            var userQry = from user in db.zUser where user.DeptCode == User.Identity.Name select user.UserId;
            var userID = userQry.FirstOrDefault();
            if (TimetableType == "0")
            {
                var lecturerId = int.Parse(Selector);
                var lecturer = (from Lecturer in db.zLecturer where Lecturer.LecturerId == lecturerId select Lecturer).FirstOrDefault();
                List<zRequest> reqList = new List<zRequest>();
                foreach (var module in lecturer.zModule)
                {
                    reqList.AddRange(module.zRequest.Where(r => r.StatusId == 1 || r.StatusId == 5));
                }

                if (selectedWeek != "All" || selectedWeek == null)
                {
                    var remList = new List<int>();
                    foreach (var zrequest in reqList)
                    {
                        var value = zrequest.zWeek.GetType().GetProperty("Week" + selectedWeek).GetValue(zrequest.zWeek, null).ToString();
                        if (value == "False")
                        {
                            remList.Add(reqList.IndexOf(zrequest));
                        }
                    }

                    foreach (var index in remList)
                    {
                        reqList.RemoveAt(index);
                    }
                }
                List<string> ReqIdList = new List<string>();
                foreach (var zrequest in reqList)
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
                ViewBag.empty = populateLecturerList();
                return View(reqList);
            }
            else if (TimetableType == "1")
            {
                var progId = int.Parse(Selector);
                var programme = (from Programme in db.zProgramme where Programme.ProgrammeCode == progId select Programme).FirstOrDefault();
                List<zRequest> reqList = new List<zRequest>();
                foreach (var module in programme.zModule)
                {
                    reqList.AddRange(module.zRequest.Where(r => r.StatusId == 1 || r.StatusId == 5));
                }

                if (selectedWeek != "All" || selectedWeek == null)
                {
                    var remList = new List<int>();
                    foreach (var zrequest in reqList)
                    {
                        var value = zrequest.zWeek.GetType().GetProperty("Week" + selectedWeek).GetValue(zrequest.zWeek, null).ToString();
                        if (value == "False")
                        {
                            remList.Add(reqList.IndexOf(zrequest));
                        }
                    }

                    foreach (var index in remList)
                    {
                        reqList.RemoveAt(index);
                    }
                }

                List<string> ReqIdList = new List<string>();
                foreach (var zrequest in reqList)
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
                ViewBag.empty = populateProgrammeList();
                return View(reqList);
            }
            else 
            {
                ViewBag.empty = populateLecturerList();
                var empty = from Request in db.zRequest where Request.RequestId == -1 select Request;
                var emptyList = new List<SelectListItem>();
                emptyList.Add(new SelectListItem { Text = "Please select a timetable type", Value = "-1" });
                ViewBag.empty = emptyList;
                return View(empty);
            }
        }

        public List<SelectListItem> populateLecturerList()
        {
            var db = new DatabaseContext();
            if (User.Identity.Name == "CA")
            {
                var lecQry = from Lecturer in db.zLecturer select Lecturer;
                var lecList = new List<SelectListItem>();
                foreach (var person in lecQry)
                {
                    lecList.Add(new SelectListItem { Text = person.LecturerName, Value = person.LecturerId.ToString() });
                }
                return lecList;
            }
            else
            {
                var lecQry = from Lecturer in db.zLecturer where Lecturer.DeptCode == User.Identity.Name select Lecturer;
                var lecList = new List<SelectListItem>();
                foreach (var person in lecQry)
                {
                    lecList.Add(new SelectListItem { Text = person.LecturerName, Value = person.LecturerId.ToString() });
                }
                return lecList;
            }
        }

        public List<SelectListItem> populateProgrammeList()
        {
            var db = new DatabaseContext();
            if (User.Identity.Name == "CA")
            {
                var progQry = from Programme in db.zProgramme select Programme;
                var progList = new List<SelectListItem>();
                foreach (var Programme in progQry)
                {
                    var progCode = Programme.DeptCode;
                    if (Programme.GradType)
                    {
                        progCode += "U";
                    }
                    else
                    {
                        progCode += "PT";
                    }
                    if (Programme.ProgramType)
                    {
                        progCode += "B";
                    }
                    else
                    {
                        progCode += "M";
                    }
                    if (Programme.ProgrammeCode < 10)
                    {
                        progCode += "0";
                    }
                    progCode += Programme.ProgrammeCode;
                    progList.Add(new SelectListItem { Text = progCode, Value = Programme.ProgrammeCode.ToString() });
                }
                return progList;
            }
            else
            {
                var progQry = from Programme in db.zProgramme where Programme.DeptCode == User.Identity.Name select Programme;
                var progList = new List<SelectListItem>();
                foreach (var Programme in progQry)
                {
                    var progCode = Programme.DeptCode;
                    if (Programme.GradType)
                    {
                        progCode += "U";
                    }
                    else
                    {
                        progCode += "PT";
                    }
                    if (Programme.ProgramType)
                    {
                        progCode += "B";
                    }
                    else
                    {
                        progCode += "M";
                    }
                    if (Programme.ProgrammeCode < 10)
                    {
                        progCode += "0";
                    }
                    progCode += Programme.ProgrammeCode;
                    progList.Add(new SelectListItem { Text = progCode, Value = Programme.ProgrammeCode.ToString() });
                }
                return progList;
            }
        }

        [HttpPost]
        public JsonResult fillSelector(int type)
        {
            if (type == 0)
            {
                var db = new DatabaseContext();
                if (User.Identity.Name == "CA")
                {
                    var lecQry = from Lecturer in db.zLecturer select Lecturer;
                    var lecList = new List<SelectListItem>();
                    foreach (var person in lecQry)
                    {
                        lecList.Add(new SelectListItem { Text = person.LecturerName, Value = person.LecturerId.ToString() });
                    }
                    return Json(lecList);
                }
                else
                {
                    var lecQry = from Lecturer in db.zLecturer where Lecturer.DeptCode == User.Identity.Name select Lecturer;
                    var lecList = new List<SelectListItem>();
                    foreach (var person in lecQry)
                    {
                        lecList.Add(new SelectListItem { Text = person.LecturerName, Value = person.LecturerId.ToString() });
                    }
                    return Json(lecList);
                }
            }
            else
            {
                var db = new DatabaseContext();
                if (User.Identity.Name == "CA")
                {
                    var progQry = from Programme in db.zProgramme select Programme;
                    var progList = new List<SelectListItem>();
                    foreach (var Programme in progQry)
                    {
                        var progCode = Programme.DeptCode;
                        if (Programme.GradType)
                        {
                            progCode += "U";
                        }
                        else
                        {
                            progCode += "PT";
                        }
                        if (Programme.ProgramType)
                        {
                            progCode += "B";
                        }
                        else
                        {
                            progCode += "M";
                        }
                        if (Programme.ProgrammeCode < 10)
                        {
                            progCode += "0";
                        }
                        progCode += Programme.ProgrammeCode;
                        progList.Add(new SelectListItem { Text = progCode, Value = Programme.ProgrammeCode.ToString() });
                    }
                    return Json(progList);
                }
                else
                {
                    var progQry = from Programme in db.zProgramme where Programme.DeptCode == User.Identity.Name select Programme;
                    var progList = new List<SelectListItem>();
                    foreach (var Programme in progQry)
                    {
                        var progCode = Programme.DeptCode;
                        if (Programme.GradType)
                        {
                            progCode += "U";
                        }
                        else
                        {
                            progCode += "PT";
                        }
                        if (Programme.ProgramType)
                        {
                            progCode += "B";
                        }
                        else
                        {
                            progCode += "M";
                        }
                        if (Programme.ProgrammeCode < 10)
                        {
                            progCode += "0";
                        }
                        progCode += Programme.ProgrammeCode;
                        progList.Add(new SelectListItem { Text = progCode, Value = Programme.ProgrammeCode.ToString() });
                    }
                    return Json(progList);
                }
            }
        }
    }
}
