using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CustomerManager.Models
{
    public class LoginUser
    {
        [Key]
        public int LoginUserId { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Mobile")]
        [Phone]
        public string Mobile { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Mail")]
        public string Mail { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public CustomerInformation Customer { get; set; }

        public Departament Departament { get; set; }

        public ICollection<Departament> DepartamentsToManage { get; set; }

        [NotMapped]
        public string OldName { get; set; }

        [Display(Name = "Departament Name")]
        [NotMapped]
        public string DepartamentName { get; set; }
    }
}