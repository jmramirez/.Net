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
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using AutoMapper;

namespace WebApplication5.Controllers
{
    [Authorize]
    public class ClassCancelationsController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<ApplicationUser> manager;
        private RepoClassCancellations repo = new RepoClassCancellations();
        private RepoCourses courses = new RepoCourses();

        public ClassCancelationsController()
        {
            db = new ApplicationDbContext();
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }


        // GET: ClassCancelations
        [Authorize(Roles = "Admin,Faculty,Student")]
        public ActionResult Index()
        {
            var currentUser = manager.FindById(User.Identity.GetUserId());
            if(currentUser != null)
            {
                if(User.IsInRole("Student"))
                {
                    var listCancellations = repo.GetListOfCancellationsByStudent(currentUser.Student.Id);
                    if(listCancellations == null)
                    {
                        return View("Index");
                    }
                    return View(repo.GetListOfCancellationsByStudent(currentUser.Student.Id));
                }
                else if (User.IsInRole("Faculty"))
                {
                    return View(repo.GetListOfCancellationsById(currentUser.Faculty.Id));
                }
                else if (User.IsInRole("Admin"))
                {
                    return View(repo.GetListOfCancellationsAll());
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        // GET: ClassCancelations/Create
        [Authorize(Roles="Faculty")]
        public ActionResult Create()
        {
            var addForm = new ClassCancelationCreateForm();
            var currentUser = manager.FindById(User.Identity.GetUserId());
            addForm.CourseId = courses.getCourseCodeByFaculty(currentUser.Faculty.Id);
            return View(addForm);
        }

        // POST: ClassCancelations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClassCancelationAdd newItem)
        {
            if(ModelState.IsValid)
            {
                var currentUser = manager.FindById(User.Identity.GetUserId());
                newItem.FacultyId = currentUser.Faculty.Id;
                var addItem = repo.CreateCancelation(newItem);
                if(addItem == null)
                {
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

        // GET: ClassCancelations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassCancelation classCancelation = db.ClassCancelations.Find(id);
            if (classCancelation == null)
            {
                return HttpNotFound();
            }
            return View(classCancelation);
        }

        // POST: ClassCancelations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CourseCode,Message,date")] ClassCancelation classCancelation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classCancelation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classCancelation);
        }

        // GET: ClassCancelations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.ExceptionMessage = "That was an invalid Record";
                return View("Error");
            }
            var cancel = repo.GetCancellations(id);
            if (cancel == null)
            {
                ViewBag.ExceptionMessage = "That record could not be deleted because it does not exist";
                return View("Error");
            }
            return View(cancel);
        }

        // POST: ClassCancelations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            repo.DeleteCancellation(id);
            return RedirectToAction("Index");
        }
    }
}
