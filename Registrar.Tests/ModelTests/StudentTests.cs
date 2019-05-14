using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registrar.Models;
using System.Collections.Generic;
using System;

namespace Registrar.Tests
{
  [TestClass]
  public class StudentTest : IDisposable
  {

    public StudentTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=registrar_test;";
    }

    public void Dispose()
    {
      Course.ClearAll();
      Student.ClearAll();
    }

    [TestMethod]
    public void StudentConstructor_CreatesInstanceOfStudent_Student()
    {
      Student newStudent = new Student("test", "TestDate");
      Assert.AreEqual(typeof(Student), newStudent.GetType());
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Student()
    {
      Student firstStudent = new Student("test", "TestDate");
      Student secondStudent = new Student("test", "TestDate");
      Assert.AreEqual(firstStudent, secondStudent);
    }

    [TestMethod]
    public void Save_SavesStudentToDatabase_Student()
    {
    Student testStudent = new Student("name", "date");
    testStudent.Save();
    List<Student> result = Student.GetAll();
    List<Student> testList = new List<Student>{testStudent};
    CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_DatabaseAssignsIdToStudent_Id()
    {
      Student testStudent = new Student("name", "date");
      testStudent.Save();
      Student savedStudent = Student.GetAll()[0];
      int result = savedStudent.Id;
      int testId = testStudent.Id;
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void Find_ReturnsCorrectStudentFromDatabase_Student()
    {

      Student testStudent= new Student("Student", "TestDate");
      testStudent.Save();

      Student foundStudent = Student.Find(testStudent.Id);

      Assert.AreEqual(testStudent, foundStudent);
    }

    [TestMethod]
    public void GetAll_ReturnsAllStudentObjects_StudentList()
    {
      Student newStudent1 = new Student("name", "date");
      newStudent1.Save();
      Student newStudent2 = new Student("name", "date");
      newStudent2.Save();
      List<Student> newList = new List<Student> { newStudent1, newStudent2 };
      List<Student> result = Student.GetAll();
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_StudentsEmptyAtFirst_List()
    {
      int result = Student.GetAll().Count;
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void GetCourses_ReturnsAllStudentCourses_CourseList()
    {
      Student testStudent = new Student("name", "date");
      testStudent.Save();
      Course testCourse1 = new Course("name", "date");
      testCourse1.Save();
      Course testCourse2 = new Course("name", "date");
      testCourse2.Save();
      testStudent.AddCourse(testCourse1);
      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course> {testCourse1};
      CollectionAssert.AreEqual(testList, result);;
    }

    [TestMethod]
    public void AddCourse_AddsCourseToStudent_CourseList()
    {
      Student testStudent = new Student ("name", "date");
      testStudent.Save();
      Course testCourse = new Course("name", "date");
      testCourse.Save();
      testStudent.AddCourse(testCourse);
      List<Course> result = testStudent.GetCourses();
      List<Course> testList = new List<Course>{testCourse};
      CollectionAssert.AreEqual(testList,result);
    }

  }
}
