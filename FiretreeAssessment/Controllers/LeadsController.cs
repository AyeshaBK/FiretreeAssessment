using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FiretreeAssessment.Models;
using PagedList;

namespace FiretreeAssessment.Controllers
{
    public class LeadsController : Controller
    {
        private PlacesDBEntities db = new PlacesDBEntities();
        
        // GET: Leads
        public ActionResult Index(string sortOrder, int? page)
        {
            //PAGING AND SORTING
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var lead = from l in db.Leads
                        select l;
            switch (sortOrder)
            {
                case "name_desc":
                    lead = lead.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    lead = lead.OrderBy(s => s.FirstName);
                    break;
            }
            
            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(lead.ToPagedList(pageNumber, pageSize));
        }

        // GET: Leads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead lead = db.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        // GET: Leads/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeadId,FirstName,LastName,Email,Message")] Lead lead)
        {
            if (ModelState.IsValid)
            {
                db.Leads.Add(lead);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lead);
        }

        // GET: Leads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead lead = db.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        // POST: Leads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeadId,FirstName,LastName,Email,Message")] Lead lead)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lead).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lead);
        }

        // GET: Leads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lead lead = db.Leads.Find(id);
            if (lead == null)
            {
                return HttpNotFound();
            }
            return View(lead);
        }

        // POST: Leads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Lead lead = db.Leads.Find(id);
            db.Leads.Remove(lead);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
