using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using AutoMapper;

namespace WebApplication5.ViewModels
{
    public class RepoCourses : RepositoryBase
    {
        public CourseFull CreateCourse(CourseCreateHttpPost newCourse)
        {
            var fac = dc.Faculties.Find(newCourse.FacultyId);
            var addedCourse = Mapper.Map<Course>(newCourse);
            addedCourse.Faculty = fac;
            dc.Courses.Add(addedCourse);
            dc.SaveChanges();
            return Mapper.Map<CourseFull>(addedCourse);
        }

        public CourseFull EditCourse(CourseFull editItem)
        {
            var itemToEdit = dc.Courses.Find(editItem.CourseId);
            if (itemToEdit == null)
            {
                return null;
            }
            else
            {
                dc.Entry(itemToEdit).CurrentValues.SetValues(editItem);
                dc.SaveChanges();
            }
            return editItem;
        }

        public void DeleteCourse(int? id)
        {
            var itemToDelete = dc.Courses.Find(id);

            if (itemToDelete == null)
            {
                return;
            }
            else
            {
                try
                {
                    dc.Courses.Remove(itemToDelete);
                    dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public CourseFull GetCourseFull(int? id)
        {
            var course = dc.Courses.Include("Faculty").SingleOrDefault(n => n.CourseId == id);
            if (course == null) return null;
            else return Mapper.Map<CourseFull>(course);
        }

        public ICollection<CourseBase> GetListOfCourseBase()
        {
            var CourseCode = dc.Courses.OrderBy(m => m.CourseName);
            List<CourseBase> list = new List<CourseBase>();
            if (CourseCode == null) return null;
            foreach (var item in CourseCode)
            {
                var course = Mapper.Map<CourseBase>(item);
                course.CourseDescription = course.CourseCode + " " + course.CourseName;
                list.Add(course);
            }
            return list;
        }

        public IEnumerable<CourseBase> GetListOfCourseBaseAvailable()
        {
            var CourseCode = dc.Courses.Where(n => n.Faculty == null).OrderBy(m => m.CourseName);
            List<CourseBase> list = new List<CourseBase>();
            if (CourseCode == null) return null;
            foreach (var item in CourseCode)
            {
                var course = Mapper.Map<CourseBase>(item);
                course.CourseDescription = course.CourseCode + " " + course.CourseName;
                list.Add(course);
            }
            return list;
        }

        public IEnumerable<CourseBase> GetListOfCourseToEdit(int? id)
        {
            var CourseCode = dc.Courses.Where(n => (n.Faculty == null) ||(n.Faculty.Id == id));
            List<CourseBase> list = new List<CourseBase>();
            if (CourseCode == null) return null;
            foreach (var item in CourseCode)
            {
                var course = Mapper.Map<CourseBase>(item);
                course.CourseDescription = course.CourseCode + " " + course.CourseName;
                list.Add(course);
            }

            return list.ToList();

        }

        public IEnumerable<CourseBase> GetCourseCodeByFaculty(int? id)
        {
            var CourseCode = dc.Courses.Include("Faculty").Where(n => n.Faculty.Id == id);
            List<CourseBase> list = new List<CourseBase>();
            foreach (var item in CourseCode)
            {
                var cours = Mapper.Map<CourseBase>(item);
                cours.CourseDescription = cours.CourseCode + " " + cours.CourseName;
                list.Add(cours);
            }
            return list.ToList();
        }

        public SelectList getCourseCodeSelectList()
        {
            var ccsl = new List<CourseBase>();
            foreach(var item in GetListOfCourseBase())
            {
                ccsl.Add(item);
            }
            SelectList sl = new SelectList(ccsl.ToList(), "CourseId", "CourseDescription");
            return sl;
        }

        public SelectList getCourseCodeAvailable()
        {
            SelectList sl = new SelectList(GetListOfCourseBaseAvailable(), "CourseId", "CourseDescription");
            return sl;
        }

        public SelectList getCourseCodeByFaculty(int? id)
        {
            SelectList sl = new SelectList(GetCourseCodeByFaculty(id), "CourseId", "CourseDescription");
            return sl;
        }

        public SelectList dates()
        {
            var dates = new List<DateTime>();
            for (DateTime date = DateTime.Now.Date; date <= new DateTime(2014, 12, 31); date = date.AddDays(1))
            {
                dates.Add(date);
            }
            var ls = new SelectList(dates);
            return ls;
        }
    }
}