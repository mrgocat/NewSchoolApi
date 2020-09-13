using Microsoft.EntityFrameworkCore;
using SchoolApi.Core.Models;
using SchoolApi.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Core.Services
{
    public class CourseService : ICourseService
    {
        private readonly SchoolContext _context;
        public CourseService(SchoolContext context)
        {
            _context = context;
        }

        public CoursePoco GetCourse(int id)
        {
            CoursePoco poco = _context.Courses.FirstOrDefault(e => e.Id == id);
            return poco;
        }

        public List<CoursePoco> GetCourses()
        {
            return _context.Courses.ToList();
        }

        public int AddCourse(CoursePoco Course)
        {
            _context.Courses.Add(Course);
            _context.SaveChanges();
            return Course.Id;
        }

        public void UpdateCourse(CoursePoco Course)
        {
            _context.Courses.Update(Course);
            _context.SaveChanges();
        }

        public void DeleteCourse(int id)
        {
            CoursePoco poco = _context.Courses.FirstOrDefault(e => e.Id == id);
            if (poco != null)
            {
                _context.Courses.Remove(poco);
                _context.SaveChanges();
            }
        }

        public List<EnrollmentPoco> GetEnrollments(int id)
        {
            List<EnrollmentPoco> list = _context.Enrollments.Where(e => e.StudentId == id).Include(e => e.Student).ToList();
            return list;
        }
    }
}
