using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Dtos;

namespace Services
{
    public class CustomerServices
    {
        Repository<Customer> customerRepository;

        Repository<Employee> employeeRepository;

        #region CustomerServices CLASS CONSTRUCTOR
        public CustomerServices()
        {
            this.customerRepository = new Repository<Customer>();

            this.employeeRepository = new Repository<Employee>();
        }
        #endregion


        #region GET ALL CUSTOMERS DTO
        public IEnumerable<CustomerDto> GetAllDto()
        {
            try
            {
                return this.customerRepository.Set()
                   .ToList()
                   .Select(c => new CustomerDto
                   {
                       Country = c.Country,
                       ContactName = c.ContactName,

                   }).ToList();
            }
            catch
            {
                Console.WriteLine("Se produjo un ERROR al intentar recuperar todos los Clientes...");

                return null;
            }
        }
        #endregion


        #region GET ALL CUSTOMERS
        public IEnumerable<Customer> GetAll()
        {
            try
            {
                return this.customerRepository.Set()
                   .ToList()
                   .Select(c => new Customer
                   {
                       CustomerID = c.CustomerID,
                       Country = c.Country,
                       ContactName = c.ContactName,
                       Orders = c.Orders,
                   }).ToList();
            }
            catch
            {
                Console.WriteLine("Se produjo un ERROR al intentar recuperar todos los Clientes...");

                return null;
            }
        }
        #endregion
        

        #region GET CUSTOMER DTO BY ID
        public CustomerDto GetCustomerDtoByID(string customerId, ServicesController services)
        {
            try
            {
                var customer = services.customerServices.customerRepository.Set().ToList()
                .FirstOrDefault(c => c.CustomerID == customerId);

                if (customer == null)
                {
                    NewLine();
                    Console.WriteLine("No existe el Cliente!");

                    return null;
                }

                var customerDto = new CustomerDto()
                {
                    CustomerID = customer.CustomerID,
                    ContactName = customer.ContactName,
                    CompanyName = customer.CompanyName,
                };

                return customerDto;
            }
            catch
            {
                Console.WriteLine($"Se produjo un ERROR al intentar recuperar el Cliente con ID : '{customerId}'...");

                return null;
            }
            
        }
        #endregion


        #region GET REAL CUSTOMER BY ID (NO DTO)
        public Customer GetCustomerByID(string customerId, ServicesController services)
        {
            try
            {
                var customer = services.customerServices.customerRepository.Set().ToList()
                .FirstOrDefault(c => c.CustomerID == customerId);

                if (customer == null)
                {
                    NewLine();
                    Console.WriteLine("No existe el Cliente!");

                    return null;
                }

                var customerByID = new Customer()
                {
                    CustomerID = customer.CustomerID,
                    ContactName = customer.ContactName,
                    CompanyName = customer.CompanyName,
                    Country = customer.Country,
                };

                return customerByID;
            }
            catch
            {
                Console.WriteLine($"Se produjo un ERROR al intentar recuperar el Cliente con ID : '{customerId}'...");

                return null;
            }
        }
        #endregion


        #region Update / UPDATE CUSTOMER
        public void Update(CustomerDto customerDto)
        {
            try
            {
                var customer = this.customerRepository.Set()
                .FirstOrDefault(x => x.CustomerID == customerDto.CustomerID);

                if (customer == null)
                    throw new Exception("El Cliente no existe");

                Console.WriteLine($"Categoria : {customerDto.CustomerID} ha sido encontrado.");
                Console.WriteLine($"Su Nombre de Contacto es : {customerDto.ContactName}.");

                //CustomerID
                NewLine();
                Console.WriteLine("Ingrese el nuevo ID del Cliente:");
                customerDto.CustomerID = Console.ReadLine();

                //ContactName
                NewLine();
                Console.WriteLine("Ingrese el nuevo Nombre de Contacto:");
                customerDto.ContactName = Console.ReadLine();

                //ContactName
                NewLine();
                Console.WriteLine("Ingrese el nuevo Nombre de Compañia:");
                customerDto.CompanyName = Console.ReadLine();

                //...

                customer.Address = customerDto.Address;
                customer.City = customerDto.City;
                customer.CompanyName = customerDto.CompanyName;
                customer.ContactName = customerDto.ContactName;
                customer.ContactTitle = customerDto.ContactTitle;
                customer.Country = customerDto.Country;
                customer.Region = customerDto.Region;
                customer.PostalCode = customerDto.PostalCode;
                customer.Phone = customerDto.Phone;
                customer.Fax = customerDto.Fax;

                this.customerRepository.Update(customer);
                this.customerRepository.SaveChanges();
            }
            catch
            {
                Console.WriteLine($"Se produjo un ERROR al intentar Actualizar el Cliente con ID : '{customerDto.CustomerID}'.");
            }
        }
        #endregion
        

        #region CREATE CUSTOMER

        public void Create(CustomerDto dto)
        {
            try
            {
                this.customerRepository.Persist(new Customer
                {
                    Address = dto.Address,
                    City = dto.City,
                    CompanyName = dto.CompanyName,
                    ContactName = dto.ContactName,
                    ContactTitle = dto.ContactTitle,
                    Country = dto.Country,
                    CustomerID = dto.CustomerID,
                    Region = dto.Region,
                    PostalCode = dto.PostalCode,
                    Phone = dto.Phone,
                    Fax = dto.Fax,
                });

                this.customerRepository.SaveChanges();
            }
            catch
            {
                Console.WriteLine("Se produjo un ERROR al intentar Crear el Cliente...");
            }
            
        }
        #endregion


        #region REMOVE CUSTOMER

        public void Remove(CustomerDto dto)
        {
            try
            {
                var customer = this.customerRepository.Set()
                .FirstOrDefault(x => x.CustomerID == dto.CustomerID);

                if (customer == null)
                    throw new Exception("El Cliente no existe");

                this.customerRepository.Remove(customer);
                this.customerRepository.SaveChanges();
            }
            catch
            {
                Console.WriteLine($"Se produjo un ERROR al intentar Eliminar el Cliente con ID : '{dto.CustomerID}'.");
            }
            
        }
        #endregion


        #region BEST CUSTOMER BY COUNTRY

        public List<BestCustomerDto> GetBestCostumer(ServicesController services)
        {
            try
            {
                var customers = services.customerServices.GetAll();

                var bestCostumers = customers
                    .Where(c => c.Country != null)
                    .GroupBy(c => c.Country)
                    .Select(k => new BestCustomerDto
                    {
                        Country = k.Key,

                        Name = k
                            .OrderByDescending(c => c.Orders
                                .Sum(o => o.Order_Details
                                .Sum(d => d.Quantity * d.Product.UnitPrice)))
                            .Select(c => c.ContactName)
                            .FirstOrDefault(),

                        TotalPurchased = k.Select(v => v.Orders
                            .Where(c => c.CustomerID == k
                            .OrderByDescending(b => b.Orders
                                .Sum(o => o.Order_Details
                                .Sum(d => d.Quantity * d.Product.UnitPrice)))
                            .Select(b => b.CustomerID)
                            .FirstOrDefault())
                            .Sum(g => g.Order_Details
                                .Sum(d => d.Quantity * d.Product.UnitPrice)))
                            .Sum()

                    }).ToList();

                return bestCostumers;

            }
            catch
            {
                Console.WriteLine("Se produjo un ERROR al intentar obtener el mejor Cliente por País...");

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
