using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using WebApplication5.Infraestructure;

namespace WebApplication5.ViewModels
{
    
    public class CourseBase
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
    }

    public class CourseFull : CourseBase
    {
        public string RoomNumber { get; set; }
        public DateTime Scheduled { get; set; }
        public ICollection<StudentBase> Students { get; set; }
        public CourseFull()
        {
            this.Students = new List<StudentBase>();
        }
    }

    public class CourseCreateForHttpGet
    {
        [Required]
        [Display(Name = "Course Name")]
        public string CourseName { get; set; }
        [Required]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }
        [Display(Name = "Room Number")]
        public string RoomNumber { get; set; }
        [Display(Name = "Falcuty")]
        public SelectList FacultiesSelectList { get; set; }
        [Required]
        [Display(Name = "Scheduled")]
        [DateRange]
        public DateTime Scheduled { get; set; }
        public void Clear()
        {
            CourseName = string.Empty;
            CourseCode = string.Empty;
            RoomNumber = string.Empty;
            Scheduled = DateTime.Now;
        }
    }

    public class CourseCreateHttpPost
    {
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string CourseCode { get; set; }
        public string RoomNumber{ get; set; }
        [Required]
        public int FacultyId { get; set; }
        [Required]
        [DateRange]
        public DateTime Scheduled { get; set; }
    }

    public class CourseCreate
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        public  string CourseCode { get; set; }
        public string RoomNumber { get; set; }
    }

    

    

    public class CourseCheck
    {
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public bool registered { get; set; }
       
    }
}