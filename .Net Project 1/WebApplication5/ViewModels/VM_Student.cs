using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication5.Infraestructure;

namespace WebApplication5.ViewModels
{
    public class StudentBase
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
    }

    public class StudentFull : StudentBase
    {
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string StudentNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Course")]
        public ICollection<CourseBase> Courses { get; set; }

        public StudentFull()
        {
            this.Courses = new List<CourseBase>();
        }
    }

    public class StudentCreateForHttpGet
    {
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        [Display(Name="Student Id")]
        public string StudentNumber { get; set; }
        [Required]
        [StringLength(40, MinimumLength = 3)]
        [Display(Name = "Fist Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        [StringLength(40, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public SelectList CoursesSelectList { get; set; }
        public void Clear()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            StudentNumber = string.Empty;
        }
    }

    public class StudentCreateForHttpPost
    {
        
        public string StudentNumber { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        [Required]
        public virtual ICollection<int> CourseId { get; set; }

    }


    public class StudentEditForm
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by 8 digits")]
        public string StudentNumber { get; set; }
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
        public ICollection<Models.Course> Courses { get; set; }
        [Display(Name = "Courses")]
        public IEnumerable<CourseCheck> coursesCheck { get; set; }
        public StudentEditForm()
        {
            this.Courses = new List<Models.Course>();
        }
    }
}