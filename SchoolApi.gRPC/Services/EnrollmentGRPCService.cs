using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SchoolApi.gRPC.Protos;
using SchoolApi.Core.Models;
using SchoolApi.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SchoolApi.gRPC.Protos.Enrollments;

namespace SchoolApi.gRPC.Services
{
    public class EnrollmentGRPCService : EnrollmentsBase
    {
        private readonly IEnrollmentService _service;
        public EnrollmentGRPCService(IEnrollmentService service)
        {
            _service = service;
        }


        public override Task<Empty> AddEnrollments(EnrollmentKey key, ServerCallContext context)
        {
            _service.addEnrollment(key.StudentId, key.CourseId);

            return Task.FromResult(new Empty());
        }
        public override Task<Empty> RemoveEnrollments(EnrollmentKey key, ServerCallContext context)
        {
            _service.removeEnrollment(key.StudentId, key.CourseId);

            return Task.FromResult(new Empty());
        }
        public override Task<StudentList> GetEnrolledStudents(CourseKey key, ServerCallContext context)
        {
            List<EnrollmentPoco> list = _service.GetEnrolledStudents(key.Id);
            StudentList rList = new StudentList();
            foreach (var item in list)
            {
                rList.StudentList_.Add(ProtoMapper.MapFromStudentPoco(item.Student));
            }
            return Task.FromResult(rList);
        }
        public override Task<CourseList> GetEnrolledCourses(StudentKey key, ServerCallContext context)
        {
            List<EnrollmentPoco> list = _service.GetEnrolledCourses(key.Id);
            CourseList rList = new CourseList();
            foreach (var item in list)
            {
                rList.CourseList_.Add(ProtoMapper.MapFromCoursePoco(item.Course));
            }
            return Task.FromResult(rList);
        }
    }
}
