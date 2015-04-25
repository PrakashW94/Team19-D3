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
            var db = new DatabaseContext();
            var userQry = from user in db.zUser where user.DeptCode == User.Identity.Name select user.UserId;
            var userID = userQry.FirstOrDefault();
            if(userID == 1)
            {
                var reqQry = from request in db.zRequest select request;
                return View(reqQry);
            }
            else
            {
                var reqQry = from request in db.zRequest where request.UserId == userID select request;
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

        public List<string> populateDeptList()
        {
            var db = new DatabaseContext();
            var deptList = new List<string>();
            var deptQry = from Department in db.zDepartment select Department.DeptCode;
            return deptQry.ToList();
        }

        public List<string> populateModuleList()
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
            return moduleList;
        }

        public List<string> populateDayList()
        {
            var db = new DatabaseContext();
            var dayQry = from Day in db.zDay orderby Day.DayId select Day.DayValue;
            return dayQry.ToList();

        }

        public List<string> populatePeriodList()
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
            return periodList;
        }

        public List<string> populateSeshLengthList()
        {
            var seshLengthList = new List<string>();
            seshLengthList.Add("1 Hour");
            for (int i = 2; i < 10; i++)
            {
                seshLengthList.Add(i + " Hours");
            }
            return seshLengthList;
        }

        public List<string> populateFacilityList()
        {
            var db = new DatabaseContext();
            var facilityQry = from Facility in db.zFacility select Facility.FacilityName;
            return facilityQry.ToList();
        }

        public List<string> populateParkList()
        {
            var db = new DatabaseContext();
            var parkQry = from Park in db.zPark select Park.ParkName;
            var parkList = new List<string>();
            parkList = parkQry.ToList();
            parkList.Insert(0, "Any");
            return parkList;
        }

        public List<string> populateBuildingList()
        {
            var db = new DatabaseContext();
            var buildingQry = from Building in db.zBuilding select Building;
            var buildingList = new List<string>();
            buildingList.Add("Any");
            foreach (var building in buildingQry)
            {
                buildingList.Add(building.BuildingCode + " - " + building.BuildingName);
            }
            return buildingList;
        }

        public List<string> populateRoomList()
        {
            var db = new DatabaseContext();
            var roomQry = from Room in db.zRoom select Room.RoomCode;
            var roomList = new List<string>();
            roomList = roomQry.ToList();
            roomList.Insert(0, "Any");
            return roomList;
        }

        public ActionResult Create()
        {
            if (User.Identity.Name == "CA")
            {
                ViewBag.deptList = populateDeptList();
            }
            ViewBag.moduleList = populateModuleList();
            ViewBag.dayList = populateDayList();
            ViewBag.periodList = populatePeriodList();
            ViewBag.seshLengthList = populateSeshLengthList();
            ViewBag.facilityList = populateFacilityList();
            ViewBag.parkList = populateParkList();
            ViewBag.buildingList = populateBuildingList();
            ViewBag.roomList = populateRoomList();
            ViewBag.Weeks = "1,2,3,4,5,6,7,8,9,10,11,12";
            return View();
        } 

        public JsonResult Building(string park)
        {
            var db = new DatabaseContext();
            if (park != "Any")
            {
                var parkId = (from Park in db.zPark where Park.ParkName == park select Park.ParkId).ToList().FirstOrDefault();
                var buildingQry = (from Building in db.zBuilding where Building.ParkId == parkId select Building);
                var buildingList = new List<string>();
                foreach (var building in buildingQry)
                {
                    buildingList.Add(building.BuildingCode + " - " + building.BuildingName);
                }
                return Json(buildingList);
            }
            else
            {
                return Json(populateBuildingList());
            }
        }

        public JsonResult Room(string buildingCode)
        {
            if (buildingCode != "Any")
            {
                var db = new DatabaseContext();
                var building = (from Building in db.zBuilding where Building.BuildingCode == buildingCode select Building.BuildingId).ToList().FirstOrDefault();
                var roomList = (from Room in db.zRoom where Room.BuildingId == building select Room.RoomCode).ToList();
                return Json(roomList);
            }
            else
            {
                return Json(populateRoomList());
            }
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
            string[] Facilities,
            string Rooms,
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

            int[] weeksIntArray = Array.ConvertAll(Weeks.Split(','), int.Parse);

            foreach (var number in weeksIntArray)
            {
                zrequest.zWeek.GetType().GetProperty("Week" + number).SetValue(zrequest.zWeek, true, null);
            }

            if (Facilities != null)
            {
                foreach (var facility in Facilities)
                {
                    var facName = facility.Replace(".", " ");
                    var facQry = from fac in db.zFacility where fac.FacilityName == facName select fac;
                    var test = facQry.ToList();
                    var item = new zFacility();
                    item.FacilityId = test.FirstOrDefault().FacilityId;
                    item.FacilityName = test.FirstOrDefault().FacilityName;
                    zrequest.zFacility.Add(item);
                }
            }
            
            var roomsList = new List<string>();
            if(Rooms == "")
            {
                roomsList.Add("0");
            }
            else
            {
                roomsList = Rooms.Split(',').ToList();
            }
            foreach (var place in roomsList)
            {
                var cap = 0;
                var type = int.Parse(place.Substring(0, 1));
                switch (type)
                {
                    case 0: 
                        break;

                    case 1:
                        var parkName = place.Substring(1);
                        var parkDB = (from Park in db.zPark where Park.ParkName == parkName select Park).FirstOrDefault();
                        var park = new zRoomBooking();
                        park.Type = type;
                        park.Capacity = cap;
                        park.zPark = new zPark();
                        park.zPark.ParkId = parkDB.ParkId;
                        park.zPark.ParkName = parkDB.ParkName;
                        zrequest.zRoomBooking.Add(park);
                        break;

                    case 2:
                        var buildingCode = place.Substring(1);
                        var buildingDB = (from Building in db.zBuilding where Building.BuildingCode == buildingCode select Building).FirstOrDefault();
                        var building = new zRoomBooking();
                        building.Type = type;
                        building.Capacity = cap;
                        building.zBuilding = new zBuilding();
                        building.zBuilding.BuildingId = buildingDB.BuildingId;
                        building.zBuilding.BuildingCode = buildingDB.BuildingCode;
                        building.zBuilding.BuildingName = buildingDB.BuildingName;
                        building.zBuilding.ParkId = buildingDB.ParkId;
                        zrequest.zRoomBooking.Add(building);
                        break;

                    case 3:
                        var roomCode = place.Substring(1);
                        var roomDB = (from Room in db.zRoom where Room.RoomCode == roomCode select Room).FirstOrDefault();
                        var room = new zRoomBooking();
                        room.Type = type;
                        room.Capacity = cap;
                        room.zRoom = new zRoom();
                        room.zRoom.RoomId = roomDB.RoomId;
                        room.zRoom.RoomCode = roomDB.RoomCode;
                        room.zRoom.Capacity = roomDB.Capacity;
                        room.zRoom.ImgLink = roomDB.ImgLink;
                        room.zRoom.Private = roomDB.Private;
                        room.zRoom.BuildingId = roomDB.BuildingId;
                        zrequest.zRoomBooking.Add(room);
                        break;
                } 
            }
            zrequest.RoomCount = (short)roomsList.Count();
            
            zrequest.RoundNo = 1;
            zrequest.Semester = 1;
            zrequest.StatusId = 3;
            
            zrequestRepository.Insert(zrequest);
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
            var db = new DatabaseContext();
            var zrequest = (zrequestRepository.Find(id));

            if (User.Identity.Name == "CA")
            {
                var deptQry = from Department in db.zDepartment select Department.DeptCode;
                ViewBag.DeptCode = new SelectList(deptQry, zrequest.zUser.DeptCode);
            }
            else
            {
                ViewBag.DeptCode = User.Identity.Name;
            }

            var selectedModule = zrequest.zModule.ModCode + " - " + zrequest.zModule.ModTitle;
            ViewBag.ModCode = new SelectList (populateModuleList(), selectedModule);

            List<string> weeksList = new List<string>();
            for (var number = 1; number < 17; number++ )
            {
                var value = zrequest.zWeek.GetType().GetProperty("Week" + number).GetValue(zrequest.zWeek);
                if (value.ToString() == "True")
                {
                    weeksList.Add(number.ToString());
                }
            }
            ViewBag.Weeks = String.Join(",", weeksList.ToArray());

            ViewBag.Day = new SelectList(populateDayList(), zrequest.zDay.DayValue);

            var selectedPeriod = zrequest.zPeriod.PeriodId + " - " + zrequest.zPeriod.PeriodValue;
            ViewBag.Period = new SelectList(populatePeriodList(), selectedPeriod);

            var selectedSeshLength = "";
            if (zrequest.SessionLength == 1)
            {
                selectedSeshLength = "1 Hour";
            }
            else
            {
                selectedSeshLength = zrequest.SessionLength + " Hours";
            }
            ViewBag.SeshLength = new SelectList (populateSeshLengthList(), selectedSeshLength);

            ViewBag.facilityList = populateFacilityList();
            List<string> selectedFacilities = new List<string>();
            foreach (var facility in zrequest.zFacility)
            {
                selectedFacilities.Add(facility.FacilityName);
            }
            ViewBag.selectedFacilities = selectedFacilities;

            ViewBag.parkList = populateParkList();
            ViewBag.buildingList = populateBuildingList();
            ViewBag.roomList = populateRoomList();

            List<string> selectedRoomsDisp = new List<string>();
            List<string> selectedRooms = new List<string>();
            var dbRooms = zrequest.zRoomBooking.Count();
            var diff = zrequest.RoomCount - dbRooms;
            for (var i = 0; i < diff; i++)
            {
                selectedRooms.Add("0");
                selectedRoomsDisp.Add("Any");

            }
            foreach (var booking in zrequest.zRoomBooking)
            {
                switch (booking.Type)
                {
                    case 1:
                        selectedRooms.Add("1" + booking.zPark.ParkName);
                        selectedRoomsDisp.Add("Any room in the " + booking.zPark.ParkName + " Park");
                        break;

                    case 2: 
                        selectedRooms.Add("2" + booking.zBuilding.BuildingCode);
                        selectedRoomsDisp.Add("Any room in " + booking.zBuilding.BuildingCode + " - " + booking.zBuilding.BuildingName);
                        break;

                    case 3:
                        selectedRooms.Add("3" + booking.zRoom.RoomCode);
                        selectedRoomsDisp.Add(booking.zRoom.RoomCode);
                        break;
                }
            }
            ViewBag.RoomDropDown = selectedRoomsDisp;
            ViewBag.Rooms = String.Join(",", selectedRooms.ToArray());
            return View(zrequestRepository.Find(id));
            /*
            List<string> selectedRoomsDisp = new List<string>();
            List<string> selectedRooms = new List<string>();
            var dbRooms = zrequest.zPark.Count + zrequest.zBuilding.Count + zrequest.zRoom.Count;
            var diff = zrequest.RoomCount - dbRooms;
            for (var i = 0; i < diff; i++)
            {
                selectedRooms.Add("0");
                selectedRoomsDisp.Add("Any");
                
            }
            foreach (var park in zrequest.zPark)
            {
                selectedRooms.Add("1" + park.ParkName);
                selectedRoomsDisp.Add("Any room in the " + park.ParkName + " Park");
            }
            foreach (var building in zrequest.zBuilding)
            {
                selectedRooms.Add("2" + building.BuildingCode);
                selectedRoomsDisp.Add("Any room in " + building.BuildingCode + " - " + building.BuildingName);
            }
            foreach (var room in zrequest.zRoom)
            {
                selectedRooms.Add("3" + room.RoomCode);
                selectedRoomsDisp.Add(room.RoomCode);
                
            }
            ViewBag.RoomDropDown = selectedRoomsDisp;
            ViewBag.Rooms = String.Join(",", selectedRooms.ToArray()); //" "; var test = 
            return View(zrequestRepository.Find(id));*/
        }

        //
        // POST: /zRequests/Edit/5

        [HttpPost]
        public ActionResult Edit
        (
            string DeptCode,
            string ModCode,
            string Day,
            string Period,
            string SeshLength,
            string Weeks,
            string[] Facilities,
            string Rooms,
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
            
            int[] weeksIntArray = Array.ConvertAll(Weeks.Split(','), int.Parse);
            foreach (var number in weeksIntArray)
            {
                zrequest.zWeek.GetType().GetProperty("Week" + number).SetValue(zrequest.zWeek, true, null);
            }
            
            if (Facilities != null)
            {
                foreach (var facility in Facilities)
                {
                    var facName = facility.Replace(".", " ");
                    var facQry = from fac in db.zFacility where fac.FacilityName == facName select fac;
                    var test = facQry.ToList();
                    var item = new zFacility();
                    item.FacilityId = test.FirstOrDefault().FacilityId;
                    item.FacilityName = test.FirstOrDefault().FacilityName;
                    zrequest.zFacility.Add(item);
                }
            }
            var roomsList = new List<string>();
            if(Rooms == "")
            {
                roomsList.Add("0");
            }
            else
            {
                roomsList = Rooms.Split(',').ToList();
            }
            foreach (var place in roomsList)
            {
                var cap = 0;
                var type = int.Parse(place.Substring(0, 1));
                switch (type)
                {
                    case 0:
                        break;

                    case 1:
                        var parkName = place.Substring(1);
                        var parkDB = (from Park in db.zPark where Park.ParkName == parkName select Park).FirstOrDefault();
                        var park = new zRoomBooking();
                        park.Type = type;
                        park.Capacity = cap;
                        park.zPark = new zPark();
                        park.zPark.ParkId = parkDB.ParkId;
                        park.zPark.ParkName = parkDB.ParkName;
                        zrequest.zRoomBooking.Add(park);
                        break;

                    case 2:
                        var buildingCode = place.Substring(1);
                        var buildingDB = (from Building in db.zBuilding where Building.BuildingCode == buildingCode select Building).FirstOrDefault();
                        var building = new zRoomBooking();
                        building.Type = type;
                        building.Capacity = cap;
                        building.zBuilding = new zBuilding();
                        building.zBuilding.BuildingId = buildingDB.BuildingId;
                        building.zBuilding.BuildingCode = buildingDB.BuildingCode;
                        building.zBuilding.BuildingName = buildingDB.BuildingName;
                        building.zBuilding.ParkId = buildingDB.ParkId;
                        zrequest.zRoomBooking.Add(building);
                        break;

                    case 3:
                        var roomCode = place.Substring(1);
                        var roomDB = (from Room in db.zRoom where Room.RoomCode == roomCode select Room).FirstOrDefault();
                        var room = new zRoomBooking();
                        room.Type = type;
                        room.Capacity = cap;
                        room.zRoom = new zRoom();
                        room.zRoom.RoomId = roomDB.RoomId;
                        room.zRoom.RoomCode = roomDB.RoomCode;
                        room.zRoom.Capacity = roomDB.Capacity;
                        room.zRoom.ImgLink = roomDB.ImgLink;
                        room.zRoom.Private = roomDB.Private;
                        room.zRoom.BuildingId = roomDB.BuildingId;
                        zrequest.zRoomBooking.Add(room);
                        break;
                }
            }
            zrequest.RoomCount = (short)roomsList.Count();
            
            zrequest.RoundNo = 1;
            zrequest.Semester = 1;
            zrequest.StatusId = 3;
            
            zrequestRepository.Update(zrequest);
            zrequestRepository.Save();

            return RedirectToAction("Index");
            /*
            if (ModelState.IsValid) {
                zrequestRepository.InsertOrUpdate(zrequest);
                zrequestRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
            */ 
            
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

        public zFacility tempFac { get; set; }
    }
}

