using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TeamProject;
using TeamProject.Models;

namespace TeamProject.Controllers
{
    [Authorize]
    public class zFacilitiesController : Controller
    {
		private readonly IzFacilityRepository zfacilityRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public zFacilitiesController() : this(new zFacilityRepository())
        {
        }

        public zFacilitiesController(IzFacilityRepository zfacilityRepository)
        {
			this.zfacilityRepository = zfacilityRepository;
        }

        //
        // GET: /zFacilities/

        public ViewResult Index()
        {
            return View(zfacilityRepository.All);
        }

        //
        // GET: /zFacilities/Details/5

        public ViewResult Details(short id)
        {
            return View(zfacilityRepository.Find(id));
        }

        //
        // GET: /zFacilities/Create

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
        // POST: /zFacilities/Create

        [HttpPost]
        public ActionResult Create(zFacility zfacility)
        {
            if (ModelState.IsValid) {
                zfacilityRepository.InsertOrUpdate(zfacility);
                zfacilityRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /zFacilities/Edit/5
 
        public ActionResult Edit(short id)
        {
            string accName = User.Identity.Name;
            if (accName == "CA")
            {
                return View(zfacilityRepository.Find(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            } 
            
        }

        //
        // POST: /zFacilities/Edit/5

        [HttpPost]
        public ActionResult Edit(zFacility zfacility)
        {
            if (ModelState.IsValid) {
                zfacilityRepository.InsertOrUpdate(zfacility);
                zfacilityRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /zFacilities/Delete/5
 
        public ActionResult Delete(short id)
        {
            string accName = User.Identity.Name;
            if (accName == "CA")
            {
                return View(zfacilityRepository.Find(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            } 
        }

        //
        // POST: /zFacilities/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            zfacilityRepository.Delete(id);
            zfacilityRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                zfacilityRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

