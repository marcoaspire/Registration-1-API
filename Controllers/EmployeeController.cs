using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RegistrationAPI.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RegistrationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UserContext _context;
        public EmployeeController(UserContext context)
        {
            _context = context;
        }

        // GET: api/<EmployeeController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_context.Employees
                .Select(e => new
                {
                    User = e.User,
                    Role = e.Role,
                    Salary = e.Salary
                })
                //.Include(d => d.Employee)
                .ToList());
        }

        // GET api/<EmployeeController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var employee = _context.Employees
                .Select(e => new
                {
                    User = e.User,
                    Role = e.Role,
                    Salary = e.Salary
                })
                .SingleOrDefault(user => user.User.UserID == id);
            return Ok(new
            {
                Employee = employee

            });
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public void Post(Employee e)
        {
        }

        // PUT api/<EmployeeController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, Employee e)
        {
            Debug.WriteLine("recibido");

            Debug.WriteLine(e.User.UserID);

            var employee = _context.Employees.SingleOrDefault(e => e.User.UserID == id);
            if (id != e.User.UserID || employee==null)
            {
                return BadRequest();
            }

            try
            {

                if (e.Role != employee.Role && e.Role!= "" && e.Role != null)
                {
                    employee.Role = e.Role;
                }
                if (e.Salary != employee.Salary && e.Salary>0)
                {
                    employee.Salary = e.Salary;
                }
                _context.Entry(employee).State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound();
            }

            return Ok(new { employee = e });
        }

        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var e =  _context.Employees.Find(id);
            if (e == null)
            {
                return NotFound();
            }

                _context.Employees.Remove(e);
                _context.SaveChanges();

                return Ok(new { msg = "Employee deleted" });
            
        }
    }
}
