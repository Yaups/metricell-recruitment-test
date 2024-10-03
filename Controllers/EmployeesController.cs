using InterviewTest.Model;
using InterviewTest.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeesController : ControllerBase
    {
        public EmployeesController(EmployeesService service)
        {
            _service = service;
        }

        private readonly EmployeesService _service;

        [HttpGet]
        public ActionResult<List<Employee>> Get()
        {
            return _service.GetAll();
        }
    }
}
