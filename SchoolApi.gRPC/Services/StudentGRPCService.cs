using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using SchoolApi.gRPC.Protos;
using SchoolApi.Core.Models;
using SchoolApi.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SchoolApi.gRPC.Protos.Students;

namespace SchoolApi.gRPC.Services
{
    public class StudentGRPCService : StudentsBase
    {

        private readonly IStudentService _service;
        public StudentGRPCService(IStudentService service)
        {
            _service = service;
        }
        public override Task<Student> GetStudent(StudentKey req, ServerCallContext context)
        {
            var poco = _service.GetStudent(req.Id);
            if (poco != null)
            {
                return Task.FromResult(ProtoMapper.MapFromStudentPoco(poco));
            }
            else
            {
                return Task.FromResult(new Student() { Id = 0 });
            }
        }
        public override Task<StudentList> GetStudents(Empty empty, ServerCallContext context)
        {
            var list = _service.GetStudents();
            StudentList rList = new StudentList();
            foreach (var item in list)
            {
                rList.StudentList_.Add(ProtoMapper.MapFromStudentPoco(item));
            }
            return Task.FromResult(rList);
        }
        public override Task<StudentKey> AddStudent(Student student, ServerCallContext context)
        {
            int id = 0;
            StudentPoco poco = ProtoMapper.MapToStudentPoco(student);
            id = _service.AddStudent(poco);

            return Task.FromResult(new StudentKey() { Id = id });
        }
        public override Task<Empty> UpdateStudent(Student student, ServerCallContext context)
        {
            StudentPoco poco = ProtoMapper.MapToStudentPoco(student);
            _service.UpdateStudent(poco);

            return Task.FromResult(new Empty());
        }
        public override Task<Empty> DeleteStudent(StudentKey key, ServerCallContext context)
        {
            _service.DeleteStudent(key.Id);

            return Task.FromResult(new Empty());
        }

    }
}
