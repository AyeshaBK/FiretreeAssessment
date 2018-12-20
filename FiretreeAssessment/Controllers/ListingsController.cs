using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FiretreeAssessment.Models;
using PagedList;

namespace FiretreeAssessment.Controllers
{
    public class ListingsController : Controller
    {
        private PlacesDBEntities db = new PlacesDBEntities();
        
        // GET: Listings
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.SuburbSortParm = String.IsNullOrEmpty(sortOrder) ? "suburb_desc" : "";

            var listing = from l in db.Listings
                       select l;
            switch (sortOrder)
            {
                case "name_desc":
                    listing = listing.OrderByDescending(s => s.Suburb);
                    break;
                default:
                    listing = listing.OrderBy(s => s.Suburb);
                    break;
            }

            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(listing.ToPagedList(pageNumber, pageSize));
       
        }

        public ActionResult Search()
        {
            var suburbLst = new List<string>();
            var SuburbQry = from s in db.Listings
                            orderby s.Suburb
                            select s.Suburb;
            suburbLst.AddRange(SuburbQry.Distinct());
            ViewBag.suburb = new SelectList(suburbLst);

            var priceLst = new List<string>();
            var PriceQry = from p in db.Listings
                           orderby p.Price
                           select p.Price.ToString();
            priceLst.AddRange(PriceQry.Distinct());
            ViewBag.price = new SelectList(priceLst);

            var bedLst = new List<string>();
            var BedQry = from b in db.Listings
                         orderby b.Bedrooms
                         select b.Bedrooms.ToString();
            bedLst.AddRange(BedQry.Distinct());
            ViewBag.beds = new SelectList(bedLst);

            return View();
        }

        [HttpPost]
        public ActionResult Search(string suburb, double? price, int? beds, string sortOrder, int? page)
        {
            var listings = (from l in db.Listings
                          select l).ToList();

            if (suburb != null && price.ToString() != null && beds.ToString() != null)
            {
                listings = listings.Where(x => x.Suburb.Contains(suburb) && x.Price.ToString().Contains(price.ToString()) && x.Bedrooms.ToString().Contains(beds.ToString())).ToList();
            }

            if (suburb != null && price.ToString() == null && beds.ToString() == null)
            {
                listings = listings.Where(x => x.Suburb.Contains(suburb)).ToList();
            }

            if (suburb == null && price.ToString() != null && beds.ToString() == null)
            {
                listings = listings.Where(x => x.Price.ToString().Contains(price.ToString())).ToList();
            }

            if (suburb == null && price.ToString() == null && beds.ToString() != null)
            {
                listings.ToList();
            }

            else
            {
                listings.ToList();
            }

            Session["search"] = listings.ToList();
            Session["count"] = listings.Count();

            if (Convert.ToInt16(Session["count"]) <= 0)
            {
                ViewBag.message = "No matching results found.";
            }

            ViewBag.SuburbSortParm = String.IsNullOrEmpty(sortOrder) ? "suburb_desc" : "";
            
            switch (sortOrder)
            {
                case "name_desc":
                    listings = listings.ToList();
                    break;
                default:
                    listings = listings.ToList();
                    break;
            }

            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View("Results", listings.ToPagedList(pageNumber, pageSize));
            
        }

        public ActionResult Results(string sortOrder, int? page)
        {
            ViewBag.SuburbSortParm = String.IsNullOrEmpty(sortOrder) ? "suburb_desc" : "";

            var listing = from l in db.Listings
                          select l;
            switch (sortOrder)
            {
                case "name_desc":
                    listing = listing.OrderByDescending(s => s.Suburb);
                    break;
                default:
                    listing = listing.OrderBy(s => s.Suburb);
                    break;
            }

            int pageNumber = (page ?? 1);
            int pageSize = 3;
            return View(listing.ToPagedList(pageNumber, pageSize));

        }

        // GET: Listings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);

            var agentId = listing.AgentId;
            var agent = db.Agents.Where(x => x.AgentId == agentId).FirstOrDefault();

            Session["agent"] = agent.Email.ToString();

            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        // GET: Leads/Create
        [ChildActionOnly]
        public ActionResult Contact()
        {
            ViewBag.agent = Session["agent"];
            return PartialView("_Contact");
        }

        // POST: Leads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Contact(Lead lead)
        {
            //CONTACT FORM
            //COMPILE EMAIL
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(Session["agent"].ToString()));  // recipients email
            message.From = new MailAddress(lead.Email);  // sender email
            message.Subject = "Query";
            message.Body = string.Format(body, lead.FirstName, lead.Email, lead.Message);
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "doej03037@gmail.com",  
                    Password = "janesgmail00"
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                await smtp.SendMailAsync(message);

                //SAVE TO LEAD TABLE
                db.Leads.Add(lead);
                db.SaveChanges();
                return RedirectToAction("Success");
            }

            //return View(lead);
        }

        public ActionResult Success()
        {
            return View();
        }

            // GET: Listings/Create
            public ActionResult Create()
        {
            var emailLst = new List<string>();
            var EmailQry = from e in db.Agents
                           orderby e.Email
                           select e.Email;
            emailLst.AddRange(EmailQry.Distinct());
            ViewBag.email = new SelectList(emailLst);

            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Listing listing)
        {
            //UPLOAD IMAGE
            string filename = Path.GetFileNameWithoutExtension(listing.ImageFile.FileName);
            string extension = Path.GetExtension(listing.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            listing.ImagePath = "~/Images/" + filename;
            filename = Path.Combine(Server.MapPath("~/Images"), filename);
            listing.ImageFile.SaveAs(filename);

            //GET AGENT
            var agent = db.Agents.Where(x => x.Email == listing.Email).FirstOrDefault();
            listing.AgentId = agent.AgentId;
            
            using (PlacesDBEntities db = new PlacesDBEntities())
            {
                //SAVE TO DB
                db.Listings.Add(listing);
                db.SaveChanges();
            }

            ModelState.Clear();
            return RedirectToAction("Index");
            
        }


        // GET: Listings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            ViewBag.AgentId = new SelectList(db.Agents, "AgentId", "ImagePath", listing.AgentId);
            return View(listing);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ListingId,ImagePath,Price,Bedrooms,RefNumber,Description,Suburb,AgentId,MarketingHeading")] Listing listing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listing).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AgentId = new SelectList(db.Agents, "AgentId", "ImagePath", listing.AgentId);
            return View(listing);
        }

        // GET: Listings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Listing listing = db.Listings.Find(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Listing listing = db.Listings.Find(id);
            db.Listings.Remove(listing);
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
