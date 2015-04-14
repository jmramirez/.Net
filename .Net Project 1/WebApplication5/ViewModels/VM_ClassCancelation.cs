using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication5.Models;
using WebApplication5.Infraestructure;

namespace WebApplication5.ViewModels
{
    public class ClassCancelationBase
    {
        [Key]
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Message { get; set; }
        
        public DateTime date { get; set; }
    }

    public class ClassCancelationFull : ClassCancelationBase
    {
        public Faculty Faculty { get; set; }
    }

    public class ClassCancelationCreateForm
    {
        [Display(Name = "Courses: ")]
        public SelectList CourseId { get; set; }
        [Required]
        [Display(Name = "Date: ")]
        [DateRange(Max="course")]
        public DateTime date { get; set; }
        public string Message { get; set; }
    }

    public class ClassCancelationAdd
    {
        [Required]
        public int CourseId { get; set; }
        public string Message { get; set; }
        [Display(Name = "Date: ")]
        [DateRange(Max = "course")]
        public DateTime date { get; set; }
        public int FacultyId { get; set; }
    }
}