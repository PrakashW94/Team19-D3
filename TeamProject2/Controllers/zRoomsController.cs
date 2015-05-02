using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject2;
using TeamProject2.Models;

namespace TeamProject2.Controllers
{
    [Authorize]
    public class zRoomsController : Controller
    {
		private readonly IzRoomRepository zroomRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public zRoomsController() : this(new zRoomRepository())
        {
        }

        public zRoomsController(IzRoomRepository zroomRepository)
        {
			this.zroomRepository = zroomRepository;
        }

        //
        // GET: /zRooms/

        public List<string> populateBuildingList()
        {//function to return a list of buildings and return it as a list of strings
            var db = new DatabaseContext();
            var buildingQry = from Building in db.zBuilding select Building;
            var buildingList = new List<string>();
            foreach (var building in buildingQry)
            {
                buildingList.Add(building.BuildingCode + " - " + building.BuildingName);
            }
            return buildingList;
        }

        public List<string> populateFacilityList()
        {//function to return a list of facilities and return it as a list of strings
            var db = new DatabaseContext();
            var facilityQry = from Facility in db.zFacility select Facility.FacilityName;
            return facilityQry.ToList();
        }

        public ViewResult Index(int? roomCap, string sortOrder)
        {
            ViewBag.RoomSortParm = String.IsNullOrEmpty(sortOrder) ? "roomCode_desc" : "";
            ViewBag.CapSortParm = sortOrder == "roomCap" ? "roomCap_desc" : "roomCap";
            ViewBag.ChosenSort = sortOrder;

            var roomQry = zroomRepository.AllIncluding(zroom => zroom.zFacility);
            if (roomCap.HasValue)
            {
                ViewBag.RoomCap = roomCap;
                roomQry = zroomRepository.AllIncluding(zroom => zroom.zFacility).Where(r => r.Capacity >= roomCap);
            }
            else
            {
                ViewBag.RoomCap = 100;
            }
            roomQry = SortRooms(roomQry, sortOrder);
            return View(roomQry);
        }

        public IQueryable<zRoom> SortRooms(IQueryable<zRoom> rooms, string sortOrder)
        {
            switch (sortOrder)
            {
                case "roomCap":
                    rooms = rooms.OrderBy(r => r.Capacity); break;
                case "roomCap_desc":
                    rooms = rooms.OrderByDescending(r => r.Capacity); break;
                case "roomCode_desc":
                    rooms = rooms.OrderByDescending(r => r.RoomCode); break;
                default:
                    rooms = rooms.OrderBy(r => r.RoomCode); break;
            }
            return (rooms);
        }

        //
        // GET: /zRooms/Details/5

        public ViewResult Details(int id)
        {
            return View(zroomRepository.Find(id));
        }

        //
        // GET: /zRooms/Create


        public ActionResult Create()
        {
            string accName = User.Identity.Name;
            if (accName == "CA")
            {
                ViewBag.BuildingList = populateBuildingList();
                ViewBag.facilityList = populateFacilityList();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        } 

        //
        // POST: /zRooms/Create

        [HttpPost]
        public ActionResult Create(string Building, string[] Facilities, zRoom zroom)
        {
            var db = new DatabaseContext();

            zroom.Private = false;
            zroom.ImgLink = "";

            var buildingCode = Building.Split(' ').FirstOrDefault();
            zroom.BuildingId = (from buildings in db.zBuilding where buildings.BuildingCode == buildingCode select buildings.BuildingId).FirstOrDefault();

            if (Facilities != null)
            {
                foreach (var facility in Facilities)
                {
                    var facName = facility.Replace(".", " ");
                    //this is to fix a bug where spaces caused a bug so i replaced the spaces with .'s
                    //and converted back when i need to use them
                    //initial replacement is in the razor
                    var facDB = (from fac in db.zFacility where fac.FacilityName == facName select fac).FirstOrDefault();
                    var item = new zFacility();
                    item.FacilityId = facDB.FacilityId;
                    item.FacilityName = facDB.FacilityName;
                    zroom.zFacility.Add(item);
                }
            }
            //load facilities

            zroomRepository.InsertOrUpdate(zroom);
            zroomRepository.Save();
            return RedirectToAction("Index");
            /*if (ModelState.IsValid) {
                zroomRepository.InsertOrUpdate(zroom);
                zroomRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}*/
        }
        
        //
        // GET: /zRooms/Edit/5
 
        public ActionResult Edit(int id)
        {
            string accName = User.Identity.Name;
            if (accName == "CA")
            {
                var zroom = zroomRepository.Find(id);
                var building = zroom.zBuilding.BuildingCode + " - " + zroom.zBuilding.BuildingName;
                ViewBag.Building = new SelectList(populateBuildingList(), building);

                ViewBag.facilityList = populateFacilityList();
                List<string> selectedFacilities = new List<string>();
                foreach (var facility in zroom.zFacility)
                {
                    selectedFacilities.Add(facility.FacilityName);
                }
                ViewBag.selectedFacilities = selectedFacilities;

                return View(zroomRepository.Find(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /zRooms/Edit/5

        [HttpPost]
        public ActionResult Edit(string Building, string[] Facilities, zRoom zroom)
        {
            var db = new DatabaseContext();

            var buildingCode = Building.Split(' ').FirstOrDefault();
            zroom.BuildingId = (from buildings in db.zBuilding where buildings.BuildingCode == buildingCode select buildings.BuildingId).FirstOrDefault();

            if (Facilities != null)
            {
                foreach (var facility in Facilities)
                {
                    var facName = facility.Replace(".", " ");
                    //this is to fix a bug where spaces caused a bug so i replaced the spaces with .'s
                    //and converted back when i need to use them
                    //initial replacement is in the razor
                    var facDB = (from fac in db.zFacility where fac.FacilityName == facName select fac).FirstOrDefault();
                    var item = new zFacility();
                    item.FacilityId = facDB.FacilityId;
                    item.FacilityName = facDB.FacilityName;
                    zroom.zFacility.Add(item);
                }
            }

            zroomRepository.InsertOrUpdate(zroom);
            zroomRepository.Save();
            return RedirectToAction("Index");
            /*if (ModelState.IsValid) {
                zroomRepository.InsertOrUpdate(zroom);
                zroomRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}*/
        }

        //
        // GET: /zRooms/Delete/5
 
        public ActionResult Delete(int id)
        {
            string accName = User.Identity.Name;
            if (accName == "CA")
            {
                return View(zroomRepository.Find(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /zRooms/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            zroomRepository.Delete(id);
            zroomRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                zroomRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

