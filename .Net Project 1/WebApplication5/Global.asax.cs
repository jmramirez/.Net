using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using WebApplication5.Models;
using WebApplication5.ViewModels;

namespace WebApplication5
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

           
            Mapper.CreateMap<Student, StudentFull>();
            Mapper.CreateMap<Student, StudentBase>();
            Mapper.CreateMap<StudentEditForm, StudentFull>();
            Mapper.CreateMap<StudentFull, StudentEditForm>();
          

            Mapper.CreateMap<Faculty, FacultyFull>();
            Mapper.CreateMap<Faculty, FacultyBase>();
            Mapper.CreateMap<FacultyFull, Faculty>();
            Mapper.CreateMap<FacultyEditForm, FacultyFull>();
            Mapper.CreateMap<FacultyFull, FacultyEditForm>();

            Mapper.CreateMap<Course, CourseFull>();
            Mapper.CreateMap<Course, CourseBase>();
            Mapper.CreateMap<CourseBase, Course>();

            Mapper.CreateMap<ClassCancelationAdd, ClassCancelation>();
            Mapper.CreateMap<ClassCancelation, ClassCancelationFull>();

            Mapper.CreateMap<StudentCreateForHttpPost, Student>();
            Mapper.CreateMap<FacultyCreateForHttpPost, Faculty>();
            Mapper.CreateMap<CourseCreateHttpPost, Course>();
        }
    }
}
