using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Wyk4.Models;
using Wyk4.Services;

namespace Wyk4.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18725;Integrated Security=True";

        private IStudentsDal _db;
        public StudentsController(IStudentsDal db)
        {
            _db = db;
        }


       
        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "select IndexNumber,FirstName,LastName,BirthDate ,Name,Semester from Student s join Enrollment e on e.IdEnrollment=s.IdEnrollment join Studies st on st.IdStudy=e.IdStudy";

                con.Open();
                SqlDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    var st = new Student();
                    st.IndexNumber = dr["IndexNumber"].ToString();
                    st.FirstName = dr["FirstName"].ToString();
                    st.LastName = dr["LastName"].ToString();
                    st.BirthDate = DateTime.Parse(dr["BirthDate"].ToString());
                    st.Name = dr["Name"].ToString();
                    st.Semester = int.Parse(dr["Semester"].ToString());
                    list.Add(st);
                }
                con.Dispose();
            }
            return Ok(list);
        }

        [HttpGet("{indexNumber}")]
        public IActionResult GetStudent(string indexNumber)
        {

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                //com.CommandText = "select * from student where indexnumber=@index";
                //com.CommandText = "select e.IdEnrollment, Semester, IdStudy, StartDate from Enrollment e join student s on e.IdEnrollment = s.IdEnrollment where indexnumber=@index";
                com.CommandText = "SELECT Name, Semester, startdate FROM Student S INNER JOIN Enrollment E on S.IdEnrollment = E.IdEnrollment INNER JOIN Studies St on E.IdStudy = St.IdStudy WHERE IndexNumber =@index";
                
                /* 1 sposób
                SqlParameter par = new SqlParameter();
                par.Value = indexNumber;
                par.ParameterName = "index";
                com.Parameters.Add(par);
                */
                com.Parameters.AddWithValue("index", indexNumber);

                con.Open();
                //var dr = com.ExecuteReader();
                SqlDataReader dr = com.ExecuteReader();
                var response = new List<string>();
                if(dr.Read())
                {
                    //var st = new Student();
                    

                    // if (dr["IndexNumber"] == DBNull.Value)
                    // {

                    //}

                    //st.IndexNumber = dr["IndexNumber"].ToString();
                    //st.FirstName = dr["FirstName"].ToString();
                    //st.LastName = dr["LastName"].ToString();

                    //en.CourseName = dr["CourseName"].ToString();
                    // en.Semester = Int32.Parse(dr["Semester"].ToString());
                    //en.StartDate = dr["StartDate"].ToString();

                    response.Add(" Studia: " + dr["Name"].ToString() + " Semestr: " + dr["Semester"].ToString() + " StartDate: " + dr["StartDate"].ToString());
                    return Ok(response);
                }
                con.Dispose();
            }
            return NotFound();
        }
        [HttpGet("ex2")]
        public IActionResult GetStudents2()
        {

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "nazwaprocedury";
                com.CommandType = System.Data.CommandType.StoredProcedure;

                com.Parameters.AddWithValue("LastName", "Kowalski");

                var dr = com.ExecuteReader();
            }
            return NotFound();
        }
        [HttpGet("ex3")]
        public IActionResult GetStudents3(string indexNumber)
        {

            using (SqlConnection con = new SqlConnection(ConString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "insert into Student(FirstName) values (@firstName)";

                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    int affectedRows = com.ExecuteNonQuery();

                    com.CommandText = "update into ...";
                    com.ExecuteNonQuery();

                    transaction.Commit();
                }catch(Exception e)
                {
                    transaction.Rollback();
                }
            }
            return Ok();
        }
    }
}