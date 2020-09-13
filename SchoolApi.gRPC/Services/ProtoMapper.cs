using SchoolApi.gRPC.Protos;
using SchoolApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.gRPC.Services
{
    public static class ProtoMapper
    {

        public static Student MapFromStudentPoco(StudentPoco poco)
        {
            return new Student()
            {
                Id = poco.Id,
                Name = poco.Name,
                Age = poco.Age
            };
        }
        public static StudentPoco MapToStudentPoco(Student student)
        {
            return new StudentPoco()
            {
                Id = student.Id,
                Name = student.Name,
                Age = student.Age
            };
        }

        public static Course MapFromCoursePoco(CoursePoco poco)
        {
            return new Course()
            {
                Id = poco.Id,
                Name = poco.Name,
            };
        }
        public static CoursePoco MapToCoursePoco(Course course)
        {
            return new CoursePoco()
            {
                Id = course.Id,
                Name = course.Name,
            };
        }
    }
}
