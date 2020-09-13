using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using NUnit.Framework;
using SchoolApi.Rest.Controllers;
using SchoolApi.Core.Models;
using SchoolApi.Core.Repository;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SchoolApi.UnitTest
{
    public class Tests
    {
        private StudentPoco _student;
        private CoursePoco _course;
        //private ILoggerFactory _loggerFactory;

        #region Cotroller_Creater
        private ServiceProvider _serviceProvider = null;
        private Dictionary<Type, ControllerBase> controllers = new Dictionary<Type, ControllerBase>();
        public void initServiceCollection()
        {
            IServiceCollection collection = new ServiceCollection();
            collection.AddDbContext<SchoolContext>(c => c.UseInMemoryDatabase("SchoolDB"));

/*            if (_loggerFactory == null)
                _loggerFactory = (ILoggerFactory)new LoggerFactory();

            _loggerFactory.AddConsole(System.Configuration.GetSection("Logging")); //log levels set in your configuration
            _loggerFactory.AddDebug(); //does all log levels

            services.AddSingleton(this._loggerFactory);*/
            collection.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
            collection.AddLogging();
        //    collection.AddTransient(typeof(IDataRepository<>), typeof(EFGenericRepository<>));
            _serviceProvider = collection.BuildServiceProvider();
        }
        public T getController<T>() where T : ControllerBase
        {
            var type = typeof(T);
            if (controllers.ContainsKey(type)) return controllers[type] as T;

            //   var constructor = type.GetConstructor(new Type[]{ typeof(IDataRepository<ApplicantEducationPoco>) });
            var constructor = type.GetConstructors()[0];

            List<object> pList = new List<object>();

            foreach (var p in constructor.GetParameters())
            {
                // CoursesController(SchoolContext ctx, ILogger<CoursesController> logger)
                if (p.ParameterType.GetTypeInfo().Name.StartsWith("SchoolContext"))
                {
                    pList.Add(_serviceProvider.GetRequiredService(p.ParameterType));
                }
                else if (p.ParameterType.GetTypeInfo().Name.StartsWith("ILogger"))
                {
                    pList.Add(_serviceProvider.GetService(p.ParameterType));
                 //   _serviceProvider.GetRequiredService
                //    pList.Add(new Logger<T>(_serviceProvider.GetRequiredService(p.ParameterType)));
                }
                else
                {
                    pList.Add(p.DefaultValue);
                }
            }

            T cont = Activator.CreateInstance(type, pList.ToArray()) as T;
            controllers.Add(type, cont);
            return cont;

        }
        #endregion

        [SetUp]
        public void Setup()
        {
            initServiceCollection();
            Student_Init();
            Course_Init();
        }
        #region PocoInitialization
        private void Student_Init()
        {
            _student = new StudentPoco()
            {
                Name = Faker.Name.FullName(),
                Age = Faker.Number.RandomNumber(20, 80)
            };
        }
        private void Course_Init()
        {
            _course = new CoursePoco()
            {
                Name = Truncate(Faker.Lorem.Sentence(), 150)
            };
        }
        #endregion 

        [Test]
        public void Cotroller_Test()
        {
            Student_Controller_Add_Test();
            Student_Controller_Update_Test();
            Student_Controller_Read_Test();
        }
        #region Controller_Test
        private void Student_Controller_Add_Test()
        {
            StudentsController studentController = getController<StudentsController>();
            ActionResult<StudentPoco> result = studentController.Create(_student);
            Assert.IsNotInstanceOf(typeof(OkResult), result);
            OkObjectResult oResult = (OkObjectResult)result.Result;
            Assert.IsNotNull(oResult);
            StudentPoco poco = (StudentPoco)(oResult).Value;
            Assert.IsNotNull(poco);
            Assert.IsTrue(_student.Name.Equals(poco.Name));
            Assert.IsTrue(_student.Age == poco.Age);
            _student = poco;
        }
        private void Student_Controller_Update_Test()
        {
            StudentsController studentController = getController<StudentsController>();
            _student.Age = Faker.Number.RandomNumber(20, 80);
            _student.Name = Faker.Name.FullName();
            ActionResult result = studentController.Update(_student);
            Assert.IsInstanceOf(typeof(OkResult), result);
        }
        private void Student_Controller_Read_Test()
        {
            StudentsController studentController = getController<StudentsController>();
            ActionResult<StudentPoco> result = studentController.Get(_student.Id);
            Assert.IsNotInstanceOf(typeof(OkResult), result);
            OkObjectResult oResult = (OkObjectResult)result.Result;
            Assert.IsNotNull(oResult);
            StudentPoco poco = (StudentPoco)(oResult).Value;
            Assert.IsNotNull(poco);
            Assert.IsTrue(_student.Name.Equals(poco.Name));
            Assert.IsTrue(_student.Age == poco.Age);
        }
        #endregion
        private string Truncate(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str)) return str;
            return str.Length <= maxLength ? str : str.Substring(0, maxLength);
        }
    }

}
