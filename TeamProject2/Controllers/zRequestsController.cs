using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject2;
using TeamProject2.Models;

namespace TeamProject2.Controllers
{   
    public class zRequestsController : Controller
    {
		private readonly IzRequestRepository zrequestRepository;
        //Scaffold Controller VehicleMake  -force -repository -DbContextType "CommerceEntities"
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
            if(userID == 1) //if the user is the central admin, show all requests
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
                    ViewData.Add(""+zrequest.RequestId, String.Join(",", weeksList.ToArray()));
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
                    ReqIdList.Add(""+zrequest.RequestId);
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

        //
        // GET: /zRequests/Details/5

        public ViewResult Allocate()
        {
            var db = new DatabaseContext();
            var reqQry = from request in db.zRequest where request.StatusId == 4 select request;
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
            return View(reqQry);
        }

        public ActionResult Successful(int id)
        {
            var zrequest = zrequestRepository.Find(id);
            zrequest.StatusId = 1;
            zrequestRepository.Update(zrequest);
            zrequestRepository.Save();
            return RedirectToAction("Allocate");
        }

        public ActionResult Unsuccessful(int id)
        {
            var zrequest = zrequestRepository.Find(id);
            zrequest.StatusId = 2;
            zrequestRepository.Update(zrequest);
            zrequestRepository.Save();
            return RedirectToAction("Allocate");
        }

        public ActionResult Modify(int id)
        {
            var zrequest = zrequestRepository.Find(id);
            var roomList = populateRoomList();
            roomList.RemoveAt(0);
            ViewBag.roomList = roomList;
            return View(zrequestRepository.Find(id));
        }

        [HttpPost]
        public ActionResult Allocate(string[] Rooms, zRequest zrequest)
        {
            var db = new DatabaseContext();
            var updatedRequest = zrequestRepository.Find(zrequest.RequestId);
            foreach (var place in Rooms)
            {
                var roomDB = (from Room in db.zRoom where Room.RoomCode == place select Room).FirstOrDefault();
                var room = new zRoom();

                room.RoomId = roomDB.RoomId;
                room.RoomCode = roomDB.RoomCode;
                room.Capacity = roomDB.Capacity;
                room.ImgLink = roomDB.ImgLink;
                room.Private = roomDB.Private;
                room.BuildingId = roomDB.BuildingId;
                updatedRequest.zRoom.Add(room);
            }
            updatedRequest.StatusId = 5;
            zrequestRepository.Modify(updatedRequest);
            zrequestRepository.Save();
            return RedirectToAction("Allocate");
        }

        public ViewResult Details(int id)
        {
            var zrequest = zrequestRepository.Find(id);
            List<string> weeksList = new List<string>();
            for (var number = 1; number < 17; number++)
            {
                var value = zrequest.zWeek.GetType().GetProperty("Week" + number).GetValue(zrequest.zWeek, null);
                if (value.ToString() == "True")
                {
                    weeksList.Add(number.ToString());
                }
            }//convert the boolean values for week1-16 to a list
            ViewBag.Weeks = String.Join(",", weeksList.ToArray());
            return View(zrequestRepository.Find(id));
        }

        //
        // GET: /zRequests/Create

        public List<string> populateDeptList()
        { //function to return a list of departments and return it as a list of strings
            var db = new DatabaseContext();
            var deptList = new List<string>();
            var deptQry = from Department in db.zDepartment select Department.DeptCode;
            return deptQry.ToList();
        }

        public List<string> populateModuleList()
        {//function to return a list of modules and return it as a list of strings
            var db = new DatabaseContext();
            var moduleList = new List<string>();
            if (User.Identity.Name == "CA")
            {// if the user is the central admin list all modules
                var moduleQry = db.zModule.Select(module => new{module.ModCode, module.ModTitle}).ToList();
                foreach (var module in moduleQry)
                {
                    string output = module.ModCode + " - " + module.ModTitle;
                    moduleList.Add(output);
                }
            }
            else
            {//otherwise list modules associated with that department
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
        {//function to return a list of days and return it as a list of strings
            var db = new DatabaseContext();
            var dayQry = from Day in db.zDay orderby Day.DayId select Day.DayValue;
            return dayQry.ToList();

        }

        public List<string> populatePeriodList()
        {//function to return a list of periods and return it as a list of strings
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
        {//function to return a list of session length and return it as a list of strings
            var seshLengthList = new List<string>();
            seshLengthList.Add("1 Hour");
            for (int i = 2; i < 10; i++)
            {
                seshLengthList.Add(i + " Hours");
            }
            return seshLengthList;
        }

        public List<string> populateFacilityList()
        {//function to return a list of facilities and return it as a list of strings
            var db = new DatabaseContext();
            var facilityQry = from Facility in db.zFacility select Facility.FacilityName;
            return facilityQry.ToList();
        }
        /*
        public List<string> populateStatusList()
        {//function to return a list of facilities and return it as a list of strings
            var db = new DatabaseContext();
            var statusList = new List<string>();
            statusList.Add("Successful");
            statusList.Add("Unsuccessful");
            return statusList;
        }
        */
        public List<string> populateParkList()
        {//function to return a list of parks and return it as a list of strings
            var db = new DatabaseContext();
            var parkQry = from Park in db.zPark select Park.ParkName;
            var parkList = new List<string>();
            parkList = parkQry.ToList();
            parkList.Insert(0, "Any");
            return parkList;
        }

        public List<string> populateBuildingList()
        {//function to return a list of buildings and return it as a list of strings
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
        {//function to return a list of rooms and return it as a list of strings
            var db = new DatabaseContext();
            var roomQry = from Room in db.zRoom select Room.RoomCode;
            var roomList = new List<string>();
            roomList = roomQry.ToList();
            roomList.Insert(0, "Any");
            return roomList;
        }

        public ActionResult Create()
        {//function to load page to create request
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
            //this is where you set the default weeks you want shown
            return View();
        }//essentially adds everything that needs to be displayed to the ViewBag 

        public JsonResult Building(string park)
        {//this function is called from the room selector, returns the buildings from the selected park
            var db = new DatabaseContext();
            if (park != "Any")
            {//if a specific park is selected, return buildings in that park
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
            {//otherwise return a list of all buildings
                return Json(populateBuildingList());
            }
        }

        public JsonResult Room(string buildingCode)
        {//this function is called from the room selector, returns the rooms from the selected building
            if (buildingCode != "Any")
            {//if a specific park is selected, return rooms in that building
                var db = new DatabaseContext();
                var building = (from Building in db.zBuilding where Building.BuildingCode == buildingCode select Building.BuildingId).ToList().FirstOrDefault();
                var roomList = (from Room in db.zRoom where Room.BuildingId == building select Room.RoomCode).ToList();
                return Json(roomList);
            }
            else
            {//else return all buildings
                return Json(populateRoomList());
            }
        }

        public JsonResult RoomCapacityCheck(string room, int capacity)
        {
            var db = new DatabaseContext();
            var roomDB = (from Room in db.zRoom where Room.RoomCode == room select Room.Capacity).FirstOrDefault();
            if (roomDB >= capacity)
            {
                return Json(new{correct = true});
            }
            else 
            {
                return Json(new{correct = false});
            }
        }

        //
        // POST: /zRequests/Create

        [HttpPost]
        public ActionResult Create
        (//this works by naming inputs by these variable names using razor
            string DeptCode, //only used if user is CA
            string ModCode,
            string Day,
            string Period,
            string SeshLength,
            string Weeks,
            string[] Facilities, //list of combo boxes
            string Rooms,
            zRequest zrequest
        )
        {//function when the create button is clicked
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
            }//load UserId
            zrequest.ModCode = ModCode.Substring(0, 6); 
            //load ModCode
            var dayQry = from day in db.zDay where day.DayValue == Day select day.DayId;
            zrequest.DayId = dayQry.FirstOrDefault();
            //load DayId
            zrequest.PeriodId = short.Parse(Period.Substring(0, 1));
            //load PeriodId
            zrequest.SessionLength = short.Parse(SeshLength.Substring(0, 1));
            //load SessionLength

            int[] weeksIntArray = Array.ConvertAll(Weeks.Split(','), int.Parse);
            //Weeks is a string of all selected weeks(numerical) seperated by a z
            foreach (var number in weeksIntArray)
            {
                zrequest.zWeek.GetType().GetProperty("Week" + number).SetValue(zrequest.zWeek, true, null);
            }
            //load weeks

            if (Facilities != null)
            {
                foreach (var facility in Facilities)
                {
                    var facName = facility.Replace(".", " ");
                    //this is to fix a bug where spaces caused a bug so i replaced the spaces with .'s
                    //and converted back when i need to use them
                    //initial replacement is in the razor
                    var facQry = from fac in db.zFacility where fac.FacilityName == facName select fac;
                    var test = facQry.ToList();
                    var item = new zFacility();
                    item.FacilityId = test.FirstOrDefault().FacilityId;
                    item.FacilityName = test.FirstOrDefault().FacilityName;
                    zrequest.zFacility.Add(item);
                }
            }
            //load facilities
            
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
                    case 0: //any room
                        break;

                    case 1: //any room in a park
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

                    case 2: //any room in a building
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

                    case 3: //specific room
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
            //load rooms that need to be stored in database
            //store of the number of rooms on the request
            zrequest.RoomCount = (short)roomsList.Count();
            //"Any Room"s are not stored so if the room count is more that whats on the database
            //the difference is "Any Room"s

            //these need to be coded still
            zrequest.RoundNo = 1;
            zrequest.Semester = 1;

            zrequest.StatusId = 3;
            //set status to pending
            zrequestRepository.Insert(zrequest);
            zrequestRepository.Save();
            return RedirectToAction("Index");
        }
        
        //
        // GET: /zRequests/Edit/5
 
        public ActionResult Edit(int id)
        {//function to initialise the edit screen given the id of the request
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
            //using the second parameter of a select list to set the default value

            List<string> weeksList = new List<string>();
            for (var number = 1; number < 17; number++ )
            {
                var value = zrequest.zWeek.GetType().GetProperty("Week" + number).GetValue(zrequest.zWeek, null);
                if (value.ToString() == "True")
                {
                    weeksList.Add(number.ToString());
                }
            }//convert the boolean values for week1-16 to a list
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
            //this bit needs to be fixed, depending on the selectePeriod, the session length list should change accordingly

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

            //two lists are initialised, one for what will be shown to the user
            //the other is a kind of code to feed what the user has selected back to the controller
            List<string> selectedRooms = new List<string>(); //this is the "code" the controller can understand
            List<string> selectedRoomsDisp = new List<string>(); //this is what is shown to the user
            
            var dbRooms = zrequest.zRoomBooking.Count(); //number of rooms in the db
            var diff = zrequest.RoomCount - dbRooms;
            for (var i = 0; i < diff; i++)
            { //diff is the number of any rooms
                selectedRooms.Add("0");
                selectedRoomsDisp.Add("Any Room");

            }
            foreach (var booking in zrequest.zRoomBooking)
            {
                switch (booking.Type)
                {//case for 0 isn't needed as 0 rooms are not stored in the database
                    case 1: //any room in a park
                        selectedRooms.Add("1" + booking.zPark.ParkName);
                        selectedRoomsDisp.Add("Any room in the " + booking.zPark.ParkName + " Park");
                        break;

                    case 2: //any room in a building
                        selectedRooms.Add("2" + booking.zBuilding.BuildingCode);
                        selectedRoomsDisp.Add("Any room in " + booking.zBuilding.BuildingCode + " - " + booking.zBuilding.BuildingName);
                        break;

                    case 3: //specific room
                        selectedRooms.Add("3" + booking.zRoom.RoomCode);
                        selectedRoomsDisp.Add(booking.zRoom.RoomCode);
                        break;
                }
            }
            ViewBag.RoomDropDown = selectedRoomsDisp;
            ViewBag.Rooms = String.Join(",", selectedRooms.ToArray());
            return View(zrequestRepository.Find(id));
        }

        //
        // POST: /zRequests/Edit/5

        [HttpPost]
        public ActionResult Edit
        (//function when the edit button is clicked
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
        {//function is very similar to create
            //lift all results from the string values and stick them in a request
            //then add that request to the db
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
            
            if (Facilities != null) //this check fixes an error if no facilities are selected
            {
                foreach (var facility in Facilities)
                {
                    var facName = facility.Replace(".", " ");
                    var facQry = from fac in db.zFacility where fac.FacilityName == facName select fac;
                    var facList = facQry.ToList();
                    var item = new zFacility();
                    item.FacilityId = facList.FirstOrDefault().FacilityId;
                    item.FacilityName = facList.FirstOrDefault().FacilityName;
                    zrequest.zFacility.Add(item);
                }
            }
            var roomsList = new List<string>();
            if(Rooms == "")
            {//if no rooms are selected, assume they're happy with a single "any room"
                roomsList.Add("0");
            }
            else
            {
                roomsList = Rooms.Split(',').ToList();
            }//otherwise process the rooms
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
            
            //these two need to be coded
            zrequest.RoundNo = 1;
            zrequest.Semester = 1;

            zrequest.StatusId = 3; //status is pending
            
            zrequestRepository.Update(zrequest);
            zrequestRepository.Save();

            return RedirectToAction("Index");            
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

