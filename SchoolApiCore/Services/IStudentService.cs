using SchoolApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Core.Services
{
    public interface IStudentService
    {
        public StudentPoco GetStudent(int id);
        public List<StudentPoco> GetStudents();
        public int AddStudent(StudentPoco student);
        public void UpdateStudent(StudentPoco student);
        public void DeleteStudent(int id);
        public List<EnrollmentPoco> GetEnrollments(int id);
    }
}
