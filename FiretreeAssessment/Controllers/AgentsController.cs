using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FiretreeAssessment.Models;
using PagedList;

namespace FiretreeAssessment.Controllers
{
    public class AgentsController : Controller
    {
        private PlacesDBEntities db = new PlacesDBEntities();

        // GET: Agents
        public ActionResult Index(string sortOrder, int? page)
        {
            //SORTING AND PAGING
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var agent = from a in db.Agents
                        select a;
            switch (sortOrder)
            {
                case "name_desc":
                    agent = agent.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    agent = agent.OrderBy(s => s.FirstName);
                    break;
            }
            
            
            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(agent.ToPagedList(pageNumber, pageSize));
        }

        // GET: Agents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);

            //using (PlacesDBEntities db = new PlacesDBEntities())
            //{
            //    agent = db.Agents.Where(x => x.AgentId == id).FirstOrDefault();
            //}

            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // GET: Agents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Agent agent)
        {
            //UPLOAD IMAGE
            string filename = Path.GetFileNameWithoutExtension(agent.ImageFile.FileName);
            string extension = Path.GetExtension(agent.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            agent.ImagePath = "~/Images/" + filename;
            filename = Path.Combine(Server.MapPath("~/Images"), filename);
            agent.ImageFile.SaveAs(filename);

            using (PlacesDBEntities db =  new PlacesDBEntities())
            {
                //SAVE TO DB
                db.Agents.Add(agent);
                db.SaveChanges();
            }

            ModelState.Clear();
            return RedirectToAction("Index");
           
        }

        // GET: Agents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // POST: Agents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgentId,ImagePath,FirstName,LastName,Cellphone,Email")] Agent agent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agent);
        }

        // GET: Agents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent agent = db.Agents.Find(id);
            if (agent == null)
            {
                return HttpNotFound();
            }
            return View(agent);
        }

        // POST: Agents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Agent agent = db.Agents.Find(id);
            db.Agents.Remove(agent);
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
