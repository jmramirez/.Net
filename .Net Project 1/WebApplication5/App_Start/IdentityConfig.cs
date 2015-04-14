using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using WebApplication5.Models;
using WebApplication5.ViewModels;
using AutoMapper;

namespace WebApplication5
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context) 
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = 
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }

    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole,string> roleStore)
            :base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<ApplicationDbContext>()));
        }
    }

    public class ApplicationDbInitializer : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        public static void InitializeIdentityForEF(ApplicationDbContext db) 
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            string name = "admin";
            string email = "admin@example.com";
            const string password = "Admin@123456";
            String[] roles = { "Admin","Faculty", "Student" };

            foreach(string rol in roles)
            {
                var ro = roleManager.FindByName(rol);
                if(ro == null)
                {
                    ro = new IdentityRole(rol);
                    var roleresult = roleManager.Create(ro);
                }
            }

            var user = userManager.FindByName(name);
            if (user == null) 
            {
                user = new ApplicationUser { UserName = name, Email = email };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            var rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Admin"))
            {
                var result = userManager.AddToRole(user.Id, "Admin");
            }

            Course uli101 = new Course();
            uli101.CourseCode = "ULI101A";
            uli101.CourseName = "Introduction to Unix/Linux and the Internet";
            uli101.Scheduled = new DateTime(2015, 4, 17, 8, 00, 00);
           
            uli101.RoomNumber = "T4040";
            db.Courses.Add(uli101);

            Course int422 = new Course();
            int422.CourseCode = "INT422B";
            int422.CourseName = "Windows Web Prgramming";
            int422.Scheduled = new DateTime(2015, 4, 17, 8, 00, 00);
            int422.RoomNumber = "T3212";
            db.Courses.Add(int422);

            Course oop344 = new Course();
            oop344.CourseCode = "OOP344C";
            oop344.CourseName = "Object Oriented Programming II Using C++";
            oop344.RoomNumber = "S3432";
            oop344.Scheduled = new DateTime(2015, 4, 17, 8, 00, 00);
            db.Courses.Add(oop344);

            Course ibc233 = new Course();
            ibc233.CourseName = "iSeries Businees Computing";
            ibc233.CourseCode = "IBC233D";
            ibc233.RoomNumber = "S3443";
            ibc233.Scheduled = new DateTime(2015, 4, 17, 8, 00, 00);
            db.Courses.Add(ibc233);

            Course jac444 = new Course();
            jac444.CourseName = "Introduction to Java for C++ Programmers";
            jac444.CourseCode = "JAC444E";
            jac444.RoomNumber = "T1223";
            jac444.Scheduled = new DateTime(2015, 4, 17, 8, 00, 00);
            db.Courses.Add(jac444);

            Student student = new Student();
            student.Id = 1;
            student.FirstName = "Bob";
            student.LastName = "Smith";
            student.studentNumber = "011111111";
            student.Courses.Add(jac444);
            student.Courses.Add(int422);
            email = "bob@example.com";
            student.Email = email;
            db.Students.Add(student);
            int422.Students.Add(student);
            jac444.Students.Add(student);
            name = student.FirstName.ToLower();
            user = userManager.FindByName(name);
            
            if (user == null)
            {
                user = new ApplicationUser { UserName = name,Email = email, Student = Mapper.Map<StudentFull>(student)};
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Student"))
            {
                var result = userManager.AddToRole(user.Id, "Student");
            }

            student = new Student();
            user = new ApplicationUser();
            student.Id = 2;
            student.FirstName = "Mary";
            student.LastName = "Brown";
            student.studentNumber = "011111112";
            student.Courses.Add(jac444);
            student.Courses.Add(uli101);
            email = "mary@example.com";
            student.Email = email;
            db.Students.Add(student);
            uli101.Students.Add(student);
            jac444.Students.Add(student);
            name = student.FirstName.ToLower();
            user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = email, Student = Mapper.Map<StudentFull>(student) };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Student"))
            {
                var result = userManager.AddToRole(user.Id, "Student");
            }

            student = new Student();
            user = new ApplicationUser();
            student.Id = 3;
            student.FirstName = "Wei";
            student.LastName = "Chen";
            student.studentNumber = "011111113";
            student.Courses.Add(ibc233);
            student.Courses.Add(int422);
            email = "wei@example.com";
            student.Email = email;
            db.Students.Add(student);
            int422.Students.Add(student);
            ibc233.Students.Add(student);
            name = student.FirstName.ToLower();
            user = userManager.FindByName(name);
            
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = email, Student = Mapper.Map<StudentFull>(student) };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Student"))
            {
                var result = userManager.AddToRole(user.Id, "Student");
            }

            student = new Student("John", "Woo", "011111114");
            user = new ApplicationUser();
            student.Id = 4;
            student.Courses.Add(oop344);
            student.Courses.Add(int422);
            email = "john@example.com";
            student.Email = email;
            db.Students.Add(student);
            int422.Students.Add(student);
            oop344.Students.Add(student);
            name = student.FirstName.ToLower();
            user = userManager.FindByName(name);
            
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = email, Student = Mapper.Map<StudentFull>(student) };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Student"))
            {
                var result = userManager.AddToRole(user.Id, "Student");
            }


            Faculty fac = new Faculty("Peter", "McIntyre", "034234678");
            email = "peter@example.com";
            fac.Email = email;
            fac.Courses.Add(jac444);
            fac.Courses.Add(ibc233);
            db.Faculties.Add(fac);
            name = fac.FirstName.ToLower();
            user = userManager.FindByName(name);
            
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = email, Faculty = Mapper.Map<FacultyFull>(fac) };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Faculty"))
            {
                var result = userManager.AddToRole(user.Id, "Faculty");
            }


            fac = new Faculty("Mark", "Fernandez", "034234678");
            user = new ApplicationUser();
            email = "mark@example.com";
            fac.Email = email;
            fac.Courses.Add(uli101);
            db.Faculties.Add(fac);
            name = fac.FirstName.ToLower();
            user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = email, Faculty = Mapper.Map<FacultyFull>(fac) };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Faculty"))
            {
                var result = userManager.AddToRole(user.Id, "Faculty");
            }

            fac = new Faculty("Hasan", "Kamal-Al-Deen", "034234678");
            user = new ApplicationUser();
            email = "hasan@example.com";
            fac.Email = email;
            fac.Courses.Add(oop344);
            db.Faculties.Add(fac);
            name = fac.FirstName.ToLower();
            user = userManager.FindByName(name);
            if (user == null)
            {
                user = new ApplicationUser { UserName = name, Email = email, Faculty = Mapper.Map<FacultyFull>(fac) };
                var result = userManager.Create(user, password);
                result = userManager.SetLockoutEnabled(user.Id, false);
            }

            rolesForUser = userManager.GetRoles(user.Id);
            if (!rolesForUser.Contains("Faculty"))
            {
                var result = userManager.AddToRole(user.Id, "Faculty");
            }


            db.SaveChanges();
        }
    }
}
