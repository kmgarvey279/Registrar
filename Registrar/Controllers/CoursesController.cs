using Microsoft.AspNetCore.Mvc;
using Registrar.Models;
using System.Collections.Generic;

namespace Registrar.Controllers
{
  public class CoursesController : Controller
  {
    [HttpGet("/courses")]
    public ActionResult Index()
    {
      List<Course> allCourses = Course.GetAll();
      return View(allCourses);
    }

    [HttpGet("/courses/new")]
    public ActionResult New()
    {
      return View();
    }

    [HttpPost("/courses")]
    public ActionResult Create(string name, string number)
    {
      Course newCourse = new Course(name, number);
      newCourse.Save();
      List<Course> allCourses = Course.GetAll();
      return View("Index", allCourses);
    }

    [HttpGet("courses/{id}")]
    public ActionResult Show(int id)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Course selectedCourse = Course.Find(id);
      List<Student> enrolledStudents = selectedCourse.GetStudents();
      List<Student> allStudents = Student.GetAll();
      model.Add("selectedCourse", selectedCourse);
      model.Add("enrolledStudents", enrolledStudents);
      model.Add("allStudents", allStudents);
      return View(model);
    }

    [HttpPost("/courses/{courseId}/students/new")]
    public ActionResult AddStudent(int studentId, int courseId)
    {
      Course course =  Course.Find(courseId);
      Student student = Student.Find(studentId);
      course.AddStudent(student);
      return RedirectToAction("Show", new { id = courseId});
    }
  }
}
