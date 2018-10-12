using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WSLogin.Controllers
{
    public class WorkbenchController : Controller
    {
        //
		 // GET: /Workbench/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /workbench/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /workbench/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /workbench/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /workbench/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /workbench/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /workbench/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /workbench/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
