using DemoProject.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly EmployeedbContext _dbContext;

        public ValuesController(EmployeedbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetEmployeeAndManagerNames()
        {
            var result = _dbContext.Employees
                .Select(e => new
                {
                    EmployeeName = e.Name,
                    ManagerName = _dbContext.Employees
                        .Where(m => m.Id == e.ManagerId)
                        .Select(m => m.Name)
                        .FirstOrDefault() ?? "No Manager"
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("manager-summary")]
        public IActionResult GetManagerSummary()
        {
            var result = _dbContext.Employees
                .GroupBy(e => e.ManagerId)
                .Select(g => new
                {
                    ManagerName = _dbContext.Employees
                        .Where(m => m.Id == g.Key)
                        .Select(m => m.Name)
                        .FirstOrDefault() ?? "No Manager",
                    TotalEmployees = g.Count()
                })
                .ToList();

            return Ok(result);
        }

        [HttpGet("managerInfo")]
        public async Task<IActionResult> GetManagerInfo()
        {
            var managerInfo = await _dbContext.Employees
                .Where(e => e.ManagerId != null)
                .Select(e => new
                {
                    ManagerName = e.Name,
                    ManagerSalary = e.EmployeeSalary
                })
                .ToListAsync();

            return Ok(managerInfo);
        }

        [HttpGet("second-highest-salary")]
        public IActionResult GetSecondHighestSalary()
        {
            var secondHighestSalary = _dbContext.Employees
                .OrderByDescending(e => e.EmployeeSalary)
                .Skip(1)
                .FirstOrDefault();

            if (secondHighestSalary != null)
            {
                var managerName = _dbContext.Employees
                    .FirstOrDefault(e => e.Id == secondHighestSalary.ManagerId)?.Name;

                return Ok(new { Salary = secondHighestSalary.EmployeeSalary, ManagerName = managerName });
            }

            return NotFound();
        }


    }

}

