using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebApplication5.ViewModels
{
    public class FacultyBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string NameFull
        {
            get { return FirstName + " " + LastName; }
        }
    }
    public class FacultyFull : FacultyBase
    {
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        [Display(Name = "Faculty Id:")]
        public string facultyId { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public virtual ICollection<CourseBase> Courses { get; set; }
        public ICollection<ClassCancelationBase> Cancellations { get; set; }
        public FacultyFull()
        {
            this.Courses = new List<CourseBase>();
            this.Cancellations = new List<ClassCancelationBase>();
        }
    }


    public class FacultyCreateForHttpGet
    {
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        [Display(Name = "Faculty Id")]
        public string facultyId { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Select One or More Courses")]
        public SelectList CoursesSelectList { get; set; }
        public void Clear()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            facultyId = string.Empty;
        }
    }

    public class FacultyCreateForHttpPost
    {
        public string facultyId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage="Select One or More Courses")]
        public virtual ICollection<int> CourseId { get; set; }
    }

  

    public class FacultyEditForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        [Display(Name = "Faculty Id:")]
        public string facultyId { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public virtual IEnumerable<int> selectedCourseCode { get; set; }
        [Display(Name="Courses")]
        public virtual IEnumerable<SelectListItem> CourseCode { get; set; }
        public ICollection<Models.Course> co { get; set; }
        public FacultyEditForm()
        {
            this.co = new List<Models.Course>();
        }
    }

}