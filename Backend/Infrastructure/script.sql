create database EmployeeManagement;

GO

use EmployeeManagement;

GO

create table employees (
  Id INT PRIMARY KEY IDENTITY (1, 1),
  FirstName VARCHAR(100) not null,
  LastName VARCHAR (50) NOT NULL,
  Email VARCHAR (50) NOT NULL,
  JobTitle VARCHAR (50) NOT NULL,
  DateOfJoining date NOT NULL
);

select * from employees;