using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SchoolApi.gRPC.Protos;
using SchoolApi.Core.Models;
using SchoolApi.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SchoolApi.gRPC.Protos.Courses;

namespace SchoolApi.gRPC.Services
{
    public class CourseGRPCService : CoursesBase
    {

        private readonly ICourseService _service;
        public CourseGRPCService(ICourseService service)
        {
            _service = service;
        }
        public override Task<Course> GetCourse(CourseKey req, ServerCallContext context)
        {
            var poco = _service.GetCourse(req.Id);
            if (poco != null)
            {
                return Task.FromResult(ProtoMapper.MapFromCoursePoco(poco));
            }
            else
            {
                return Task.FromResult(new Course() { Id = 0 });
            }
        }
        public override Task<CourseList> GetCourses(Empty empty, ServerCallContext context)
        {
            var list = _service.GetCourses();
            CourseList rList = new CourseList();
            foreach (var item in list)
            {
                rList.CourseList_.Add(ProtoMapper.MapFromCoursePoco(item));
            }
            return Task.FromResult(rList);
        }
        public override Task<CourseKey> AddCourse(Course course, ServerCallContext context)
        {
            CoursePoco poco = ProtoMapper.MapToCoursePoco(course);
            int id = _service.AddCourse(poco);

            return Task.FromResult(new CourseKey() { Id = id });
        }
        public override Task<Empty> UpdateCourse(Course course, ServerCallContext context)
        {
            CoursePoco poco = ProtoMapper.MapToCoursePoco(course);
            _service.UpdateCourse(poco);

            return Task.FromResult(new Empty());
        }
        public override Task<Empty> DeleteCourse(CourseKey key, ServerCallContext context)
        {
            _service.DeleteCourse(key.Id);

            return Task.FromResult(new Empty());
        }

    }
}
