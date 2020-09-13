using Microsoft.EntityFrameworkCore;
using SchoolApi.Core.Models;
using SchoolApi.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly SchoolContext _context;
        public StudentService(SchoolContext context)
        {
            _context = context;
        }

        public StudentPoco GetStudent(int id)
        {
            StudentPoco poco = _context.Students.FirstOrDefault(e => e.Id == id);
            return poco;
        }

        public List<StudentPoco> GetStudents()
        {
            return _context.Students.ToList();
        }

        public int AddStudent(StudentPoco student)
        {
            _context.Students.Add(student);
            _context.SaveChanges();
            return student.Id;
        }

        public void UpdateStudent(StudentPoco student)
        {
            _context.Students.Update(student);
            _context.SaveChanges();
        }

        public void DeleteStudent(int id)
        {
            StudentPoco poco = _context.Students.FirstOrDefault(e => e.Id == id);
            if (poco != null)
            {
                _context.Students.Remove(poco);
                _context.SaveChanges();
            }
        }

        public List<EnrollmentPoco> GetEnrollments(int id)
        {
            List<EnrollmentPoco> list = _context.Enrollments.Where(e => e.StudentId == id).Include(e => e.Course).ToList();
            return list;
        }
    }
}
