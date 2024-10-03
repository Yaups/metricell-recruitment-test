using InterviewTest.DTOs;
using InterviewTest.Model;
using InterviewTest.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace InterviewTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ListController : ControllerBase
    {
        public ListController(EmployeesService service)
        {
            _service = service;
        }

        private readonly EmployeesService _service;

        [HttpGet("{name}")]
        public ActionResult<Employee> Find(string name)
        {
            var employee = _service.FindByName(name);

            if (employee is null)
                return NotFound();

            return employee;
        }

        [HttpPost]
        public ActionResult<Employee> Create(Employee newEmployee)
        {
            var employeeWithExistingName = _service.FindByName(newEmployee.Name);
            if (employeeWithExistingName is not null)
                return BadRequest($"Employee with name {newEmployee.Name} already exists");

            if (newEmployee.Value < 0)
                return BadRequest("Employee's value must be positive");

            var createdEmployee = _service.Create(newEmployee);

            if (createdEmployee is null)
                return BadRequest("Employee could not be added");

            return createdEmployee;
        }

        [HttpPut("{name}")]
        public IActionResult Update(string name, Employee updatedEmployee)
        {
            var employeeWithExistingName = _service.FindByName(updatedEmployee.Name);
            if (employeeWithExistingName is not null && name != updatedEmployee.Name)
                return BadRequest($"Employee with name {updatedEmployee.Name} already exists");

            if (updatedEmployee.Value < 0)
                return BadRequest("Employee's value must be positive");

            var employeeToUpdate = _service.FindByName(name);

            if (employeeToUpdate is null)
                return NotFound("Person to update does not exist in the database");

            _service.FindByNameAndUpdate(name, updatedEmployee);

            return NoContent();
        }

        [HttpDelete("{name}")]
        public IActionResult Delete(string name)
        {
            var employee = _service.FindByName(name);

            if (employee is null)
                return NotFound();

            _service.FindByNameAndRemove(name);

            return NoContent();
        }

        [HttpGet("sum")]
        public ActionResult<SumQueryResponse> ExecuteSumQuery()
        {
            var sumQueryResult = _service.ExecuteSumQuery();

            return sumQueryResult;
        }

        [HttpPut("increment")]
        public IActionResult ExecuteIncrementQuery()
        {
            _service.ExecuteIncrementQuery();

            return NoContent();
        }
    }
}
