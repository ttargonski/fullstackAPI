using FullStack.API.Data;
using FullStack.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FullStack.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly FullstackDbContext _fullstackDbContext;

        public EmployeesController(FullstackDbContext fullstackDbContext)
        {
            _fullstackDbContext = fullstackDbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployess()
        {
            var employees = await _fullstackDbContext.Employees.ToListAsync();

            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(Employee employeeResquest)
        {
            employeeResquest.Id = Guid.NewGuid();
            await _fullstackDbContext.Employees.AddAsync(employeeResquest);
            await _fullstackDbContext.SaveChangesAsync();

            return Ok(employeeResquest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee(Guid id)
        {
            var emp = await _fullstackDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);
            
            if(emp == null) {
                return NotFound();
            }

            return Ok(emp);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee(Guid id, Employee employee)
        {
            var emp = await _fullstackDbContext.Employees.FindAsync(id);

            if (emp == null)
            {
                return NotFound();
            }

            emp.Name = employee.Name;
            emp.Email = employee.Email; 
            emp.Phone = employee.Phone; 
            emp.Salary = employee.Salary;
            emp.Department = employee.Department;

            await _fullstackDbContext.SaveChangesAsync();

            return Ok(emp);
            
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid Id)
        {
            var emp = await _fullstackDbContext.Employees.FindAsync(Id);

            if (emp == null)
            {
                return NotFound();
            }

            _fullstackDbContext.Employees.Remove(emp);  
            await _fullstackDbContext.SaveChangesAsync();
            
            return Ok(Id);
        }
    }
}
