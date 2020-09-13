using SchoolApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Core.Services
{
    public interface IEnrollmentService
    {
        public void addEnrollment(int studentId, int courseId);
        public void removeEnrollment(int studentId, int courseId);
        public List<EnrollmentPoco> GetEnrolledStudents(int id);
        public List<EnrollmentPoco> GetEnrolledCourses(int id);
    }
}
