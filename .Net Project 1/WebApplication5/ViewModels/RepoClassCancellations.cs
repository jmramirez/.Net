using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using AutoMapper;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace WebApplication5.ViewModels
{
    public class RepoClassCancellations : RepositoryBase
    {
       /* public IEnumerable<ClassCancelationFull> CreateCancelation(ClassCancelationAdd newItem)
        {
            var listCancellations = new List<ClassCancelationFull>();
            var addedCancellation = Mapper.Map<ClassCancelation>(newItem);
            var fac = dc.Faculties.Find(newItem.FacultyId);
            foreach (var courses in newItem.CourseId)
            {
                var course = dc.Courses.Find(courses);
                if (fac == null)
                {
                    return null;
                }
                else
                {
                    addedCancellation.Faculty = fac;
                    addedCancellation.CourseCode = course.CourseCode;
                    TimeSpan ts = new TimeSpan(course.Scheduled.Hour, course.Scheduled.Minute, course.Scheduled.Second);
                    addedCancellation.date = addedCancellation.date + ts;
                    var cancellation = Mapper.Map<ClassCancelationFull>(addedCancellation);
                    listCancellations.Add(cancellation);
                    dc.SaveChanges();
                }
            }
            return listCancellations;
        } */

        public ClassCancelationFull CreateCancelation(ClassCancelationAdd newItem)
        {
            var fac = dc.Faculties.Find(newItem.FacultyId);
            var addedCancelation = Mapper.Map<ClassCancelation>(newItem);
            addedCancelation.Faculty = fac;
            dc.ClassCancelations.Add(addedCancelation);
            dc.SaveChanges();
            return Mapper.Map<ClassCancelationFull>(addedCancelation);
        }

        public void DeleteCancellation(int? id)
        {
            var CancToDelete = dc.ClassCancelations.SingleOrDefault(n => n.Id == id);

            if (CancToDelete == null)
            {
                return;
            }
            else
            {
                try
                {
                    dc.ClassCancelations.Remove(CancToDelete);
                    dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public ClassCancelationFull GetCancellations(int? id)
        {
            var cancel = dc.ClassCancelations.Include("Faculty").SingleOrDefault(n => n.Id == id);
            if (cancel == null) return null;
            else return Mapper.Map<ClassCancelationFull>(cancel);
        }

        public IEnumerable<ClassCancelationFull> GetListOfCancellationsById(int id)
        {
            var cancellations = dc.ClassCancelations.Where(n => n.Faculty.Id == id);
            if (cancellations == null) return null;
            else return Mapper.Map<IEnumerable<ClassCancelationFull>>(cancellations);
        }

        public IEnumerable<ClassCancelationFull> GetListOfCancellationsAll()
        {
            var cancellations = dc.ClassCancelations.Include("Faculty");
            if (cancellations == null) return null;
            else return Mapper.Map<IEnumerable<ClassCancelationFull>>(cancellations);
        }

        public IEnumerable<ClassCancelationFull> GetListOfCancellationsByStudent(int? id)
        {
            var student = dc.Students.Include("Courses").SingleOrDefault(n => n.Id == id);
            var listCancellations = new List<ClassCancelationFull>();
            var currentDatetime = DateTime.Now;
            foreach (var item in student.Courses)
            {
                var cancel = dc.ClassCancelations.Include("Faculty").SingleOrDefault(m => m.CourseId == item.CourseId && EntityFunctions.TruncateTime(m.date) == currentDatetime.Date);
                if (cancel != null)
                {
                    var cancellation = Mapper.Map<ClassCancelationFull>(cancel);
                    listCancellations.Add(cancellation);
                }
            }
            if (listCancellations == null) return null;
            return listCancellations;
        }
    }
}