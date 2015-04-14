using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using WebApplication5.ViewModels;
using AutoMapper;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace WebApplication5.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StudentsController : Controller
    {
        private RepoStudents repo = new RepoStudents();
        private RepoCourses courses = new RepoCourses();
        private VM_Error vme = new VM_Error();

        static StudentCreateForHttpGet studenttocreate = new StudentCreateForHttpGet();
        static StudentEditForm studentToEdit = new StudentEditForm();

        public StudentsController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())), new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
        {

        }

        public StudentsController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolManager)
        {
            UserManager = userManager;
            RoleManager = rolManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public RoleManager<IdentityRole> RoleManager { get; private set; }

        // GET: Students
        public ActionResult Index()
        {
            return View(repo.GetStudentsFull());
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                var errors = new ViewModels.VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "No id specified";
                return View("Error",errors);
            }
            var student = repo.getStudentFull(id);
            if (student == null)
            {
                var errors = new ViewModels.VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record does not exist";
                return View("Error", errors);
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            
            studenttocreate.CoursesSelectList = courses.getCourseCodeSelectList();
            return View(studenttocreate);
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StudentCreateForHttpPost newItem)
        {
            if (ModelState.IsValid)
            {
                var addItem = repo.CreateStudent(newItem);

                if(!RoleManager.RoleExists("Student"))
                {
                    var roleresult = RoleManager.Create(new IdentityRole("Student"));
                }

                var user = new ApplicationUser() { UserName = (newItem.FirstName).ToLower(), Email = newItem.Email };
                var password = "Admin@123456";
                user.Student = addItem;
                var result = UserManager.Create(user, password);
                if(result.Succeeded)
                {
                    var uresult = UserManager.AddToRole(user.Id, "Student");
                }

                if (addItem == null)
                {
                    return View("Error",vme.GetErrorModel(null,ModelState));
                }
                else
                {
                    studenttocreate.Clear();
                    return RedirectToAction("Details", new { Id = addItem.Id });
                }
            }
            else
            {
                if (newItem.CourseId == null) ModelState.AddModelError("CoursesSelectList", "Select One or More Courses");
                studenttocreate.StudentNumber = newItem.StudentNumber;
                studenttocreate.FirstName = newItem.FirstName;
                studenttocreate.LastName = newItem.LastName;
                return View(studenttocreate);
            }
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var student = repo.getStudentFull(id);

            if (student == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record can not be edited because it does not exist";    
                return View("Error", errors);
            }
            
            var listCourses = courses.GetListOfCourseBase();
            var cCheck = new List<CourseCheck>();
            foreach(var c in listCourses)
            {
                var cc = new CourseCheck();
                cc.CourseId = c.CourseId;
                cc.CourseName = c.CourseName;
                cc.CourseCode = c.CourseCode;
                cc.CourseDescription = cc.CourseCode+" "+cc.CourseName;
                var cs = student.Courses.FirstOrDefault(n => n.CourseId == c.CourseId );
                if (cs != null)
                {
                    cc.registered = true;
                }
                cCheck.Add(cc);
            }
            var cour = courses.GetListOfCourseBase().Select(co => new SelectListItem { Value = co.CourseId.ToString(), Text = co.CourseDescription });
            studentToEdit = Mapper.Map<StudentEditForm>(student);
            studentToEdit.coursesCheck = cCheck;
            
            

            return View(studentToEdit);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StudentEditForm editedItem, FormCollection form)
        {
            string prueba = form[5];

            List<bool> ls = new List<bool>();

            var check = prueba.Split(',');

            for (int i = 0; i < check.Count(); i++)
            {
                if(check[i]=="true")
                {
                    ls.Add(true);
                    i++;
                }
                else
                {
                    ls.Add(false);
                }
            }

            int cuantos = ls.Count();

            if (ModelState.IsValid)
                {
                    int cuan = 0;
                    
                    foreach(var c in studentToEdit.coursesCheck)
                    {
                        c.registered = ls[cuan];
                        cuan++;
                    }
                    
                    editedItem.coursesCheck = studentToEdit.coursesCheck;
                    var newItem = repo.EditStudent(editedItem);
                    if (newItem == null)
                    {
                        ViewBag.ExceptionMessage = "Record " + editedItem.Id + " was not found.";
                        return View("Error");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return View("Error");
                }
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var student = repo.getStudentFull(id);
            if (student == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record could not be deleted because it does not exist";
                return View("Error", errors);
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.DeleteStudent(id);
            return RedirectToAction("Index");
        }
    }
}
