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


        #region EmployeeServices CLASS CONSTRUCTOR
        public EmployeeServices()
        {
            this.employeeRepository = new Repository<Employee>();
        }
        #endregion

        
        #region GET ALL EMPLOYEES
        public IEnumerable<EmployeeDto> GetAll()
        {
            try
            {
                return this.employeeRepository.Set()
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
            catch
            {
                NewLine();
                Console.WriteLine("Se produjo un ERROR al intentar obtener todos los Empleados.");

                return null;
            }
        }
        #endregion


        #region GET REAL EMPLOYEE BY ID (NO DTO)
        public Employee GetEmployeeByID(Nullable<int> employeeId,ServicesController services)
        {
            try
            {
                var employee = services.employeeServices.employeeRepository.Set().ToList()
                .FirstOrDefault(e => e.EmployeeID == employeeId);

                if (employee == null)
                {
                    NewLine();
                    Console.WriteLine("No existe el Empleado!");

                    return null;
                }

                return employee;

            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar obtener el Empleado con ID : '{employeeId}'.");

                return null;
            }
        }
        #endregion
        

        #region GET EMPLOYEE DTO BY ID
        public EmployeeDto GetEmployeeDtoByID(Nullable<int> employeeId,ServicesController services)
        {
            try
            {
                var employee = services.employeeServices.employeeRepository.Set().ToList()
                .FirstOrDefault(e => e.EmployeeID == employeeId);

                if (employee == null)
                {
                    NewLine();
                    Console.WriteLine("No existe el Empleado!");

                    return null;
                }

                var employeeDto = new EmployeeDto()
                {
                    EmployeeID = employee.EmployeeID,
                };

                return employeeDto;
            }
            catch
            {
                NewLine();
                Console.WriteLine($"Se produjo un ERROR al intentar obtener el Empleado con ID : '{employeeId}'.");

                return null;
            }
        }
        #endregion

        
        #region NEW CONSOLE EMPTY COMMAND LINE
        public void NewLine()
        {
            Console.WriteLine("");
        }
        #endregion
    }
}
