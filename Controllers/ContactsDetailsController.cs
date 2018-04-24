using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManager.Models;
using System.Web.Security;
using CustomerManager.Attributes;
using Microsoft.AspNet.Identity;

namespace CustomerManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "Customer")]
    public class ContactsDetailsController : Controller
    {
        private CustomersContext db = new CustomersContext();

        // GET: ContactsDetails
        public ActionResult Index()
        {
            var username = User.Identity.GetUserName();

            var listOfContactsForCustomer = (from user in db.ContactsDetail
                                          where user.CustomerInformation.Name == username
                                          select user).ToList();

            return View(db.ContactsDetail.ToList());
        }

        // GET: ContactsDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactsDetail contactsDetail = db.ContactsDetail.Find(id);
            if (contactsDetail == null)
            {
                return HttpNotFound();
            }
            return View(contactsDetail);
        }

        // GET: ContactsDetails/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ContactsDetails/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ContactDetailId,Name,Role,Phone,Mail")] ContactsDetail contactsDetail)
        {
            if (ModelState.IsValid)
            {
                db.ContactsDetail.Add(contactsDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contactsDetail);
        }

        // GET: ContactsDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactsDetail contactsDetail = db.ContactsDetail.Find(id);
            if (contactsDetail == null)
            {
                return HttpNotFound();
            }
            return View(contactsDetail);
        }

        // POST: ContactsDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ContactDetailId,Name,Role,Phone,Mail")] ContactsDetail contactsDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactsDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contactsDetail);
        }

        // GET: ContactsDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactsDetail contactsDetail = db.ContactsDetail.Find(id);
            if (contactsDetail == null)
            {
                return HttpNotFound();
            }
            return View(contactsDetail);
        }

        // POST: ContactsDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ContactsDetail contactsDetail = db.ContactsDetail.Find(id);
            db.ContactsDetail.Remove(contactsDetail);
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
