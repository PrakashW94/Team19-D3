using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject;
using TeamProject.Models;

namespace TeamProject.Controllers
{   
    public class zLecturersController : Controller
    {
		private readonly IzLecturerRepository zlecturerRepository;

		// If you are using Dependency Injection, you can delete the following constructor
        public zLecturersController() : this(new zLecturerRepository())
        {
        }

        public zLecturersController(IzLecturerRepository zlecturerRepository)
        {
			this.zlecturerRepository = zlecturerRepository;
        }

        //
        // GET: /zLecturers/

        public ViewResult Index()
        {
            return View(zlecturerRepository.All);
        }

        //
        // GET: /zLecturers/Details/5

        public ViewResult Details(int id)
        {
            return View(zlecturerRepository.Find(id));
        }

        //
        // GET: /zLecturers/Create

        public List<string> populateModuleList()
        {//function to return a list of modules and return it as a list of strings
            var db = new DatabaseContext();
            var moduleList = new List<string>();
            if (User.Identity.Name == "CA")
            {// if the user is the central admin list all modules
                var moduleQry = db.zModule.Select(module => new { module.ModCode, module.ModTitle }).ToList();
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

        public ActionResult Create()
        {
            ViewBag.ModList = new SelectList(populateModuleList());
            return View();
        } 

        //
        // POST: /zLecturers/Create

        [HttpPost]
        public ActionResult Create(string Modules, zLecturer zlecturer)
        {
            /*
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
            */
            var db = new DatabaseContext();
            var ModList = Modules.Split(',').ToList();
            foreach (var module in ModList)
            {
                var selectedMod = (from mod in db.zModule where mod.ModCode == module select mod).FirstOrDefault();
                var item = new zModule();
                item.DeptCode = selectedMod.DeptCode;
                item.ModCode = selectedMod.ModCode;
                item.ModTitle = selectedMod.ModTitle;
                item.Part = selectedMod.Part;
                zlecturer.zModule.Add(item);
            }
            zlecturerRepository.InsertOrUpdate(zlecturer);
            zlecturerRepository.Save();
            return RedirectToAction("Index");

        }
        
        //
        // GET: /zLecturers/Edit/5
 
        public ActionResult Edit(int id)
        {
            var zlecturer = zlecturerRepository.Find(id);
            List<string> selectedModules = new List<string>();
            List<string> selectedModulesDisp = new List<string>();
            foreach (var module in zlecturer.zModule)
            {
                selectedModules.Add(module.ModCode);
                selectedModulesDisp.Add(module.ModCode + " - " + module.ModTitle);
            }
            ViewBag.ModList = new SelectList(populateModuleList());
            ViewBag.Modules = String.Join(",", selectedModules.ToArray());
            ViewBag.ModDropDown = selectedModulesDisp;
            return View(zlecturerRepository.Find(id));
        }

        //
        // POST: /zLecturers/Edit/5

        [HttpPost]
        public ActionResult Edit(string Modules, zLecturer zlecturer)
        {
            var db = new DatabaseContext();
            var ModList = Modules.Split(',').ToList();
            foreach (var module in ModList)
            {
                var selectedMod = (from mod in db.zModule where mod.ModCode == module select mod).FirstOrDefault();
                var item = new zModule();
                item.DeptCode = selectedMod.DeptCode;
                item.ModCode = selectedMod.ModCode;
                item.ModTitle = selectedMod.ModTitle;
                item.Part = selectedMod.Part;
                zlecturer.zModule.Add(item);
            }
            zlecturerRepository.InsertOrUpdate(zlecturer);
            zlecturerRepository.Save();
            return RedirectToAction("Index");

        }

        //
        // GET: /zLecturers/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(zlecturerRepository.Find(id));
        }

        //
        // POST: /zLecturers/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            zlecturerRepository.Delete(id);
            zlecturerRepository.Save();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                zlecturerRepository.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

