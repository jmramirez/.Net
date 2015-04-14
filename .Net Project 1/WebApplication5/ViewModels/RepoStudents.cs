using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using WebApplication5.Models;

namespace WebApplication5.ViewModels
{
    public class RepoStudents : RepositoryBase
    {
       

        public StudentFull CreateStudent(StudentCreateForHttpPost newItem)
        {
            var addedStudent = Mapper.Map<Student>(newItem);
            foreach(var item in newItem.CourseId)
            {
                addedStudent.Courses.Add(dc.Courses.Find(item));
                dc.Courses.Find(item).Students.Add(addedStudent);
            }
            dc.Students.Add(addedStudent);
            dc.SaveChanges();
            return Mapper.Map<StudentFull>(addedStudent);
        }

        public StudentFull CreateStudent(StudentFull st, string selCourses = "")
        {
            Student s = new Student();
            s.studentNumber = st.StudentNumber;
            s.FirstName = st.FirstName;
            s.LastName = st.LastName;
            s.Email = st.Email;
            if(selCourses != "")
            {
                foreach(var item in selCourses.Split(','))
                {
                    var itemInt32 = Convert.ToInt32(item);
                    var c = dc.Courses.FirstOrDefault(cc => cc.CourseId == itemInt32);
                    s.Courses.Add(c);
                }
            }
            dc.Students.Add(s);
            dc.SaveChanges();
            return getStudentFull(s.Id);
        }

        public IEnumerable<StudentBase> GetStudentBase()
        {
            var st = dc.Students.OrderBy(n => n.LastName);
            return Mapper.Map<IEnumerable<StudentBase>>(st);
        }

        public StudentFull getStudentFull(int? id)
        {
            var st = dc.Students.Include("Courses").SingleOrDefault(n => n.Id == id);
            return Mapper.Map<StudentFull>(st);
        }

        public IEnumerable<StudentFull> GetStudentsFull()
        {
            var st = dc.Students.Include("Courses").OrderBy(n => n.LastName);
            if (st == null) return null;
            return Mapper.Map<IEnumerable<StudentFull>>(st);
        }

       

        public StudentFull EditStudent(StudentEditForm newStudent)
        {
            var StudentToEdit = dc.Students.Include("Courses").SingleOrDefault(n => n.Id == newStudent.Id);
            if (newStudent.coursesCheck != null)
            {
                foreach (var item in newStudent.coursesCheck)
                {
                    if (item.registered)
                    {
                        newStudent.Courses.Add(dc.Courses.Find(item.CourseId));
                    }
                }
            }

            if (StudentToEdit == null)
            {
                return null;
            }
            else
            {
                StudentToEdit.Courses = newStudent.Courses;
                dc.Entry(StudentToEdit).CurrentValues.SetValues(newStudent);
                dc.SaveChanges();
            }
            return Mapper.Map<StudentFull>(newStudent);
        }

        public void DeleteStudent(int? id)
        {
            var studentToDelete = dc.Students.Find(id);

            if (studentToDelete == null)
            {
                return;
            }
            else
            {
                try
                {
                    dc.Students.Remove(studentToDelete);
                    dc.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}