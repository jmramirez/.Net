using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApplication5.Models
{
    public class RoleViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }
}