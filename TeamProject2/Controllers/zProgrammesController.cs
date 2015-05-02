using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeamProject2;
using TeamProject2.Models;

namespace TeamProject2.Controllers
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

        public ViewResult Details(int id)
        {
            return View(zprogrammeRepository.Find(id));
        }

        public List<string> populateModuleList()
        {//function to return a list of modules and return it as a list of strings
            var db = new DatabaseContext();
            var moduleList = new List<string>();
            var moduleQry = db.zModule.Select(module => new { module.ModCode, module.ModTitle }).ToList();
            foreach (var module in moduleQry)
            {
                string output = module.ModCode + " - " + module.ModTitle;
                moduleList.Add(output);
            }
            

            return moduleList;
        }

        public List<string> populateDeptList()
        { //function to return a list of departments and return it as a list of strings
            var db = new DatabaseContext();
            var deptQry = from Department in db.zDepartment select Department.DeptCode;
            return deptQry.ToList();
        }

        public List<string> populatePartList()
        { //function to return a list of departments and return it as a list of strings
            var db = new DatabaseContext();
            var partList = new List<string>();
            partList.Add("A");
            partList.Add("B");
            partList.Add("C");
            partList.Add("D");
            partList.Add("F");
            partList.Add("I");
            partList.Add("P");
            return partList;
        }

        //
        // GET: /zProgrammes/Create

        public ActionResult Create()
        {
            ViewBag.ModList = new SelectList(populateModuleList());
            
            ViewBag.deptList = populateDeptList();
            ViewBag.partList = populatePartList();
            return View();
        } 

        //
        // POST: /zProgrammes/Create

        [HttpPost]
        public ActionResult Create(string Modules, string DeptCode, string Part, zProgramme zprogramme)
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
                zprogramme.zModule.Add(item);
            }
            zprogrammeRepository.InsertOrUpdate(zprogramme);
            zprogrammeRepository.Save();
            return RedirectToAction("Index");
        }
        
        //
        // GET: /zProgrammes/Edit/5
 
        public ActionResult Edit(int id)
        {
            var zprogramme = zprogrammeRepository.Find(id);
            List<string> selectedModules = new List<string>();
            List<string> selectedModulesDisp = new List<string>();
            foreach (var module in zprogramme.zModule)
            {
                selectedModules.Add(module.ModCode);
                selectedModulesDisp.Add(module.ModCode + " - " + module.ModTitle);
            }
            ViewBag.ModList = new SelectList(populateModuleList());
            ViewBag.Modules = String.Join(",", selectedModules.ToArray());
            ViewBag.ModDropDown = selectedModulesDisp;
            ViewBag.deptList = new SelectList(populateDeptList(), zprogramme.DeptCode);
            ViewBag.partList = new SelectList(populatePartList(), zprogramme.Part);

            return View(zprogrammeRepository.Find(id));
        }

        //
        // POST: /zProgrammes/Edit/5

        [HttpPost]
        public ActionResult Edit(string Modules, string partList, string deptList, zProgramme zprogramme)
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
                zprogramme.zModule.Add(item);
            }
            zprogramme.Part = partList;
            zprogramme.DeptCode = deptList;

            zprogrammeRepository.InsertOrUpdate(zprogramme);
            zprogrammeRepository.Save();
            return RedirectToAction("Index");
        }

        //
        // GET: /zProgrammes/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View(zprogrammeRepository.Find(id));
        }

        //
        // POST: /zProgrammes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
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

