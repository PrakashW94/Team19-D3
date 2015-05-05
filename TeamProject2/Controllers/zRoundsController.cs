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
    public class zRoundsController : Controller
    {
		private readonly IzRoundRepository zroundRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public zRoundsController() : this(new zRoundRepository())
        {
        }

        public zRoundsController(IzRoundRepository zroundRepository)
        {
			this.zroundRepository = zroundRepository;
        }

        //
        // GET: /zRounds/

        public ViewResult Index()
        {
            return View(zroundRepository.All);
        }

        //
        // GET: /zRounds/Details/5

        public ViewResult Details(short id)
        {
            return View(zroundRepository.Find(id));
        }

        //
        // GET: /zRounds/Create

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
        // POST: /zRounds/Create

        [HttpPost]
        public ActionResult Create(zRound zround)
        {
            if (ModelState.IsValid) {
                zroundRepository.InsertOrUpdate(zround);
                zroundRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }
        
        //
        // GET: /zRounds/Edit/5
 
        public ActionResult Edit(short id)
        {
            string accName = User.Identity.Name;
            if (accName == "CA")
            {
                return View(zroundRepository.Find(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            } 
            
        }

        //
        // POST: /zRounds/Edit/5

        [HttpPost]
        public ActionResult Edit(zRound zround)
        {
            if (ModelState.IsValid) {
                zroundRepository.InsertOrUpdate(zround);
                zroundRepository.Save();
                return RedirectToAction("Index");
            } else {
				return View();
			}
        }

        //
        // GET: /zRounds/Delete/5
 
        public ActionResult Delete(short id)
        {
            string accName = User.Identity.Name;
            if (accName == "CA")
            {
                return View(zroundRepository.Find(id));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //
        // POST: /zRounds/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(short id)
        {
            zroundRepository.Delete(id);
            zroundRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                zroundRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

