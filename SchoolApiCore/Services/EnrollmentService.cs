using Microsoft.EntityFrameworkCore;
using SchoolApi.Core.Models;
using SchoolApi.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Core.Services
{
    public class EnrollmentService : IEnrollmentService
    {
        private readonly SchoolContext _context;
        public EnrollmentService(SchoolContext context)
        {
            _context = context;
        }
        public void addEnrollment(int studentId, int courseId)
        {
            _context.Enrollments.Add(new EnrollmentPoco() { StudentId = studentId, CourseId = courseId });
            _context.SaveChanges();
        }

        public void removeEnrollment(int studentId, int courseId)
        {
            var enrollment = _context.Enrollments.FirstOrDefault(e => e.StudentId == studentId && e.CourseId == courseId);
            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
        }

        public List<EnrollmentPoco> GetEnrolledCourses(int studentId)
        {
            List<EnrollmentPoco> list = _context.Enrollments.Where(e => e.StudentId == studentId).Include(e => e.Course).ToList();
            return list;
        }

        public List<EnrollmentPoco> GetEnrolledStudents(int courseId)
        {
            List<EnrollmentPoco> list = _context.Enrollments.Where(e => e.CourseId == courseId).Include(e => e.Student).ToList();
            return list;
        }
    }
}
