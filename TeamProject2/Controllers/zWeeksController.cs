using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject2;
using TeamProject2.Models;

namespace TeamProject2.Controllers
{   
    public class zWeeksController : Controller
    {
		private readonly IzWeekRepository zweekRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public zWeeksController() : this(new zWeekRepository())
        {
        }

        public zWeeksController(IzWeekRepository zweekRepository)
        {
			this.zweekRepository = zweekRepository;
        }

        //
        // GET: /zWeeks/

        public ViewResult Index()
        {
            return View(zweekRepository.AllIncluding(zweek => zweek.zRequest));
        }

        //
        // GET: /zWeeks/Details/5

        public ViewResult Details(int id)
        {
            return View(zweekRepository.Find(id));
        }

        //
        // GET: /zWeeks/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /zWeeks/Create

        [HttpPost]
        public ActionResult Create(zWeek zweek)
        {
            if (ModelState.IsValid) {
                zweekRepository.InsertOrUpdate(zweek);
                zweekRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /zWeeks/Edit/5
 
        public ActionResult Edit(int id)
        {
             return View(zweekRepository.Find(id));
        }

        //
        // POST: /zWeeks/Edit/5

        [HttpPost]
        public ActionResult Edit(zWeek zweek)
        {
            if (ModelState.IsValid) {
                zweekRepository.InsertOrUpdate(zweek);
                zweekRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /zWeeks/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(zweekRepository.Find(id));
        }

        //
        // POST: /zWeeks/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            zweekRepository.Delete(id);
            zweekRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                zweekRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

