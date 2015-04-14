using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication5.Models;
using AutoMapper;

namespace WebApplication5.ViewModels
{
    public class RepoFaculty : RepositoryBase
    {
        public FacultyFull AddFaculty(FacultyCreateForHttpPost newFaculty)
        {
            var addedFaculty = Mapper.Map<Faculty>(newFaculty);
            foreach (var item in newFaculty.CourseId)
            {
                addedFaculty.Courses.Add(dc.Courses.Find(item));
            }
            dc.Faculties.Add(addedFaculty);
            dc.SaveChanges();
            return Mapper.Map<FacultyFull>(addedFaculty);
        }

        public IEnumerable<FacultyBase> GetFacultyBase()
        {
            var st = dc.Faculties.OrderBy(n => n.LastName);
            return Mapper.Map<IEnumerable<FacultyBase>>(st);
        }

        public FacultyFull getFacultyFull(int? id)
        {
            var fac = dc.Faculties.Include("Courses").SingleOrDefault(n => n.Id == id);
            return Mapper.Map<FacultyFull>(fac);
        }

        public IEnumerable<FacultyFull> GetFacultiesFull()
        {
            var fac = dc.Faculties.Include("Courses").OrderBy(n => n.LastName);
            return Mapper.Map<IEnumerable<FacultyFull>>(fac);
        }

        public FacultyFull EditFaculty(FacultyEditForm newFaculty)
        {
            var FacultyToEdit = dc.Faculties.Include("Courses").SingleOrDefault(n => n.Id == newFaculty.Id);

            if (newFaculty.selectedCourseCode != null)
            {
                foreach (var item in newFaculty.selectedCourseCode)
                {
                    newFaculty.co.Add(dc.Courses.Find(item));
                }
            }
            if (FacultyToEdit == null)
            {
                return null;
            }
            else
            {
                FacultyToEdit.Courses = newFaculty.co;
                dc.Entry(FacultyToEdit).CurrentValues.SetValues(newFaculty);
                dc.SaveChanges();
            }
            return Mapper.Map<FacultyFull>(newFaculty);
        }

        public void DeleteFaculty(int? id)
        {
            var FacultyToDelete = dc.Faculties.Include("courses").SingleOrDefault(n => n.Id == id);

            if (FacultyToDelete == null)
            {
                return;
            }
            else
            {
                try
                {
                    foreach (var item in FacultyToDelete.Courses)
                    {
                        item.Faculty = null;
                    }
                    dc.Faculties.Remove(FacultyToDelete);
                    dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public SelectList getFacultySelectList()
        {
            SelectList sl = new SelectList(GetFacultyBase(), "Id", "NameFull");
            return sl;
        }
    }
}