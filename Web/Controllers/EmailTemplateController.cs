using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace mojoPortal.Web.Controllers
{
    public class EmailTemplateController : Controller
    {
        // GET: EmailTemplate
        public ActionResult Index()
        {
            return View();
        }

        // GET: EmailTemplate/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: EmailTemplate/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmailTemplate/Create
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

        // GET: EmailTemplate/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: EmailTemplate/Edit/5
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

        // GET: EmailTemplate/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: EmailTemplate/Delete/5
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
