using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Registrar.Models
{
  public class Student
  {
    private string _name;
    public string Name { get {return _name;} }

    private string _enrollmentDate;
    public string EnrollmentDate { get { return _enrollmentDate;} }

    private int _id;
    public int Id { get { return _id;} }

    public Student(string name, string enrollmentDate, int id = 0)
    {
      _name = name;
      _enrollmentDate = enrollmentDate;
      _id =id;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM students;";
      cmd.ExecuteNonQuery();
      conn.Close();
      if ( conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students (name, enrollment_date) VALUES (@name, @enrollmentDate);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      MySqlParameter enrollmentDate = new MySqlParameter();
      enrollmentDate.ParameterName = "@enrollmentDate";
      enrollmentDate.Value = this._enrollmentDate;
      cmd.Parameters.Add(enrollmentDate);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Student> GetAll()
    {
      List<Student> allStudents = new List<Student> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int StudentId = rdr.GetInt32(0);
        string StudentName = rdr.GetString(1);
        string StudentDate = rdr.GetString(2);
        Student newStudent = new Student(StudentName, StudentDate, StudentId);
        allStudents.Add(newStudent);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allStudents;
    }

    public static Student Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM students WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int StudentId = 0;
      string StudentName = "";
      string StudentDate = "";

      while(rdr.Read())
      {
        StudentId = rdr.GetInt32(0);
        StudentName = rdr.GetString(1);
        StudentDate = rdr.GetString(2);
      }

      Student newStudent = new Student(StudentName, StudentDate, StudentId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newStudent;
    }

    public override bool Equals(System.Object otherStudent)
    {
      if(!(otherStudent is Student))
      {
        return false;
      }
      else
      {
        Student newStudent = (Student) otherStudent;
        bool idEquality = this.Id.Equals(newStudent.Id);
        bool nameEquality = this.Name.Equals(newStudent.Name);
        bool enrollmentDateEquality = this.EnrollmentDate.Equals(newStudent.EnrollmentDate);
        return (idEquality && nameEquality && enrollmentDateEquality);
      }
    }

    public void AddCourse(Course newCourse)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);";
      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@CourseId";
      course_id.Value = newCourse.Id;
      cmd.Parameters.Add(course_id);
      MySqlParameter student_id = new MySqlParameter();
      student_id.ParameterName = "@StudentId";
      student_id.Value = _id;
      cmd.Parameters.Add(student_id);
      cmd.ExecuteNonQuery();
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
  }

    public List<Course> GetCourses()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT course_id FROM students_courses WHERE student_id = @StudentId;";
      MySqlParameter studentIdParameter = new MySqlParameter();
      studentIdParameter.ParameterName = "@StudentId";
      studentIdParameter.Value = _id;
      cmd.Parameters.Add(studentIdParameter);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> courseIds = new List<int> {};
      while(rdr.Read())
      {
        int courseId = rdr.GetInt32(0);
        courseIds.Add(courseId);
      }
      rdr.Dispose();
      List<Course> courses = new List<Course> {};
      foreach (int courseId in courseIds)
      {
        var courseQuery = conn.CreateCommand() as MySqlCommand;
        courseQuery.CommandText = @"SELECT * FROM courses WHERE id = @CourseId;";
        MySqlParameter courseIdParameter = new MySqlParameter();
        courseIdParameter.ParameterName = "@CourseId";
        courseIdParameter.Value = courseId;
        courseQuery.Parameters.Add(courseIdParameter);
        var courseQueryRdr = courseQuery.ExecuteReader() as MySqlDataReader;
        while(courseQueryRdr.Read())
        {
          int thisCourseId = courseQueryRdr.GetInt32(0);
          string courseName = courseQueryRdr.GetString(1);
          string courseNumber = courseQueryRdr.GetString(2);
          Course foundCourse = new Course(courseName, courseNumber, thisCourseId);
          courses.Add(foundCourse);
        }
        courseQueryRdr.Dispose();
      }
      conn.Close();
      if(conn!= null)
      {
        conn.Dispose();
      }
      return courses;
    }
      public override int GetHashCode()
        {
        return this.Id.GetHashCode();
        }

  }
}
