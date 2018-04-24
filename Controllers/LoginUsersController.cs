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
using Microsoft.AspNet.Identity.EntityFramework;
using CustomerManager.Helpers;

namespace CustomerManager.Controllers
{
    [AccessDeniedAuthorize(Roles = "Customer")]
    public class LoginUsersController : Controller
    {
        private CustomersContext db = new CustomersContext();

        // GET: LoginUsers
        public ActionResult Index()
        {
            var username = User.Identity.GetUserName();
         
            var listOfUsersForCustomer = db.LoginUser.Where(u => u.Customer.Name == username).Include(u => u.Departament).ToList();

            return View(listOfUsersForCustomer);
        }

        // GET: LoginUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            LoginUser loginUser = db.LoginUser.Where(u => u.LoginUserId == id).Include(u => u.Departament).FirstOrDefault();
            if (loginUser == null)
            {
                return HttpNotFound();
            }
            return View(loginUser);
        }

        // GET: LoginUsers/Create
        public ActionResult Create()
        {
            var loginUser = new LoginUser();

            var username = User.Identity.GetUserName();

            loginUser.Customer = (from customer in db.CustomerInformation
                                  where customer.Name == username
                                  select customer).FirstOrDefault();


            var listOfDepartamets = (from departament in db.Departament
                                     where departament.Customer.Name == username
                                     select departament).ToList().Select(u => new SelectListItem
                                     {
                                         Text = u.Name,
                                         Value = u.Name
                                     });

            ViewBag.ListOfDepartaments = listOfDepartamets;
            return View(loginUser);
        }

        // POST: LoginUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LoginUserId,Name,Mobile,Mail,UserName,Password,OldName,Departament,DepartamentName")] LoginUser loginUser)
        {
            var username = User.Identity.GetUserName();

            if (ModelState.IsValid)
            {
                UserActionsHelper.CreateUser(loginUser.UserName, loginUser.Mail, loginUser.Password, "User");
                loginUser.OldName = loginUser.UserName;

                loginUser.Customer = (from customer in db.CustomerInformation
                                      where customer.Name == username
                                      select customer).FirstOrDefault();

                loginUser.Departament = (from departament in db.Departament
                                         where departament.Name == loginUser.DepartamentName
                                         select departament).FirstOrDefault();

                db.LoginUser.Add(loginUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var listOfDepartamets = (from departament in db.Departament
                                     where departament.Customer.Name == username
                                     select departament).ToList().Select(u => new SelectListItem
                                     {
                                         Text = u.Name,
                                         Value = u.DepartamentId.ToString()
                                     });

            ViewBag.ListOfDepartaments = listOfDepartamets;

            return View(loginUser);
        }

        // GET: LoginUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginUser loginUser = db.LoginUser.Find(id);
            loginUser.OldName = loginUser.UserName;
            if (loginUser == null)
            {
                return HttpNotFound();
            }

            var username = User.Identity.GetUserName();

            loginUser.Customer = (from customer in db.CustomerInformation
                                  where customer.Name == username
                                  select customer).FirstOrDefault();


            var listOfDepartamets = (from departament in db.Departament
                                     where departament.Customer.Name == username
                                     select departament).ToList().Select(u => new SelectListItem
                                     {
                                         Text = u.Name,
                                         Value = u.Name
                                     });

            ViewBag.ListOfDepartaments = listOfDepartamets;

            return View(loginUser);
        }

        // POST: LoginUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoginUserId,Name,Mobile,Mail,UserName,Password,OldName,Departament,DepartamentName")] LoginUser loginUser)
        {
            var username = User.Identity.GetUserName();
            if (ModelState.IsValid)
            {
                UserActionsHelper.ChangeUserData(loginUser.OldName, loginUser.UserName, loginUser.Mail, loginUser.Password);
                loginUser.OldName = loginUser.UserName;

                loginUser.Departament = (from departament in db.Departament
                                         where departament.Name == loginUser.DepartamentName
                                         select departament).FirstOrDefault();

                try
                {
                    //I admit that it's not legal
                    var sqlCommand = String.Format("update dbo.LoginUsers set Departament_DepartamentId = {0} where LoginUserId = {1}", loginUser.Departament.DepartamentId, loginUser.LoginUserId);
                    db.Database.ExecuteSqlCommand(sqlCommand);
                    db.Entry(loginUser).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    ModelState.AddModelError("FullName", "Query error");
                    return View(loginUser);
                }
            }
            var listOfDepartamets = (from departament in db.Departament
                                     where departament.Customer.Name == username
                                     select departament).ToList().Select(u => new SelectListItem
                                     {
                                         Text = u.Name,
                                         Value = u.DepartamentId.ToString()
                                     });

            ViewBag.ListOfDepartaments = listOfDepartamets;
            return View(loginUser);
        }

        // GET: LoginUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoginUser loginUser = db.LoginUser.Where(u => u.LoginUserId == id).Include(u => u.Departament).FirstOrDefault();
            if (loginUser == null)
            {
                return HttpNotFound();
            }
            return View(loginUser);
        }

        // POST: LoginUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoginUser loginUser = db.LoginUser.Include(c => c.Departament).FirstOrDefault(c => c.LoginUserId == id);
            try
            {
                db.LoginUser.Remove(loginUser);
                db.SaveChanges();
            }
            catch
            {
                ModelState.AddModelError("FullName", "You should remove all dependencies first");
                return View(loginUser);
            }
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
