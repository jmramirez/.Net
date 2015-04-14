using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;
using WebApplication5.Infraestructure;
using WebApplication5.ViewModels;

namespace WebApplication5.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public virtual FacultyFull Faculty {get;set;}
        public virtual StudentFull Student { get; set; }
    }

    public class Person
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public Person()
        {
            FirstName = LastName = string.Empty;
        }
        public Person(string f, string l)
        {
            FirstName = f;
            LastName = l;
        }
    }

    public class Faculty : Person
    {
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by * digits")]
        public string facultyId { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public ICollection<Course> Courses { get; set; }
        public ICollection<ClassCancelation> Cancellations { get; set; }
        public virtual ApplicationUser User { get; set; }
        public Faculty()
            : base()
        {
            this.Courses = new List<Course>();
            this.Cancellations = new List<ClassCancelation>();
            facultyId = string.Empty;
        }
        public Faculty(string f, string l, string fid)
            : base(f, l)
        {
            this.Courses = new List<Course>();
            this.Cancellations = new List<ClassCancelation>();
            facultyId = fid;
        }
    }

    public class Student : Person
    {
        [Required]
        [RegularExpression("^[0][0-9]{8}$", ErrorMessage = "0 followed by * digits")]
        public string studentNumber { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        public virtual ApplicationUser User { get; set; }
        public ICollection<Course> Courses { get; set; }
        public Student()
            : base()
        {
            this.Courses = new List<Course>();
            studentNumber = string.Empty;
        }
        public Student(string f, string l, string sid)
            : base(f, l)
        {
            this.Courses = new List<Course>();
            studentNumber = sid;
        }
    }

    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string RoomNumber { get; set; }
        public DateTime Scheduled { get; set; }
        public List<Student> Students { get; set; }
        public Faculty Faculty { get; set; }
        public Course()
        {
            this.Students = new List<Student>();
            this.Scheduled = DateTime.Now;
        }
    }

    public class Message
    {
        [Key]
        public int Id { get; set; }
        public String Course { get; set; }
        public string FacultyName { get; set; }
        public DateTime date { get; set; }
        public string messageC { get; set; }
        public Faculty Faculty { get; set; }
    }

    public class ClassCancelation
    {
        [Key]
        public int Id { get; set; }
        public Faculty Faculty { get; set; }
        public int CourseId { get; set; }
        public string Message { get; set; }
        public DateTime date { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }



        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ClassCancelation> ClassCancelations { get; set; }

        public System.Data.Entity.DbSet<WebApplication5.Models.RoleViewModel> RoleViewModels { get; set; }

        public System.Data.Entity.DbSet<WebApplication5.Models.EditUserViewModel> EditUserViewModels { get; set; }
    }
}