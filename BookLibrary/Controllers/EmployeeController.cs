using Backend.Domain.Entities;
using Backend.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;
        
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(EmployeeService employeeService, ILogger<EmployeeController> logger)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var employees = await _employeeService.Get();
                return Ok(employees);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet("{id}")]        
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                var employee = await _employeeService.Get(Id);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpGet("/Employee/GetEmployeesYearsOfService/{Id}")]
        public IActionResult GetEmployeesYearsOfService(int Id)
        {
            try
            {
                var result = _employeeService.GetEmployeesYearsOfService(Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            try
            {
                var Id = await _employeeService.Create(employee);
                return Ok(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(Employee employee)
        {
            try
            {
                var Id = await _employeeService.Update(employee);
                return Ok(Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                await _employeeService.Delete(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        
    }
}
