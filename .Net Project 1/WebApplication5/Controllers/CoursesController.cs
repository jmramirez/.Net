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

namespace WebApplication5.Controllers
{
    public class CoursesController : Controller
    {
        private RepoCourses repo = new RepoCourses();
        private RepoFaculty faculties = new RepoFaculty();
        private VM_Error vme = new VM_Error();

        static CourseCreateForHttpGet coursetocreate = new CourseCreateForHttpGet();



        // GET: Courses
        public ActionResult Index()
        {
            return View(repo.GetListOfCourseBase());
        }

        // GET: Courses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var course = repo.GetCourseFull(id);
            if (course == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record could not be found";
                return View("Error", errors);
            }
            return View(course);
        }

        // GET: Courses/Create
        public ActionResult Create()
        {
            coursetocreate.FacultiesSelectList = faculties.getFacultySelectList();
            coursetocreate.Scheduled = DateTime.Now;
            return View(coursetocreate);
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CourseCreateHttpPost newItem)
        {
            if (ModelState.IsValid)
            {
                var addItem = repo.CreateCourse(newItem);
                if (addItem == null)
                {
                    return View("Error",vme.GetErrorModel(null,ModelState));
                }
                else
                {
                    coursetocreate.Clear();
                    return RedirectToAction("Details", new { Id = addItem.CourseId });
                }
            }
            else
            {
                if (newItem.FacultyId == null) ModelState.AddModelError("CoursesSelectList", "Select One or More Courses");
                coursetocreate.CourseCode = newItem.CourseCode;
                coursetocreate.CourseName = newItem.CourseName;
                coursetocreate.Scheduled = newItem.Scheduled;
                coursetocreate.RoomNumber = newItem.RoomNumber;
                return View(coursetocreate);
            }
        }

        // GET: Courses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var course = repo.GetCourseFull(id);
            if (course == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record can not be edited Because it does not exist";
                return View("Error", errors);
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CourseId,CourseName,CourseCode,RoomNumber,scheduled")] CourseFull editedItem)
        {
            if (ModelState.IsValid)
            {
                var newItem = repo.EditCourse(editedItem);
                if (newItem == null)
                {
                    ViewBag.ExceptionMessage = "Record " + editedItem.CourseId + " was not found.";
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

        // GET: Courses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That was an Invalid Record";
                return View("Error", errors);
            }
            var course = repo.GetCourseFull(id);
            if (course == null)
            {
                var errors = new VM_Error();
                errors.ErrorMessages["ExceptionMessage"] = "That Record could not be deleted because it does not exist";
                return View("Error", errors);
            }
            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.DeleteCourse(id);
            return RedirectToAction("Index");
        }
    }
}
