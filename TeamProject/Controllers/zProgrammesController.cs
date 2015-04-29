using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject;
using TeamProject.Models;

namespace TeamProject.Controllers
{   
    public class zProgrammesController : Controller
    {
		private readonly IzProgrammeRepository zprogrammeRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public zProgrammesController() : this(new zProgrammeRepository())
        {
        }

        public zProgrammesController(IzProgrammeRepository zprogrammeRepository)
        {
			this.zprogrammeRepository = zprogrammeRepository;
        }

        //
        // GET: /zProgrammes/

        public ViewResult Index()
        {
            return View(zprogrammeRepository.All);
        }

        //
        // GET: /zProgrammes/Details/5

        public ViewResult Details(string id)
        {
            return View(zprogrammeRepository.Find(id));
        }

        //
        // GET: /zProgrammes/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /zProgrammes/Create

        [HttpPost]
        public ActionResult Create(zProgramme zprogramme)
        {
            if (ModelState.IsValid) {
                zprogrammeRepository.InsertOrUpdate(zprogramme);
                zprogrammeRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /zProgrammes/Edit/5
 
        public ActionResult Edit(string id)
        {
             return View(zprogrammeRepository.Find(id));
        }

        //
        // POST: /zProgrammes/Edit/5

        [HttpPost]
        public ActionResult Edit(zProgramme zprogramme)
        {
            if (ModelState.IsValid) {
                zprogrammeRepository.InsertOrUpdate(zprogramme);
                zprogrammeRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /zProgrammes/Delete/5
 
        public ActionResult Delete(string id)
        {
            return View(zprogrammeRepository.Find(id));
        }

        //
        // POST: /zProgrammes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string id)
        {
            zprogrammeRepository.Delete(id);
            zprogrammeRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                zprogrammeRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

