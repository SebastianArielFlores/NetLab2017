using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Dtos;

namespace Services
{
    public class EmployeeServices
    {
        Repository<Employee> employeeRepository;

        public EmployeeServices()
        {
            employeeRepository = new Repository<Employee>();
        }

        #region GET ALL EMPLOYEES
        public IEnumerable<EmployeeDto> GetAll()
        {
            return employeeRepository.Set()
                   .ToList()
                   .Select(e => new EmployeeDto
                   {
                       EmployeeID = e.EmployeeID,
                       LastName = e.LastName,
                       FirstName = e.FirstName,
                       /*
                       Title = e.Title,
                       TitleOfCourtesy = e.TitleOfCourtesy,
                       BirthDate = e.BirthDate,
                       HireDate = e.HireDate,
                       Address = e.Address,
                       City = e.City,
                       Region = e.Region,
                       PostalCode = e.PostalCode,
                       Country = e.Country,
                       HomePhone = e.HomePhone,
                       Extension = e.Extension,
                       //Photo=e.Photo,
                       Notes = e.Notes,
                       ReportsTo = e.ReportsTo,
                       PhotoPath = e.PhotoPath,

                       //Employees1=e = e.Employees1,
                       //Employee1 = e.Employee1
                       //Orders
                       */
                   }).ToList();
        }
        #endregion


        #region GET REAL EMPLOYEE BY ID (NO DTO)
        //public EmployeeDto GetEmployeeByID(Nullable<int> employeeId)
        public Employee GetEmployeeByID(Nullable<int> employeeId,ServicesController services)
        {
            var employee = services.employeeServices.employeeRepository.Set().ToList()
                .FirstOrDefault(e => e.EmployeeID == employeeId);

            if (employee == null)
            {
                Console.WriteLine("No existe el Empleado!");
                return null;
            }

            //var employeeDto = new EmployeeDto()
            var employeeDto = new Employee()
            {
                EmployeeID = employee.EmployeeID,
                //ContactName = employee.ContactName,
                //CompanyName = employee.CompanyName,
            };

            return employee;

        }
        #endregion



        #region GET EMPLOYEE DTO BY ID
        //public EmployeeDto GetEmployeeByID(Nullable<int> employeeId)
        public EmployeeDto GetEmployeeDtoByID(Nullable<int> employeeId,ServicesController services)
        {
            var employee = services.employeeServices.employeeRepository.Set().ToList()
                .FirstOrDefault(e => e.EmployeeID == employeeId);

            if (employee == null)
            {
                Console.WriteLine("No existe el Empleado!");
                return null;
            }

            //var employeeDto = new EmployeeDto()
            var employeeDto = new EmployeeDto()
            {
                EmployeeID = employee.EmployeeID,
                //ContactName = employee.ContactName,
                //CompanyName = employee.CompanyName,
            };

            return employeeDto;

        }
        #endregion
    }
}
