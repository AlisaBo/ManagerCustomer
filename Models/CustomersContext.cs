using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CustomerManager.Models
{
    public class CustomersContext : DbContext
    {
        public DbSet<CustomerInformation> CustomerInformation { get; set; }
        public DbSet<ContactsDetail> ContactsDetail { get; set; }
        public DbSet<Departament> Departament { get; set; }
        public DbSet<LoginUser> LoginUser { get; set; }
    }
}