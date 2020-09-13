using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchoolApi.gRPC.Protos;
using System.Net.Http;

namespace SchoolApi.gRPC.UnitTest
{
    [TestClass]
    public class SchoolApiGRPCUnitTest
    {
        private GrpcChannel _channel;

        [TestInitialize]
        public void Initialize()
        {
            var httpHandler = new HttpClientHandler();
            // Return `true` to allow certificates that are untrusted/invalid
            httpHandler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            _channel = GrpcChannel.ForAddress("https://localhost:5001",
                new GrpcChannelOptions
                {
                    HttpHandler = httpHandler
                });
        }

        [TestMethod]
        public void StudentRPCTest()
        {
            var client = new Students.StudentsClient(_channel);
            Student student = new Student()
            {
                Id = 0,
                Name = "name1",
                Age = 33
            };
            StudentKey key = client.AddStudent(student);
            Assert.IsTrue(key.Id > 0);
            student.Id = key.Id;

            Student temp = client.GetStudent(key);
            Assert.AreEqual(temp.Id, student.Id);
            Assert.AreEqual(temp.Name, student.Name);
            Assert.AreEqual(temp.Age, student.Age);

            StudentList list = client.GetStudents(new Empty());
            foreach(var item in list.StudentList_)
            {
                if (item.Id != student.Id) continue;
                Assert.AreEqual(item.Name, student.Name);
                Assert.AreEqual(item.Age, student.Age);
            }

            student.Name = "Name2";
            student.Age = 43;
            client.UpdateStudent(student);
            temp = client.GetStudent(key);
            Assert.AreEqual(temp.Id, student.Id);
            Assert.AreEqual(temp.Name, student.Name);
            Assert.AreEqual(temp.Age, student.Age);

            client.DeleteStudent(key);
            temp = client.GetStudent(key);
            Assert.IsTrue(temp.Id == 0); ;
        }
        [TestMethod]
        public void CourseRPCTest()
        {
            var client = new Courses.CoursesClient(_channel);
            Course course = new Course()
            {
                Id = 0,
                Name = "name1",
            };
            CourseKey key = client.AddCourse(course);
            Assert.IsTrue(key.Id > 0);
            course.Id = key.Id;

            Course temp = client.GetCourse(key);
            Assert.AreEqual(temp.Id, course.Id);
            Assert.AreEqual(temp.Name, course.Name);

            CourseList list = client.GetCourses(new Empty());
            foreach (var item in list.CourseList_)
            {
                if (item.Id != course.Id) continue;
                Assert.AreEqual(item.Name, course.Name);
            }

            course.Name = "Name2";
            client.UpdateCourse(course);
            temp = client.GetCourse(key);
            Assert.AreEqual(temp.Id, course.Id);
            Assert.AreEqual(temp.Name, course.Name);

            client.DeleteCourse(key);
            temp = client.GetCourse(key);
            Assert.IsTrue(temp.Id == 0); ;
        }

        [TestMethod]
        public void EnrollmentRPCTest()
        {
            var clientCourse = new Courses.CoursesClient(_channel);
            var clientStudent = new Students.StudentsClient(_channel);
            var clientEnrollment = new Enrollments.EnrollmentsClient(_channel);
            Course course = new Course()
            {
                Id = 0,
                Name = "name1",
            };
            CourseKey courseKey = clientCourse.AddCourse(course);
            Assert.IsTrue(courseKey.Id > 0);
            course.Id = courseKey.Id;

            Student student = new Student()
            {
                Id = 0,
                Name = "name1",
                Age = 33
            };
            StudentKey studentKey = clientStudent.AddStudent(student);
            Assert.IsTrue(studentKey.Id > 0);

            student.Name = "Name2";
            student.Age = 44;
            studentKey = clientStudent.AddStudent(student);

            Assert.IsTrue(studentKey.Id > 0);
            student.Id = studentKey.Id;

            clientEnrollment.AddEnrollments(new EnrollmentKey() { StudentId = student.Id, CourseId = course.Id });

            StudentList studentList = clientEnrollment.GetEnrolledStudents(new CourseKey() { Id = course.Id });
            int cnt = 0;
            foreach (var item in studentList.StudentList_)
            {
                Assert.AreEqual(item.Id, student.Id);
                Assert.AreEqual(item.Name, student.Name);
                Assert.AreEqual(item.Age, student.Age);
                cnt++;
            }
            Assert.IsTrue(cnt == 1);

            CourseList courseList = clientEnrollment.GetEnrolledCourses(new StudentKey() { Id = student.Id });
            cnt = 0;
            foreach (var item in courseList.CourseList_)
            {
                Assert.AreEqual(item.Id, course.Id);
                Assert.AreEqual(item.Name, course.Name);
                cnt++;
            }
            Assert.IsTrue(cnt == 1);

            clientEnrollment.RemoveEnrollments(new EnrollmentKey() { StudentId = student.Id, CourseId = course.Id });

            cnt = 0;
            studentList = clientEnrollment.GetEnrolledStudents(new CourseKey() { Id = course.Id });
            foreach (var item in studentList.StudentList_)
            {
                cnt++;
            }
            Assert.IsTrue(cnt == 0);

            courseList = clientEnrollment.GetEnrolledCourses(new StudentKey() { Id = student.Id });
            foreach (var item in courseList.CourseList_)
            {
                cnt++;
            }
            Assert.IsTrue(cnt == 0);

        }

    }
}
