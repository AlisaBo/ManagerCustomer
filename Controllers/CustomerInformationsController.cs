using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManager.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using CustomerManager.Helpers;
using System.Security.Claims;
using CustomerManager.Attributes;

namespace CustomerManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class CustomerInformationsController : Controller
    {
        private CustomersContext db = new CustomersContext();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: CustomerInformations
        public ActionResult Index()
        {
            return View(db.CustomerInformation.ToList());
        }

        // GET: CustomerInformations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInformation customerInformation = db.CustomerInformation.Find(id);
            if (customerInformation == null)
            {
                return HttpNotFound();
            }
            return View(customerInformation);
        }

        // GET: CustomerInformations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerInformations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> Create([Bind(Include = "CustomerId,Name,Address,Email,Phone,Password,Comments,IsMunicipalityCustomer,NumberOfSchools,OldName")] CustomerInformation customerInformation)
        {
            if (ModelState.IsValid)
            {
                UserActionsHelper.CreateUser(customerInformation.Name, customerInformation.Email, customerInformation.Password, "Customer");

                customerInformation.OldName = customerInformation.Name;

                db.CustomerInformation.Add(customerInformation);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(customerInformation);
        }

        // GET: CustomerInformations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInformation customerInformation = db.CustomerInformation.Find(id);
            customerInformation.OldName = customerInformation.Name;
            if (customerInformation == null)
            {
                return HttpNotFound();
            }

            return View(customerInformation);
        }

        // POST: CustomerInformations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,Name,Address,Email,Phone,Password,Comments,IsMunicipalityCustomer,NumberOfSchools,OldName")] CustomerInformation customerInformation)
        {
            if (ModelState.IsValid)
            {
                UserActionsHelper.ChangeUserData(customerInformation.OldName, customerInformation.Name, customerInformation.Email, customerInformation.Password);
                customerInformation.OldName = customerInformation.Name;

                db.Entry(customerInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(customerInformation);
        }

        // GET: CustomerInformations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerInformation customerInformation = db.CustomerInformation.Find(id);
            if (customerInformation == null)
            {
                return HttpNotFound();
            }
            return View(customerInformation);
        }

        // POST: CustomerInformations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerInformation customerInformation = db.CustomerInformation.Include(c=>c.LoginUsers).
                Include(c=>c.ContactsDetails).Include(c=>c.Departaments).FirstOrDefault(c=>c.CustomerId==id);

            db.CustomerInformation.Remove(customerInformation);
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
