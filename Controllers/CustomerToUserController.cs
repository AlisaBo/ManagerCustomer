using CustomerManager.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CustomerManager.Controllers
{
    public class CustomerToUserController : Controller
    {
        private CustomersContext db = new CustomersContext();
        // GET: CustomerToUser
        public ActionResult Index()
        {
            var username = User.Identity.GetUserName();

            var loginDetails = db.LoginUser.Where(x => x.UserName == username).Include(y => y.Customer).FirstOrDefault();

            var customerToUserData = (from customer in db.CustomerInformation
                                      where customer.Name == loginDetails.Customer.Name
                                      select customer).FirstOrDefault();

            return View(customerToUserData);
        }
    }
}