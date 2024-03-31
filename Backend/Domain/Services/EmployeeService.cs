using Backend.Controllers;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Backend.Domain.Services
{
    public class EmployeeService
    {
        private readonly EmployeeDbContext _employeeDbContext;

        private readonly ILogger<EmployeeController> _logger;

        public EmployeeService(EmployeeDbContext bookDbContext, ILogger<EmployeeController> logger)
        {
            _employeeDbContext = bookDbContext;
            _logger = logger;
        }

        public async Task<IEnumerable<Employee>> Get()
        {
            var employees = await _employeeDbContext.Employees.ToListAsync();
            return employees;
        }

        public async Task<Employee> Get(int Id)
        {
            var employee = await _employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);
            return employee;
        }

        public async Task<int> Create(Employee employee)
        {
            await _employeeDbContext.Employees.AddAsync(employee);
            await _employeeDbContext.SaveChangesAsync();
            return employee.Id;
        }

        public async Task<int> Update(Employee employee)
        {
            _employeeDbContext.Employees.Update(employee);
            await _employeeDbContext.SaveChangesAsync();
            return employee.Id;
        }

        public async Task Delete(int Id)
        {
            var employee = await _employeeDbContext.Employees.FirstOrDefaultAsync(x => x.Id == Id);
            _employeeDbContext.Employees.Remove(employee);
            await _employeeDbContext.SaveChangesAsync();            
        }

        public int GetEmployeesYearsOfService(int Id)
        {
            var employee = _employeeDbContext.Employees.FirstOrDefault(x => x.Id == Id);

            if(employee == null)
                throw new Exception($"Employee with Id {Id} not found");

            var yearOfService = DateTime.Now - employee.DateOfJoining;
            return (int) yearOfService.TotalDays / 365;
            
        }

    }
}
