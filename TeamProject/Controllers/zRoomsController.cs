using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject;
using TeamProject.Models;

namespace TeamProject.Controllers
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

        public ViewResult Index()
        {
            return View(zroomRepository.AllIncluding(zroom => zroom.zFacility));
        }

        //
        // GET: /zRooms/Details/5

        public ViewResult Details(string id)
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
        public ActionResult Create(zRoom zroom)
        {
            if (ModelState.IsValid) {
                zroomRepository.InsertOrUpdate(zroom);
                zroomRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /zRooms/Edit/5
 
        public ActionResult Edit(string id)
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
        // POST: /zRooms/Edit/5

        [HttpPost]
        public ActionResult Edit(zRoom zroom)
        {
            if (ModelState.IsValid) {
                zroomRepository.InsertOrUpdate(zroom);
                zroomRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /zRooms/Delete/5
 
        public ActionResult Delete(string id)
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
        public ActionResult DeleteConfirmed(string id)
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

