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
    public class FacultiesController : Controller
    {
        private RepoFaculty repo = new RepoFaculty();
        private RepoCourses courses = new RepoCourses();
        private VM_Error vme = new VM_Error();

        static FacultyCreateForHttpGet facultytocreate = new FacultyCreateForHttpGet();

        public FacultiesController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())), new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext())))
        {

        }

        public FacultiesController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolManager)
        {
            UserManager = userManager;
            RoleManager = rolManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        public RoleManager<IdentityRole> RoleManager { get; private set; }

        // GET: Faculties
        public ActionResult Index()
        {
            return View(repo.GetFacultiesFull());
        }

        // GET: Faculties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var faculty = repo.getFacultyFull(id);
            if (faculty == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record could not be found";
                return View("Error", errors);
            }
            return View(faculty);
        }

        // GET: Faculties/Create
        public ActionResult Create()
        {
            facultytocreate.CoursesSelectList = courses.getCourseCodeAvailable();
            return View(facultytocreate);
        }

        // POST: Faculties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FacultyCreateForHttpPost newItem, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                var addItem = repo.AddFaculty(newItem);
                
                if(!RoleManager.RoleExists("Faculty"))
                {
                    var roleresult = RoleManager.Create(new IdentityRole("Faculty"));
                }

                var user = new ApplicationUser() { UserName = (newItem.FirstName).ToLower(), Email = newItem.Email };
                var password = "Admin@123456";
                user.Faculty = addItem;
                var result = UserManager.Create(user, password);
                if(result.Succeeded)
                {
                    var uresult = UserManager.AddToRole(user.Id, "Faculty");
                }

                if (addItem == null)
                {
                    return View("Error",vme.GetErrorModel(null,ModelState));
                }
                else
                {
                    facultytocreate.Clear();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                if (newItem.CourseId == null) ModelState.AddModelError("CoursesSelectList", "Select One or More Courses");
                facultytocreate.facultyId = newItem.facultyId;
                facultytocreate.FirstName = newItem.FirstName;
                facultytocreate.LastName = newItem.LastName;
                return View(facultytocreate);
            }
        }

        // GET: Faculties/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var faculty = repo.getFacultyFull(id);
            if (faculty == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record can not be edited Because it does not exist";
                return View("Error", errors);
            }
            var cour = courses.GetListOfCourseToEdit(id).Select(co => new SelectListItem { Value = co.CourseId.ToString(), Text = co.CourseCode + " " + co.CourseName });
            var model = new FacultyEditForm();
            model = Mapper.Map<FacultyEditForm>(faculty);
            model.selectedCourseCode = faculty.Courses.Select(x => x.CourseId);
            model.CourseCode = cour;
            return View(model);
        }

        // POST: Faculties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FacultyEditForm editedItem)
        {
            if (ModelState.IsValid)
            {
                var newItem = repo.EditFaculty(editedItem);
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

        // GET: Faculties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var faculty = repo.getFacultyFull(id);
            if (faculty == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record could not be deleted because it does not exist";
                return View("Error", errors);
            }
            return View(faculty);
        }

        // POST: Faculties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.DeleteFaculty(id);
            return RedirectToAction("Index");
        }
    }
}
