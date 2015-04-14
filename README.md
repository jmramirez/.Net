<h1>.Net all projects created using ASP.NET MVC</h1>

In this repository you can fine different projects created using ASP.NET and the different architectures MVC 
and also MVVM. There is also use of jQuery, and bootstrap. 

<h2>Project 1</h2>

As an introduction to this project I will mention that it was pretty much designing an application that allowed faculty to login and cancel a class any given day, and students can log in a see the cancellations for the courses they are enrolled.

This project was challenging in the sense that I was not clear the requirements for each model, so it was necessary for me develop the basic models such as Faculty, Students, and Courses with a list of attributes that I changed as I was developing the application.

To complete this assignment I developed first the part related to the CRUDs and class cancellations, and then I added the security part.

For the final product I had developed the ApplicationUser, Admin, Faculty, Student, Course, and ClassCancelation models.
For the ViewModels I have developed VM_Admin, VM_ClassCancellations, VM_Courses, VM_Faculty,  VM_Students and also the Repo classes beginning with the RepositoryBase and the Repos for Admin, ClassCancellations, Courses, Faculty, and Students.

The application is able to Create, Edit, Delete, and Update Faculties, Students, Courses, and Admins.  Faculty can create a cancellation for a particular course, and also can delete the cancellation.

Students are capable to see if they have a cancellation for the day. 

Admin can add, delete, or update Faculty, Students and Courses, and also they can see all the cancellations for all the faculties. 

This app can be tested here : <a href=http://testapp31.azurewebsites.net/>Project 1</a>

UserNames:                  Password:               Rol:
admin                       Admin@123456            admin
mark                        Admin@123456            faculty
bob                         Admin@123456            student


