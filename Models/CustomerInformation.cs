using CustomerManager.Attributes;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CustomerManager.Models
{
    public class CustomerInformation
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Municipality customer")]
        [Required]
        public bool IsMunicipalityCustomer { get; set; }


        [RequiredIf("IsMunicipalityCustomer", true)]
        [Display(Name = "Number Of Schools")]
        public int? NumberOfSchools { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        public string OldName { get; set; } 

        public ICollection<ContactsDetail> ContactsDetails { get; set; }

        public ICollection<LoginUser> LoginUsers { get; set; }

        public ICollection<Departament> Departaments { get; set; }
    }
}