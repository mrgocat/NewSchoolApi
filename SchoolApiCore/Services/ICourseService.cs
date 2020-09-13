using SchoolApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Core.Services
{
    public interface ICourseService
    {
        public CoursePoco GetCourse(int id);
        public List<CoursePoco> GetCourses();
        public int AddCourse(CoursePoco Course);
        public void UpdateCourse(CoursePoco Course);
        public void DeleteCourse(int id);
        public List<EnrollmentPoco> GetEnrollments(int id);
    }
}
