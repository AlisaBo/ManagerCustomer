using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CustomerManager.Models;
using CustomerManager.Attributes;
using Microsoft.AspNet.Identity;

namespace CustomerManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "Customer")]
    public class DepartamentsController : Controller
    {
        private CustomersContext db = new CustomersContext();

        // GET: Departaments
        public ActionResult Index()
        {
            var username = User.Identity.GetUserName();

            var listOfDepartamets = db.Departament.Where(d => d.Customer.Name == username).Include(manager => manager.Manager).ToList();

            return View(listOfDepartamets);
        }

        // GET: Departaments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departament departament = db.Departament.Include(manager => manager.Manager).Where(d=>d.DepartamentId==id).FirstOrDefault();
            if (departament == null)
            {
                return HttpNotFound();
            }
            return View(departament);
        }

        // GET: Departaments/Create
        public ActionResult Create()
        {
            var departament = new Departament();

            var username = User.Identity.GetUserName();

            departament.Customer = (from customer in db.CustomerInformation
                                    where customer.Name == username
                                    select customer).FirstOrDefault();


            var listOfUsers = (from loginUser in db.LoginUser
                               where loginUser.Customer.Name == username
                               select loginUser).ToList().Select(u => new SelectListItem
                               {
                                   Text = u.Name,
                                   Value = u.Name
                               });

            ViewBag.ListOfUsers = listOfUsers;

            return View();
        }

        // POST: Departaments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartamentId,Name,Address,ManagerName")] Departament departament)
        {
            if (ModelState.IsValid)
            {
                var username = User.Identity.GetUserName();

                departament.Customer = (from customer in db.CustomerInformation
                                        where customer.Name == username
                                        select customer).FirstOrDefault();

                departament.Manager = (from users in db.LoginUser
                                       where users.Name == departament.ManagerName
                                       select users).FirstOrDefault();

                db.Departament.Add(departament);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(departament);
        }

        // GET: Departaments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Departament departament = db.Departament.Find(id);
            if (departament == null)
            {
                return HttpNotFound();
            }

            var username = User.Identity.GetUserName();

            departament.Customer = (from customer in db.CustomerInformation
                                    where customer.Name == username
                                    select customer).FirstOrDefault();


            var listOfUsers = (from users in db.LoginUser
                               where users.Customer.Name == username
                               select users).ToList().Select(u => new SelectListItem
                               {
                                   Text = u.Name,
                                   Value = u.Name
                               });

            ViewBag.ListOfUsers = listOfUsers;

            return View(departament);
        }

        // POST: Departaments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartamentId,Name,Address,ManagerName")] Departament departament)
        {
            if (ModelState.IsValid)
            {
                departament.Manager = (from users in db.LoginUser
                                       where users.Name == departament.ManagerName
                                       select users).FirstOrDefault();
                try
                {
                    //I admit that it's not legal
                    var sqlCommand = String.Format("update dbo.Departaments set Manager_LoginUserId = {0} where DepartamentId = {1}", departament.Manager.LoginUserId, departament.DepartamentId);
                    db.Database.ExecuteSqlCommand(sqlCommand);
                    db.Entry(departament).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("FullName", "Query error");
                    return View(departament);
                }
            }

            return View(departament);
        }

        // GET: Departaments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            Departament departament = db.Departament.Where(c => c.DepartamentId == id).Include(m => m.Manager).FirstOrDefault();
            if (departament == null)
            {
                return HttpNotFound();
            }
            return View(departament);
        }

        // POST: Departaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Departament departament = db.Departament.Find(id);
            db.Departament.Remove(departament);
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
