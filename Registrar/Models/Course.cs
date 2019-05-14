using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Registrar.Models
{
  public class Course
  {
    private string _name;
    public string Name { get { return _name;} }

    private string _number;
    public string Number { get { return _number;} }

    private int _id;
    public int Id { get { return _id;} }

    public Course(string name, string number, int id =0)
    {
      _name = name;
      _number = number;
      _id = id;
    }

    public static void ClearAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM courses;";
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
      cmd.CommandText = @"INSERT INTO courses (name, number) VALUES (@name, @number);";
      MySqlParameter name = new MySqlParameter();
      name.ParameterName = "@name";
      name.Value = this._name;
      cmd.Parameters.Add(name);
      MySqlParameter number = new MySqlParameter();
      number.ParameterName = "@number";
      number.Value = this._number;
      cmd.Parameters.Add(number);
      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Course> GetAll()
    {
      List<Course> allCourses = new List<Course> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int Id = rdr.GetInt32(0);
        string Name = rdr.GetString(1);
        string Number = rdr.GetString(2);
        Course newCourse = new Course(Name, Number, Id);
        allCourses.Add(newCourse);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCourses;
    }

    public static Course Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM courses WHERE id = (@searchId);";
      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int CourseId = 0;
      string CourseName = "";
      string CourseNumber = "";
      while(rdr.Read())
      {
        CourseId = rdr.GetInt32(0);
        CourseName = rdr.GetString(1);
        CourseNumber = rdr.GetString(2);
      }
      Course newCourse = new Course(CourseName, CourseNumber, CourseId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newCourse;
    }

    public List<Student> GetStudents()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT student_id FROM students_courses WHERE course_id = @CourseId;";
      MySqlParameter CourseIdParameter = new MySqlParameter();
      CourseIdParameter.ParameterName = "@CourseId";
      CourseIdParameter.Value = _id;
      cmd.Parameters.Add(CourseIdParameter);
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      List<int> studentIds = new List<int> {};
      while(rdr.Read())
      {
        int Id = rdr.GetInt32(0);
        studentIds.Add(Id);

      }
      rdr.Dispose();
      List<Student> students = new List<Student> {};
      foreach (int Id in studentIds)
      {
        var StudentQuery = conn.CreateCommand() as MySqlCommand;
        StudentQuery.CommandText = @"SELECT * FROM students WHERE id = @StudentId;";
        MySqlParameter StudentIdParameter = new MySqlParameter();
        StudentIdParameter.ParameterName = "@StudentId";
        StudentIdParameter.Value = Id;
        StudentQuery.Parameters.Add(StudentIdParameter);
        var studentQueryRdr = StudentQuery.ExecuteReader() as MySqlDataReader;
        while(studentQueryRdr.Read())
        {
          int StudentId = studentQueryRdr.GetInt32(0);
          string StudentName = studentQueryRdr.GetString(1);
          string StudentDate = studentQueryRdr.GetString(2);
          Student foundStudent = new Student (StudentName, StudentDate, StudentId);
          students.Add(foundStudent);

        }
        studentQueryRdr.Dispose();
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return students;
    }

    public override bool Equals(System.Object otherCourse)
    {
      if(!(otherCourse is Course))
      {
        return false;
      }
      else
      {
        Course newCourse = (Course) otherCourse;
        bool idEquality = this.Id.Equals(newCourse.Id);
        bool nameEquality = this.Name.Equals(newCourse.Name);
        bool numberEquaity = this.Number.Equals(newCourse.Number);
        return (idEquality && nameEquality && numberEquaity);
      }
    }

    public void AddStudent(Student newStudent)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO students_courses (student_id, course_id) VALUES (@StudentId, @CourseId);";
      MySqlParameter course_id = new MySqlParameter();
      course_id.ParameterName = "@CourseId";
      course_id.Value = newStudent.Id;
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

  }
}
