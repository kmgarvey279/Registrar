using Microsoft.VisualStudio.TestTools.UnitTesting;
using Registrar.Models;
using System.Collections.Generic;
using System;

namespace Registrar.Tests
{
  [TestClass]
  public class CourseTest : IDisposable
  {

    public CourseTest()
    {
      DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=registrar_test;";
    }

    public void Dispose()
    {
      Course.ClearAll();
      Student.ClearAll();
    }

    [TestMethod]
    public void CourseConstructor_CreatesInstanceOfCourse_Course()
    {
      Course newCourse = new Course("test course", "CourseNum");
      Assert.AreEqual(typeof(Course), newCourse.GetType());
    }

    [TestMethod]
    public void Equals_ReturnsTrueIfNamesAreTheSame_Course()
    {
      Course firstCourse = new Course("Test", "1");
      Course secondCourse = new Course("Test", "1");
      Assert.AreEqual(firstCourse, secondCourse);
    }

    [TestMethod]
    public void Save_SavesCourseToDatabase_Course()
    {
    Course testCourse = new Course("name", "number");
    testCourse.Save();
    List<Course> result = Course.GetAll();
    List<Course> testList = new List<Course>{testCourse};
    CollectionAssert.AreEqual(testList, result);
    }

    [TestMethod]
    public void Save_DatabaseAssignsIdToCourse_Id()
    {
      Course testCourse = new Course("name", "number");
      testCourse.Save();
      Course savedCourse = Course.GetAll()[0];
      int result = savedCourse.Id;
      int testId = testCourse.Id;
      Assert.AreEqual(testId, result);
    }

    [TestMethod]
    public void GetAll_ReturnsAllCourseObjects_CourseList()
    {
      Course newCourse1 = new Course("name", "number");
      newCourse1.Save();
      Course newCourse2 = new Course("name", "number");
      newCourse2.Save();
      List<Course> newList = new List<Course> { newCourse1, newCourse2 };
      List<Course> result = Course.GetAll();
      CollectionAssert.AreEqual(newList, result);
    }

    [TestMethod]
    public void GetAll_CoursesEmptyAtFirst_List()
    {
      int result = Course.GetAll().Count;
      Assert.AreEqual(0, result);
    }

    [TestMethod]
    public void Find_ReturnsCategoryInDatabase_Category()
    {

      Course testCourse = new Course("Household chores", "Test", 1);
      testCourse.Save();

      Course foundCourse = Course.Find(testCourse.Id);

      Assert.AreEqual(testCourse, foundCourse);
    }

    [TestMethod]
    public void Find_ReturnsCourseInDatabase_Course()
    {
      Course testCourse = new Course("name", "number");
      testCourse.Save();
      Course foundCourse = Course.Find(testCourse.Id);
      Assert.AreEqual(testCourse, foundCourse);
    }
  }
}
