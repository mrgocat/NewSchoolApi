using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolApi.Core.Models
{
    public class EnrollmentPoco
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public StudentPoco Student { get; set; }
        public CoursePoco Course { get; set; }
    }
}
