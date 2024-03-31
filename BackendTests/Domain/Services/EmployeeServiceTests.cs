using Backend.Controllers;
using Backend.Domain.Entities;
using Backend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.EntityFrameworkCore;

namespace Backend.Domain.Services.Tests
{
    namespace YourUnitTestProjectNamespace
    {
        [TestClass]
        public class EmployeeServiceTests
        {
            private readonly EmployeeDbContext _employeeDbContextMock;
            private readonly Mock<ILogger<EmployeeController>> _loggerMock;

            public EmployeeServiceTests()
            {
                var options = new DbContextOptionsBuilder<EmployeeDbContext>()
                    .UseInMemoryDatabase(databaseName: "EmployeesManagementDb")
                    .Options;

                _employeeDbContextMock = new EmployeeDbContext(options);

                _loggerMock = new Mock<ILogger<EmployeeController>>();
            }

            [TestMethod]
            public async Task Get_ReturnsEmployees()
            {
                // Arrange
                var sut = new EmployeeService(_employeeDbContextMock, _loggerMock.Object);
                await sut.Create(TestDataHelper.GetFakeEmployeeList()[0]);

                // Act
                var result = await sut.Get();

                // Assert
                var expected = TestDataHelper.GetFakeEmployeeList()[0].FirstName;                
                Assert.IsTrue(result.Any());
                Assert.IsTrue(expected == result.First().FirstName);
            }

            [TestMethod]
            public async Task Create_ReturnsEmployeeId()
            {   
                // Arrange                
                var sut = new EmployeeService(_employeeDbContextMock, _loggerMock.Object);

                // Act
                var result = await sut.Create(TestDataHelper.GetFakeEmployeeList()[0]);

                // Assert
                Assert.IsNotNull(result);
            }

            [TestMethod]
            public async Task Update_ReturnsEmployeeId()
            {
                // Arrange
                var sut = new EmployeeService(_employeeDbContextMock, _loggerMock.Object);
                var employee = TestDataHelper.GetFakeEmployeeList()[0];
                var id = await sut.Create(employee);

                // Act
                employee.FirstName = "New first Name";
                var result = await sut.Update(employee);

                // Assert
                Assert.AreEqual(id, result);
            }

            [TestMethod]
            public async Task Delete_NotReturnsEmployee()
            {
                // Arrange
                var sut = new EmployeeService(_employeeDbContextMock, _loggerMock.Object);
                var employee = TestDataHelper.GetFakeEmployeeList()[0];
                var id = await sut.Create(employee);

                // Act
                await sut.Delete(id);
                var result = await sut.Get();

                // Assert
                Assert.IsFalse(result.Any(x=>x.Id == id));
            }            
            
            [TestMethod]
            public async Task GetEmployeesYearsOfService_Returns_ForEachEmployee()
            {
                // Arrange
                var sut = new EmployeeService(_employeeDbContextMock, _loggerMock.Object);
                var employee = TestDataHelper.GetFakeEmployeeList()[0];
                var id = await sut.Create(employee);

                // Act
                var result = sut.GetEmployeesYearsOfService();

                // Assert
                Assert.IsTrue(result.Any(x=>x.Item2 == 10));
            }
        }
    }

    public static class TestDataHelper
    {
        public static List<Employee> GetFakeEmployeeList()
        {
            return new List<Employee> { new Employee { DateOfJoining = DateTime.Now.AddYears(-10), Email = "test@test.com", FirstName = "test", LastName = "test", JobTitle = "JobTest" } };
        }
    }    
}